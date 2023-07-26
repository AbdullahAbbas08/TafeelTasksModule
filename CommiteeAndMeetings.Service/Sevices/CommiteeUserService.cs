using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.BLL.Hosting;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Enums;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using HelperServices;
using HelperServices.LinqHelpers;
using IHelperServices;
using IHelperServices.Models;
using LinqHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Models;
using Models.ProjectionModels;
using NJsonSchema.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using static iTextSharp.text.pdf.AcroFields;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class CommiteeUserService : BusinessService<CommiteeMember, CommiteeMemberDTO>, ICommiteeUserService
    {
        public readonly IUnitOfWork _unitOfWork;
        //IHelperServices.ISessionServices _sessionServices;
        private readonly IHelperServices.ISessionServices _sessionServices;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISecurityService _securityService;
        ICommitteeNotificationService _committeeNotificationService;
        ICommiteeLocalizationService _commiteeLocalizationService;
        public readonly MasarContext _context;
        private readonly IMailServices _MailServices;
        private readonly ICommitteeMeetingSystemSettingService _systemSettingsService;
        ISmsServices smsServices;
        public CommiteeUserService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISmsServices _smsServices, ICommitteeMeetingSystemSettingService systemSettingsService , IMailServices MailServices , ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings, IHttpContextAccessor contextAccessor, ICommitteeNotificationService committeeNotificationService, ICommiteeLocalizationService commiteeLocalizationService)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _sessionServices = sessionServices;
            _contextAccessor = contextAccessor;
            _securityService = securityService;
            _committeeNotificationService = committeeNotificationService;
            _commiteeLocalizationService = commiteeLocalizationService;
            _MailServices = MailServices;
            _systemSettingsService = systemSettingsService;
            _context = new MasarContext();
            smsServices = _smsServices;
        }

        public DataSourceResult<CommiteeMember> GetActiveUsersByCommitteeId(DataSourceRequest dataSourceRequest, int CommitteeId)
        {
            IQueryable query = this._UnitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == CommitteeId && x.Active);
            return query.Cast<CommiteeMember>().ToDataSourceResult(dataSourceRequest);
        }
        public string ActiveDisactiveMember(string ids, bool Active,MemberState memberState)
        {
            CommiteeMember member = default;
            foreach (var id in ids.Split(","))
            {
                member = this._UnitOfWork.GetRepository<CommiteeMember>().GetById(id, false);
                member.Active = Active;
                member.MemberState = memberState;
                this._UnitOfWork.GetRepository<CommiteeMember>().Update(member);
            }
            
            // send email
            string Message = "";
            string mailSubject = "";

           // var user = _unitOfWork.GetRepository<User>().GetAll().Where(x => x.UserId == item.UserId).FirstOrDefault();

            //Send SMS If This user has mobile Number
           

            // check if committe Date is open or has begin and end date
            string commiteeDate;
            if (member.Commitee.ValidityPeriod[0].ValidityPeriodTo == default && member.Commitee.ValidityPeriod[0].ValidityPeriodTo == default)
            {
                commiteeDate = "لجنة دائمة";
            }
            else
            {

                commiteeDate = $" من تاريخ {member.Commitee.ValidityPeriod[0].ValidityPeriodFrom.ToString("MM/dd/yyyy")} -  إلى تاريخ {member.Commitee.ValidityPeriod[0].ValidityPeriodTo.ToString("MM/dd/yyyy")} ";
            }
            CommiteeMemberDTO commiteeMemberDTO = new CommiteeMemberDTO();
            var item = _Mapper.Map(member, commiteeMemberDTO);
            GetMailMessageForMeeting(item, ref Message, ref mailSubject,member.Commitee.Name, commiteeDate);
            AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
            Task.Run(() =>
            {
                _MailServices.SendNotificationEmail(member.User.Email, mailSubject,
                    null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                    );

            });
            return ids;
        }
        public List<VwLookUpReturnUser> GetExternalUsers(int take, int skip, string search)
      {
            var Users = _unitOfWork.GetRepository<User>().GetAll();
            var query = Users.Where(x => x.Enabled && x.ExternalUser).Where(u => u.FullNameAr.Contains(search) || u.FullNameEn.Contains(search) || u.FullNameFn.Contains(search) || search == null || search =="");
            var pagedQuery = query.Skip((skip ) * take).Take(take).Select(x => new VwLookUpReturnUser
            {
                Id = x.UserId,
                Name = _sessionServices.CultureIsArabic == true ? x.FullNameAr : x.FullNameEn
            }).ToList();
            //var query = this._UnitOfWork.GetRepository<User>().GetAll().Where(x => x.ExternalUser && (string.IsNullOrEmpty(search) || x.Username.Contains(search) || x.FullNameAr.Contains(search) || x.FullNameEn.Contains(search))).Take(take).Skip(skip).Select(x => new LookUpDTO
            //{
            //    Id = x.UserId,
            //    Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
            //}).ToList();
            //return query;
            //int userId = (int) _sessionServices.UserId;
            //var look = _context.VwLookUpReturnUsers.FromSqlInterpolated($"dbo.[GetUserForCommitteInternalOrExternal]{userId},{skip+1},{take},{search},{"External"},{_sessionServices.CultureIsArabic}").ToList();
            //return look;
            return pagedQuery;
        }
        public override IEnumerable<CommiteeMemberDTO> Insert(IEnumerable<CommiteeMemberDTO> entities)
        {
            foreach (var item in entities)
            {
                if (!string.IsNullOrEmpty(item.CommiteeIdEncrypt))
                {
                    item.CommiteeId = _sessionServices.UserIdAndRoleIdAfterDecrypt(item.CommiteeIdEncrypt,true).Id;
                    foreach (var Role in item.CommiteeRoles)
                    {
                        Role.CommiteeId = _sessionServices.UserIdAndRoleIdAfterDecrypt(Role.CommiteeIdEncrypt,true).Id;
                    }
                }
            }

            var User = base.Insert(entities);
            var latestItemInsert = _unitOfWork.GetRepository<CommiteeMember>().GetAll().OrderByDescending(x => x.CommiteeMemberId).FirstOrDefault();
            var committe = _unitOfWork.GetRepository<Commitee>().GetAll().Where(x => x.CommiteeId == latestItemInsert.CommiteeId).FirstOrDefault();
            foreach (var item in entities)
            {
                //item.CommiteeId = _sessionServices.UserIdAndRoleIdAfterDecrypt(item.CommiteeId).Id.ToString();
                item.CommiteeMemberId = latestItemInsert.CommiteeMemberId;
                
                var loc = _unitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddCommiteeMemberNotificationText");
                if (item.UserId != _sessionServices.UserId)
                {
                    CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                    {
                        IsRead = false,
                        UserId = (int)item.UserId,
                        TextAR = loc.CommiteeLocalizationAr,
                        TextEn = loc.CommiteeLocalizationEn,
                        CommiteeId = item.CommiteeId
                    };
                    List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                    _committeeNotificationService.Insert(committeeNotifications);
                }
                // Send Mail
                // if user is deserve not send mail and sms
                if (!item.IsReserveMember)
                { 

                    string Message = "";
                    string mailSubject = "";

                    var user = _unitOfWork.GetRepository<User>().GetAll().Where(x => x.UserId == item.UserId).FirstOrDefault();

                    //Send SMS If This user has mobile Number
                    Task.Run(() =>
                    {
                        //Here is a new thread
                        if (user.NotificationBySms && !string.IsNullOrEmpty(user.Mobile))
                        {
                            smsServices.SendSMS(user.Mobile, new string[0] { }, "تم إضافتك إلى لجنة", null);

                        }
                    });

                    // check if committe Date is open or has begin and end date
                    string commiteeDate;
                    if (committe.ValidityPeriod[0].ValidityPeriodTo == default && committe.ValidityPeriod[0].ValidityPeriodTo == default)
                    {
                        commiteeDate = "لجنة دائمة";
                    }
                    else
                    {

                        commiteeDate =$" من تاريخ {committe.ValidityPeriod[0].ValidityPeriodFrom.ToString("dd/MM/yyyy")} -  إلى تاريخ {committe.ValidityPeriod[0].ValidityPeriodTo.ToString("dd/MM/yyyy")} ";
                    }
                    GetMailMessageForMeeting(item, ref Message, ref mailSubject, committe.Name ,commiteeDate);
                AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                Task.Run(() =>
                {
                    _MailServices.SendNotificationEmail(user.Email, mailSubject,
                        null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                        );

                });
                }
            }

            User.FirstOrDefault().CommiteeMemberId = latestItemInsert.CommiteeMemberId;

            return User;
        }
        // Confirm or Reject From Mail And Change State 

        public bool ConfirmChangeMemberState(string commiteeMemberId , MemberState memberState)
        {
            try
            {
                string decodeduserid = HttpUtility.UrlDecode(commiteeMemberId);
                UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _sessionServices.UserIdAndRoleIdAfterDecrypt(decodeduserid, false);
                var item =_unitOfWork.GetRepository<CommiteeMember>().GetAll().FirstOrDefault(x => x.CommiteeMemberId == UserIdAndUserRoleId.Id);
                 item.MemberState = memberState;
                 _unitOfWork.GetRepository<CommiteeMember>().Update(item);
                 _unitOfWork.SaveChanges();
                return true;
            }
            catch
            {

            return false;
            }
        }

        

        public List<VwLookUpReturnUser> GetInternalUsers(int take, int skip, string search)
        {
            var Users = _unitOfWork.GetRepository<User>().GetAll();
            var query = Users.Where(x=>x.Enabled && !x.ExternalUser).Where(u => u.FullNameAr.Contains(search) || u.FullNameEn.Contains(search) || u.FullNameFn.Contains(search) || search == null || search == "");
            var pagedQuery = query.Skip((skip ) * take).Take(take).Select(x => new VwLookUpReturnUser
            {
                Id = x.UserId,
                Name = _sessionServices.CultureIsArabic == true ?x.FullNameAr :x.FullNameEn
            }).ToList();

          
            //var query = this._UnitOfWork.GetRepository<User>().GetAll().Where(x => !x.ExternalUser && x.Enabled &&(string.IsNullOrEmpty(search) || x.Username.Contains(search) || x.FullNameAr.Contains(search) || x.FullNameEn.Contains(search))).Take(take).Skip(skip).Select(x => new LookUpDTO
            //{
            //    Id = x.UserId,
            //    Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
            //}).ToList();
            //return query;
           // int userId =(int) _sessionServices.UserId;
           //var look = _context.VwLookUpReturnUsers.FromSqlInterpolated($"dbo.[GetUserForCommitteInternalOrExternal]{userId},{skip+1},{take},{search},{"Internal"},{_sessionServices.CultureIsArabic}").ToList();
            //look.Select(x => new LookUpDTO
            //{
            //    Id = x.Id,
            //    Name = x.Name
            //}).ToList();
            return pagedQuery;
        }

        public List<LookUpDTO> GetRoles(int take, int skip)
        {
            var query = this._UnitOfWork.GetRepository<CommiteeRole>().GetAll().Take(take).Skip(skip).Select(x => new LookUpDTO
            {
                Id = x.CommiteeRoleId,
                Name = _sessionServices.CultureIsArabic ? x.CommiteeRolesNameAr : x.CommiteeRolesNameEn
            }).ToList();
            return query;
        }
        public DataSourceResult<CommiteeMemberDTO> GetAllWithRoles(DataSourceRequest dataSourceRequest)
        {
            IQueryable query = this._UnitOfWork.GetRepository<CommiteeMember>().GetAll().Include(c => c.CommiteeRoles).Where(x => x.Active);
            return query.Cast<CommiteeMemberDTO>().ToDataSourceResult(dataSourceRequest);
        }
        public DataSourceResult<CommiteeMemberDTO> GetAllByType(DataSourceRequest dataSourceRequest, bool external)
        {
            IQueryable query = this._UnitOfWork.GetRepository<CommiteeMember>().GetAll(false).Where(x => x.User.ExternalUser == external);
            return query.ProjectTo<CommiteeMemberDTO>(_Mapper.ConfigurationProvider).ToDataSourceResult(dataSourceRequest);
        }

        public CommiteeRoleDTO GetCommitteRole(int commiteeRoleId)
        {
            IQueryable query = this._UnitOfWork.GetRepository<CommiteeRole>().GetAll(false).Where(c => c.CommiteeRoleId == commiteeRoleId);
            return query.ProjectTo<CommiteeRoleDTO>(_Mapper.ConfigurationProvider).FirstOrDefault();
        }

        public UserDetailsDTO GetCommitteUser(int userId)
        {
            var query = this._UnitOfWork.GetRepository<User>().GetAll().Where(x => x.UserId == userId);
            return query.ProjectTo<UserDetailsDTO>(_Mapper.ConfigurationProvider).FirstOrDefault();
        }

        public DataSourceResult<CommiteeMemberDTO> GetAllWithCounts(DataSourceRequest dataSourceRequest, string SearchName)
        {
           // int? commiteeId = 0;
            if (Convert.ToBoolean(dataSourceRequest.Filter?.Field == "CommiteeId"))
            {
                UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId =
                    _sessionServices.UserIdAndRoleIdAfterDecrypt(dataSourceRequest.Filter?.Value,false);

               // commiteeId = UserIdAndUserRoleId.Id;
                dataSourceRequest.Filter.Value = UserIdAndUserRoleId.Id.ToString();
            }

            if (SearchName != null && SearchName != "" && SearchName != string.Empty)
            {
                var query = this._UnitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.User.FullNameAr.Contains(SearchName) || x.User.FullNameEn.Contains(SearchName)).Select(x => new CommiteeMemberDTO
                {
                    Active = x.Active,
                    CommiteeId = x.CommiteeId,
                    CommiteeMemberId = x.CommiteeMemberId,
                    MemberState = x.MemberState,
                    CommiteeRoles = x.CommiteeRoles.Where(x => x.Enabled && (x.EnableUntil == null || x.EnableUntil > DateTimeOffset.Now)).Select(x => new CommiteeUsersRoleDTO
                    {
                        CommiteeId = x.CommiteeId,
                        CreatedUser = _unitOfWork.GetRepository<User>(false).GetAll(false).Select(y => new UserDetailsDTO
                        {
                            UserId = y.UserId,
                            UserName = y.Username,
                            FullNameAr = y.FullNameAr,
                            FullNameEn = y.FullNameEn,
                        }).FirstOrDefault(z => z.UserId == x.CreatedBy),
                        Delegated = x.Delegated,
                        Enabled = x.Enabled,
                        UserId = x.UserId,
                        EnableUntil = x.EnableUntil,
                        Notes = x.Notes,
                        RoleId = x.RoleId,
                        RoleNameAR = _unitOfWork.GetRepository<CommiteeRole>(false).GetAll(false).FirstOrDefault(a => a.CommiteeRoleId == x.RoleId).CommiteeRolesNameAr,
                        RoleNameEn = _unitOfWork.GetRepository<CommiteeRole>(false).GetAll(false).FirstOrDefault(a => a.CommiteeRoleId == x.RoleId).CommiteeRolesNameEn,
                        CommiteeUsersRoleId = x.CommiteeUsersRoleId,
                        IsMangerRole = _unitOfWork.GetRepository<CommiteeRole>(false).GetAll(false).FirstOrDefault(a => a.CommiteeRoleId == x.RoleId).IsMangerRole,
                        CreatedOn = x.CreatedOn

                    }).ToList(),
                    //_Mapper.Map(, typeof(CommiteeUsersRole), typeof(CommiteeUsersRoleDTO)) as List<CommiteeUsersRoleDTO>,
                    CountOfTasks = _unitOfWork.GetRepository<CommiteeTask>(false).GetAll(false).Where(z => z.MainAssinedUserId == x.UserId && z.CommiteeId == x.CommiteeId).Count(),
                    CountOfAttachments = _unitOfWork.GetRepository<CommiteeSavedAttachment>(false).GetAll(false).Where(z => z.CreatedBy == x.UserId && z.CommiteeId == x.CommiteeId).Count(),
                    User = GetUserdata(x.User),
                    UserId = x.UserId,
                    CreatedOn = x.CreatedOn,
                    IsLogin = _unitOfWork.GetRepository<UserToken>(false).GetAll(false).Where(a => a.UserId == x.UserId).FirstOrDefault() != null ? true : false
                }).ToList();
                return query.Cast<CommiteeMemberDTO>().AsQueryable().ToDataSourceResult(dataSourceRequest);
            }
            else
            {

                var query = this._UnitOfWork.GetRepository<CommiteeMember>().GetAll().Select(x => new CommiteeMemberDTO
                {
                    Active =x.Active,
                    MemberState = x.MemberState,
                    CommiteeId = x.CommiteeId,
                    CommiteeMemberId = x.CommiteeMemberId,
                    CommiteeRoles = x.CommiteeRoles.Where(x => x.Enabled && (x.EnableUntil == null || x.EnableUntil > DateTimeOffset.Now)).Select(x => new CommiteeUsersRoleDTO
                    {
                        CommiteeId = x.CommiteeId,
                        CreatedUser = _unitOfWork.GetRepository<User>(false).GetAll(false).Select(y => new UserDetailsDTO
                        {
                            UserId = y.UserId,
                            UserName = y.Username,
                            FullNameAr = y.FullNameAr,
                            FullNameEn = y.FullNameEn,
                        }).FirstOrDefault(z => z.UserId == x.CreatedBy),
                        Delegated = x.Delegated,
                        Enabled = x.Enabled,
                        UserId = x.UserId,
                        EnableUntil = x.EnableUntil,
                        Notes = x.Notes,
                        RoleId = x.RoleId,
                        RoleNameAR = _unitOfWork.GetRepository<CommiteeRole>(false).GetAll(false).FirstOrDefault(a => a.CommiteeRoleId == x.RoleId).CommiteeRolesNameAr,
                        RoleNameEn = _unitOfWork.GetRepository<CommiteeRole>(false).GetAll(false).FirstOrDefault(a => a.CommiteeRoleId == x.RoleId).CommiteeRolesNameEn,
                        CommiteeUsersRoleId = x.CommiteeUsersRoleId,
                        IsMangerRole = _unitOfWork.GetRepository<CommiteeRole>(false).GetAll(false).FirstOrDefault(a => a.CommiteeRoleId == x.RoleId).IsMangerRole,
                        CreatedOn = x.CreatedOn
                    }).ToList(),
                    //_Mapper.Map(, typeof(CommiteeUsersRole), typeof(CommiteeUsersRoleDTO)) as List<CommiteeUsersRoleDTO>,
                    CountOfTasks = _unitOfWork.GetRepository<CommiteeTask>(false).GetAll(false).Where(z => z.MainAssinedUserId == x.UserId && z.CommiteeId == x.CommiteeId).Count(),
                    CountOfAttachments = _unitOfWork.GetRepository<CommiteeSavedAttachment>(false).GetAll(false).Where(z => z.CreatedBy == x.UserId && z.CommiteeId == x.CommiteeId).Count(),
                    User = GetUserdata(x.User),
                    UserId = x.UserId,
                    CreatedOn = x.CreatedOn,
                    IsLogin = _unitOfWork.GetRepository<UserToken>(false).GetAll(false).Where(a => a.UserId == x.UserId).FirstOrDefault() != null ? true : false,
                });
                return query.Cast<CommiteeMemberDTO>().AsQueryable().ToDataSourceResult(dataSourceRequest);
            }

            //.Where(x => (SearchName == null || SearchName == string.Empty || (SearchName.Contains(x.User.FullNameAr) || SearchName.Contains(x.User.FullNameEn))));

        }

        private static UserDetailsDTO GetUserdata(User user)
        {
            return new UserDetailsDTO
            {
                UserId = user.UserId,
                FullNameAr = user.FullNameAr,
                FullNameEn = user.FullNameEn,
                Email = user.Email,
                ProfileImage = user.ProfileImage,
                ExternalUser = user.ExternalUser
            };
        }

        public CommiteeUsersRoleDTO DelegateUser(int userId, int committeId, int committeMemberId, string Note, DateTimeOffset? ToDate)
        {
            var MangerRole = _unitOfWork.GetRepository<CommiteeRole>().GetAll().Where(c => c.IsMangerRole).FirstOrDefault();
            if (MangerRole == null)
            {
                return null;
            }
            else
            {
                CommiteeUsersRoleDTO usersRole = new CommiteeUsersRoleDTO
                {
                    CommiteeId = committeId,
                    Enabled = true,
                    RoleId = MangerRole.CommiteeRoleId,
                    UserId = userId,
                    RoleNameAR = _unitOfWork.GetRepository<CommiteeRole>(false).GetAll(false).FirstOrDefault(a => a.CommiteeRoleId == MangerRole.CommiteeRoleId).CommiteeRolesNameAr,
                    RoleNameEn = _unitOfWork.GetRepository<CommiteeRole>(false).GetAll(false).FirstOrDefault(a => a.CommiteeRoleId == MangerRole.CommiteeRoleId).CommiteeRolesNameEn,
                    Notes = Note,
                    EnableUntil = ToDate,
                    Delegated = true,
                    IsMangerRole = _unitOfWork.GetRepository<CommiteeRole>(false).GetAll(false).FirstOrDefault(a => a.CommiteeRoleId == MangerRole.CommiteeRoleId).IsMangerRole,
                    CreatedOn = DateTime.Now
                    //CreatedUser = _unitOfWork.GetRepository<User>().GetAll().FirstOrDefault(c => c.UserId == _sessionServices.UserId)
                };
                var NewMember = _unitOfWork.GetRepository<CommiteeUsersRole>().Insert(new CommiteeUsersRole
                {
                    CommiteeId = committeId,
                    Enabled = true,
                    EnableUntil = ToDate,
                    Notes = Note,
                    RoleId = MangerRole.CommiteeRoleId,
                    UserId = userId,
                    Delegated = true,
                    CommiteeMemberId = committeMemberId,
                    CreatedOn = DateTime.Now
                });

                var loc = _unitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "DelegateUserNotificationText");
                if (NewMember.UserId != _sessionServices.UserId)
                {
                    CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                    {
                        IsRead = false,
                        UserId = (int)NewMember.UserId,
                        TextAR = loc.CommiteeLocalizationAr,
                        TextEn = loc.CommiteeLocalizationEn,
                        CommiteeId = NewMember.CommiteeId
                    };
                    List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                    _committeeNotificationService.Insert(committeeNotifications);
                }
                usersRole.CommiteeUsersRoleId = NewMember.CommiteeUsersRoleId;
                usersRole.CreatedUser = _unitOfWork.GetRepository<User>().GetAll().Select(x => new UserDetailsDTO
                {
                    UserId = x.UserId,
                    UserName = x.Username,
                    FullNameAr = x.FullNameAr,
                    FullNameEn = x.FullNameEn,
                }).FirstOrDefault(c => c.UserId == _sessionServices.UserId);

                return usersRole;
            }
        }
        public UserProfileDetailsDTO GetUserProfile()
        {
            UserProfileDetailsDTO user = _unitOfWork.GetRepository<User>().GetAll().Include(x => x.Genders).Where(c => c.UserId == _sessionServices.UserId).Select(x => new UserProfileDetailsDTO
            {
                UserId = x.UserId,
                UserName = x.Username,
                FullNameAr = x.FullNameAr,
                FullNameEn = x.FullNameEn,
                FullNameFn = x.FullNameFn,
                Email = x.Email,
                Mobile = x.Mobile,
                WorkPhoneNumber = x.WorkPhoneNumber,
                Nationality = _sessionServices.Culture.ToLower() == "ar" ? x.Nationality.NationalityNameAr
                                    : _sessionServices.Culture.ToLower() == "en" ? x.Nationality.NationalityNameEn
                                    : x.Nationality.NationalityNameFn,
                EmployeeNumber = x.EmployeeNumber,
                SSN = x.Ssn,
                IqamaNumber = x.IqamaNumber,
                Address = x.Address,
                IsCorrespondent = x.IsCorrespondent,
                IsCorrespondentForAllOrganizations = x.IsCorrespondentForAllOrganizations,
                NotificationBySMS = x.NotificationBySms,
                NotificationByMail = x.NotificationByMail,
                IsIndividual = x.IsIndividual,
                ProfileImage = x.ProfileImage,
                IsAdmin = x.IsAdmin,
                HasFactorAuth = x.HasFactorAuth,
                HasSignatureFactorAuth = x.HasSignatureFactorAuth,
                Gender = _sessionServices.Culture.ToLower() == "ar" ? _UnitOfWork.GetRepository<Gender>(false).GetAll(false).FirstOrDefault(C => C.GenderId == x.GenderId).GenderNameAr
                    : _UnitOfWork.GetRepository<Gender>(false).GetAll(false).FirstOrDefault(C => C.GenderId == x.GenderId).GenderNameFn
                    ,
                JobTitle = _sessionServices.Culture.ToLower() == "ar" ? _UnitOfWork.GetRepository<JobTitle>(false).GetAll(false).FirstOrDefault(C => C.JobTitleId == x.JobTitleId).JobTitleNameAr
                    : _UnitOfWork.GetRepository<JobTitle>(false).GetAll(false).FirstOrDefault(C => C.JobTitleId == x.JobTitleId).JobTitleNameEn

                //JobTitle = _sessionServices.Culture.ToLower() == "ar" ? x.j.JobTitleAr
                //: _sessionServices.Culture.ToLower() == "en" ? x.JobTitleEn : x.JobTitleFn
            }).FirstOrDefault();
            return user;
        }
        public UserDetailsDTO AddUserImage(int userId, byte[] ProfileImage, string ProfileImageMimeType)
        {

            MemoryStream ms = new MemoryStream(ProfileImage);
            Image newImage = GetReducedImage(100, 100, ms);
            byte[] ProfileImageThumbnail = ImageToByteArray(newImage);
            UserDetailsDTO userDetailsDTO = new UserDetailsDTO();
            User currentUser = _unitOfWork.GetRepository<User>().GetById(userId);

            currentUser.UserId = userId;
            currentUser.ProfileImage = ProfileImageThumbnail;
            currentUser.ProfileImageMimeType = ProfileImageMimeType;
            _UnitOfWork.GetRepository<User>().Update(currentUser);
            return _Mapper.Map(currentUser, userDetailsDTO);
        }
        public int GetCurrentUserId()
        {
            ClaimsIdentity claimsIdentity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            Claim userDataClaim = claimsIdentity?.FindFirst(ClaimTypes.UserData);
            string userId = userDataClaim?.Value;
            return string.IsNullOrWhiteSpace(userId) ? 0 : int.Parse(userId);
        }

        public Task<User> GetCurrentUserAsync()
        {
            int userId = GetCurrentUserId();
            return FindUserAsync(userId);
        }
        public async Task<User> FindUserAsync(int userId)
        {
            return await _unitOfWork.GetRepository<User>().FindAsync(userId);
        }
        public async Task<(bool Succeeded, string Error)> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            string currentPasswordHash = _securityService.GetSha256Hash(currentPassword);
            // User previousUser = user.ShallowCopy();
            if (user.Password != currentPasswordHash)
            {
                return (false, "Current password is wrong.");
            }

            user.Password = _securityService.GetSha256Hash(newPassword);
            user.PasswordUpdatedOn = DateTime.UtcNow;
            user.SerialNumber = Guid.NewGuid().ToString("N"); // To force other logins to expire.
            await _unitOfWork.SaveChangesAsync();

            PasswordHistory passwordHistory = new PasswordHistory()
            {
                IsFromAdmin = false,
                Password = newPassword,
                UserName = user.Username
            };
            _unitOfWork.GetRepository<PasswordHistory>().Insert(new List<PasswordHistory>() { passwordHistory });

            return (true, string.Empty);
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            if (imageIn != null)
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            }
            return ms.ToArray();
        }
        private Image GetReducedImage(int width, int height, Stream resourceImage)
        {
            try
            {
                Image image = Image.FromStream(resourceImage);
                Image thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);

                return thumb;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool DisableDelegateUser(int userRoleID)
        {
            try
            {
                var UsersRole = _unitOfWork.GetRepository<CommiteeUsersRole>().GetById(userRoleID);
                UsersRole.Enabled = false;
                UsersRole.EnableUntil = DateTime.Now;
                _unitOfWork.GetRepository<CommiteeUsersRole>().Update(UsersRole);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public string GetRoleName(int roleId, string Culture)
        {
            return Culture == "Ar" ? _unitOfWork.GetRepository<CommiteeRole>().GetAll().Where(x => x.CommiteeRoleId == roleId).FirstOrDefault().CommiteeRolesNameAr :
                  _unitOfWork.GetRepository<CommiteeRole>().GetAll().Where(x => x.CommiteeRoleId == roleId).FirstOrDefault().CommiteeRolesNameEn;
        }

        public bool CheckIfUserExixt(int commiteeId, int userId, int roleId)
        {
            return _unitOfWork.GetRepository<CommiteeUsersRole>().GetAll().Where(x => x.UserId == userId && !x.Delegated && x.CommiteeId == commiteeId).Count() > 0;
        }

        public string GetUserRole(int userId, int committeeId)
        {
            var CommiteeUsersRole = _unitOfWork.GetRepository<CommiteeUsersRole>().GetAll().Where(x => x.CommiteeId == committeeId && x.UserId == userId).FirstOrDefault();
            if (CommiteeUsersRole == null)
            {
                return _sessionServices.CultureIsArabic ? "لا يوجد دور وظيفى " : "No Role";
            }
            var userRole = CommiteeUsersRole.Role;
            return _sessionServices.CultureIsArabic ? userRole.CommiteeRolesNameAr : userRole.CommiteeRolesNameEn;
        }

        public List<LookUpDTO> GetDeleatedUsers(int committeeId)
        {
            var query = this._UnitOfWork.GetRepository<CommiteeUsersRole>().GetAll().Where(x => x.Delegated && x.Enabled && x.CommiteeId == committeeId).Select(x => new LookUpDTO
            {
                Id = x.UserId,
                Name = _sessionServices.CultureIsArabic ? x.User.FullNameAr : x.User.FullNameEn
            }).Distinct().ToList();
            return query;
        }





        //private UserDetailsDTO GetUser(int userId)
        //{
        //    IQueryable query = this._UnitOfWork.GetRepository<User>().GetAll(false).Where(c => c.UserId == userId);
        //    return query.ProjectTo<UserDetailsDTO>(_Mapper.ConfigurationProvider).FirstOrDefault();
        //}

        //private  CommiteeRoleDTO GetUserRole(int commiteeRoleId)
        //{
        //    IQueryable query = _UnitOfWork.GetRepository<CommiteeRole>().GetAll(false).Where(c => c.CommiteeRoleId == commiteeRoleId);
        //    return query.ProjectTo<CommiteeRoleDTO>(_Mapper.ConfigurationProvider).FirstOrDefault();
        //}

        // Mail 
        // Get Mail Message For meeting 
        public void GetMailMessageForMeeting(CommiteeMemberDTO commiteeMemberDTO, ref string mailMessage, ref string mailSubject, string mailTitle , string commiteeDate)
        {
            try
            {


                string subject = _commiteeLocalizationService.GetLocaliztionByCode("AddCommiteeMemberNotificationText", _sessionServices.CultureIsArabic);
                var JobTitleAr = _commiteeLocalizationService.GetLocaliztionByCode("commiteeName", true);
                var JobTitleEn = _commiteeLocalizationService.GetLocaliztionByCode("commiteeName", false);
                var CommitteDateAr = _commiteeLocalizationService.GetLocaliztionByCode("CommitteDate", true);
                var CommitteDateEn = _commiteeLocalizationService.GetLocaliztionByCode("CommitteDate", false);
                var taskCreatorAr = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", true);
                var taskCreatorEn = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", false);
                var taskDetailsLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("ConfirmMemberState", true);
                var taskDetailsLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("ConfirmMemberState", false);
                var RejectMemberStateLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("RejectMemberState", true);
                var RejectMemberStateLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("RejectMemberState", false);
                mailSubject = subject;
                string systemsettinglink = _systemSettingsService.GetByCode("taskDetailsLink").SystemSettingValue;
                if (commiteeMemberDTO.UserId == 0)
                {
                    
                        var lastInsertedMemberId = _unitOfWork.GetRepository<CommiteeMember>().GetAll().OrderByDescending(x => x.CommiteeMemberId).FirstOrDefault().CommiteeMemberId;
                        commiteeMemberDTO.CommiteeMemberId = lastInsertedMemberId;
                    
                    
                }
                string meetingEncyption = Encription.EncriptStringAES(commiteeMemberDTO.CommiteeMemberId.ToString());
                string unicodedCommiteeMemberId = HttpUtility.UrlEncode(meetingEncyption);
                string taskAfterReplace = null;
                var ComfirmDetailsLink = "";
                var RejectDetailsLink = "";
                

                ComfirmDetailsLink = $"{systemsettinglink}/assets/pages/confirmCommitte.html?commiteeMemberId={unicodedCommiteeMemberId}&memberState={2}";

                RejectDetailsLink = $"{systemsettinglink}/assets/pages/confirmCommitte.html?commiteeMemberId={unicodedCommiteeMemberId}&memberState={4}";
                                     

                string CreateDate = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("ar-AE"));
                string CreateDateEn = DateTime.Now.ToString("dd-MM-yyyy");

                //string Subject = ;
                //string lblTransactionType = emailParams.lblTransactionType;
                //string lblTransactionDate = emailParams.lblTransactionDate;
                //string lblDelegationFromOrg = emailParams.lblDelegationFromOrg;
                //string lblUrl = emailParams.lblTransURL;
                //string lblRequiredAction = emailParams.lblRequiredAction;
                //string lblTransactionNumberEn = emailParams.lblTransactionNumberEn;
                //string lblTransactionSubjectEn = emailParams.lblTransactionSubjectEn;
                //string lblTransactionTypeEn = emailParams.lblTransactionTypeEn;
                //string lblTransactionDateEn = emailParams.lblTransactionDateEn;
                //string lblDelegationFromOrgEn = emailParams.lblDelegationFromOrgEn;
                //string lblUrlEn = emailParams.lblTransURLEn;
                //string lblRequiredActionEn = emailParams.lblRequiredActionEn;
                //string lblMailHeader = emailParams.lblMailHeader;
                //string lblThanks = emailParams.ThanksLocalization;
                //string lblThanks2 = emailParams.ThanksLocalization2;
                //string incomingOrgNameAr = string.Empty;
                //string incomingOrgNameEn = string.Empty;
                //string IncomingOrganizationTR = string.Empty;
                //string ExternalDelegationOrgsTR = string.Empty;
                //string EmailValueColor = emailParams.EmailValueColor;
                //string EmailLblColor = emailParams.EmailLblColor;
                string Email_style = @"
	                                        text-align: center;
	                                        flex-direction: column;
	                                        justify-content: center;
	                                        align-items: center;
                                            direction: rtl;
                                        ";
                string Email_image_style = @" 
                                            text-align: center !important;
	                                        margin: auto !important;
	                                        justify-content: center;
	                                        margin: 32px;";
                string image_style = @"margin: 0;";
                string table_style = @"
	                                        width: 100%;
	                                        margin-bottom: -5px;
	                                        direction: rtl;
	                                        border: 1px solid #cccccc;
	                                        display: table;
	                                        border-collapse: collapse;
	                                        border-spacing: 2px;
	                                        border-color: grey;";
                string tr_style = @" 
	                                        //white-space: normal;
	                                        //line-height: normal;
	                                        font-weight: normal;
	                                        font-size: medium;
	                                        font-style: normal;
	                                        color: -internal-quirk-inherit;
	                                        text-align: start;
	                                        font-variant: normal;
                                    ";
                string td_style = @"
                                    padding: 10px;
	                                width: 3px;
	                                margin-bottom: -3px;
	                                font-weight: 600;
	                                margin: -6px;
                                    
                                    ";
                string td_style_En = @"
                                        padding: 10px;
	                                    width: 3px;
	                                    margin-bottom: -3px;
	                                    font-weight: 600;
	                                    margin: -6px;
	                                    direction: ltr;";
                string w_20 = @"width: 20%;";
                string w_30 = @"width: 30%;";
                string rtl = @"direction: rtl;";
                string text_center = @" text-align: center;";
                string tr_display = @" display: {displayOption}; ";

                // Add url
                string HtmlString_new = $@"                                           
                                          <div  style='{Email_style}'>
                                           <img style='{Email_image_style}' src='cid:TopHeader'>
                                           <div style='
		                                        width: 100%;
		                                        display: flex;
		                                        flex-direction: column;
		                                        justify-content: center;
		                                        margin: 0;
		                                        padding: 0;
                                                
	                                        '>		
		                                 <table dir='ltr' style='width: 100%' border='1'>
			                                <tr style='{tr_style}'>
				                                <td colspan='5' style='{td_style}'>
				                                    <h2 style=' text-align: center; background: #13817E; padding: 9px 0; font-weight: 900; margin-bottom: 0px; color: #fff;'> دعوه للإضافة على اللجنه </h2>
                                                </td>
			                                </tr>
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {JobTitleEn} </td>   
				                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'><span style='unicode-bidi: bidi-override;'>{mailTitle}</span></td>
				                                <td style='{td_style + ';' + w_20 + rtl}text-align: center;'> {JobTitleAr} </td>
                                                

                                            </tr > 
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {CommitteDateEn} </td>   
				                                <td colspan='3' style='{td_style}text-align: center;'><span>{commiteeDate}</span></td>
				                                <td style='{td_style + ';' + w_20 + rtl}text-align: center;'> {CommitteDateAr} </td>
                                                

                                            </tr > 
                                            <tr style='{tr_style}'>
				                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {taskCreatorEn} </td>
				                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'>{_sessionServices.EmployeeFullNameAr}</td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {taskCreatorAr} </td>   
                                            </tr > 
                                            <tr style='{tr_style}'>
				                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {taskDetailsLinkEn}  OR   {RejectMemberStateLinkEn} </td>
				                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'><pre>     <a href ='{ComfirmDetailsLink}'>Confirm<a/>       <a href ='{RejectDetailsLink}'>Reject<a/>     </pre></td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {taskDetailsLinkAr}   أو    {RejectMemberStateLinkAr} </td>   
                                            </tr > 
                                            </table>
                                                </div>                                                     
                                                    </div>";
                //text-align:center

                //< td style = '{td_style_En + '; ' + w_20};background:{EmailLblColor}' >{ lblRequiredActionEn} </ td > 
                //< td style = '{td_style + '; ' + w_20 + rtl};background:{EmailLblColor}' >{ lblRequiredAction} </ td >



                //str HtmlString = new StringBuilder();
                //HtmlString.Append("<div style='width:680px;Margin:0 auto;'><img src='cid:TopHeader'  style='width:100% !important;min-width:680px !important;text-align:right; float:right!important'/> ");
                //HtmlString.Append("<img src='cid:header'  style='width:100% !important;min-width:680px !important;text-align:right;float:right!important'/>");
                //HtmlString.Append("<table style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl; margin-bottom: 20px; padding - right: 6%'>");
                //HtmlString.Append("<tbody>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionNumber + "</span></th>  <td style='border-bottom: 1px solid #EEE;'>" + TransactionNumber + "</td>  </tr>");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblTransactionSubject + "</span></th><td style='border-bottom: 1px solid #EEE;'> " + trans_Subject + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionType + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + TransactionType + "</td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'> <span > " + lblTransactionDate + "</span></th><td style='border-bottom: 1px solid #EEE;'>" + CreateDate + "   </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblDelegationFromOrg + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + delegatedFrom + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblImportance_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ImportanceName + " </td></tr> ");
                //HtmlString.Append("<tr><th style='border-bottom: 1px solid #EEE;direction:rtl;    background-color: silver;'><span >  " + lblConfidentiality_level + "</span></th><td style='border-bottom: 1px solid #EEE;'>  " + ConfidentialityName + " </td></tr> ");
                //HtmlString.Append("</tbody></table >");
                //HtmlString.Append("<img src='cid:footer'  style='width:100%;text-align:right;line-height:20px;font-size:15px;direction:rtl;'/></div>");


                mailMessage = HtmlString_new;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AlternateView CreateAlternateView(string message, object p, string v)
        {
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(message, null, "text/html");

            string path_TopHeader = Path.Combine(Hosting.AngularRootPath, "assets//images//EmailImages//TopHeader.jpg"); //My TopHeader
                                                                                                                         //------------------------------------TopHeader Image
            LinkedResource imagelink_TopHeader = new LinkedResource(path_TopHeader, "image/png")
            {
                ContentId = "TopHeader",

                TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            };
            htmlView.LinkedResources.Add(imagelink_TopHeader);
            //--------------------------------------------------header Image
            //string pathheader = Path.Combine(Hosting.AngularRootPath, "assets//images//EmailImages//header.jpg"); //My Header


            //LinkedResource imagelink_header = new LinkedResource(pathheader, "image/png")
            //{
            //    ContentId = "header",

            //    TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            //};
            //htmlView.LinkedResources.Add(imagelink_header);

            ////--------------------------------------------------Footer Image
            //string path_footer = Path.Combine(Hosting.AngularRootPath, "assets//images//EmailImages//footer.jpg"); //My footer


            //LinkedResource imagelink_Footer = new LinkedResource(path_footer, "image/png")
            //{
            //    ContentId = "footer",

            //    TransferEncoding = System.Net.Mime.TransferEncoding.Base64
            //};
            //htmlView.LinkedResources.Add(imagelink_Footer);
            return htmlView;
        }
    }
}