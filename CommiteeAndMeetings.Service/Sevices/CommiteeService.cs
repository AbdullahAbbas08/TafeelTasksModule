using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.BLL.Migrations;
using CommiteeAndMeetings.BLL.Reuseable_Geneirc_Function;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Enums;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using DbContexts.MasarContext.ProjectionModels;
using DotLiquid;
using HelperServices;
using HelperServices.LinqHelpers;
using IHelperServices;
using IHelperServices.Models;
using iTextSharp.text;
using LinqHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Models;
using Models.ProjectionModels;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using static DotLiquid.Variable;
using static iTextSharp.text.pdf.AcroFields;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class CommiteeService : BusinessService<Commitee, CommiteeDTO>, ICommiteeService
    {
        IUnitOfWork _unitOfWork;
        private readonly ICommiteeUserPermissionService commiteeUserPermissionService;
        private readonly ICommiteeRolePermissionService commiteeRolePermissionService;
        private readonly IMapper mapper;
        IHelperServices.ISessionServices _sessionServices;
        ICommitteeNotificationService _committeeNotificationService;
        ICommiteeLocalizationService _commiteeLocalizationService;
        public readonly MasarContext _context;
        private readonly HelperFunction helperFunction;
        public CommiteeService(IUnitOfWork unitOfWork,
            ICommiteeUserPermissionService commiteeUserPermissionService,
            ICommiteeRolePermissionService commiteeRolePermissionService, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings, ICommitteeNotificationService committeeNotificationService, ICommiteeLocalizationService commiteeLocalizationService)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            this.commiteeUserPermissionService = commiteeUserPermissionService;
            this.commiteeRolePermissionService = commiteeRolePermissionService;
            this.mapper = mapper;
            _sessionServices = sessionServices;
            _committeeNotificationService = committeeNotificationService;
            _commiteeLocalizationService = commiteeLocalizationService;
            _context = new MasarContext();
            helperFunction = new HelperFunction();
        }

        public List<CommiteeDTO> CustomInsert(CommiteeDTO commiteeDTO)
        {
            //var _commitee = mapper.Map<Commitee>(commiteeDTO);
            //Commitee commitee = _unitOfWork.GetRepository<Commitee>().Insert(_commitee);

            //if (commitee != null)
            //{
            //    int RoleId = 1;
            //    List<CommiteeRolePermission> RolePermissions = commiteeRolePermissionService.GetAllPermission().Where(x => x.RoleId == RoleId).ToList();
            //    List<CommiteeUserPermissionDTO> commiteeUserPermissionDTO = new();
            //    foreach (var item in RolePermissions)
            //    {
            //        commiteeUserPermissionDTO.Add(new CommiteeUserPermissionDTO
            //        {
            //            RoleId = RoleId,
            //            PermissionId = item.PermissionId,
            //            UserId = int.Parse(commitee.CurrenHeadUnitId.ToString()),
            //            Enabled = item.Permission.Enabled,
            //            CommiteeId = commitee.CommiteeId,
            //            IsDelegated = false
            //        });
            //    }
            //    if (commiteeUserPermissionDTO.Count() > 0) commiteeUserPermissionService.Insert(commiteeUserPermissionDTO);
            //}
            //var commiteeDto = mapper.Map<CommiteeDTO>(commitee);
            return new List<CommiteeDTO>();
        }

        public int? GetCommitteRole(int CommitteId)
        {
            var x = _unitOfWork.GetRepository<CommiteeUsersRole>().GetAll().Where(x => x.UserId == _sessionServices.UserId && x.Enabled && x.CommiteeId == CommitteId).FirstOrDefault()?.CommiteeUsersRoleId;
            return x;
        }
        public override DataSourceResult<CommiteeDTO> GetAll<CommiteeDTO>(DataSourceRequest dataSourceRequest, bool WithTracking = true)
        {
            if (helperFunction.get_Permission(_sessionServices.UserRoleIdOrignal, "ForAllCommitte"))
            {
                IQueryable queryAll = this._UnitOfWork.GetRepository<Commitee>().GetAll(WithTracking).OrderByDescending(x => x.CreatedOn);

                return queryAll.ProjectTo<CommiteeDTO>(_Mapper.ConfigurationProvider).ToDataSourceResult(dataSourceRequest);
            }
            IQueryable query = this._UnitOfWork.GetRepository<Commitee>().GetAll(WithTracking).OrderByDescending(x => x.CreatedOn).Where(x => x.Members.Any(c => c.UserId == _sessionServices.UserId && (c.MemberState == MemberState.Active || c.MemberState == MemberState.Pending)));

            return query.ProjectTo<CommiteeDTO>(_Mapper.ConfigurationProvider).ToDataSourceResult(dataSourceRequest);
        }
        public override IEnumerable<CommiteeDTO> Update(IEnumerable<CommiteeDTO> Entities)
        {

            List<CommiteeDTO> commitees = new List<CommiteeDTO>();
            foreach (var item in Entities)
            {
                item.CommiteeId = _sessionServices.UserIdAndRoleIdAfterDecrypt(item.CommiteeIdEncrpt, true).Id;

                Commitee commitee = _unitOfWork.GetRepository<Commitee>().GetById(item.CommiteeId);
                if (item.CurrenHeadUnitId != commitee.CurrenHeadUnitId)
                {
                    var committeUser = _unitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == item.CommiteeId && x.UserId == commitee.CurrenHeadUnitId).FirstOrDefault();
                    if (committeUser != null)
                    {

                        var RoleId = _unitOfWork.GetRepository<CommiteeRole>().GetAll().FirstOrDefault(x => x.IsMangerRole).CommiteeRoleId;
                        var userRole = _unitOfWork.GetRepository<CommiteeUsersRole>().GetAll().Where(x => x.CommiteeId == item.CommiteeId && x.UserId == commitee.CurrenHeadUnitId && x.RoleId == RoleId).FirstOrDefault();
                        if (item.CurrenHeadUnitId == null)
                        {

                            _unitOfWork.GetRepository<CommiteeMember>().Delete(committeUser);
                            _unitOfWork.GetRepository<CommiteeUsersRole>().Delete(userRole);
                        }
                        else
                        {
                            committeUser.UserId = (int)item.CurrenHeadUnitId;
                            _unitOfWork.GetRepository<CommiteeMember>().Update(committeUser);
                            userRole.UserId = (int)item.CurrenHeadUnitId;
                            _unitOfWork.GetRepository<CommiteeUsersRole>().Update(userRole);
                        }
                    }

                }
                if (item.CommitteeSecretaryId != commitee.CommitteeSecretaryId)
                {
                    var committeUser = _unitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == item.CommiteeId && x.UserId == commitee.CommitteeSecretaryId).FirstOrDefault();
                    if (committeUser != null)
                    {
                        // get roleId From systemSetting
                        var RoleId = _unitOfWork.GetRepository<CommitteeMeetingSystemSetting>().GetAll().FirstOrDefault(x => x.SystemSettingCode == "CommitteeSecretaryRoleId").SystemSettingValue;
                        //var RoleId = _unitOfWork.GetRepository<CommiteeRole>().GetAll().FirstOrDefault(x => x.CommiteeRolesNameEn == "Committee Secretary").CommiteeRoleId;
                        var userRole = _unitOfWork.GetRepository<CommiteeUsersRole>().GetAll().Where(x => x.CommiteeId == item.CommiteeId && x.UserId == commitee.CommitteeSecretaryId && x.RoleId == int.Parse(RoleId)).FirstOrDefault();
                        if (item.CommitteeSecretaryId == null)
                        {

                            _unitOfWork.GetRepository<CommiteeMember>().Delete(committeUser);
                            _unitOfWork.GetRepository<CommiteeUsersRole>().Delete(userRole);
                        }
                        else
                        {
                            committeUser.UserId = (int)item.CommitteeSecretaryId;
                            _unitOfWork.GetRepository<CommiteeMember>().Update(committeUser);
                            userRole.UserId = (int)item.CommitteeSecretaryId;
                            _unitOfWork.GetRepository<CommiteeUsersRole>().Update(userRole);
                        }
                    }
                }
                commitee.CategoryId = item.CategoryId;
                commitee.CommiteeTypeId = item.CommiteeTypeId;
                commitee.CurrenHeadUnitId = item.CurrenHeadUnitId;
                commitee.CommitteeSecretaryId = item.CommitteeSecretaryId;
                commitee.CurrentStatusDate = item.CurrentStatusDate;
                commitee.CurrentStatusId = item.CurrentStatusId;
                commitee.CurrentStatusReasonId = item.CurrentStatusReasonId;
                commitee.DepartmentLinkId = item.DepartmentLinkId;
                commitee.Description = item.Description;
                commitee.EnableDecisions = item.EnableDecisions;
                commitee.EnableTransactions = item.EnableTransactions;
                commitee.IsSecrete = item.IsSecrete;
                commitee.Name = item.Name;
                commitee.ParentCommiteeId = item.ParentCommiteeId;
                commitee.Title = item.Title;
                commitee.UpdatedBy = _sessionServices.UserId;
                commitee.UpdatedOn = DateTime.Now;
                _unitOfWork.GetRepository<Commitee>().Update(commitee);
                commitees.Add(_Mapper.Map(commitee, typeof(Commitee), typeof(CommiteeDTO)) as CommiteeDTO);
            }
            return commitees;
        }
        public override IEnumerable<CommiteeDTO> Insert(IEnumerable<CommiteeDTO> entities)
        {
            var committees = base.Insert(entities);
            var committee = _unitOfWork.GetRepository<Commitee>().GetAll().OrderByDescending(x=>x.CommiteeId).FirstOrDefault();
            //foreach (var committee in committees)
            //{
                CommiteeMember member = new CommiteeMember
                {
                    CommiteeId = committee.CommiteeId,
                    UserId = (int)committee.CurrenHeadUnitId,
                    Active = true,
                    MemberState = MemberState.Active,
                    IsReserveMember = false

                };
                _unitOfWork.GetRepository<CommiteeMember>().Insert(member);

                CommiteeUsersRole commiteeMember = new CommiteeUsersRole
                {
                    Enabled = true,
                    CommiteeId = committee.CommiteeId,
                    RoleId = _unitOfWork.GetRepository<CommiteeRole>().GetAll().FirstOrDefault(x => x.IsMangerRole).CommiteeRoleId,
                    UserId = (int)committee.CurrenHeadUnitId,
                    CommiteeMemberId = member.CommiteeMemberId
                };
                _unitOfWork.GetRepository<CommiteeUsersRole>().Insert(commiteeMember);
                var loc = _unitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewCommitteeNotificationText");
                if (committee.CurrenHeadUnitId != _sessionServices.UserId)
                {
                    CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                    {
                        IsRead = false,
                        UserId = (int)committee.CurrenHeadUnitId,
                        TextAR = loc.CommiteeLocalizationAr + " " + committee.Name,
                        TextEn = loc.CommiteeLocalizationEn + " " + committee.Name,
                        CommiteeId = committee.CommiteeId
                    };
                    List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                    _committeeNotificationService.Insert(committeeNotifications);
                }

                // for committe Secretary
                CommiteeMember memberSecretary = new CommiteeMember
                {
                    CommiteeId = committee.CommiteeId,
                    UserId = (int)committee.CommitteeSecretaryId,
                    Active = true,
                    MemberState = MemberState.Active,
                    IsReserveMember = true
                };
                _unitOfWork.GetRepository<CommiteeMember>().Insert(memberSecretary);
                CommiteeUsersRole commiteeMembermemberSecretary = new CommiteeUsersRole
                {
                    Enabled = true,
                    CommiteeId = committee.CommiteeId,
                    RoleId = int.Parse(_unitOfWork.GetRepository<CommitteeMeetingSystemSetting>().GetAll().FirstOrDefault(x => x.SystemSettingCode == "CommitteeSecretaryRoleId").SystemSettingValue),
                    UserId = (int)committee.CommitteeSecretaryId,
                    CommiteeMemberId = memberSecretary.CommiteeMemberId
                };
                _unitOfWork.GetRepository<CommiteeUsersRole>().Insert(commiteeMembermemberSecretary);

                var locForSecretary = _unitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewCommitteeNotificationText");
                if (committee.CommitteeSecretaryId != _sessionServices.UserId)
                {
                    CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                    {
                        IsRead = false,
                        UserId = (int)committee.CommitteeSecretaryId,
                        TextAR = locForSecretary.CommiteeLocalizationAr + " " + committee.Name,
                        TextEn = locForSecretary.CommiteeLocalizationEn + " " + committee.Name,
                        CommiteeId = committee.CommiteeId
                    };
                    List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                    _committeeNotificationService.Insert(committeeNotifications);
                }
            //}


            return committees;
        }
        public DataSourceResult<LookUpDTO> GetCommitteeLookup(DataSourceRequest dataSourceRequest, bool WithTracking = true, int? ParentId = null)
        {
            if (ParentId == null)
            {
                IQueryable query = this._UnitOfWork.GetRepository<Commitee>().GetAll(WithTracking).Where(x => x.Members.Any(c => c.UserId == _sessionServices.UserId));
                return query.Cast<Commitee>().Select(x => new LookUpDTO
                {
                    Id = x.CommiteeId,
                    Name = x.Name
                }).ToDataSourceResult(dataSourceRequest);
            }

            else
            {
                var committeChileds = GetChild((int)ParentId);
                IQueryable query = this._UnitOfWork.GetRepository<Commitee>().GetAll(WithTracking).Where(x => x.Members.Any(c => c.UserId == _sessionServices.UserId)).Where(q => !committeChileds.Select(z => z.CommiteeId).Contains(q.CommiteeId));
                return query.Cast<Commitee>().Select(x => new LookUpDTO
                {
                    Id = x.CommiteeId,
                    Name = x.Name
                }).ToDataSourceResult(dataSourceRequest);
            }
        }
        public List<Commitee> GetChild(int id)
        {
            //DBEntities db = new DBEntities();
            var commitees = this._UnitOfWork.GetRepository<Commitee>().GetAll().Where(x => x.ParentCommiteeId == id || x.CommiteeId == id).ToList();

            var child = commitees.AsEnumerable().Union(
                                       this._UnitOfWork.GetRepository<Commitee>().GetAll().AsEnumerable().Where(x => x.ParentCommiteeId == id).SelectMany(y => GetChild(y.CommiteeId))).ToList();
            return child;
        }
        public DataSourceResult<LookUpDTO> GetOrgnaztionLookup(DataSourceRequest dataSourceRequest, bool WithTracking = true)
        {
            IQueryable query = this._UnitOfWork.GetRepository<Organization>().GetAll(WithTracking).Where(x => !x.IsOuterOrganization);
            return query.Cast<Organization>().Select(x => new LookUpDTO
            {
                Id = x.OrganizationId,
                Name = _sessionServices.CultureIsArabic ? x.OrganizationNameAr : x.OrganizationNameEn
            }).ToDataSourceResult(dataSourceRequest);

        }
        public OrganizationSessionDTO GetOrgnaztionFromSession()
        {
            OrganizationSessionDTO organizationSessionDTO = new OrganizationSessionDTO()
            {
                Id = (int)_sessionServices.OrganizationId,
                OrganizationAr = _sessionServices.OrganizationNameAr,
                OrganizationEn = _sessionServices.OrganizationNameEn

            };
            return organizationSessionDTO;
        }
        public DataSourceResult<LookUpDTO> GetCommitteeHeadUnitLookup(DataSourceRequest dataSourceRequest, int? orgId, bool WithTracking = true)
        {
            // check Permission of AssignTaskToAllEmployees or AssignTaskToAllOrgChildEmployees 
            if (orgId == null)
            {

                orgId = (int)_sessionServices.OrganizationId;
            }

            //var x = this._UnitOfWork.GetRepository<Permission>().GetAll().Where(x => x.PermissionCode == "AssignTaskToAllOrgChildEmployees").FirstOrDefault();
            bool orgAndChild = helperFunction.get_Permission(_sessionServices.UserRoleIdOrignal, "AssignTaskToAllOrgChildEmployees");
            bool orgAll = helperFunction.get_Permission(_sessionServices.UserRoleIdOrignal, "AssignTaskToAllEmployees");
            if (orgAll)
            {
                IQueryable query = this._UnitOfWork.GetRepository<DAL.Domains.User>().GetAll(WithTracking)
                   .Where(x => x.Enabled && !x.DeletedOn.HasValue && (x.EnabledUntil == null || x.EnabledUntil > DateTime.Now));
                return query.Cast<DAL.Domains.User>().Select(x => new LookUpDTO
                {
                    Id = x.UserId,
                    Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
                }).ToDataSourceResult(dataSourceRequest);


            }
            else if (orgAndChild)
            {



                var returnFromStoredEnumerable = _context.vm_EmpInOrgaHierarchies.FromSqlInterpolated($"exec [dbo].[sp_GetEmployeesByOrganizationHierarchyToReport] {orgId}").AsEnumerable().ToList();
                // var res = returnFromStoredEnumerable.AsEnumerable().ToDataSourceResult(dataSourceRequest, true).Data.ToList();

                //var x = returnFromStoredEnumerable.AsQueryable();
                return returnFromStoredEnumerable.AsQueryable().Select(x => new LookUpDTO
                {
                    Id = x.Id,
                    Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
                }).ToDataSourceResult(dataSourceRequest);

            }
            // var permission = this._context.Database.($"exec sp_getPermissions {AuthUser.UserId},{DefaultUserRole.OrganizationId},{DefaultUserRole.RoleId}").ToList();
            else
            {

                var returnFromStored = _context.vm_EmpInOrgaHierarchies.FromSqlInterpolated($"exec [dbo].[sp_GetEmployeesByOrganizationToReport] {orgId}").AsEnumerable().ToList();

                return returnFromStored.AsQueryable().Select(x => new LookUpDTO
                {
                    Id = x.Id,
                    Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
                }).ToDataSourceResult(dataSourceRequest);
            }

        }

        public DataSourceResult<LookUpDTO> GetCommitteeHeadUnitLookupUserAndOrganization(DataSourceRequest dataSourceRequest, int? orgId, bool WithTracking = true)
        {
            // check Permission of AssignTaskToAllEmployees or AssignTaskToAllOrgChildEmployees 
            if (orgId == null)
            {

                orgId = (int)_sessionServices.OrganizationId;
            }

            //var x = this._UnitOfWork.GetRepository<Permission>().GetAll().Where(x => x.PermissionCode == "AssignTaskToAllOrgChildEmployees").FirstOrDefault();
            bool orgAndChild = helperFunction.get_Permission(_sessionServices.UserRoleIdOrignal, "AssignTaskToAllOrgChildEmployees");
            bool orgAll = helperFunction.get_Permission(_sessionServices.UserRoleIdOrignal, "AssignTaskToAllEmployees");
            if (orgAll)
            {
                IQueryable query = this._UnitOfWork.GetRepository<DAL.Domains.User>().GetAll(WithTracking)
                   .Where(x => x.Enabled && !x.DeletedOn.HasValue && (x.EnabledUntil == null || x.EnabledUntil > DateTime.Now) && (orgId == null || x.UserRoleUsers.Any(x => x.OrganizationId == orgId)));
                return query.Cast<DAL.Domains.User>().Select(x => new LookUpDTO
                {
                    Id = x.UserId,
                    Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
                }).ToDataSourceResult(dataSourceRequest);


            }
            else if (orgAndChild)
            {



                var returnFromStoredEnumerable = _context.vm_EmpInOrgaHierarchies.FromSqlInterpolated($"exec [dbo].[sp_GetEmployeesByOrganizationHierarchyToReport] {orgId}").AsEnumerable().ToList();
                // var res = returnFromStoredEnumerable.AsEnumerable().ToDataSourceResult(dataSourceRequest, true).Data.ToList();

                //var x = returnFromStoredEnumerable.AsQueryable();
                return returnFromStoredEnumerable.AsQueryable().Select(x => new LookUpDTO
                {
                    Id = x.Id,
                    Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
                }).ToDataSourceResult(dataSourceRequest);

            }
            // var permission = this._context.Database.($"exec sp_getPermissions {AuthUser.UserId},{DefaultUserRole.OrganizationId},{DefaultUserRole.RoleId}").ToList();
            else
            {

                var returnFromStored = _context.vm_EmpInOrgaHierarchies.FromSqlInterpolated($"exec [dbo].[sp_GetEmployeesByOrganizationToReport] {orgId}").AsEnumerable().ToList();

                return returnFromStored.AsQueryable().Select(x => new LookUpDTO
                {
                    Id = x.Id,
                    Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
                }).ToDataSourceResult(dataSourceRequest);
            }

        }


        public List<LookUpDTO> GetMeetingHeadUnitLookupUserAndOrganization(List<int> orgsList, bool WithTracking = true)
        {
            // check Permission of AssignTaskToAllEmployees or AssignTaskToAllOrgChildEmployees 
            //bool orgAndChild = helperFunction.get_Permission(_sessionServices.UserRoleIdOrignal, "AssignTaskToAllOrgChildEmployees");
            //bool orgAll = helperFunction.get_Permission(_sessionServices.UserRoleIdOrignal, "AssignTaskToAllEmployees");
            List<LookUpDTO> lookUpDTOs = new List<LookUpDTO>();
            //if (orgAll)
            //{
            foreach (var orgId in orgsList)
            {
                var query = (from u in _context.Users
                             join J in _context.JobTitles
                             on u.JobTitleId equals J.JobTitleId
                             join uR in _context.UserRoles
                            on u.UserId equals uR.UserId
                             where uR.OrganizationId == orgId && (uR.EnabledUntil == null || uR.EnabledUntil > DateTime.Now) && u.IsEmployee == true
                             select u).Distinct();
                //IQueryable query = this._UnitOfWork.GetRepository<DAL.Domains.User>().GetAll(WithTracking)
                //   .Where(x => x.Enabled && !x.DeletedOn.HasValue && (x.EnabledUntil == null || x.EnabledUntil > DateTime.Now) && x.IsEmployee == true && (/*orgId == null ||*/ x.UserRoleUsers.Any(x => x.OrganizationId == orgId)));
                var lookUps = query.Cast<DAL.Domains.User>().Select(x => new LookUpDTO
                {
                    Id = x.UserId,
                    Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
                }).ToList();
                lookUpDTOs.AddRange(lookUps);
            }
            return lookUpDTOs;
            //}
            //else if (orgAndChild)
            //{

            //    foreach (var orgId in orgsList)
            //    {

            //        var returnFromStoredEnumerable = _context.vm_EmpInOrgaHierarchies.FromSqlInterpolated($"exec [dbo].[sp_GetEmployeesByOrganizationHierarchyToReport] {orgId}").AsEnumerable().ToList();
            //        // var res = returnFromStoredEnumerable.AsEnumerable().ToDataSourceResult(dataSourceRequest, true).Data.ToList();

            //        //var x = returnFromStoredEnumerable.AsQueryable();
            //        var lookUps = returnFromStoredEnumerable.AsQueryable().Select(x => new LookUpDTO
            //        {
            //            Id = x.Id,
            //            Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
            //        }).ToList();
            //        lookUpDTOs.AddRange(lookUps);
            //    }
            //    return lookUpDTOs;


            //}
            //// var permission = this._context.Database.($"exec sp_getPermissions {AuthUser.UserId},{DefaultUserRole.OrganizationId},{DefaultUserRole.RoleId}").ToList();
            //else
            //{
            //    foreach (var orgId in orgsList)
            //    {
            //        var returnFromStored = _context.vm_EmpInOrgaHierarchies.FromSqlInterpolated($"exec [dbo].[sp_GetEmployeesByOrganizationToReport] {orgId}").AsEnumerable().ToList();

            //        var lookUps = returnFromStored.AsQueryable().Select(x => new LookUpDTO
            //        {
            //            Id = x.Id,
            //            Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
            //        }).ToList();
            //        lookUpDTOs.AddRange(lookUps);
            //    }
            //    return lookUpDTOs;
            //}


        }



        public DataSourceResult<LookUpDTO> GetOrganizationLookup(DataSourceRequest dataSourceRequest, bool fromAttendee, bool WithTracking = true)
        {

            int orgId = (int)_sessionServices.OrganizationId;
            if (fromAttendee)
            {
                var returnFromStoredEnumerable = _context.Vm_OrgInOrgaHierarchies.FromSqlInterpolated($"exec [dbo].[Sp_GetOrganizationAllChildrenAndItSelf] {orgId}").AsEnumerable().ToList();
                // var res = returnFromStoredEnumerable.AsEnumerable().ToDataSourceResult(dataSourceRequest, true).Data.ToList();

                //var x = returnFromStoredEnumerable.AsQueryable();
                return returnFromStoredEnumerable.AsQueryable().Select(x => new LookUpDTO
                {
                    Id = x.Id,
                    Name = _sessionServices.CultureIsArabic ? x.OrganizationAr : x.OrganizationEn
                }).ToDataSourceResult(dataSourceRequest);
            }

            //var x = this._UnitOfWork.GetRepository<Permission>().GetAll().Where(x => x.PermissionCode == "AssignTaskToAllOrgChildEmployees").FirstOrDefault();
            bool orgAndChild = helperFunction.get_Permission(_sessionServices.UserRoleIdOrignal, "AssignTaskToAllOrgChildEmployees");
            bool orgAll = helperFunction.get_Permission(_sessionServices.UserRoleIdOrignal, "AssignTaskToAllEmployees");

            if (orgAll)
            {
                IQueryable query = this._UnitOfWork.GetRepository<DAL.Domains.Organization>().GetAll(WithTracking).Where(x => !x.IsOuterOrganization);
                // .Where(x => x.Enabled && !x.DeletedOn.HasValue && (x.EnabledUntil == null || x.EnabledUntil > DateTime.Now));
                return query.Cast<Organization>().Select(x => new LookUpDTO
                {
                    Id = x.OrganizationId,
                    Name = _sessionServices.CultureIsArabic ? x.OrganizationNameAr : x.OrganizationNameEn
                }).ToDataSourceResult(dataSourceRequest);


            }
            else if (orgAndChild)
            {



                var returnFromStoredEnumerable = _context.Vm_OrgInOrgaHierarchies.FromSqlInterpolated($"exec [dbo].[Sp_GetOrganizationAllChildrenAndItSelf] {orgId}").AsEnumerable().ToList();
                // var res = returnFromStoredEnumerable.AsEnumerable().ToDataSourceResult(dataSourceRequest, true).Data.ToList();

                //var x = returnFromStoredEnumerable.AsQueryable();
                return returnFromStoredEnumerable.AsQueryable().Select(x => new LookUpDTO
                {
                    Id = x.Id,
                    Name = _sessionServices.CultureIsArabic ? x.OrganizationAr : x.OrganizationEn
                }).ToDataSourceResult(dataSourceRequest);

            }
            // var permission = this._context.Database.($"exec sp_getPermissions {AuthUser.UserId},{DefaultUserRole.OrganizationId},{DefaultUserRole.RoleId}").ToList();
            else
            {

                IQueryable query = this._UnitOfWork.GetRepository<DAL.Domains.Organization>().GetAll()
                 .Where(x => x.OrganizationId == orgId);
                return query.Cast<DAL.Domains.Organization>().Where(x => !x.IsOuterOrganization).Select(x => new LookUpDTO
                {
                    Id = x.OrganizationId,
                    Name = _sessionServices.CultureIsArabic ? x.OrganizationNameAr : x.OrganizationNameEn
                }).ToDataSourceResult(dataSourceRequest);


            }

        }
        public CommiteeDTO GetCommitteeDetailsWithValidityPeriod(int id, ISession _session)
        {

            var Mapping = _Mapper.ConfigurationProvider.FindTypeMapFor(typeof(Commitee), typeof(CommiteeDTO));
            if (Mapping == null)
            {
                Mapping = _Mapper.ConfigurationProvider.ResolveTypeMap(typeof(Commitee), typeof(CommiteeDTO));
            }
            if (helperFunction.get_Permission(_sessionServices.UserRoleIdOrignal, "ForAllCommitte"))
            {
                Commitee commiteeFromAllPermission = this._UnitOfWork.GetRepository<Commitee>().GetAll()
               //.Where(x => x.Members.Any(x => x.UserId == _sessionServices.UserId && x.Active) || x.CurrenHeadUnitId == _sessionServices.UserId)
               //.Include(c => c.ValidityPeriod)
               .FirstOrDefault(c => c.CommiteeId == id);
                //commitee.CommiteeRoles = commitee.CommiteeRoles.Where(x => x.Enabled).ToList();
                var comitteDtoFromAllPermission = _Mapper.Map(commiteeFromAllPermission, typeof(Commitee), typeof(CommiteeDTO)) as CommiteeDTO;
                // comitteDto.CommiteeRoles= comitteDto.CommiteeRoles.Where(x => x.Enabled&&x.UserId==_sessionServices.UserId).ToList();
                if (comitteDtoFromAllPermission == null) return new CommiteeDTO();
                _session.SetString("UserRoleId", GetCommitteRole(id).ToString());
                return comitteDtoFromAllPermission;
            }
            Commitee commitee = this._UnitOfWork.GetRepository<Commitee>().GetAll()
                .Where(x => x.Members.Any(x => x.UserId == _sessionServices.UserId && x.Active) || x.CurrenHeadUnitId == _sessionServices.UserId)
                //.Include(c => c.ValidityPeriod)
                .FirstOrDefault(c => c.CommiteeId == id);
            //commitee.CommiteeRoles = commitee.CommiteeRoles.Where(x => x.Enabled).ToList();
            var comitteDto = _Mapper.Map(commitee, typeof(Commitee), typeof(CommiteeDTO)) as CommiteeDTO;
            // comitteDto.CommiteeRoles= comitteDto.CommiteeRoles.Where(x => x.Enabled&&x.UserId==_sessionServices.UserId).ToList();
            if (comitteDto == null) return new CommiteeDTO();
            _session.SetString("UserRoleId", GetCommitteRole(id).ToString());
            return comitteDto;
        }

        public CommiteeDTO GetCommitteeNames(int id)
        {

            var Mapping = _Mapper.ConfigurationProvider.FindTypeMapFor(typeof(Commitee), typeof(CommiteeDTO));
            if (Mapping == null)
            {
                Mapping = _Mapper.ConfigurationProvider.ResolveTypeMap(typeof(Commitee), typeof(CommiteeDTO));
            }


            var commitee = this._UnitOfWork.GetRepository<Commitee>().GetAll()
            .Where(x => x.Members.Any(x => x.UserId == _sessionServices.UserId && x.Active) || x.CurrenHeadUnitId == _sessionServices.UserId).Select(x => new CommiteeDTO { CommiteeId = x.CommiteeId, Name = x.Name, Title = x.Title, Description = x.Description })
            .FirstOrDefault(c => c.CommiteeId == id);
            //commitee.CommiteeRoles = commitee.CommiteeRoles.Where(x => x.Enabled).ToList();
            //var comitteDto = _Mapper.Map(commitee, typeof(Commitee), typeof(CommiteeDTO)) as CommiteeDTO;
            // comitteDto.CommiteeRoles= comitteDto.CommiteeRoles.Where(x => x.Enabled&&x.UserId==_sessionServices.UserId).ToList();
            if (commitee == null) return new CommiteeDTO();
            return commitee;
        }

        public CommitteWallDTO GetCommitteWall(int take, int skip, int committeId, DateTime? dateFrom, DateTime? dateTo, string SearchText, bool asc)
        {
            var wall = new CommitteWallDTO();
            if (asc)
            {

                var committeitem = _unitOfWork.GetRepository<Commitee>().GetAll().Where(x => x.CommiteeId == committeId).FirstOrDefault();
                var committeMember = _unitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == committeId).ToList();
                var headOfUnitId = committeitem.CurrenHeadUnitId;
                var AttchmentAll = new AllCommiteeAttachmentDTO();
                if (headOfUnitId != _sessionServices.UserId && committeMember.All(x => x.UserId != _sessionServices.UserId))
                {
                    wall.Tasks = _unitOfWork.GetRepository<CommiteeTask>().GetAll().OrderBy(x => x.CreatedOn)
             .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
             // .Where(x => x.IsShared || x.MainAssinedUserId == _sessionServices.UserId || x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
             .Where(z => SearchText == "" || SearchText == null || z.Title.Contains(SearchText)).Take(take).Skip(skip)
             .Select(x => new CommiteeTaskDTO
             {
                 AssistantUsers = x.AssistantUsers.Select(y => new UserTaskDTO
                 {
                     CommiteeTaskId = (int)y.CommiteeTaskId,
                     UserId = y.UserId,
                     UserTaskId = y.UserTaskId,
                     User = new UserDetailsDTO
                     {
                         UserId = y.UserId,
                         UserName = y.User.Username,
                         FullNameAr = y.User.FullNameAr,
                         FullNameEn = y.User.FullNameEn,
                         FullNameFn = y.User.FullNameFn,
                         ProfileImage = y.User.ProfileImage
                     },
                 }).ToList(),
                 TaskAttachments = _Mapper.Map<List<CommitteeTaskAttachmentDTO>>(x.TaskAttachments),
                 //MultiMission = _Mapper.Map<List<CommiteetaskMultiMissionDTO>>(x.MultiMission),
                 MultiMission = x.MultiMission.Select(y => new CommiteetaskMultiMissionDTO
                 {
                     CommiteeTaskMultiMissionId = y.CommiteeTaskMultiMissionId,
                     Name = y.Name,
                     state = y.state,
                     EndDateMultiMission = y.EndDateMultiMission,
                     CommiteeTaskMultiMissionUserDTOs = y.CommiteeTaskMultiMissionUsers.Select(a => new CommiteeTaskMultiMissionUserDTO
                     {
                         CommiteeTaskMultiMissionId = a.CommiteeTaskMultiMissionId,
                         CommiteeTaskMultiMissionUserId = a.CommiteeTaskMultiMissionUserId,

                         UserDetailsDto = new UserDetailsDTO()
                         {
                             UserId = a.UserId,
                             UserName = a.User.Username,
                             FullNameAr = a.User.FullNameAr,
                             FullNameEn = a.User.FullNameEn,
                             ProfileImage = a.User.ProfileImage
                         },
                         UserId = a.UserId,
                         FullNameAr = a.User.FullNameAr,
                         FullNameEn = a.User.FullNameEn,
                     }).ToList(),
                 }).ToList(),
                 CommiteeId = x.CommiteeId,
                 CommiteeTaskId = x.CommiteeTaskId,
                 Completed = x.Completed,
                 CreatedBy = x.CreatedBy,
                 CreatedByRole = x.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                 {
                     CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                     Role = new CommiteeDetailsRoleDTO
                     {
                         CommiteeRoleId = x.CreatedByRole.Role.CommiteeRoleId,
                         CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                         CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                     }
                 },
                 CreatedByRoleId = x.CreatedByRoleId,
                 CreatedOn = x.CreatedOn,
                 CreatedByUser = x.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                 {
                     UserId = x.CreatedByUser.UserId,
                     UserName = x.CreatedByUser.Username,
                     FullNameAr = x.CreatedByUser.FullNameAr,
                     FullNameEn = x.CreatedByUser.FullNameEn,
                     FullNameFn = x.CreatedByUser.FullNameFn,
                     ProfileImage = x.CreatedByUser.ProfileImage
                 },
                 EndDate = x.EndDate,
                 StartDate = x.StartDate,
                 IsMain = x.IsMain,
                 IsShared = x.IsShared,
                 MainAssinedUser = new UserDetailsDTO
                 {
                     UserId = x.MainAssinedUser.UserId,
                     UserName = x.MainAssinedUser.Username,
                     FullNameAr = x.MainAssinedUser.FullNameAr,
                     FullNameEn = x.MainAssinedUser.FullNameEn,
                     FullNameFn = x.MainAssinedUser.FullNameFn,
                     ProfileImage = x.MainAssinedUser.ProfileImage
                 },
                 MainAssinedUserId = x.MainAssinedUserId,
                 Title = x.Title,
                 TaskDetails = x.TaskDetails,
                 TaskComments = x.TaskComments.Select(y => new TaskCommentDTO
                 {
                     CommentId = y.CommentId,
                     Comment = new CommentDTO
                     {
                         CommentId = (int)y.CommentId,
                         Text = y.Comment.Text,
                         SavedAttachments = y.Comment.SavedAttachments.Select(a => new SavedAttachmentDTO
                         {
                             AttachmentName = a.AttachmentName,
                             AttachmentTypeId = a.AttachmentTypeId,
                             CreatedBy = a.CreatedBy,
                             CreatedByRole = a.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                             {
                                 CommiteeUsersRoleId = a.CreatedByRole.CommiteeUsersRoleId,
                                 Role = new CommiteeDetailsRoleDTO
                                 {
                                     CommiteeRoleId = a.CreatedByRole.CommiteeUsersRoleId,
                                     CommiteeRolesNameAr = a.CreatedByRole.Role.CommiteeRolesNameAr,
                                     CommiteeRolesNameEn = a.CreatedByRole.Role.CommiteeRolesNameEn
                                 }
                             },
                             CreatedByRoleId = a.CreatedByRoleId,
                             CreatedByUser = a.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                             {
                                 UserId = a.CreatedByUser.UserId,
                                 UserName = a.CreatedByUser.Username,
                                 FullNameAr = a.CreatedByUser.FullNameAr,
                                 FullNameEn = a.CreatedByUser.FullNameEn,
                                 FullNameFn = a.CreatedByUser.FullNameFn,
                                 ProfileImage = a.CreatedByUser.ProfileImage
                             },
                             Height = a.Height,
                             IsDisabled = a.IsDisabled,
                             LFEntryId = a.LFEntryId,
                             MimeType = a.MimeType,
                             Notes = a.Notes,
                             PagesCount = a.PagesCount,
                             SavedAttachmentId = a.SavedAttachmentId,
                             Size = a.Size,
                             Width = a.Width

                         }).ToList(),
                         CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                         {
                             UserId = y.CreatedByUser.UserId,
                             UserName = y.CreatedByUser.Username,
                             FullNameAr = y.CreatedByUser.FullNameAr,
                             FullNameEn = y.CreatedByUser.FullNameEn,
                             FullNameFn = y.CreatedByUser.FullNameFn,
                             ProfileImage = y.CreatedByUser.ProfileImage
                         },
                         CreatedBy = y.CreatedBy
                     },
                     CreatedBy = y.CreatedBy,
                     CreatedByRoleId = y.CreatedByRoleId,
                     CreatedOn = y.CreatedOn,
                     TaskCommentId = y.TaskCommentId,
                     TaskId = y.TaskId,
                     CreatedByRole = y.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                     {
                         CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                         Role = new CommiteeDetailsRoleDTO
                         {
                             CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                             CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                             CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                         }
                     },
                     CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                     {
                         UserId = y.CreatedByUser.UserId,
                         UserName = y.CreatedByUser.Username,
                         FullNameAr = y.CreatedByUser.FullNameAr,
                         FullNameEn = y.CreatedByUser.FullNameEn,
                         FullNameFn = y.CreatedByUser.FullNameFn,
                         ProfileImage = y.CreatedByUser.ProfileImage
                     },
                     UpdatedBy = y.UpdatedBy,
                     UpdatedOn = y.UpdatedOn
                 }).ToList()
             }).ToDataSource(take, skip, false);

                    wall.Attachments = _unitOfWork.GetRepository<CommiteeSavedAttachment>().GetAll().OrderBy(x => x.CreatedOn)
                            .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
                            //.Where(x => x.AllUsers || x.AttachmentUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
                            .Where(z => SearchText == "" || SearchText == null || z.Description.Contains(SearchText))
                            .Select(x => new CommiteeAttachmentDTO
                            {
                                AllUsers = x.AllUsers,
                                AttachmentComments = x.AttachmentComments.Select(y => new AttachmentCommentDTO
                                {
                                    AttachmentCommentId = y.AttachmentCommentId,
                                    AttachmentId = y.AttachmentId,
                                    Comment = new CommentDTO
                                    {
                                        CommentId = y.CommentId,
                                        Text = y.Comment.Text,
                                        CreatedByUser = y.Comment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                        {
                                            UserId = y.CreatedByUser.UserId,
                                            UserName = y.CreatedByUser.Username,
                                            FullNameAr = y.CreatedByUser.FullNameAr,
                                            FullNameEn = y.CreatedByUser.FullNameEn,
                                            FullNameFn = y.CreatedByUser.FullNameFn,
                                            ProfileImage = y.CreatedByUser.ProfileImage
                                        },
                                    },
                                    CommentId = y.CommentId,
                                    CreatedBy = y.CreatedBy,
                                    CreatedByRoleId = y.CreatedByRoleId,
                                    CreatedByRole = y.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                    {
                                        CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                        Role = new CommiteeDetailsRoleDTO
                                        {
                                            CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                            CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                            CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                        }
                                    },
                                    CreatedByUser = new UserDetailsDTO
                                    {
                                        UserId = y.CreatedByUser.UserId,
                                        UserName = y.CreatedByUser.Username,
                                        FullNameAr = y.CreatedByUser.FullNameAr,
                                        FullNameEn = y.CreatedByUser.FullNameEn,
                                        FullNameFn = y.CreatedByUser.FullNameFn,
                                        ProfileImage = y.CreatedByUser.ProfileImage
                                    },
                                    CreatedOn = y.CreatedOn,

                                }).ToList(),
                                CommiteeAttachmentId = x.CommiteeAttachmentId,
                                CommiteeId = x.CommiteeId,
                                CreatedBy = x.CreatedBy,
                                CreatedOn = x.CreatedOn,
                                Description = x.Description,
                                Attachments = x.Attachments.Select(y => new SavedAttachmentDTO
                                {
                                    AttachmentName = y.AttachmentName,
                                    AttachmentTypeId = y.AttachmentTypeId,
                                    CreatedBy = y.CreatedBy,
                                    CreatedByRole = y.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                    {
                                        CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                        Role = new CommiteeDetailsRoleDTO
                                        {
                                            CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                            CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                            CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                        }
                                    },
                                    CreatedByRoleId = y.CreatedByRoleId,
                                    CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                                    {
                                        UserId = y.CreatedByUser.UserId,
                                        UserName = y.CreatedByUser.Username,
                                        FullNameAr = y.CreatedByUser.FullNameAr,
                                        FullNameEn = y.CreatedByUser.FullNameEn,
                                        FullNameFn = y.CreatedByUser.FullNameFn,
                                        ProfileImage = y.CreatedByUser.ProfileImage
                                    },
                                    Height = y.Height,
                                    IsDisabled = y.IsDisabled,
                                    LFEntryId = y.LFEntryId,
                                    MimeType = y.MimeType,
                                    Notes = y.Notes,
                                    PagesCount = y.PagesCount,
                                    SavedAttachmentId = y.SavedAttachmentId,
                                    Size = y.Size,
                                    Width = y.Width

                                }).ToList(),
                                AttachmentUsers = x.AttachmentUsers.Select(y => new AttachmentUserDTO
                                {
                                    AttachmentUserId = y.AttachmentUserId,
                                    CommiteeAttachmentId = y.CommiteeAttachmentId,
                                    User = new UserDetailsDTO
                                    {
                                        UserId = y.User.UserId,
                                        UserName = y.User.Username,
                                        FullNameAr = y.User.FullNameAr,
                                        FullNameEn = y.User.FullNameEn,
                                        FullNameFn = y.User.FullNameFn,
                                        ProfileImage = y.User.ProfileImage
                                    },
                                    UserId = y.UserId
                                }).ToList(),
                                CreatedByRole = x.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                {
                                    CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                    Role = new CommiteeDetailsRoleDTO
                                    {
                                        CommiteeRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                        CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                                        CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                                    }
                                },
                                CreatedByRoleId = x.CreatedByRoleId,
                                CreatedByUser = x.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                                {
                                    UserId = x.CreatedByUser.UserId,
                                    UserName = x.CreatedByUser.Username,
                                    FullNameAr = x.CreatedByUser.FullNameAr,
                                    FullNameEn = x.CreatedByUser.FullNameEn,
                                    FullNameFn = x.CreatedByUser.FullNameFn,
                                    ProfileImage = x.CreatedByUser.ProfileImage
                                }
                            }).ToDataSource(take, skip, false);

                    wall.Surveys = _unitOfWork.GetRepository<Survey>().GetAll().OrderBy(x => x.CreatedOn)
                                     .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
                                     // .Where(x => x.IsShared || x.SurveyUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
                                     .Where(z => SearchText == "" || SearchText == null || z.Subject.Contains(SearchText))
                                        .Select(x => new SurveyDTO
                                        {
                                            Attachments = x.Attachments.Select(y => new SurveyAttachmentDTO
                                            {
                                                Attachment = new SavedAttachmentDTO
                                                {
                                                    AttachmentName = y.Attachment.AttachmentName,
                                                    AttachmentTypeId = y.Attachment.AttachmentTypeId,
                                                    CreatedBy = y.Attachment.CreatedBy,
                                                    CreatedByRole = y.Attachment.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                                    {
                                                        CommiteeUsersRoleId = y.Attachment.CreatedByRole.CommiteeUsersRoleId,
                                                        Role = new CommiteeDetailsRoleDTO
                                                        {
                                                            CommiteeRoleId = y.Attachment.CreatedByRole.CommiteeUsersRoleId,
                                                            CommiteeRolesNameAr = y.Attachment.CreatedByRole.Role.CommiteeRolesNameAr,
                                                            CommiteeRolesNameEn = y.Attachment.CreatedByRole.Role.CommiteeRolesNameEn
                                                        }
                                                    },
                                                    CreatedByRoleId = y.Attachment.CreatedByRoleId,
                                                    CreatedByUser = y.Attachment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                                    {
                                                        UserId = y.Attachment.CreatedByUser.UserId,
                                                        UserName = y.Attachment.CreatedByUser.Username,
                                                        FullNameAr = y.Attachment.CreatedByUser.FullNameAr,
                                                        FullNameEn = y.Attachment.CreatedByUser.FullNameEn,
                                                        FullNameFn = y.Attachment.CreatedByUser.FullNameFn,
                                                        ProfileImage = y.Attachment.CreatedByUser.ProfileImage
                                                    },
                                                    Height = y.Attachment.Height,
                                                    IsDisabled = y.Attachment.IsDisabled,
                                                    LFEntryId = y.Attachment.LFEntryId,
                                                    MimeType = y.Attachment.MimeType,
                                                    Notes = y.Attachment.Notes,
                                                    PagesCount = y.Attachment.PagesCount,
                                                    SavedAttachmentId = y.Attachment.SavedAttachmentId,
                                                    Size = y.Attachment.Size,
                                                    Width = y.Attachment.Width
                                                },
                                                AttachmentId = y.AttachmentId,
                                                SurveyAttachmentId = y.SurveyAttachmentId,
                                                SurveyId = y.SurveyId,

                                            }).ToList(),
                                            Comments = x.Comments.Select(y => new SurveyCommentDTO
                                            {
                                                Comment = new CommentDTO
                                                {
                                                    CreatedOn = y.Comment.CreatedOn,
                                                    CommentId = y.Comment.CommentId,
                                                    Text = y.Comment.Text,
                                                    CreatedByUser = y.Comment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                                    {
                                                        UserId = y.Comment.CreatedByUser.UserId,
                                                        UserName = y.Comment.CreatedByUser.Username,
                                                        FullNameAr = y.Comment.CreatedByUser.FullNameAr,
                                                        FullNameEn = y.Comment.CreatedByUser.FullNameEn,
                                                        FullNameFn = y.Comment.CreatedByUser.FullNameFn,
                                                        ProfileImage = y.Comment.CreatedByUser.ProfileImage
                                                    },
                                                },
                                                CommentId = y.CommentId,
                                                CreatedBy = y.CreatedBy,
                                                CreatedByRole = y.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                                {
                                                    CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                                    Role = new CommiteeDetailsRoleDTO
                                                    {
                                                        CommiteeRoleId = y.CreatedByRole.Role.CommiteeRoleId,
                                                        CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                                        CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                                    },
                                                },
                                                CreatedByRoleId = y.CreatedByRoleId,
                                                CreatedByUser = y.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                                {
                                                    UserId = y.CreatedByUser.UserId,
                                                    UserName = y.CreatedByUser.Username,
                                                    FullNameAr = y.CreatedByUser.FullNameAr,
                                                    FullNameEn = y.CreatedByUser.FullNameEn,
                                                    FullNameFn = y.CreatedByUser.FullNameFn,
                                                    ProfileImage = y.CreatedByUser.ProfileImage
                                                },
                                                CreatedOn = y.CreatedOn,
                                                SurveyCommentId = y.SurveyCommentId,
                                                SurveyId = y.SurveyId,

                                            }).ToList(),
                                            CommiteeId = x.CommiteeId,
                                            CreatedBy = x.CreatedBy,
                                            SurveyEndDate = x.SurveyEndDate,
                                            CreatedByRole = x.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                            {
                                                CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                                Role = new CommiteeDetailsRoleDTO
                                                {
                                                    CommiteeRoleId = x.CreatedByRole.Role.CommiteeRoleId,
                                                    CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                                                    CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                                                },
                                            },
                                            CreatedByRoleId = x.CreatedByRoleId,
                                            CreatedByUser = x.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                            {
                                                UserId = x.CreatedByUser.UserId,
                                                UserName = x.CreatedByUser.Username,
                                                FullNameAr = x.CreatedByUser.FullNameAr,
                                                FullNameEn = x.CreatedByUser.FullNameEn,
                                                FullNameFn = x.CreatedByUser.FullNameFn,
                                                ProfileImage = x.CreatedByUser.ProfileImage
                                            },
                                            CreatedOn = x.CreatedOn,
                                            IsShared = x.IsShared,
                                            MeetingId = x.MeetingId,
                                            MeetingTopicId = x.MeetingTopicId,
                                            Multi = x.Multi,
                                            Subject = x.Subject,
                                            SurveyId = x.SurveyId,
                                            SurveyAnswers = x.SurveyAnswers.Select(y => new SurveyAnswerDTO
                                            {
                                                Answer = y.Answer,
                                                SurveyAnswerId = y.SurveyAnswerId,
                                                SurveyAnswerUsers = y.SurveyAnswerUsers.Select(z => new SurveyAnswerUserDTO
                                                {
                                                    SurveyAnswerId = z.SurveyAnswerId,
                                                    SurveyAnswerUserId = z.SurveyAnswerUserId,
                                                    UserId = z.UserId,
                                                    User = new UserDetailsDTO
                                                    {
                                                        UserId = z.User.UserId,
                                                        UserName = z.User.Username,
                                                        FullNameAr = z.User.FullNameAr,
                                                        FullNameEn = z.User.FullNameEn,
                                                        FullNameFn = z.User.FullNameFn,
                                                        ProfileImage = z.User.ProfileImage
                                                    }
                                                }).ToList(),
                                                SurveyId = y.SurveyId
                                            }).ToList(),
                                            SurveyUsers = x.SurveyUsers.Select(y => new SurveyUserDTO
                                            {
                                                SurveyId = y.SurveyId,
                                                UserId = y.UserId,
                                                SurveyUserId = y.SurveyUserId
                                            }).ToList()
                                        }).ToDataSource(take, skip, false);
                }
                else
                {
                    wall.Tasks = _unitOfWork.GetRepository<CommiteeTask>().GetAll().OrderBy(x => x.CreatedOn)
                .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
                .Where(x => x.IsShared || x.MainAssinedUserId == _sessionServices.UserId || x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
                .Where(z => SearchText == "" || SearchText == null || z.Title.Contains(SearchText)).Take(take).Skip(skip)
                .Select(x => new CommiteeTaskDTO
                {
                    AssistantUsers = x.AssistantUsers.Select(y => new UserTaskDTO
                    {
                        CommiteeTaskId = (int)y.CommiteeTaskId,
                        UserId = y.UserId,
                        UserTaskId = y.UserTaskId,
                        User = new UserDetailsDTO
                        {
                            UserId = y.UserId,
                            UserName = y.User.Username,
                            FullNameAr = y.User.FullNameAr,
                            FullNameEn = y.User.FullNameEn,
                            FullNameFn = y.User.FullNameFn,
                            ProfileImage = y.User.ProfileImage
                        },
                    }).ToList(),
                    TaskAttachments = _Mapper.Map<List<CommitteeTaskAttachmentDTO>>(x.TaskAttachments),
                    //MultiMission = _Mapper.Map<List<CommiteetaskMultiMissionDTO>>(x.MultiMission),
                    MultiMission = x.MultiMission.Select(y => new CommiteetaskMultiMissionDTO
                    {
                        CommiteeTaskMultiMissionId = y.CommiteeTaskMultiMissionId,
                        Name = y.Name,
                        state = y.state,
                        EndDateMultiMission = y.EndDateMultiMission,
                        CommiteeTaskMultiMissionUserDTOs = y.CommiteeTaskMultiMissionUsers.Select(a => new CommiteeTaskMultiMissionUserDTO
                        {
                            CommiteeTaskMultiMissionId = a.CommiteeTaskMultiMissionId,
                            CommiteeTaskMultiMissionUserId = a.CommiteeTaskMultiMissionUserId,

                            UserDetailsDto = new UserDetailsDTO()
                            {
                                UserId = a.UserId,
                                UserName = a.User.Username,
                                FullNameAr = a.User.FullNameAr,
                                FullNameEn = a.User.FullNameEn,
                                ProfileImage = a.User.ProfileImage
                            },
                            UserId = a.UserId,
                            FullNameAr = a.User.FullNameAr,
                            FullNameEn = a.User.FullNameEn,
                        }).ToList(),
                    }).ToList(),
                    CommiteeId = x.CommiteeId,
                    CommiteeTaskId = x.CommiteeTaskId,
                    Completed = x.Completed,
                    CreatedBy = x.CreatedBy,
                    CreatedByRole = x.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                    {
                        CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                        Role = new CommiteeDetailsRoleDTO
                        {
                            CommiteeRoleId = x.CreatedByRole.Role.CommiteeRoleId,
                            CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                            CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                        }
                    },
                    CreatedByRoleId = x.CreatedByRoleId,
                    CreatedOn = x.CreatedOn,
                    CreatedByUser = x.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                    {
                        UserId = x.CreatedByUser.UserId,
                        UserName = x.CreatedByUser.Username,
                        FullNameAr = x.CreatedByUser.FullNameAr,
                        FullNameEn = x.CreatedByUser.FullNameEn,
                        FullNameFn = x.CreatedByUser.FullNameFn,
                        ProfileImage = x.CreatedByUser.ProfileImage
                    },
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    IsMain = x.IsMain,
                    IsShared = x.IsShared,
                    MainAssinedUser = new UserDetailsDTO
                    {
                        UserId = x.MainAssinedUser.UserId,
                        UserName = x.MainAssinedUser.Username,
                        FullNameAr = x.MainAssinedUser.FullNameAr,
                        FullNameEn = x.MainAssinedUser.FullNameEn,
                        FullNameFn = x.MainAssinedUser.FullNameFn,
                        ProfileImage = x.MainAssinedUser.ProfileImage
                    },
                    MainAssinedUserId = x.MainAssinedUserId,
                    Title = x.Title,
                    TaskDetails = x.TaskDetails,
                    TaskComments = x.TaskComments.Select(y => new TaskCommentDTO
                    {
                        CommentId = y.CommentId,
                        Comment = new CommentDTO
                        {
                            CommentId = (int)y.CommentId,
                            Text = y.Comment.Text,
                            SavedAttachments = y.Comment.SavedAttachments.Select(a => new SavedAttachmentDTO
                            {
                                AttachmentName = a.AttachmentName,
                                AttachmentTypeId = a.AttachmentTypeId,
                                CreatedBy = a.CreatedBy,
                                CreatedByRole = a.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                {
                                    CommiteeUsersRoleId = a.CreatedByRole.CommiteeUsersRoleId,
                                    Role = new CommiteeDetailsRoleDTO
                                    {
                                        CommiteeRoleId = a.CreatedByRole.CommiteeUsersRoleId,
                                        CommiteeRolesNameAr = a.CreatedByRole.Role.CommiteeRolesNameAr,
                                        CommiteeRolesNameEn = a.CreatedByRole.Role.CommiteeRolesNameEn
                                    }
                                },
                                CreatedByRoleId = a.CreatedByRoleId,
                                CreatedByUser = a.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                                {
                                    UserId = a.CreatedByUser.UserId,
                                    UserName = a.CreatedByUser.Username,
                                    FullNameAr = a.CreatedByUser.FullNameAr,
                                    FullNameEn = a.CreatedByUser.FullNameEn,
                                    FullNameFn = a.CreatedByUser.FullNameFn,
                                    ProfileImage = a.CreatedByUser.ProfileImage
                                },
                                Height = a.Height,
                                IsDisabled = a.IsDisabled,
                                LFEntryId = a.LFEntryId,
                                MimeType = a.MimeType,
                                Notes = a.Notes,
                                PagesCount = a.PagesCount,
                                SavedAttachmentId = a.SavedAttachmentId,
                                Size = a.Size,
                                Width = a.Width

                            }).ToList(),
                            CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                            {
                                UserId = y.CreatedByUser.UserId,
                                UserName = y.CreatedByUser.Username,
                                FullNameAr = y.CreatedByUser.FullNameAr,
                                FullNameEn = y.CreatedByUser.FullNameEn,
                                FullNameFn = y.CreatedByUser.FullNameFn,
                                ProfileImage = y.CreatedByUser.ProfileImage
                            },
                            CreatedBy = y.CreatedBy
                        },
                        CreatedBy = y.CreatedBy,
                        CreatedByRoleId = y.CreatedByRoleId,
                        CreatedOn = y.CreatedOn,
                        TaskCommentId = y.TaskCommentId,
                        TaskId = y.TaskId,
                        CreatedByRole = y.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                        {
                            CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                            Role = new CommiteeDetailsRoleDTO
                            {
                                CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                            }
                        },
                        CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                        {
                            UserId = y.CreatedByUser.UserId,
                            UserName = y.CreatedByUser.Username,
                            FullNameAr = y.CreatedByUser.FullNameAr,
                            FullNameEn = y.CreatedByUser.FullNameEn,
                            FullNameFn = y.CreatedByUser.FullNameFn,
                            ProfileImage = y.CreatedByUser.ProfileImage
                        },
                        UpdatedBy = y.UpdatedBy,
                        UpdatedOn = y.UpdatedOn
                    }).ToList()
                }).ToDataSource(take, skip, false);

                    wall.Attachments = _unitOfWork.GetRepository<CommiteeSavedAttachment>().GetAll().OrderBy(x => x.CreatedOn)
                            .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
                            .Where(x => x.AllUsers || x.AttachmentUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
                            .Where(z => SearchText == "" || SearchText == null || z.Description.Contains(SearchText))
                            .Select(x => new CommiteeAttachmentDTO
                            {
                                AllUsers = x.AllUsers,
                                AttachmentComments = x.AttachmentComments.Select(y => new AttachmentCommentDTO
                                {
                                    AttachmentCommentId = y.AttachmentCommentId,
                                    AttachmentId = y.AttachmentId,
                                    Comment = new CommentDTO
                                    {
                                        CommentId = y.CommentId,
                                        Text = y.Comment.Text,
                                        CreatedByUser = y.Comment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                        {
                                            UserId = y.CreatedByUser.UserId,
                                            UserName = y.CreatedByUser.Username,
                                            FullNameAr = y.CreatedByUser.FullNameAr,
                                            FullNameEn = y.CreatedByUser.FullNameEn,
                                            FullNameFn = y.CreatedByUser.FullNameFn,
                                            ProfileImage = y.CreatedByUser.ProfileImage
                                        },
                                    },
                                    CommentId = y.CommentId,
                                    CreatedBy = y.CreatedBy,
                                    CreatedByRoleId = y.CreatedByRoleId,
                                    CreatedByRole = y.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                    {
                                        CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                        Role = new CommiteeDetailsRoleDTO
                                        {
                                            CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                            CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                            CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                        }
                                    },
                                    CreatedByUser = new UserDetailsDTO
                                    {
                                        UserId = y.CreatedByUser.UserId,
                                        UserName = y.CreatedByUser.Username,
                                        FullNameAr = y.CreatedByUser.FullNameAr,
                                        FullNameEn = y.CreatedByUser.FullNameEn,
                                        FullNameFn = y.CreatedByUser.FullNameFn,
                                        ProfileImage = y.CreatedByUser.ProfileImage
                                    },
                                    CreatedOn = y.CreatedOn,

                                }).ToList(),
                                CommiteeAttachmentId = x.CommiteeAttachmentId,
                                CommiteeId = x.CommiteeId,
                                CreatedBy = x.CreatedBy,
                                CreatedOn = x.CreatedOn,
                                Description = x.Description,
                                Attachments = x.Attachments.Select(y => new SavedAttachmentDTO
                                {
                                    AttachmentName = y.AttachmentName,
                                    AttachmentTypeId = y.AttachmentTypeId,
                                    CreatedBy = y.CreatedBy,
                                    CreatedByRole = y.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                    {
                                        CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                        Role = new CommiteeDetailsRoleDTO
                                        {
                                            CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                            CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                            CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                        }
                                    },
                                    CreatedByRoleId = y.CreatedByRoleId,
                                    CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                                    {
                                        UserId = y.CreatedByUser.UserId,
                                        UserName = y.CreatedByUser.Username,
                                        FullNameAr = y.CreatedByUser.FullNameAr,
                                        FullNameEn = y.CreatedByUser.FullNameEn,
                                        FullNameFn = y.CreatedByUser.FullNameFn,
                                        ProfileImage = y.CreatedByUser.ProfileImage
                                    },
                                    Height = y.Height,
                                    IsDisabled = y.IsDisabled,
                                    LFEntryId = y.LFEntryId,
                                    MimeType = y.MimeType,
                                    Notes = y.Notes,
                                    PagesCount = y.PagesCount,
                                    SavedAttachmentId = y.SavedAttachmentId,
                                    Size = y.Size,
                                    Width = y.Width

                                }).ToList(),
                                AttachmentUsers = x.AttachmentUsers.Select(y => new AttachmentUserDTO
                                {
                                    AttachmentUserId = y.AttachmentUserId,
                                    CommiteeAttachmentId = y.CommiteeAttachmentId,
                                    User = new UserDetailsDTO
                                    {
                                        UserId = y.User.UserId,
                                        UserName = y.User.Username,
                                        FullNameAr = y.User.FullNameAr,
                                        FullNameEn = y.User.FullNameEn,
                                        FullNameFn = y.User.FullNameFn,
                                        ProfileImage = y.User.ProfileImage
                                    },
                                    UserId = y.UserId
                                }).ToList(),
                                CreatedByRole = x.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                {
                                    CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                    Role = new CommiteeDetailsRoleDTO
                                    {
                                        CommiteeRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                        CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                                        CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                                    }
                                },
                                CreatedByRoleId = x.CreatedByRoleId,
                                CreatedByUser = x.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                                {
                                    UserId = x.CreatedByUser.UserId,
                                    UserName = x.CreatedByUser.Username,
                                    FullNameAr = x.CreatedByUser.FullNameAr,
                                    FullNameEn = x.CreatedByUser.FullNameEn,
                                    FullNameFn = x.CreatedByUser.FullNameFn,
                                    ProfileImage = x.CreatedByUser.ProfileImage
                                }
                            }).ToDataSource(take, skip, false);

                    wall.Surveys = _unitOfWork.GetRepository<Survey>().GetAll().OrderBy(x => x.CreatedOn)
                                     .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
                                     .Where(x => x.IsShared || x.SurveyUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
                                     .Where(z => SearchText == "" || SearchText == null || z.Subject.Contains(SearchText))
                                        .Select(x => new SurveyDTO
                                        {
                                            Attachments = x.Attachments.Select(y => new SurveyAttachmentDTO
                                            {
                                                Attachment = new SavedAttachmentDTO
                                                {
                                                    AttachmentName = y.Attachment.AttachmentName,
                                                    AttachmentTypeId = y.Attachment.AttachmentTypeId,
                                                    CreatedBy = y.Attachment.CreatedBy,
                                                    CreatedByRole = y.Attachment.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                                    {
                                                        CommiteeUsersRoleId = y.Attachment.CreatedByRole.CommiteeUsersRoleId,
                                                        Role = new CommiteeDetailsRoleDTO
                                                        {
                                                            CommiteeRoleId = y.Attachment.CreatedByRole.CommiteeUsersRoleId,
                                                            CommiteeRolesNameAr = y.Attachment.CreatedByRole.Role.CommiteeRolesNameAr,
                                                            CommiteeRolesNameEn = y.Attachment.CreatedByRole.Role.CommiteeRolesNameEn
                                                        }
                                                    },
                                                    CreatedByRoleId = y.Attachment.CreatedByRoleId,
                                                    CreatedByUser = y.Attachment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                                    {
                                                        UserId = y.Attachment.CreatedByUser.UserId,
                                                        UserName = y.Attachment.CreatedByUser.Username,
                                                        FullNameAr = y.Attachment.CreatedByUser.FullNameAr,
                                                        FullNameEn = y.Attachment.CreatedByUser.FullNameEn,
                                                        FullNameFn = y.Attachment.CreatedByUser.FullNameFn,
                                                        ProfileImage = y.Attachment.CreatedByUser.ProfileImage
                                                    },
                                                    Height = y.Attachment.Height,
                                                    IsDisabled = y.Attachment.IsDisabled,
                                                    LFEntryId = y.Attachment.LFEntryId,
                                                    MimeType = y.Attachment.MimeType,
                                                    Notes = y.Attachment.Notes,
                                                    PagesCount = y.Attachment.PagesCount,
                                                    SavedAttachmentId = y.Attachment.SavedAttachmentId,
                                                    Size = y.Attachment.Size,
                                                    Width = y.Attachment.Width
                                                },
                                                AttachmentId = y.AttachmentId,
                                                SurveyAttachmentId = y.SurveyAttachmentId,
                                                SurveyId = y.SurveyId,

                                            }).ToList(),
                                            Comments = x.Comments.Select(y => new SurveyCommentDTO
                                            {
                                                Comment = new CommentDTO
                                                {
                                                    CreatedOn = y.Comment.CreatedOn,
                                                    CommentId = y.Comment.CommentId,
                                                    Text = y.Comment.Text,
                                                    CreatedByUser = y.Comment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                                    {
                                                        UserId = y.Comment.CreatedByUser.UserId,
                                                        UserName = y.Comment.CreatedByUser.Username,
                                                        FullNameAr = y.Comment.CreatedByUser.FullNameAr,
                                                        FullNameEn = y.Comment.CreatedByUser.FullNameEn,
                                                        FullNameFn = y.Comment.CreatedByUser.FullNameFn,
                                                        ProfileImage = y.Comment.CreatedByUser.ProfileImage
                                                    },
                                                },
                                                CommentId = y.CommentId,
                                                CreatedBy = y.CreatedBy,
                                                CreatedByRole = y.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                                {
                                                    CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                                    Role = new CommiteeDetailsRoleDTO
                                                    {
                                                        CommiteeRoleId = y.CreatedByRole.Role.CommiteeRoleId,
                                                        CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                                        CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                                    },
                                                },
                                                CreatedByRoleId = y.CreatedByRoleId,
                                                CreatedByUser = y.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                                {
                                                    UserId = y.CreatedByUser.UserId,
                                                    UserName = y.CreatedByUser.Username,
                                                    FullNameAr = y.CreatedByUser.FullNameAr,
                                                    FullNameEn = y.CreatedByUser.FullNameEn,
                                                    FullNameFn = y.CreatedByUser.FullNameFn,
                                                    ProfileImage = y.CreatedByUser.ProfileImage
                                                },
                                                CreatedOn = y.CreatedOn,
                                                SurveyCommentId = y.SurveyCommentId,
                                                SurveyId = y.SurveyId,

                                            }).ToList(),
                                            CommiteeId = x.CommiteeId,
                                            CreatedBy = x.CreatedBy,
                                            SurveyEndDate = x.SurveyEndDate,
                                            CreatedByRole = x.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                            {
                                                CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                                Role = new CommiteeDetailsRoleDTO
                                                {
                                                    CommiteeRoleId = x.CreatedByRole.Role.CommiteeRoleId,
                                                    CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                                                    CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                                                },
                                            },
                                            CreatedByRoleId = x.CreatedByRoleId,
                                            CreatedByUser = x.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                            {
                                                UserId = x.CreatedByUser.UserId,
                                                UserName = x.CreatedByUser.Username,
                                                FullNameAr = x.CreatedByUser.FullNameAr,
                                                FullNameEn = x.CreatedByUser.FullNameEn,
                                                FullNameFn = x.CreatedByUser.FullNameFn,
                                                ProfileImage = x.CreatedByUser.ProfileImage
                                            },
                                            CreatedOn = x.CreatedOn,
                                            IsShared = x.IsShared,
                                            MeetingId = x.MeetingId,
                                            MeetingTopicId = x.MeetingTopicId,
                                            Multi = x.Multi,
                                            Subject = x.Subject,
                                            SurveyId = x.SurveyId,
                                            SurveyAnswers = x.SurveyAnswers.Select(y => new SurveyAnswerDTO
                                            {
                                                Answer = y.Answer,
                                                SurveyAnswerId = y.SurveyAnswerId,
                                                SurveyAnswerUsers = y.SurveyAnswerUsers.Select(z => new SurveyAnswerUserDTO
                                                {
                                                    SurveyAnswerId = z.SurveyAnswerId,
                                                    SurveyAnswerUserId = z.SurveyAnswerUserId,
                                                    UserId = z.UserId,
                                                    User = new UserDetailsDTO
                                                    {
                                                        UserId = z.User.UserId,
                                                        UserName = z.User.Username,
                                                        FullNameAr = z.User.FullNameAr,
                                                        FullNameEn = z.User.FullNameEn,
                                                        FullNameFn = z.User.FullNameFn,
                                                        ProfileImage = z.User.ProfileImage
                                                    }
                                                }).ToList(),
                                                SurveyId = y.SurveyId
                                            }).ToList(),
                                            SurveyUsers = x.SurveyUsers.Select(y => new SurveyUserDTO
                                            {
                                                SurveyId = y.SurveyId,
                                                UserId = y.UserId,
                                                SurveyUserId = y.SurveyUserId
                                            }).ToList()
                                        }).ToDataSource(take, skip, false);
                }

            }
            else
            {
                var committeitem = _unitOfWork.GetRepository<Commitee>().GetAll().Where(x => x.CommiteeId == committeId).FirstOrDefault();
                var committeMember = _unitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == committeId).ToList();
                var headOfUnitId = committeitem.CurrenHeadUnitId;
                var AttchmentAll = new AllCommiteeAttachmentDTO();
                if (headOfUnitId != _sessionServices.UserId && committeMember.All(x => x.UserId != _sessionServices.UserId))
                {
                    wall.Tasks = _unitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
               .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
               //.Where(x => x.IsShared || x.MainAssinedUserId == _sessionServices.UserId || x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
               .Where(z => SearchText == "" || SearchText == null || z.Title.Contains(SearchText))
               .Select(x => new CommiteeTaskDTO
               {
                   AssistantUsers = x.AssistantUsers.Select(y => new UserTaskDTO
                   {
                       CommiteeTaskId = (int)y.CommiteeTaskId,
                       UserId = y.UserId,
                       UserTaskId = y.UserTaskId,
                       User = new UserDetailsDTO
                       {
                           UserId = y.UserId,
                           UserName = y.User.Username,
                           FullNameAr = y.User.FullNameAr,
                           FullNameEn = y.User.FullNameEn,
                           FullNameFn = y.User.FullNameFn,
                           ProfileImage = y.User.ProfileImage
                       },
                   }).ToList(),
                   ComiteeTaskCategory = _Mapper.Map<ComiteeTaskCategoryDTO>(x.ComiteeTaskCategory),
                   TaskAttachments = _Mapper.Map<List<CommitteeTaskAttachmentDTO>>(x.TaskAttachments),
                   //MultiMission = _Mapper.Map<List<CommiteetaskMultiMissionDTO>>(x.MultiMission),
                   MultiMission = x.MultiMission.Select(y => new CommiteetaskMultiMissionDTO
                   {
                       CommiteeTaskMultiMissionId = y.CommiteeTaskMultiMissionId,
                       Name = y.Name,
                       state = y.state,
                       EndDateMultiMission = y.EndDateMultiMission,

                       CommiteeTaskMultiMissionUserDTOs = y.CommiteeTaskMultiMissionUsers.Select(a => new CommiteeTaskMultiMissionUserDTO
                       {
                           CommiteeTaskMultiMissionId = a.CommiteeTaskMultiMissionId,
                           CommiteeTaskMultiMissionUserId = a.CommiteeTaskMultiMissionUserId,

                           UserDetailsDto = new UserDetailsDTO()
                           {
                               UserId = a.UserId,
                               UserName = a.User.Username,
                               FullNameAr = a.User.FullNameAr,
                               FullNameEn = a.User.FullNameEn,
                               ProfileImage = a.User.ProfileImage
                           },
                           UserId = a.UserId,
                           FullNameAr = a.User.FullNameAr,
                           FullNameEn = a.User.FullNameEn,
                       }).ToList(),
                   }).ToList(),
                   CommiteeId = x.CommiteeId,
                   CommiteeTaskId = x.CommiteeTaskId,
                   Completed = x.Completed,
                   CreatedBy = x.CreatedBy,
                   CreatedByRole = x.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                   {
                       CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                       Role = new CommiteeDetailsRoleDTO
                       {
                           CommiteeRoleId = x.CreatedByRole.Role.CommiteeRoleId,
                           CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                           CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                       }
                   },
                   CreatedByRoleId = x.CreatedByRoleId,
                   CreatedOn = x.CreatedOn,
                   CreatedByUser = x.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                   {
                       UserId = x.CreatedByUser.UserId,
                       UserName = x.CreatedByUser.Username,
                       FullNameAr = x.CreatedByUser.FullNameAr,
                       FullNameEn = x.CreatedByUser.FullNameEn,
                       FullNameFn = x.CreatedByUser.FullNameFn,
                       ProfileImage = x.CreatedByUser.ProfileImage
                   },
                   EndDate = x.EndDate,
                   StartDate = x.StartDate,
                   IsMain = x.IsMain,
                   IsShared = x.IsShared,
                   MainAssinedUser = new UserDetailsDTO
                   {
                       UserId = x.MainAssinedUser.UserId,
                       UserName = x.MainAssinedUser.Username,
                       FullNameAr = x.MainAssinedUser.FullNameAr,
                       FullNameEn = x.MainAssinedUser.FullNameEn,
                       FullNameFn = x.MainAssinedUser.FullNameFn,
                       ProfileImage = x.MainAssinedUser.ProfileImage
                   },
                   MainAssinedUserId = x.MainAssinedUserId,
                   Title = x.Title,
                   TaskDetails = x.TaskDetails,
                   TaskComments = x.TaskComments.Select(y => new TaskCommentDTO
                   {
                       CommentId = y.CommentId,
                       Comment = new CommentDTO
                       {
                           CommentId = y.CommentId,
                           Text = y.Comment.Text,
                           SavedAttachments = y.Comment.SavedAttachments.Select(a => new SavedAttachmentDTO
                           {
                               AttachmentName = a.AttachmentName,
                               AttachmentTypeId = a.AttachmentTypeId,
                               CreatedBy = a.CreatedBy,
                               CreatedByRole = a.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                               {
                                   CommiteeUsersRoleId = a.CreatedByRole.CommiteeUsersRoleId,
                                   Role = new CommiteeDetailsRoleDTO
                                   {
                                       CommiteeRoleId = a.CreatedByRole.CommiteeUsersRoleId,
                                       CommiteeRolesNameAr = a.CreatedByRole.Role.CommiteeRolesNameAr,
                                       CommiteeRolesNameEn = a.CreatedByRole.Role.CommiteeRolesNameEn
                                   }
                               },
                               CreatedByRoleId = a.CreatedByRoleId,
                               CreatedByUser = a.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                               {
                                   UserId = a.CreatedByUser.UserId,
                                   UserName = a.CreatedByUser.Username,
                                   FullNameAr = a.CreatedByUser.FullNameAr,
                                   FullNameEn = a.CreatedByUser.FullNameEn,
                                   FullNameFn = a.CreatedByUser.FullNameFn,
                                   ProfileImage = a.CreatedByUser.ProfileImage
                               },
                               Height = a.Height,
                               IsDisabled = a.IsDisabled,
                               LFEntryId = a.LFEntryId,
                               MimeType = a.MimeType,
                               Notes = a.Notes,
                               PagesCount = a.PagesCount,
                               SavedAttachmentId = a.SavedAttachmentId,
                               Size = a.Size,
                               Width = a.Width

                           }).ToList(),
                           CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                           {
                               UserId = y.CreatedByUser.UserId,
                               UserName = y.CreatedByUser.Username,
                               FullNameAr = y.CreatedByUser.FullNameAr,
                               FullNameEn = y.CreatedByUser.FullNameEn,
                               FullNameFn = y.CreatedByUser.FullNameFn,
                               ProfileImage = y.CreatedByUser.ProfileImage
                           },
                           CreatedBy = y.CreatedBy
                       },
                       CreatedBy = y.CreatedBy,
                       CreatedByRoleId = y.CreatedByRoleId,
                       CreatedOn = y.CreatedOn,
                       TaskCommentId = y.TaskCommentId,
                       TaskId = y.TaskId,
                       CreatedByRole = y.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                       {
                           CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                           Role = new CommiteeDetailsRoleDTO
                           {
                               CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                               CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                               CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                           }
                       },
                       CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                       {
                           UserId = y.CreatedByUser.UserId,
                           UserName = y.CreatedByUser.Username,
                           FullNameAr = y.CreatedByUser.FullNameAr,
                           FullNameEn = y.CreatedByUser.FullNameEn,
                           FullNameFn = y.CreatedByUser.FullNameFn,
                           ProfileImage = y.CreatedByUser.ProfileImage
                       },
                       UpdatedBy = y.UpdatedBy,
                       UpdatedOn = y.UpdatedOn
                   }).ToList()
               }).ToDataSource(take, skip, false);


                    wall.Attachments = _unitOfWork.GetRepository<CommiteeSavedAttachment>().GetAll().OrderByDescending(x => x.CreatedOn)
                    .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
                    //.Where(x => x.AllUsers || x.AttachmentUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
                    .Where(z => SearchText == "" || SearchText == null || z.Description.Contains(SearchText))
                    .Select(x => new CommiteeAttachmentDTO
                    {
                        AllUsers = x.AllUsers,
                        AttachmentComments = x.AttachmentComments.Select(y => new AttachmentCommentDTO
                        {
                            AttachmentCommentId = y.AttachmentCommentId,
                            AttachmentId = y.AttachmentId,
                            Comment = new CommentDTO
                            {
                                CommentId = y.CommentId,
                                Text = y.Comment.Text,
                                CreatedByUser = y.Comment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                {
                                    UserId = y.CreatedByUser.UserId,
                                    UserName = y.CreatedByUser.Username,
                                    FullNameAr = y.CreatedByUser.FullNameAr,
                                    FullNameEn = y.CreatedByUser.FullNameEn,
                                    FullNameFn = y.CreatedByUser.FullNameFn,
                                    ProfileImage = y.CreatedByUser.ProfileImage
                                },
                            },
                            CommentId = y.CommentId,
                            CreatedBy = y.CreatedBy,
                            CreatedByRoleId = y.CreatedByRoleId,
                            CreatedByRole = y.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                            {
                                CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                Role = new CommiteeDetailsRoleDTO
                                {
                                    CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                    CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                    CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                }
                            },
                            CreatedByUser = new UserDetailsDTO
                            {
                                UserId = y.CreatedByUser.UserId,
                                UserName = y.CreatedByUser.Username,
                                FullNameAr = y.CreatedByUser.FullNameAr,
                                FullNameEn = y.CreatedByUser.FullNameEn,
                                FullNameFn = y.CreatedByUser.FullNameFn,
                                ProfileImage = y.CreatedByUser.ProfileImage
                            },
                            CreatedOn = y.CreatedOn,

                        }).ToList(),
                        CommiteeAttachmentId = x.CommiteeAttachmentId,
                        CommiteeId = x.CommiteeId,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        Description = x.Description,
                        Attachments = x.Attachments.Select(y => new SavedAttachmentDTO
                        {
                            AttachmentName = y.AttachmentName,
                            AttachmentTypeId = y.AttachmentTypeId,
                            CreatedBy = y.CreatedBy,
                            CreatedByRole = y.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                            {
                                CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                Role = new CommiteeDetailsRoleDTO
                                {
                                    CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                    CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                    CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                }
                            },
                            CreatedByRoleId = y.CreatedByRoleId,
                            CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                            {
                                UserId = y.CreatedByUser.UserId,
                                UserName = y.CreatedByUser.Username,
                                FullNameAr = y.CreatedByUser.FullNameAr,
                                FullNameEn = y.CreatedByUser.FullNameEn,
                                FullNameFn = y.CreatedByUser.FullNameFn,
                                ProfileImage = y.CreatedByUser.ProfileImage
                            },
                            Height = y.Height,
                            IsDisabled = y.IsDisabled,
                            LFEntryId = y.LFEntryId,
                            MimeType = y.MimeType,
                            Notes = y.Notes,
                            PagesCount = y.PagesCount,
                            SavedAttachmentId = y.SavedAttachmentId,
                            Size = y.Size,
                            Width = y.Width

                        }).ToList(),
                        AttachmentUsers = x.AttachmentUsers.Select(y => new AttachmentUserDTO
                        {
                            AttachmentUserId = y.AttachmentUserId,
                            CommiteeAttachmentId = y.CommiteeAttachmentId,
                            User = new UserDetailsDTO
                            {
                                UserId = y.User.UserId,
                                UserName = y.User.Username,
                                FullNameAr = y.User.FullNameAr,
                                FullNameEn = y.User.FullNameEn,
                                FullNameFn = y.User.FullNameFn,
                                ProfileImage = y.User.ProfileImage
                            },
                            UserId = y.UserId
                        }).ToList(),
                        CreatedByRole = x.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                        {
                            CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                            Role = new CommiteeDetailsRoleDTO
                            {
                                CommiteeRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                                CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                            }
                        },
                        CreatedByRoleId = x.CreatedByRoleId,
                        CreatedByUser = x.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                        {
                            UserId = x.CreatedByUser.UserId,
                            UserName = x.CreatedByUser.Username,
                            FullNameAr = x.CreatedByUser.FullNameAr,
                            FullNameEn = x.CreatedByUser.FullNameEn,
                            FullNameFn = x.CreatedByUser.FullNameFn,
                            ProfileImage = x.CreatedByUser.ProfileImage
                        }
                    }).ToDataSource(take, skip);


                    wall.Surveys = _unitOfWork.GetRepository<Survey>().GetAll().OrderByDescending(x => x.CreatedOn)
                                    .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
                                    // .Where(x => x.IsShared || x.SurveyUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
                                    .Where(z => SearchText == "" || SearchText == null || z.Subject.Contains(SearchText))
                                    .Select(x => new SurveyDTO
                                    {
                                        Attachments = x.Attachments.Select(y => new SurveyAttachmentDTO
                                        {
                                            Attachment = new SavedAttachmentDTO
                                            {
                                                AttachmentName = y.Attachment.AttachmentName,
                                                AttachmentTypeId = y.Attachment.AttachmentTypeId,
                                                CreatedBy = y.Attachment.CreatedBy,
                                                CreatedByRole = y.Attachment.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                                {
                                                    CommiteeUsersRoleId = y.Attachment.CreatedByRole.CommiteeUsersRoleId,
                                                    Role = new CommiteeDetailsRoleDTO
                                                    {
                                                        CommiteeRoleId = y.Attachment.CreatedByRole.CommiteeUsersRoleId,
                                                        CommiteeRolesNameAr = y.Attachment.CreatedByRole.Role.CommiteeRolesNameAr,
                                                        CommiteeRolesNameEn = y.Attachment.CreatedByRole.Role.CommiteeRolesNameEn
                                                    }
                                                },
                                                CreatedByRoleId = y.Attachment.CreatedByRoleId,
                                                CreatedByUser = y.Attachment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                                {
                                                    UserId = y.Attachment.CreatedByUser.UserId,
                                                    UserName = y.Attachment.CreatedByUser.Username,
                                                    FullNameAr = y.Attachment.CreatedByUser.FullNameAr,
                                                    FullNameEn = y.Attachment.CreatedByUser.FullNameEn,
                                                    FullNameFn = y.Attachment.CreatedByUser.FullNameFn,
                                                    ProfileImage = y.Attachment.CreatedByUser.ProfileImage
                                                },
                                                Height = y.Attachment.Height,
                                                IsDisabled = y.Attachment.IsDisabled,
                                                LFEntryId = y.Attachment.LFEntryId,
                                                MimeType = y.Attachment.MimeType,
                                                Notes = y.Attachment.Notes,
                                                PagesCount = y.Attachment.PagesCount,
                                                SavedAttachmentId = y.Attachment.SavedAttachmentId,
                                                Size = y.Attachment.Size,
                                                Width = y.Attachment.Width
                                            },
                                            AttachmentId = y.AttachmentId,
                                            SurveyAttachmentId = y.SurveyAttachmentId,
                                            SurveyId = y.SurveyId
                                        }).ToList(),
                                        Comments = x.Comments.Select(y => new SurveyCommentDTO
                                        {
                                            Comment = new CommentDTO
                                            {
                                                CreatedOn = y.Comment.CreatedOn,
                                                CommentId = y.Comment.CommentId,
                                                Text = y.Comment.Text,
                                                CreatedByUser = y.Comment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                                {
                                                    UserId = y.Comment.CreatedByUser.UserId,
                                                    UserName = y.Comment.CreatedByUser.Username,
                                                    FullNameAr = y.Comment.CreatedByUser.FullNameAr,
                                                    FullNameEn = y.Comment.CreatedByUser.FullNameEn,
                                                    FullNameFn = y.Comment.CreatedByUser.FullNameFn,
                                                    ProfileImage = y.Comment.CreatedByUser.ProfileImage
                                                },
                                            },
                                            CommentId = y.CommentId,
                                            CreatedBy = y.CreatedBy,
                                            CreatedByRole = y.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                            {
                                                CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                                Role = new CommiteeDetailsRoleDTO
                                                {
                                                    CommiteeRoleId = y.CreatedByRole.Role.CommiteeRoleId,
                                                    CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                                    CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                                },
                                            },
                                            CreatedByRoleId = y.CreatedByRoleId,
                                            CreatedByUser = y.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                            {
                                                UserId = y.CreatedByUser.UserId,
                                                UserName = y.CreatedByUser.Username,
                                                FullNameAr = y.CreatedByUser.FullNameAr,
                                                FullNameEn = y.CreatedByUser.FullNameEn,
                                                FullNameFn = y.CreatedByUser.FullNameFn,
                                                ProfileImage = y.CreatedByUser.ProfileImage
                                            },
                                            CreatedOn = y.CreatedOn,
                                            SurveyCommentId = y.SurveyCommentId,
                                            SurveyId = y.SurveyId,

                                        }).ToList(),
                                        CommiteeId = x.CommiteeId,
                                        CreatedBy = x.CreatedBy,
                                        CreatedByRole = x.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                        {
                                            CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                            Role = new CommiteeDetailsRoleDTO
                                            {
                                                CommiteeRoleId = x.CreatedByRole.Role.CommiteeRoleId,
                                                CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                                                CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                                            },
                                        },
                                        CreatedByRoleId = x.CreatedByRoleId,
                                        CreatedByUser = x.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                        {
                                            UserId = x.CreatedByUser.UserId,
                                            UserName = x.CreatedByUser.Username,
                                            FullNameAr = x.CreatedByUser.FullNameAr,
                                            FullNameEn = x.CreatedByUser.FullNameEn,
                                            FullNameFn = x.CreatedByUser.FullNameFn,
                                            ProfileImage = x.CreatedByUser.ProfileImage
                                        },
                                        CreatedOn = x.CreatedOn,
                                        IsShared = x.IsShared,
                                        MeetingId = x.MeetingId,
                                        MeetingTopicId = x.MeetingTopicId,
                                        Multi = x.Multi,
                                        Subject = x.Subject,
                                        SurveyId = x.SurveyId,
                                        SurveyEndDate = x.SurveyEndDate,
                                        SurveyAnswers = x.SurveyAnswers.Select(y => new SurveyAnswerDTO
                                        {
                                            Answer = y.Answer,
                                            SurveyAnswerId = y.SurveyAnswerId,
                                            SurveyAnswerUsers = y.SurveyAnswerUsers.Select(z => new SurveyAnswerUserDTO
                                            {
                                                SurveyAnswerId = z.SurveyAnswerId,
                                                SurveyAnswerUserId = z.SurveyAnswerUserId,
                                                UserId = z.UserId,
                                                User = new UserDetailsDTO
                                                {
                                                    UserId = z.User.UserId,
                                                    UserName = z.User.Username,
                                                    FullNameAr = z.User.FullNameAr,
                                                    FullNameEn = z.User.FullNameEn,
                                                    FullNameFn = z.User.FullNameFn,
                                                    ProfileImage = z.User.ProfileImage
                                                }
                                            }).ToList(),
                                            SurveyId = y.SurveyId
                                        }).ToList(),
                                        SurveyUsers = x.SurveyUsers.Select(y => new SurveyUserDTO
                                        {
                                            SurveyId = y.SurveyId,
                                            UserId = y.UserId,
                                            SurveyUserId = y.SurveyUserId
                                        }).ToList()
                                    }).ToDataSource(take, skip, false);
                }

                else
                {
                    wall.Tasks = _unitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
             .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
             .Where(x => x.IsShared || x.MainAssinedUserId == _sessionServices.UserId || x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
             .Where(z => SearchText == "" || SearchText == null || z.Title.Contains(SearchText))
             .Select(x => new CommiteeTaskDTO
             {
                 AssistantUsers = x.AssistantUsers.Select(y => new UserTaskDTO
                 {
                     CommiteeTaskId = (int)y.CommiteeTaskId,
                     UserId = y.UserId,
                     UserTaskId = y.UserTaskId,
                     User = new UserDetailsDTO
                     {
                         UserId = y.UserId,
                         UserName = y.User.Username,
                         FullNameAr = y.User.FullNameAr,
                         FullNameEn = y.User.FullNameEn,
                         FullNameFn = y.User.FullNameFn,
                         ProfileImage = y.User.ProfileImage
                     },
                 }).ToList(),
                 ComiteeTaskCategory = _Mapper.Map<ComiteeTaskCategoryDTO>(x.ComiteeTaskCategory),
                 TaskAttachments = _Mapper.Map<List<CommitteeTaskAttachmentDTO>>(x.TaskAttachments),
                 //MultiMission = _Mapper.Map<List<CommiteetaskMultiMissionDTO>>(x.MultiMission),
                 MultiMission = x.MultiMission.Select(y => new CommiteetaskMultiMissionDTO
                 {
                     CommiteeTaskMultiMissionId = y.CommiteeTaskMultiMissionId,
                     Name = y.Name,
                     state = y.state,
                     EndDateMultiMission = y.EndDateMultiMission,

                     CommiteeTaskMultiMissionUserDTOs = y.CommiteeTaskMultiMissionUsers.Select(a => new CommiteeTaskMultiMissionUserDTO
                     {
                         CommiteeTaskMultiMissionId = a.CommiteeTaskMultiMissionId,
                         CommiteeTaskMultiMissionUserId = a.CommiteeTaskMultiMissionUserId,

                         UserDetailsDto = new UserDetailsDTO()
                         {
                             UserId = a.UserId,
                             UserName = a.User.Username,
                             FullNameAr = a.User.FullNameAr,
                             FullNameEn = a.User.FullNameEn,
                             ProfileImage = a.User.ProfileImage
                         },
                         UserId = a.UserId,
                         FullNameAr = a.User.FullNameAr,
                         FullNameEn = a.User.FullNameEn,
                     }).ToList(),
                 }).ToList(),
                 CommiteeId = x.CommiteeId,
                 CommiteeTaskId = x.CommiteeTaskId,
                 Completed = x.Completed,
                 CreatedBy = x.CreatedBy,
                 CreatedByRole = x.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                 {
                     CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                     Role = new CommiteeDetailsRoleDTO
                     {
                         CommiteeRoleId = x.CreatedByRole.Role.CommiteeRoleId,
                         CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                         CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                     }
                 },
                 CreatedByRoleId = x.CreatedByRoleId,
                 CreatedOn = x.CreatedOn,
                 CreatedByUser = x.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                 {
                     UserId = x.CreatedByUser.UserId,
                     UserName = x.CreatedByUser.Username,
                     FullNameAr = x.CreatedByUser.FullNameAr,
                     FullNameEn = x.CreatedByUser.FullNameEn,
                     FullNameFn = x.CreatedByUser.FullNameFn,
                     ProfileImage = x.CreatedByUser.ProfileImage
                 },
                 EndDate = x.EndDate,
                 StartDate = x.StartDate,
                 IsMain = x.IsMain,
                 IsShared = x.IsShared,
                 MainAssinedUser = new UserDetailsDTO
                 {
                     UserId = x.MainAssinedUser.UserId,
                     UserName = x.MainAssinedUser.Username,
                     FullNameAr = x.MainAssinedUser.FullNameAr,
                     FullNameEn = x.MainAssinedUser.FullNameEn,
                     FullNameFn = x.MainAssinedUser.FullNameFn,
                     ProfileImage = x.MainAssinedUser.ProfileImage
                 },
                 MainAssinedUserId = x.MainAssinedUserId,
                 Title = x.Title,
                 TaskDetails = x.TaskDetails,
                 TaskComments = x.TaskComments.Select(y => new TaskCommentDTO
                 {
                     CommentId = y.CommentId,
                     Comment = new CommentDTO
                     {
                         CommentId = y.CommentId,
                         Text = y.Comment.Text,
                         SavedAttachments = y.Comment.SavedAttachments.Select(a => new SavedAttachmentDTO
                         {
                             AttachmentName = a.AttachmentName,
                             AttachmentTypeId = a.AttachmentTypeId,
                             CreatedBy = a.CreatedBy,
                             CreatedByRole = a.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                             {
                                 CommiteeUsersRoleId = a.CreatedByRole.CommiteeUsersRoleId,
                                 Role = new CommiteeDetailsRoleDTO
                                 {
                                     CommiteeRoleId = a.CreatedByRole.CommiteeUsersRoleId,
                                     CommiteeRolesNameAr = a.CreatedByRole.Role.CommiteeRolesNameAr,
                                     CommiteeRolesNameEn = a.CreatedByRole.Role.CommiteeRolesNameEn
                                 }
                             },
                             CreatedByRoleId = a.CreatedByRoleId,
                             CreatedByUser = a.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                             {
                                 UserId = a.CreatedByUser.UserId,
                                 UserName = a.CreatedByUser.Username,
                                 FullNameAr = a.CreatedByUser.FullNameAr,
                                 FullNameEn = a.CreatedByUser.FullNameEn,
                                 FullNameFn = a.CreatedByUser.FullNameFn,
                                 ProfileImage = a.CreatedByUser.ProfileImage
                             },
                             Height = a.Height,
                             IsDisabled = a.IsDisabled,
                             LFEntryId = a.LFEntryId,
                             MimeType = a.MimeType,
                             Notes = a.Notes,
                             PagesCount = a.PagesCount,
                             SavedAttachmentId = a.SavedAttachmentId,
                             Size = a.Size,
                             Width = a.Width

                         }).ToList(),
                         CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                         {
                             UserId = y.CreatedByUser.UserId,
                             UserName = y.CreatedByUser.Username,
                             FullNameAr = y.CreatedByUser.FullNameAr,
                             FullNameEn = y.CreatedByUser.FullNameEn,
                             FullNameFn = y.CreatedByUser.FullNameFn,
                             ProfileImage = y.CreatedByUser.ProfileImage
                         },
                         CreatedBy = y.CreatedBy
                     },
                     CreatedBy = y.CreatedBy,
                     CreatedByRoleId = y.CreatedByRoleId,
                     CreatedOn = y.CreatedOn,
                     TaskCommentId = y.TaskCommentId,
                     TaskId = y.TaskId,
                     CreatedByRole = y.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                     {
                         CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                         Role = new CommiteeDetailsRoleDTO
                         {
                             CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                             CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                             CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                         }
                     },
                     CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                     {
                         UserId = y.CreatedByUser.UserId,
                         UserName = y.CreatedByUser.Username,
                         FullNameAr = y.CreatedByUser.FullNameAr,
                         FullNameEn = y.CreatedByUser.FullNameEn,
                         FullNameFn = y.CreatedByUser.FullNameFn,
                         ProfileImage = y.CreatedByUser.ProfileImage
                     },
                     UpdatedBy = y.UpdatedBy,
                     UpdatedOn = y.UpdatedOn
                 }).ToList()
             }).ToDataSource(take, skip, false);


                    wall.Attachments = _unitOfWork.GetRepository<CommiteeSavedAttachment>().GetAll().OrderByDescending(x => x.CreatedOn)
                    .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
                    .Where(x => x.AllUsers || x.AttachmentUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
                    .Where(z => SearchText == "" || SearchText == null || z.Description.Contains(SearchText))
                    .Select(x => new CommiteeAttachmentDTO
                    {
                        AllUsers = x.AllUsers,
                        AttachmentComments = x.AttachmentComments.Select(y => new AttachmentCommentDTO
                        {
                            AttachmentCommentId = y.AttachmentCommentId,
                            AttachmentId = y.AttachmentId,
                            Comment = new CommentDTO
                            {
                                CommentId = y.CommentId,
                                Text = y.Comment.Text,
                                CreatedByUser = y.Comment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                {
                                    UserId = y.CreatedByUser.UserId,
                                    UserName = y.CreatedByUser.Username,
                                    FullNameAr = y.CreatedByUser.FullNameAr,
                                    FullNameEn = y.CreatedByUser.FullNameEn,
                                    FullNameFn = y.CreatedByUser.FullNameFn,
                                    ProfileImage = y.CreatedByUser.ProfileImage
                                },
                            },
                            CommentId = y.CommentId,
                            CreatedBy = y.CreatedBy,
                            CreatedByRoleId = y.CreatedByRoleId,
                            CreatedByRole = y.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                            {
                                CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                Role = new CommiteeDetailsRoleDTO
                                {
                                    CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                    CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                    CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                }
                            },
                            CreatedByUser = new UserDetailsDTO
                            {
                                UserId = y.CreatedByUser.UserId,
                                UserName = y.CreatedByUser.Username,
                                FullNameAr = y.CreatedByUser.FullNameAr,
                                FullNameEn = y.CreatedByUser.FullNameEn,
                                FullNameFn = y.CreatedByUser.FullNameFn,
                                ProfileImage = y.CreatedByUser.ProfileImage
                            },
                            CreatedOn = y.CreatedOn,

                        }).ToList(),
                        CommiteeAttachmentId = x.CommiteeAttachmentId,
                        CommiteeId = x.CommiteeId,
                        CreatedBy = x.CreatedBy,
                        CreatedOn = x.CreatedOn,
                        Description = x.Description,
                        Attachments = x.Attachments.Select(y => new SavedAttachmentDTO
                        {
                            AttachmentName = y.AttachmentName,
                            AttachmentTypeId = y.AttachmentTypeId,
                            CreatedBy = y.CreatedBy,
                            CreatedByRole = y.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                            {
                                CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                Role = new CommiteeDetailsRoleDTO
                                {
                                    CommiteeRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                    CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                    CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                }
                            },
                            CreatedByRoleId = y.CreatedByRoleId,
                            CreatedByUser = y.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                            {
                                UserId = y.CreatedByUser.UserId,
                                UserName = y.CreatedByUser.Username,
                                FullNameAr = y.CreatedByUser.FullNameAr,
                                FullNameEn = y.CreatedByUser.FullNameEn,
                                FullNameFn = y.CreatedByUser.FullNameFn,
                                ProfileImage = y.CreatedByUser.ProfileImage
                            },
                            Height = y.Height,
                            IsDisabled = y.IsDisabled,
                            LFEntryId = y.LFEntryId,
                            MimeType = y.MimeType,
                            Notes = y.Notes,
                            PagesCount = y.PagesCount,
                            SavedAttachmentId = y.SavedAttachmentId,
                            Size = y.Size,
                            Width = y.Width

                        }).ToList(),
                        AttachmentUsers = x.AttachmentUsers.Select(y => new AttachmentUserDTO
                        {
                            AttachmentUserId = y.AttachmentUserId,
                            CommiteeAttachmentId = y.CommiteeAttachmentId,
                            User = new UserDetailsDTO
                            {
                                UserId = y.User.UserId,
                                UserName = y.User.Username,
                                FullNameAr = y.User.FullNameAr,
                                FullNameEn = y.User.FullNameEn,
                                FullNameFn = y.User.FullNameFn,
                                ProfileImage = y.User.ProfileImage
                            },
                            UserId = y.UserId
                        }).ToList(),
                        CreatedByRole = x.CreatedByRoleId == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                        {
                            CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                            Role = new CommiteeDetailsRoleDTO
                            {
                                CommiteeRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                                CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                            }
                        },
                        CreatedByRoleId = x.CreatedByRoleId,
                        CreatedByUser = x.CreatedBy == null ? new UserDetailsDTO() : new UserDetailsDTO
                        {
                            UserId = x.CreatedByUser.UserId,
                            UserName = x.CreatedByUser.Username,
                            FullNameAr = x.CreatedByUser.FullNameAr,
                            FullNameEn = x.CreatedByUser.FullNameEn,
                            FullNameFn = x.CreatedByUser.FullNameFn,
                            ProfileImage = x.CreatedByUser.ProfileImage
                        }
                    }).ToDataSource(take, skip, false);


                    wall.Surveys = _unitOfWork.GetRepository<Survey>().GetAll().OrderByDescending(x => x.CreatedOn)
                                    .Where(x => x.CommiteeId == committeId && (dateFrom == null || x.CreatedOn >= dateFrom) && (dateTo == null || x.CreatedOn <= dateTo))
                                    .Where(x => x.IsShared || x.SurveyUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
                                    .Where(z => SearchText == "" || SearchText == null || z.Subject.Contains(SearchText))
                                    .Select(x => new SurveyDTO
                                    {
                                        Attachments = x.Attachments.Select(y => new SurveyAttachmentDTO
                                        {
                                            Attachment = new SavedAttachmentDTO
                                            {
                                                AttachmentName = y.Attachment.AttachmentName,
                                                AttachmentTypeId = y.Attachment.AttachmentTypeId,
                                                CreatedBy = y.Attachment.CreatedBy,
                                                CreatedByRole = y.Attachment.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                                {
                                                    CommiteeUsersRoleId = y.Attachment.CreatedByRole.CommiteeUsersRoleId,
                                                    Role = new CommiteeDetailsRoleDTO
                                                    {
                                                        CommiteeRoleId = y.Attachment.CreatedByRole.CommiteeUsersRoleId,
                                                        CommiteeRolesNameAr = y.Attachment.CreatedByRole.Role.CommiteeRolesNameAr,
                                                        CommiteeRolesNameEn = y.Attachment.CreatedByRole.Role.CommiteeRolesNameEn
                                                    }
                                                },
                                                CreatedByRoleId = y.Attachment.CreatedByRoleId,
                                                CreatedByUser = y.Attachment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                                {
                                                    UserId = y.Attachment.CreatedByUser.UserId,
                                                    UserName = y.Attachment.CreatedByUser.Username,
                                                    FullNameAr = y.Attachment.CreatedByUser.FullNameAr,
                                                    FullNameEn = y.Attachment.CreatedByUser.FullNameEn,
                                                    FullNameFn = y.Attachment.CreatedByUser.FullNameFn,
                                                    ProfileImage = y.Attachment.CreatedByUser.ProfileImage
                                                },
                                                Height = y.Attachment.Height,
                                                IsDisabled = y.Attachment.IsDisabled,
                                                LFEntryId = y.Attachment.LFEntryId,
                                                MimeType = y.Attachment.MimeType,
                                                Notes = y.Attachment.Notes,
                                                PagesCount = y.Attachment.PagesCount,
                                                SavedAttachmentId = y.Attachment.SavedAttachmentId,
                                                Size = y.Attachment.Size,
                                                Width = y.Attachment.Width
                                            },
                                            AttachmentId = y.AttachmentId,
                                            SurveyAttachmentId = y.SurveyAttachmentId,
                                            SurveyId = y.SurveyId
                                        }).ToList(),
                                        Comments = x.Comments.Select(y => new SurveyCommentDTO
                                        {
                                            Comment = new CommentDTO
                                            {
                                                CreatedOn = y.Comment.CreatedOn,
                                                CommentId = y.Comment.CommentId,
                                                Text = y.Comment.Text,
                                                CreatedByUser = y.Comment.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                                {
                                                    UserId = y.Comment.CreatedByUser.UserId,
                                                    UserName = y.Comment.CreatedByUser.Username,
                                                    FullNameAr = y.Comment.CreatedByUser.FullNameAr,
                                                    FullNameEn = y.Comment.CreatedByUser.FullNameEn,
                                                    FullNameFn = y.Comment.CreatedByUser.FullNameFn,
                                                    ProfileImage = y.Comment.CreatedByUser.ProfileImage
                                                },
                                            },
                                            CommentId = y.CommentId,
                                            CreatedBy = y.CreatedBy,
                                            CreatedByRole = y.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                            {
                                                CommiteeUsersRoleId = y.CreatedByRole.CommiteeUsersRoleId,
                                                Role = new CommiteeDetailsRoleDTO
                                                {
                                                    CommiteeRoleId = y.CreatedByRole.Role.CommiteeRoleId,
                                                    CommiteeRolesNameAr = y.CreatedByRole.Role.CommiteeRolesNameAr,
                                                    CommiteeRolesNameEn = y.CreatedByRole.Role.CommiteeRolesNameEn
                                                },
                                            },
                                            CreatedByRoleId = y.CreatedByRoleId,
                                            CreatedByUser = y.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                            {
                                                UserId = y.CreatedByUser.UserId,
                                                UserName = y.CreatedByUser.Username,
                                                FullNameAr = y.CreatedByUser.FullNameAr,
                                                FullNameEn = y.CreatedByUser.FullNameEn,
                                                FullNameFn = y.CreatedByUser.FullNameFn,
                                                ProfileImage = y.CreatedByUser.ProfileImage
                                            },
                                            CreatedOn = y.CreatedOn,
                                            SurveyCommentId = y.SurveyCommentId,
                                            SurveyId = y.SurveyId,

                                        }).ToList(),
                                        CommiteeId = x.CommiteeId,
                                        CreatedBy = x.CreatedBy,
                                        CreatedByRole = x.CreatedByRole == null ? new CommiteeDetailsUsersRoleDTO() : new CommiteeDetailsUsersRoleDTO
                                        {
                                            CommiteeUsersRoleId = x.CreatedByRole.CommiteeUsersRoleId,
                                            Role = new CommiteeDetailsRoleDTO
                                            {
                                                CommiteeRoleId = x.CreatedByRole.Role.CommiteeRoleId,
                                                CommiteeRolesNameAr = x.CreatedByRole.Role.CommiteeRolesNameAr,
                                                CommiteeRolesNameEn = x.CreatedByRole.Role.CommiteeRolesNameEn
                                            },
                                        },
                                        CreatedByRoleId = x.CreatedByRoleId,
                                        CreatedByUser = x.CreatedByUser == null ? new UserDetailsDTO() : new UserDetailsDTO
                                        {
                                            UserId = x.CreatedByUser.UserId,
                                            UserName = x.CreatedByUser.Username,
                                            FullNameAr = x.CreatedByUser.FullNameAr,
                                            FullNameEn = x.CreatedByUser.FullNameEn,
                                            FullNameFn = x.CreatedByUser.FullNameFn,
                                            ProfileImage = x.CreatedByUser.ProfileImage
                                        },
                                        CreatedOn = x.CreatedOn,
                                        IsShared = x.IsShared,
                                        MeetingId = x.MeetingId,
                                        MeetingTopicId = x.MeetingTopicId,
                                        Multi = x.Multi,
                                        Subject = x.Subject,
                                        SurveyId = x.SurveyId,
                                        SurveyEndDate = x.SurveyEndDate,
                                        SurveyAnswers = x.SurveyAnswers.Select(y => new SurveyAnswerDTO
                                        {
                                            Answer = y.Answer,
                                            SurveyAnswerId = y.SurveyAnswerId,
                                            SurveyAnswerUsers = y.SurveyAnswerUsers.Select(z => new SurveyAnswerUserDTO
                                            {
                                                SurveyAnswerId = z.SurveyAnswerId,
                                                SurveyAnswerUserId = z.SurveyAnswerUserId,
                                                UserId = z.UserId,
                                                User = new UserDetailsDTO
                                                {
                                                    UserId = z.User.UserId,
                                                    UserName = z.User.Username,
                                                    FullNameAr = z.User.FullNameAr,
                                                    FullNameEn = z.User.FullNameEn,
                                                    FullNameFn = z.User.FullNameFn,
                                                    ProfileImage = z.User.ProfileImage
                                                }
                                            }).ToList(),
                                            SurveyId = y.SurveyId
                                        }).ToList(),
                                        SurveyUsers = x.SurveyUsers.Select(y => new SurveyUserDTO
                                        {
                                            SurveyId = y.SurveyId,
                                            UserId = y.UserId,
                                            SurveyUserId = y.SurveyUserId
                                        }).ToList()
                                    }).ToDataSource(take, skip, false);
                }


            }
            return wall;
        }
        public bool Archive(int committeId)
        {

            try
            {
                var ActiveValidityPeriod = _unitOfWork.GetRepository<ValidityPeriod>().GetAll().Where(c => c.CommiteeId == committeId).FirstOrDefault(c => c.PeriodState == PeriodState.Active || c.PeriodState == PeriodState.Extend);
                var ValidityPeriods = _unitOfWork.GetRepository<ValidityPeriod>().GetAll().Where(c => c.CommiteeId == committeId).ToList();
                var committe = _unitOfWork.GetRepository<Commitee>().GetAll().Where(c => c.CommiteeId == committeId).FirstOrDefault();

                if (ActiveValidityPeriod != null)
                {
                    if (ActiveValidityPeriod.ValidityPeriodTo == default)
                    {

                        ActiveValidityPeriod.ValidityPeriodFrom = (committe.CreatedOn).Value.UtcDateTime;
                        ActiveValidityPeriod.ValidityPeriodTo = DateTime.Now;
                        ActiveValidityPeriod.PeriodState = PeriodState.Archive;
                        _unitOfWork.GetRepository<ValidityPeriod>().Update(ActiveValidityPeriod);
                        _unitOfWork.SaveChanges();
                        ValidityPeriod NewValidityPeriod = new ValidityPeriod
                        {
                            CommiteeId = committeId,
                            PeriodState = PeriodState.Active,
                            ValidityPeriodFrom = (DateTime)DateTime.Now,
                            ValidityPeriodTo = default
                        };
                        _unitOfWork.GetRepository<ValidityPeriod>().Insert(NewValidityPeriod);
                        _unitOfWork.SaveChanges();
                        return true;
                    }

                    if (ActiveValidityPeriod.ValidityPeriodTo >= DateTime.Now)
                    {
                        var oldDateTo = ActiveValidityPeriod.ValidityPeriodTo;
                        ActiveValidityPeriod.ValidityPeriodTo = (DateTime)DateTime.Now;
                        ActiveValidityPeriod.PeriodState = PeriodState.Archive;
                        _unitOfWork.GetRepository<ValidityPeriod>().Update(ActiveValidityPeriod);
                        _unitOfWork.SaveChanges();
                        ValidityPeriod NewValidityPeriod = new ValidityPeriod
                        {
                            CommiteeId = committeId,
                            PeriodState = PeriodState.Active,
                            ValidityPeriodFrom = (DateTime)DateTime.Now,
                            ValidityPeriodTo = (DateTime)oldDateTo
                        };
                        _unitOfWork.GetRepository<ValidityPeriod>().Insert(NewValidityPeriod);
                        _unitOfWork.SaveChanges();
                        return true;
                    }
                    else
                    {
                        ActiveValidityPeriod.PeriodState = PeriodState.Archive;
                        _unitOfWork.GetRepository<ValidityPeriod>().Update(ActiveValidityPeriod);
                        _unitOfWork.SaveChanges();
                        return true;
                    }
                }

                return false;
            }

            catch (Exception ex)
            {

                return false;
            }
        }
        public List<CountResultDTO> CommitteStatistic(int committeId)
        {
            // Get Union Statistic
            List<CountResultDTO> countResultFromUnion = new List<CountResultDTO>();
            var TaskOrg = new Dictionary<string, int>();
            TaskOrg["AttachmentCount"] = 0;
            TaskOrg["SurveyCount"] = 0;
            TaskOrg["TaskCount"] = 0;
            TaskOrg["MeetingCount"] = 0;
            TaskOrg["InboxCount"] = 0;
            TaskOrg["outboxCount"] = 0;
            //TaskOrg["outboxCount"] = 0;
            //AttachmentCount
            var AttachmentCount = _unitOfWork.GetRepository<CommiteeSavedAttachment>().GetAll().OrderByDescending(x => x.CreatedOn)
                         .Where(x => x.CommiteeId == committeId)
                         .Where(x => x.AllUsers || x.AttachmentUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId).ToList();
            //.Where(z => SearchText == "" || SearchText == null || z.Description.Contains(SearchText))

            TaskOrg["AttachmentCount"] += AttachmentCount.Count;
            //surveyCount
            var surveyCount = _unitOfWork.GetRepository<Survey>().GetAll().OrderByDescending(x => x.CreatedOn)
                                 .Where(x => x.CommiteeId == committeId)
                                 .Where(x => x.IsShared || x.SurveyUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId).ToList();
            //.Where(z => SearchText == "" || SearchText == null || z.Subject.Contains(SearchText))


            TaskOrg["SurveyCount"] += surveyCount.Count;

            //TaskCount 
            var taskCount = _unitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                                .Where(x => x.CommiteeId == committeId).ToList();
            //.Where(x => x.IsShared || x.SurveyUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId).ToList();

            TaskOrg["TaskCount"] += taskCount.Count;
            var MeetingCount = _unitOfWork.GetRepository<Meeting>().GetAll().OrderByDescending(x => x.CreatedOn)
                                .Where(x => x.CommitteId == committeId).ToList();
            //.Where(x => x.IsShared || x.SurveyUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId).ToList();

            TaskOrg["MeetingCount"] += MeetingCount.Count;

            //TransactionOutboxCount
            int userId = _sessionServices.UserId.Value;

            var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
            var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
            var userRoleId = userRole.UserRoleId;

            int? fromIncomingOrganizationId = null;//inboxFilterFields?.FromIncomingOrganizationId;
            bool? IsEmployeeFilter = null;// inboxFilterFields?.IsEmployeeFilter;
            int? fromId = null; // inboxFilterFields?.FromId;
            int? Case = null;// inboxFilterFields?.FilterCase;
                             //var roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
                             //var userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
                             //UserRoleId = userRole.UserRoleId;
                             //,{ fromIncomingOrganizationId},{ null},{ null},{ IsEmployeeFilter},{ fromId},{ Case},{ 0},{ CommitteId}


            var outboxCount = this._context.COUNTS.FromSqlInterpolated($"exec sp_GetOutboxForCommittee {1},{10},{true},{0},{userId},{false},{null},{1},{userRoleId},{null},{null},{null},{null},{null},{1},{0},{committeId}").AsEnumerable().FirstOrDefault().CNT;

            TaskOrg["outboxCount"] += (int)outboxCount;
            //TransactionInboxCount



            int organizationId = 0;




            roleId = _unitOfWork.GetRepository<Role>().GetAll().Where(x => x.IsCommitteRole).FirstOrDefault().RoleId;
            userRole = _unitOfWork.GetRepository<UserRole>().GetAll().Where(x => x.RoleId == roleId && x.UserId == _sessionServices.UserId).FirstOrDefault();
            userRoleId = userRole.UserRoleId;

            var countData = _context.COUNTS.FromSqlInterpolated($"exec sp_GetInbox {1},{10},{true},{organizationId},{userId},{false},{null},{1},{userRoleId},{null},{null},{null},{IsEmployeeFilter},{null},{1},{false},{false},{committeId},{null},{null}").AsEnumerable().FirstOrDefault().CNT;


            //var countData = _context.COUNTS.FromSqlInterpolated($"sp_GetInbox {1},{10},{false},{organizationId},{userId},{false},{null},{1},{userRoleId},{null},{null},{null},{IsEmployeeFilter},{null},{1},{0},{true},{null},{null},{null}").AsEnumerable().FirstOrDefault().CNT;

            TaskOrg["InboxCount"] += (int)countData;


            foreach (var itemInDictionary in TaskOrg)
            {
                countResultFromUnion.Add(new CountResultDTO { Name = itemInDictionary.Key, Count = itemInDictionary.Value });
            }
            return countResultFromUnion;


        }
        public bool Extend(int committeId, DateTime dateTo)
        {
            try
            {
                var ActiveValidityPeriod = _unitOfWork.GetRepository<ValidityPeriod>().GetAll().Where(c => c.CommiteeId == committeId).FirstOrDefault(c => c.PeriodState == PeriodState.Active || c.PeriodState == PeriodState.Extend);
                var ValidityPeriods = _unitOfWork.GetRepository<ValidityPeriod>().GetAll().Where(c => c.CommiteeId == committeId).ToList();

                if (ActiveValidityPeriod.ValidityPeriodTo <= dateTo)
                {
                    ActiveValidityPeriod.ValidityPeriodTo = dateTo;
                    ActiveValidityPeriod.PeriodState = PeriodState.Extend;
                    _unitOfWork.GetRepository<ValidityPeriod>().Update(ActiveValidityPeriod);
                    _unitOfWork.SaveChanges();

                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public async Task<List<UserTaskCountDTO>> GetTasksPerUserAsync(int committeId)
        {
            var userTasks = _unitOfWork.GetRepository<CommiteeTask>().GetAll().Where(x => x.CommiteeId == committeId).GroupBy(x => x.MainAssinedUserId, s => new { Completed = s.Completed ? 1 : 0 }).Select(
                x => new UserTaskCountDTO
                {
                    FinishedTaskCount = x.Sum(s => s.Completed),
                    TotalTaskCount = x.Count(),
                    User = _unitOfWork.GetRepository<User>(false).GetAll(false).Select(x => new UserDetailsDTO
                    {
                        UserId = x.UserId,
                        FullNameAr = x.FullNameAr,
                        FullNameEn = x.FullNameEn,
                        Email = x.Email,
                        ProfileImage = x.ProfileImage,
                        ExternalUser = x.ExternalUser
                    }).FirstOrDefault(c => c.UserId == x.Key)
                });
            return await userTasks.ToListAsync();
        }
        public Task<List<AttachemntTypeCountDTO>> GetAttachemntPerType(int committeId)
        {
            var AttachemntType = _unitOfWork.GetRepository<SavedAttachment>().GetAll()
                .Where(x => x.CommiteeSavedAttachments.Any(x => x.CommiteeId == committeId)).GroupBy(x => x.MimeType).Select(
                x => new AttachemntTypeCountDTO
                {
                    Type = x.Key,
                    Count = x.Count(),
                    TotalCount = _unitOfWork.GetRepository<SavedAttachment>(false).GetAll(false)
                    .Where(x => x.CommiteeSavedAttachments.Any(x => x.CommiteeId == committeId)).Count()
                }).ToListAsync();
            return AttachemntType;
        }
        public List<LineChartDTO> GetActiviteyPerMonth(int committeId)
        {
            var TaskCount = (from d in _unitOfWork.GetRepository<CommiteeTask>().GetAll().Where(x => x.CommiteeId == committeId)
                             group d by new { Year = d.CreatedOn.Value.Year, Month = d.CreatedOn.Value.Month } into g
                             select new { Year = g.Key.Year, Month = g.Key.Month, Total = g.Count(), }).AsEnumerable()
             .Select(g => new LineChartDTO
             {
                 name = g.Year + "-" + g.Month,
                 value = g.Total,
             });
            var SurveyCount = (from d in _unitOfWork.GetRepository<Survey>().GetAll().Where(x => x.CommiteeId == committeId)
                               group d by new { Year = d.CreatedOn.Value.Year, Month = d.CreatedOn.Value.Month } into g
                               select new { Year = g.Key.Year, Month = g.Key.Month, Total = g.Count(), }).AsEnumerable()
            .Select(g => new LineChartDTO
            {
                name = g.Year + "-" + g.Month,
                value = g.Total,
            });
            var AttachmentCount = (from d in _unitOfWork.GetRepository<CommiteeSavedAttachment>().GetAll().Where(x => x.CommiteeId == committeId)
                                   group d by new { Year = d.CreatedOn.Value.Year, Month = d.CreatedOn.Value.Month } into g
                                   select new { Year = g.Key.Year, Month = g.Key.Month, Total = g.Count(), }).AsEnumerable()
          .Select(g => new LineChartDTO
          {
              name = g.Year + "-" + g.Month,
              value = g.Total,
          });
            return TaskCount.Union(SurveyCount).Union(AttachmentCount).GroupBy(x => x.name)
                  .Select(x => new LineChartDTO
                  {
                      name = x.Key,
                      value = x.Sum(x => x.value)
                  })
                  .ToList();
        }
        public List<CommiteeUsersRoleDTO> GetCommitteeRoles(int committeId, int userId)
        {
            #region get User Permissions
            int userID = userId == 0 ? int.Parse(_sessionServices.UserId.ToString()) : userId;
            IQueryable<CommiteeUsersPermission> UserPermission = _unitOfWork.GetRepository<CommiteeUsersPermission>().GetAll().Where(x => x.UserId == userID && x.CommiteeId == committeId);

            var Customeroles = _unitOfWork.GetRepository<CommiteeUsersRole>()
                               .GetAll()
                               .Where(x => UserPermission.Select(y => y.RoleId).Contains(x.RoleId))
                               .Where(x => x.Role.RolePermissions.Any(rp => UserPermission.Select(up => up.PermissionId).Contains(rp.PermissionId)))
                               .Include(x => x.Role)
                               .ThenInclude(x => x.RolePermissions)
                               .ThenInclude(x => x.Permission)
                               .Where(x => x.CommiteeId == committeId && x.UserId == userID);

            foreach (var item in Customeroles)
            {
                foreach (var item1 in item.Role.RolePermissions)
                {
                    item1.Permission.Enabled = UserPermission.FirstOrDefault(x => x.PermissionId == item1.PermissionId).Enabled;
                }
            }
            #endregion

            var roles = _unitOfWork.GetRepository<CommiteeUsersRole>().GetAll().Include(x => x.Role).ThenInclude(x => x.RolePermissions).ThenInclude(x => x.Permission).Where(x => x.CommiteeId == committeId && x.Enabled && x.UserId == userID);
            List<CommiteeUsersRoleDTO> resultRoles = roles.Select(x => new CommiteeUsersRoleDTO
            {
                CommiteeId = x.CommiteeId,
                CommiteeUsersRoleId = x.CommiteeUsersRoleId,
                Delegated = x.Delegated,
                Enabled = x.Enabled,
                EnableUntil = x.EnableUntil,
                Notes = x.Notes,
                Role = new CommiteeRoleDTO
                {
                    CommiteeRoleId = x.Role.CommiteeRoleId,
                    CommiteeRolesNameAr = x.Role.CommiteeRolesNameAr,
                    CommiteeRolesNameEn = x.Role.CommiteeRolesNameEn,
                    IsMangerRole = x.Role.IsMangerRole,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    RolePermissions = x.Role.RolePermissions.Where(x => !UserPermission.Select(x => x.PermissionId).Contains(x.PermissionId)).Select(z => new CommiteeRolePermissionDTO
                    {
                        CommiteeRolePermissionId = z.CommiteeRolePermissionId,
                        CreatedBy = z.CreatedBy,
                        CreatedOn = z.CreatedOn,
                        PermissionId = z.PermissionId,
                        RoleId = z.RoleId,

                        Permission = new CommitePermissionDTO
                        {
                            CommitePermissionId = z.Permission.CommitePermissionId,
                            CommitePermissionNameAr = z.Permission.CommitePermissionNameAr,
                            CommitePermissionNameEn = z.Permission.CommitePermissionNameEn,
                            CommitePermissionNameFn = z.Permission.CommitePermissionNameFn,
                            Enabled = z.Permission.Enabled,
                            Method = z.Permission.Method,
                            PermissionCode = z.Permission.PermissionCode,
                            IsDeleted = z.Permission.IsDeleted,
                        }
                    }
                    ).ToList(),
                },
                RoleId = x.RoleId,
                RoleNameAR = x.Role.CommiteeRolesNameAr,
                RoleNameEn = x.Role.CommiteeRolesNameAr,
                UserId = x.UserId

            }).ToList();
            var CustomerolesDto = mapper.Map<List<CommiteeUsersRoleDTO>>(Customeroles);
            var FinalResult = resultRoles.Concat(CustomerolesDto);

            return FinalResult.ToList();
        }
        public List<CommiteeUsersRoleDTO> GetCommitteeRolesNew(int committeId, int userId)
        {
            int userID = userId == 0 ? int.Parse(_sessionServices.UserId.ToString()) : userId;
            IQueryable<CommiteeUsersPermission> UserPermission = _unitOfWork.GetRepository<CommiteeUsersPermission>().GetAll().Where(x => x.UserId == userID && x.CommiteeId == committeId);


            var roles = _unitOfWork.GetRepository<CommiteeUsersRole>()
                                    .GetAll()
                                    .Include(x => x.Role)
                                    .ThenInclude(x => x.RolePermissions)
                                    .ThenInclude(x => x.Permission)
                                    .Where(x => x.CommiteeId == committeId && x.Enabled && x.UserId == userID);
            List<CommiteeUsersRoleDTO> CommiteeUsersRoleResult = roles.Select(x => new CommiteeUsersRoleDTO
            {
                CommiteeId = x.CommiteeId,
                CommiteeUsersRoleId = x.CommiteeUsersRoleId,
                Delegated = x.Delegated,
                Enabled = x.Enabled,
                EnableUntil = x.EnableUntil,
                Notes = x.Notes,
                Role = new CommiteeRoleDTO
                {
                    CommiteeRoleId = x.Role.CommiteeRoleId,
                    CommiteeRolesNameAr = x.Role.CommiteeRolesNameAr,
                    CommiteeRolesNameEn = x.Role.CommiteeRolesNameEn,
                    IsMangerRole = x.Role.IsMangerRole,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    RolePermissions = x.Role.RolePermissions.Where(x => !UserPermission.Select(x => x.PermissionId).Contains(x.PermissionId)).Select(z => new CommiteeRolePermissionDTO
                    {
                        CommiteeRolePermissionId = z.CommiteeRolePermissionId,
                        CreatedBy = z.CreatedBy,
                        CreatedOn = z.CreatedOn,
                        PermissionId = z.PermissionId,
                        RoleId = z.RoleId,
                        Permission = new CommitePermissionDTO
                        {
                            CommitePermissionId = z.Permission.CommitePermissionId,
                            CommitePermissionNameAr = z.Permission.CommitePermissionNameAr,
                            CommitePermissionNameEn = z.Permission.CommitePermissionNameEn,
                            CommitePermissionNameFn = z.Permission.CommitePermissionNameFn,
                            Enabled = z.Permission.Enabled,
                            Method = z.Permission.Method,
                            PermissionCode = z.Permission.PermissionCode,
                            IsDeleted = z.Permission.IsDeleted,
                        }
                    }
                    ).ToList(),
                },
                RoleId = x.RoleId,
                RoleNameAR = x.Role.CommiteeRolesNameAr,
                RoleNameEn = x.Role.CommiteeRolesNameAr,
                UserId = x.UserId
            }).ToList();
            List<CommitePermissionDTO> Permissions = new();
            for (int i = 0; i < CommiteeUsersRoleResult.Count(); i++)
            {
                foreach (var item in UserPermission)
                {
                    if (!item.Enabled)
                    {
                        CommiteeUsersRoleResult = CommiteeUsersRoleResult.Where(x => x.RoleId == item.RoleId &&
                                                                                x.CommiteeId == item.CommiteeId &&
                                                                                !x.Role.RolePermissions
                                                                                .Select(x => x.PermissionId)
                                                                                .Contains(item.PermissionId)).ToList();

                    }
                    else
                    {
                        var permission = _unitOfWork.GetRepository<CommitePermission>().GetAll().FirstOrDefault(x => x.CommitePermissionId == item.PermissionId);
                        if (CommiteeUsersRoleResult[0].RoleId == item.RoleId && CommiteeUsersRoleResult[0].CommiteeId == item.CommiteeId)
                        {
                            CommiteeUsersRoleResult[0].Role.RolePermissions.Add(new CommiteeRolePermissionDTO
                            {
                                Permission = new CommitePermissionDTO
                                {
                                    CommitePermissionId = permission.CommitePermissionId,
                                    CommitePermissionNameAr = permission.CommitePermissionNameAr,
                                    CommitePermissionNameEn = permission.CommitePermissionNameEn,
                                    CommitePermissionNameFn = permission.CommitePermissionNameFn,
                                    Enabled = item.Enabled,
                                    Method = permission.Method,
                                    PermissionCode = permission.PermissionCode,
                                    IsDeleted = permission.IsDeleted,
                                },
                            });
                        }
                    }
                }
            }

            return CommiteeUsersRoleResult.ToList();
        }




        public bool Disactive(int committeId)
        {
            try
            {
                var committe = _unitOfWork.GetRepository<Commitee>().GetById(committeId);
                if (committe.CurrentStatusId == 1)
                {
                    committe.CurrentStatusId = 2;
                }
                else
                {
                    committe.CurrentStatusId = 1;
                }
                _unitOfWork.GetRepository<Commitee>().Update(committe);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
        public List<CommiteeDTO> GetCommittesTree()
        {
            if (helperFunction.get_Permission(_sessionServices.UserRoleIdOrignal, "ForAllCommitte"))
            {
                IQueryable queryAll = this._UnitOfWork.GetRepository<Commitee>().GetAll().OrderByDescending(x => x.CreatedOn);

                return queryAll.ProjectTo<CommiteeDTO>(_Mapper.ConfigurationProvider).ToList();
            }
            IQueryable query = this._UnitOfWork.GetRepository<Commitee>().GetAll().OrderByDescending(x => x.CreatedOn).Where(x => x.Members.Any(c => c.UserId == _sessionServices.UserId && c.Active));
            return query.ProjectTo<CommiteeDTO>(_Mapper.ConfigurationProvider).ToList();
        }

        public List<MeetingSummaryDTO> GetMeetingsByCommitteId(int committeId)
        {
            return _unitOfWork.GetRepository<Meeting>().GetAll().Where(x => x.CommitteId == committeId)
                .Select(x => new MeetingSummaryDTO
                {
                    Date = x.Date,
                    Id = x.Id,
                    MeetingFromTime = x.MeetingFromTime,
                    MeetingToTime = x.MeetingToTime,
                    PhysicalLocation = x.PhysicalLocation,
                    Subject = x.Subject,
                    Title = x.Title
                }).ToList();
        }


        //private static UserDetailsDTO GetUserdata(int userid)
        //{
        //    return;
        //    //  return new UserDetailsDTO
        //    //{
        //    //    UserId = user.UserId,
        //    //    FullNameAr = user.FullNameAr,
        //    //    FullNameEn = user.FullNameEn,
        //    //    Email = user.Email,
        //    //    ProfileImage = user.ProfileImage,
        //    //    ExternalUser = user.ExternalUser
        //    //};
        //}
    }
}
