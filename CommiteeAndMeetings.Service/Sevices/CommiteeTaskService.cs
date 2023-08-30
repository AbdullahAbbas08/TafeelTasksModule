using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Castle.Core.Internal;
using ClosedXML.Excel;
using CommiteeAndMeetings.BLL;
using CommiteeAndMeetings.BLL.BaseObjects.Repositories;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.BLL.Hosting;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Enums;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DotLiquid;
using HelperServices;
using HelperServices.LinqHelpers;
using IHelperServices;
using IHelperServices.Models;
using Laserfiche.RepositoryAccess.Activity;
using LinqHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Models;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Mail;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using static iTextSharp.text.pdf.AcroFields;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class CommiteeTaskService : BusinessService<CommiteeTask, CommiteeTaskDTO>, ICommiteeTaskService
    {
        IUnitOfWork _unitOfWork;
        IHelperServices.ISessionServices _sessionServices;
        ICommitteeNotificationService _committeeNotificationService;
        ICommiteeLocalizationService _commiteeLocalizationService;
        private readonly ICommitteeMeetingSystemSettingService _systemSettingsService;
        IUsersService _usersService;
        IMailServices mailServices;
        ISmsServices smsServices;
        public readonly MasarContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CommiteeTaskService(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment, IMapper mapper, IStringLocalizer stringLocalizer, ISmsServices _smsServices, ICommitteeMeetingSystemSettingService systemSettingsService, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings, IMailServices _mailServices, IUsersService usersService, ICommitteeNotificationService committeeNotificationService, ICommiteeLocalizationService commiteeLocalizationService)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _unitOfWork = unitOfWork;
            _committeeNotificationService = committeeNotificationService;
            _commiteeLocalizationService = commiteeLocalizationService;
            _sessionServices = sessionServices;
            mailServices = _mailServices;
            smsServices = _smsServices;
            _usersService = usersService;
            _systemSettingsService = systemSettingsService;
            _context = new MasarContext();
            _hostingEnvironment = hostingEnvironment;
        }
        public override DataSourceResult<CommiteeTaskDTO> GetAll<CommiteeTaskDTO>(DataSourceRequest dataSourceRequest, bool WithTracking = true)
        {
            IQueryable query = this._UnitOfWork.GetRepository<CommiteeTask>().GetAll(WithTracking).OrderByDescending(x => x.CreatedOn).Where(x => x.IsShared || x.MainAssinedUserId == _sessionServices.UserId || x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId);

            var res = query.ProjectTo<CommiteeTaskDTO>(_Mapper.ConfigurationProvider).ToDataSourceResult(dataSourceRequest);
            res.Data = res.Data.ToList();
            return res;
        }
        // Get Details by CommiteeTaskId
        public CommiteeTaskDTO GetDetailsById(int CommiteeTaskId)
        {
            var query = this._UnitOfWork.GetRepository<CommiteeTask>().GetAll().Where(x => x.CommiteeTaskId == CommiteeTaskId);
            var dta = _Mapper.Map<List<CommiteeTaskDTO>>(query);
            //var dta = query.ProjectTo<CommiteeTaskDTO>(_Mapper.ConfigurationProvider);
            foreach (var item in dta)
            {

                foreach (var itemMission in item.MultiMission)
                {
                    itemMission.CommiteeTaskMultiMissionUserDTOs = _unitOfWork.GetRepository<CommiteeTaskMultiMissionUser>().GetAll().Where(x => x.CommiteeTaskMultiMissionId == itemMission.CommiteeTaskMultiMissionId)
                        .Select(x => new CommiteeTaskMultiMissionUserDTO
                        {
                            // CommiteeTaskMultiMissionId = x.CommiteeTaskMultiMissionId,
                            CommiteeTaskMultiMissionUserId = x.CommiteeTaskMultiMissionUserId,
                            UserDetailsDto = new UserDetailsDTO()
                            {
                                UserId = x.UserId,
                                UserName = x.User.Username,
                                FullNameAr = x.User.FullNameAr,
                                FullNameEn = x.User.FullNameEn,
                                ProfileImage = x.User.ProfileImage
                            },
                            UserId = x.UserId
                        }).ToList();


                }
            }

            return dta.FirstOrDefault();
            //var committeTask = this._UnitOfWork.GetRepository<CommiteeTask>().GetAll().Where(x => x.CommiteeTaskId == CommiteeTaskId).FirstOrDefault();
            //var res= committeTask.ProjectTo<CommiteeTaskDTO>(_Mapper.ConfigurationProvider).FirstOrDefault();
            //return committeTask;

        }
        public DataSourceResult<CommiteeTaskDTO> GetAllwithfilters(DataSourceRequest dataSourceRequest, TaskFilterEnum requiredTasks, ParamsSearchFilterDTO paramsSearchFilterDTO = null, int? userId = null, int? organizationId = null, bool WithTracking = true)
        {
            bool filter = Convert.ToBoolean(dataSourceRequest.Filter.Filters?.Any(z => z.Field == "commiteeId"));
            if (organizationId != null)
            {
                var returnFromStoredEnumerable = _context.vm_EmpInOrgaHierarchies.FromSqlInterpolated($"exec [dbo].[sp_GetEmployeesByOrganizationHierarchyToReport] {organizationId}").ToList();
                // var res = returnFromStoredEnumerable.AsEnumerable().ToDataSourceResult(dataSourceRequest, true).Data.ToList();

                //var x = returnFromStoredEnumerable.AsQueryable();
                List<LookUpDTO> lookupUser = returnFromStoredEnumerable.AsQueryable().Select(x => new LookUpDTO
                {
                    Id = x.Id,
                    Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
                }).ToList();

                CommiteeTaskDTOAndCount commiteeTaskDTOAndCount = new CommiteeTaskDTOAndCount()
                {

                    tasks = new List<CommiteeTaskDTO>(),
                    CountTasks = 0
                };

                foreach (var item in lookupUser)
                {
                    IQueryable<CommiteeTask> query = this._UnitOfWork.GetRepository<CommiteeTask>().GetAll(WithTracking).OrderByDescending(x => x.CreatedOn)
                    .Where(x => (paramsSearchFilterDTO.ValiditayPeriodFrom == null || x.CreatedOn >= paramsSearchFilterDTO.ValiditayPeriodFrom.Value) &&
                                (paramsSearchFilterDTO.ValidatiyPeriodTo == null || x.CreatedOn <= paramsSearchFilterDTO.ValidatiyPeriodTo.Value) &&
                                (x.MeetingId == paramsSearchFilterDTO.MeetingId || paramsSearchFilterDTO.MeetingId == null) &&
                                (x.CommiteeId == paramsSearchFilterDTO.CommiteeId || paramsSearchFilterDTO.CommiteeId == null))
                   .Where(x =>
               ((x.IsShared && filter) || x.MainAssinedUserId == item.Id || x.AssistantUsers.Any(z => z.UserId == item.Id)
               || x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == item.Id))
               || x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == item.Id)) ||
                 // ((paramsSearchFilterDTO.ValiditayPeriodFrom == null || x.CreatedOn >= paramsSearchFilterDTO.ValiditayPeriodFrom) && (paramsSearchFilterDTO.ValidatiyPeriodTo == null || x.CreatedOn <= paramsSearchFilterDTO.ValidatiyPeriodTo)) ||
                 x.CreatedBy == item.Id));
                    switch (requiredTasks)
                    {
                        case TaskFilterEnum.All:
                            break;
                        case TaskFilterEnum.late:
                            query = query.Where(x => x.EndDate <= DateTimeOffset.Now && x.MainAssinedUserId == item.Id && !x.Completed);
                            break;
                        case TaskFilterEnum.closedLate:
                            query = query.Where(x => x.MainAssinedUserId == item.Id && x.Completed && x.CompleteDate >= x.EndDate);
                            break;
                        case TaskFilterEnum.sentlate:
                            query = query.Where(x => x.EndDate < DateTimeOffset.Now && x.CreatedBy == item.Id && !x.Completed);
                            break;
                        case TaskFilterEnum.sent:
                            query = query.Where(x => x.CreatedBy == item.Id && !x.Completed);
                            break;
                        case TaskFilterEnum.closed:
                            query = query.Where(x => x.Completed && x.MainAssinedUserId == item.Id && x.CompleteDate < x.EndDate);
                            break;
                        case TaskFilterEnum.toBeImplemented:
                            query = query.Where(x => !x.Completed && x.MainAssinedUser.UserId == item.Id);
                            break;
                        case TaskFilterEnum.Helpers:
                            query = query.Where(x => x.AssistantUsers.Any(z => z.UserId == item.Id) || x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == item.Id)));

                            break;
                        case TaskFilterEnum.AllClosed:
                            query = query.Where(x => x.Completed && x.MainAssinedUserId == item.Id);
                            break;
                        case TaskFilterEnum.TasksToView:
                            query = query.Where(x => x.MainAssinedUserId != item.Id && x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == item.Id))
                              && x.CreatedBy != item.Id);
                            break;
                        default:
                            break;
                    }
                    if (paramsSearchFilterDTO.FromDate.HasValue)
                        query = query.Where(x => x.EndDate >= paramsSearchFilterDTO.FromDate.Value);
                    if (paramsSearchFilterDTO.ToDate.HasValue)
                        query = query.Where(x => x.EndDate <= paramsSearchFilterDTO.ToDate.Value);
                    if (paramsSearchFilterDTO.MainUserId.HasValue)
                        query = query.Where(x => x.MainAssinedUserId == paramsSearchFilterDTO.MainUserId.Value);
                    if (paramsSearchFilterDTO.MainAssinedUserId.HasValue)
                        //query = query.Where(x => x.AssistantUsers.Any(z => z.UserId == paramsSearchFilterDTO.MainAssinedUserId.Value));
                        query = query.Where(x => x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(z => z.UserId == paramsSearchFilterDTO.MainAssinedUserId.Value)));


                    if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field == "commiteeId")))
                    {
                        UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId =
                            _sessionServices.UserIdAndRoleIdAfterDecrypt((dataSourceRequest.Filter.Filters.Where(z => z.Field == "commiteeId").FirstOrDefault()?.Value), false);

                        var commiteeId = UserIdAndUserRoleId.Id;
                        query = query.Where(x => x.CommiteeId == commiteeId);
                    }
                    if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field.ToLower() == "comiteetaskcategoryid")))
                    {
                        var comiteetaskcategoryid = Convert.ToInt32((dataSourceRequest.Filter.Filters.Where(z => z.Field.ToLower() == "comiteetaskcategoryid").FirstOrDefault()?.Value));
                        query = query.Where(x => x.ComiteeTaskCategoryId == comiteetaskcategoryid);
                    }
                    if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field.ToLower() == "title")))
                    {
                        var title = (dataSourceRequest.Filter.Filters.Where(z => z.Field.ToLower() == "title").FirstOrDefault()?.Value).ToString();
                        query = query.Where(x => x.Title.Contains(title));
                    }

                    var dta = _Mapper.Map<List<CommiteeTaskDTO>>(query).ToList();

                    foreach (var itemtask in dta)
                    {
                        foreach (var itemMission in itemtask.MultiMission)
                        {
                            itemMission.CommiteeTaskMultiMissionUserDTOs = _unitOfWork.GetRepository<CommiteeTaskMultiMissionUser>().GetAll().Where(x => x.CommiteeTaskMultiMissionId == itemMission.CommiteeTaskMultiMissionId)
                                .Select(x => new CommiteeTaskMultiMissionUserDTO
                                {
                                    CommiteeTaskMultiMissionId = x.CommiteeTaskMultiMissionId,
                                    CommiteeTaskMultiMissionUserId = x.CommiteeTaskMultiMissionUserId,
                                    UserDetailsDto = new UserDetailsDTO()
                                    {
                                        UserId = x.UserId,
                                        UserName = x.User.Username,
                                        FullNameAr = x.User.FullNameAr,
                                        FullNameEn = x.User.FullNameEn,
                                        ProfileImage = x.User.ProfileImage
                                    },
                                    UserId = x.UserId
                                }).ToList();


                        }
                        itemtask.MainAssinedUser = _unitOfWork.GetRepository<User>().GetAll().Where(x => x.UserId == itemtask.MainAssinedUser.UserId)
                                .Select(x => new UserDetailsDTO()
                                {
                                    UserId = x.UserId,
                                    UserName = x.Username,
                                    FullNameAr = x.FullNameAr,
                                    FullNameEn = x.FullNameEn,
                                    ProfileImage = x.ProfileImage
                                }).FirstOrDefault();
                    }


                    commiteeTaskDTOAndCount.tasks.AddRange(dta);
                    commiteeTaskDTOAndCount.CountTasks += query.Count();

                }
                // return
                DataSourceResult<CommiteeTaskDTO> returned = new DataSourceResult<CommiteeTaskDTO>
                {
                    Count = commiteeTaskDTOAndCount.CountTasks,
                    Data = commiteeTaskDTOAndCount.tasks.Skip(dataSourceRequest.Skip).Take(dataSourceRequest.Take)

                };
                return returned;

            }
            if (userId == null)
            {
                // Get All task in Committe With All CommittePermission



                if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field == "commiteeId")))
                {

                    UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId =
                        _sessionServices.UserIdAndRoleIdAfterDecrypt((dataSourceRequest.Filter.Filters.Where(z => z.Field == "commiteeId").FirstOrDefault()?.Value), false);

                    var commiteeId = UserIdAndUserRoleId.Id;

                    // check if userId is head of unit or member in committe
                    var committeitem = _unitOfWork.GetRepository<Commitee>().GetAll().Where(x => x.CommiteeId == commiteeId).FirstOrDefault();
                    var committeMember = _unitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == commiteeId).ToList();
                    var headOfUnitId = committeitem.CurrenHeadUnitId;

                    if (headOfUnitId != _sessionServices.UserId && committeMember.All(x => x.UserId != _sessionServices.UserId))
                    {
                        IQueryable<CommiteeTask> queryCommitee = this._UnitOfWork.GetRepository<CommiteeTask>().GetAll(WithTracking).OrderByDescending(x => x.CreatedOn)
                   .Where(x => (paramsSearchFilterDTO.ValiditayPeriodFrom == null || x.CreatedOn >= paramsSearchFilterDTO.ValiditayPeriodFrom.Value) &&
                         (paramsSearchFilterDTO.ValidatiyPeriodTo == null || x.CreatedOn <= paramsSearchFilterDTO.ValidatiyPeriodTo.Value) &&
                              (x.MeetingId == paramsSearchFilterDTO.MeetingId || paramsSearchFilterDTO.MeetingId == null) &&
                                (x.CommiteeId == paramsSearchFilterDTO.CommiteeId || paramsSearchFilterDTO.CommiteeId == null));
                        //((x.IsShared && filter) || x.MainAssinedUserId == _sessionServices.UserId || x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId)
                        //|| x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == _sessionServices.UserId))
                        //|| x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == _sessionServices.UserId)) ||
                        //  // ((paramsSearchFilterDTO.ValiditayPeriodFrom == null || x.CreatedOn >= paramsSearchFilterDTO.ValiditayPeriodFrom) && (paramsSearchFilterDTO.ValidatiyPeriodTo == null || x.CreatedOn <= paramsSearchFilterDTO.ValidatiyPeriodTo)) ||
                        //  x.CreatedBy == _sessionServices.UserId));
                        switch (requiredTasks)
                        {
                            case TaskFilterEnum.All:
                                break;
                            case TaskFilterEnum.late:
                                queryCommitee = queryCommitee.Where(x => x.EndDate <= DateTimeOffset.Now && !x.Completed);
                                break;
                            case TaskFilterEnum.closedLate:
                                queryCommitee = queryCommitee.Where(x => x.Completed && x.CompleteDate >= x.EndDate);
                                break;
                            case TaskFilterEnum.sentlate:
                                queryCommitee = queryCommitee.Where(x => x.EndDate < DateTimeOffset.Now && !x.Completed);
                                break;
                            case TaskFilterEnum.sent:
                                queryCommitee = queryCommitee.Where(x => !x.Completed);
                                break;
                            case TaskFilterEnum.closed:
                                queryCommitee = queryCommitee.Where(x => x.Completed && x.CompleteDate < x.EndDate);
                                break;
                            case TaskFilterEnum.toBeImplemented:
                                queryCommitee = queryCommitee.Where(x => !x.Completed);
                                break;
                            case TaskFilterEnum.Helpers:
                                //queryCommitee = queryCommitee.Where();
                                break;
                            case TaskFilterEnum.AllClosed:
                                queryCommitee = queryCommitee.Where(x => x.Completed);
                                break;
                            case TaskFilterEnum.TasksToView:
                                //queryCommitee = queryCommitee.Where(x => x.MainAssinedUserId != _sessionServices.UserId
                                //&& x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == _sessionServices.UserId)) && x.CreatedBy != _sessionServices.UserId);
                                break;
                            default:
                                break;
                        }
                        if (paramsSearchFilterDTO.FromDate.HasValue)
                            queryCommitee = queryCommitee.Where(x => x.EndDate >= paramsSearchFilterDTO.FromDate.Value);
                        if (paramsSearchFilterDTO.ToDate.HasValue)
                            queryCommitee = queryCommitee.Where(x => x.EndDate <= paramsSearchFilterDTO.ToDate.Value);
                        if (paramsSearchFilterDTO.MainUserId.HasValue)
                            queryCommitee = queryCommitee.Where(x => x.MainAssinedUserId == paramsSearchFilterDTO.MainUserId.Value);
                        if (paramsSearchFilterDTO.MainAssinedUserId.HasValue)
                            //query = query.Where(x => x.AssistantUsers.Any(z => z.UserId == paramsSearchFilterDTO.MainAssinedUserId.Value));
                            queryCommitee = queryCommitee.Where(x => x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(z => z.UserId == paramsSearchFilterDTO.MainAssinedUserId.Value)));


                        //if (paramsSearchFilterDTO.ValidatiyPeriodTo.HasValue)
                        //    query = query.Where(x => x.CreatedOn <= paramsSearchFilterDTO.ValidatiyPeriodTo.Value);
                        //if (paramsSearchFilterDTO.ValiditayPeriodFrom.HasValue)
                        //    query = query.Where(x => x.AssistantUsers.Any(z => z.CreatedOn  >= paramsSearchFilterDTO.ValiditayPeriodFrom.Value));

                        //if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field == "commiteeId")))
                        //{
                        //    UserIdAndRoleIdAfterDecryptDTO UserIdAndCommitteRoleId =
                        //        _sessionServices.UserIdAndRoleIdAfterDecrypt((dataSourceRequest.Filter.Filters.Where(z => z.Field == "commiteeId").FirstOrDefault()?.Value), false);

                        //    var commiteId = UserIdAndCommitteRoleId.Id;
                        //    queryCommitee = queryCommitee.Where(x => x.CommiteeId == commiteId);
                        //}
                        if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field.ToLower() == "comiteetaskcategoryid")))
                        {
                            var comiteetaskcategoryid = Convert.ToInt32((dataSourceRequest.Filter.Filters.Where(z => z.Field.ToLower() == "comiteetaskcategoryid").FirstOrDefault()?.Value));
                            queryCommitee = queryCommitee.Where(x => x.ComiteeTaskCategoryId == comiteetaskcategoryid);
                        }
                        if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field.ToLower() == "title")))
                        {
                            var title = (dataSourceRequest.Filter.Filters.Where(z => z.Field.ToLower() == "title").FirstOrDefault()?.Value).ToString();
                            queryCommitee = queryCommitee.Where(x => x.Title.Contains(title));
                        }

                        //if (requiredTasks == TaskFilterEnum.late)
                        //{
                        //    dta.ForEach(x => x.Islated = true);
                        //}


                        //if (query.Where(x => x.MainAssinedUserId != _sessionServices.UserId && x.AssistantUsers.Any(z => z.UserId != _sessionServices.UserId)).Any())
                        //{
                        //    var listQuery = query.ToList();

                        //    for (int i = 0; i < listQuery.Count(); i++)
                        //    {
                        //        //.Where(x => x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == _sessionServices.UserId))).Any() ? true : false;
                        //        listQuery[i].TaskToView = listQuery[i].TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == _sessionServices.UserId)) ? true : false;
                        //    }
                        //}



                        var dtaCommitte = _Mapper.Map<List<CommiteeTaskDTO>>(queryCommitee.Skip(dataSourceRequest.Skip).Take(dataSourceRequest.Take).ToList());

                        foreach (var itemtask in dtaCommitte)
                        {
                            foreach (var itemMission in itemtask.MultiMission)
                            {
                                itemMission.CommiteeTaskMultiMissionUserDTOs = _unitOfWork.GetRepository<CommiteeTaskMultiMissionUser>().GetAll().Where(x => x.CommiteeTaskMultiMissionId == itemMission.CommiteeTaskMultiMissionId)
                                    .Select(x => new CommiteeTaskMultiMissionUserDTO
                                    {
                                        CommiteeTaskMultiMissionId = x.CommiteeTaskMultiMissionId,
                                        CommiteeTaskMultiMissionUserId = x.CommiteeTaskMultiMissionUserId,

                                        UserDetailsDto = new UserDetailsDTO()
                                        {
                                            UserId = x.UserId,
                                            UserName = x.User.Username,
                                            FullNameAr = x.User.FullNameAr,
                                            FullNameEn = x.User.FullNameEn,
                                            ProfileImage = x.User.ProfileImage
                                        },
                                        UserId = x.UserId,
                                        FullNameAr = x.User.FullNameAr,
                                        FullNameEn = x.User.FullNameEn,
                                    }).ToList();


                            }
                            itemtask.MainAssinedUser = _unitOfWork.GetRepository<User>().GetAll().Where(x => x.UserId == itemtask.MainAssinedUser.UserId)
                                    .Select(x => new UserDetailsDTO()
                                    {
                                        UserId = x.UserId,
                                        UserName = x.Username,
                                        FullNameAr = x.FullNameAr,
                                        FullNameEn = x.FullNameEn,
                                        ProfileImage = x.ProfileImage
                                    }).FirstOrDefault();
                        }


                        DataSourceResult<CommiteeTaskDTO> returnedCommitee = new DataSourceResult<CommiteeTaskDTO>
                        {
                            Count = queryCommitee.Count(),
                            Data = dtaCommitte

                        };
                        return returnedCommitee;

                    }
                }

                IQueryable<CommiteeTask> query = this._UnitOfWork.GetRepository<CommiteeTask>().GetAll(WithTracking).Include(x => x.Meeting).OrderByDescending(x => x.CreatedOn)
                     .Where(x => (paramsSearchFilterDTO.ValiditayPeriodFrom == null || x.CreatedOn >= paramsSearchFilterDTO.ValiditayPeriodFrom.Value) &&
                            (paramsSearchFilterDTO.ValidatiyPeriodTo == null || x.CreatedOn <= paramsSearchFilterDTO.ValidatiyPeriodTo.Value) &&
                                 (x.MeetingId == paramsSearchFilterDTO.MeetingId || paramsSearchFilterDTO.MeetingId == null) &&
                                (x.CommiteeId == paramsSearchFilterDTO.CommiteeId || paramsSearchFilterDTO.CommiteeId == null))
                    .Where(x =>
                ((x.IsShared && filter) || x.MainAssinedUserId == _sessionServices.UserId || x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId)
                || x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == _sessionServices.UserId))
                || x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == _sessionServices.UserId)) ||
                  // ((paramsSearchFilterDTO.ValiditayPeriodFrom == null || x.CreatedOn >= paramsSearchFilterDTO.ValiditayPeriodFrom) && (paramsSearchFilterDTO.ValidatiyPeriodTo == null || x.CreatedOn <= paramsSearchFilterDTO.ValidatiyPeriodTo)) ||
                  x.CreatedBy == _sessionServices.UserId));
                switch (requiredTasks)
                {
                    case TaskFilterEnum.All:
                        break;
                    case TaskFilterEnum.late:
                        query = query.Where(x => x.EndDate <= DateTimeOffset.Now && x.MainAssinedUserId == _sessionServices.UserId && !x.Completed);
                        break;
                    case TaskFilterEnum.closedLate:
                        query = query.Where(x => x.MainAssinedUserId == _sessionServices.UserId && x.Completed && x.CompleteDate >= x.EndDate);
                        break;
                    case TaskFilterEnum.sentlate:
                        query = query.Where(x => x.EndDate < DateTimeOffset.Now && x.CreatedBy == _sessionServices.UserId && !x.Completed);
                        break;
                    case TaskFilterEnum.sent:
                        query = query.Where(x => x.CreatedBy == _sessionServices.UserId && !x.Completed);
                        break;
                    case TaskFilterEnum.closed:
                        query = query.Where(x => x.Completed && x.MainAssinedUserId == _sessionServices.UserId && x.CompleteDate < x.EndDate);
                        break;
                    case TaskFilterEnum.toBeImplemented:
                        query = query.Where(x => !x.Completed && x.MainAssinedUser.UserId == _sessionServices.UserId);
                        break;
                    case TaskFilterEnum.Helpers:
                        query = query.Where(x => x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId) || x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == _sessionServices.UserId)));
                        break;
                    case TaskFilterEnum.AllClosed:
                        query = query.Where(x => x.Completed && x.MainAssinedUserId == _sessionServices.UserId);
                        break;
                    case TaskFilterEnum.TasksToView:
                        query = query.Where(x => x.MainAssinedUserId != _sessionServices.UserId
                        && x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == _sessionServices.UserId)) && x.CreatedBy != _sessionServices.UserId);
                        break;
                    default:
                        break;
                }
                if (paramsSearchFilterDTO.FromDate.HasValue)
                    query = query.Where(x => x.EndDate >= paramsSearchFilterDTO.FromDate.Value);
                if (paramsSearchFilterDTO.ToDate.HasValue)
                    query = query.Where(x => x.EndDate <= paramsSearchFilterDTO.ToDate.Value);
                if (paramsSearchFilterDTO.MainUserId.HasValue)
                    query = query.Where(x => x.MainAssinedUserId == paramsSearchFilterDTO.MainUserId.Value);
                if (paramsSearchFilterDTO.MainAssinedUserId.HasValue)
                    //query = query.Where(x => x.AssistantUsers.Any(z => z.UserId == paramsSearchFilterDTO.MainAssinedUserId.Value));
                    query = query.Where(x => x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(z => z.UserId == paramsSearchFilterDTO.MainAssinedUserId.Value)));


                //if (paramsSearchFilterDTO.ValidatiyPeriodTo.HasValue)
                //    query = query.Where(x => x.CreatedOn <= paramsSearchFilterDTO.ValidatiyPeriodTo.Value);
                //if (paramsSearchFilterDTO.ValiditayPeriodFrom.HasValue)
                //    query = query.Where(x => x.AssistantUsers.Any(z => z.CreatedOn  >= paramsSearchFilterDTO.ValiditayPeriodFrom.Value));

                if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field == "commiteeId")))
                {
                    UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId =
                        _sessionServices.UserIdAndRoleIdAfterDecrypt((dataSourceRequest.Filter.Filters.Where(z => z.Field == "commiteeId").FirstOrDefault()?.Value), false);

                    var commiteeId = UserIdAndUserRoleId.Id;
                    query = query.Where(x => x.CommiteeId == commiteeId);
                }
                if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field.ToLower() == "comiteetaskcategoryid")))
                {
                    var comiteetaskcategoryid = Convert.ToInt32((dataSourceRequest.Filter.Filters.Where(z => z.Field.ToLower() == "comiteetaskcategoryid").FirstOrDefault()?.Value));
                    query = query.Where(x => x.ComiteeTaskCategoryId == comiteetaskcategoryid);
                }
                if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field.ToLower() == "title")))
                {
                    var title = (dataSourceRequest.Filter.Filters.Where(z => z.Field.ToLower() == "title").FirstOrDefault()?.Value).ToString();
                    query = query.Where(x => x.Title.Contains(title));
                }

                //if (requiredTasks == TaskFilterEnum.late)
                //{
                //    dta.ForEach(x => x.Islated = true);
                //}


                //if (query.Where(x => x.MainAssinedUserId != _sessionServices.UserId && x.AssistantUsers.Any(z => z.UserId != _sessionServices.UserId)).Any())
                //{
                //    var listQuery = query.ToList();

                //    for (int i = 0; i < listQuery.Count(); i++)
                //    {
                //        //.Where(x => x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == _sessionServices.UserId))).Any() ? true : false;
                //        listQuery[i].TaskToView = listQuery[i].TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == _sessionServices.UserId)) ? true : false;
                //    }
                //}



                var dta = _Mapper.Map<List<CommiteeTaskDTO>>(query.Skip(dataSourceRequest.Skip).Take(dataSourceRequest.Take).ToList());

                foreach (var itemtask in dta)
                {
                    foreach (var itemMission in itemtask.MultiMission)
                    {
                        itemMission.CommiteeTaskMultiMissionUserDTOs = _unitOfWork.GetRepository<CommiteeTaskMultiMissionUser>().GetAll().Where(x => x.CommiteeTaskMultiMissionId == itemMission.CommiteeTaskMultiMissionId)
                            .Select(x => new CommiteeTaskMultiMissionUserDTO
                            {
                                CommiteeTaskMultiMissionId = x.CommiteeTaskMultiMissionId,
                                CommiteeTaskMultiMissionUserId = x.CommiteeTaskMultiMissionUserId,

                                UserDetailsDto = new UserDetailsDTO()
                                {
                                    UserId = x.UserId,
                                    UserName = x.User.Username,
                                    FullNameAr = x.User.FullNameAr,
                                    FullNameEn = x.User.FullNameEn,
                                    ProfileImage = x.User.ProfileImage
                                },
                                UserId = x.UserId,
                                FullNameAr = x.User.FullNameAr,
                                FullNameEn = x.User.FullNameEn,
                            }).ToList();


                    }
                    itemtask.MainAssinedUser = _unitOfWork.GetRepository<User>().GetAll().Where(x => x.UserId == itemtask.MainAssinedUser.UserId)
                            .Select(x => new UserDetailsDTO()
                            {
                                UserId = x.UserId,
                                UserName = x.Username,
                                FullNameAr = x.FullNameAr,
                                FullNameEn = x.FullNameEn,
                                ProfileImage = x.ProfileImage
                            }).FirstOrDefault();
                }


                DataSourceResult<CommiteeTaskDTO> returned = new DataSourceResult<CommiteeTaskDTO>
                {
                    Count = query.Count(),
                    Data = dta

                };
                return returned;
            }
            else
            {

                IQueryable<CommiteeTask> query = this._UnitOfWork.GetRepository<CommiteeTask>().GetAll(WithTracking).OrderByDescending(x => x.CreatedOn)
                    .Where(x => (paramsSearchFilterDTO.ValiditayPeriodFrom == null || x.CreatedOn >= paramsSearchFilterDTO.ValiditayPeriodFrom.Value) && (paramsSearchFilterDTO.ValidatiyPeriodTo == null || x.CreatedOn <= paramsSearchFilterDTO.ValidatiyPeriodTo.Value))
                   .Where(x =>
               ((x.IsShared && filter) || x.MainAssinedUserId == userId || x.AssistantUsers.Any(z => z.UserId == userId)
               || x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == userId))
               || x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == userId)) ||
                 // ((paramsSearchFilterDTO.ValiditayPeriodFrom == null || x.CreatedOn >= paramsSearchFilterDTO.ValiditayPeriodFrom) && (paramsSearchFilterDTO.ValidatiyPeriodTo == null || x.CreatedOn <= paramsSearchFilterDTO.ValidatiyPeriodTo)) ||
                 x.CreatedBy == userId));
                switch (requiredTasks)
                {
                    case TaskFilterEnum.All:
                        break;
                    case TaskFilterEnum.late:
                        query = query.Where(x => x.EndDate <= DateTimeOffset.Now && x.MainAssinedUserId == userId && !x.Completed);
                        break;
                    case TaskFilterEnum.closedLate:
                        query = query.Where(x => x.MainAssinedUserId == userId && x.Completed && x.CompleteDate >= x.EndDate);
                        break;
                    case TaskFilterEnum.sentlate:
                        query = query.Where(x => x.EndDate < DateTimeOffset.Now && x.CreatedBy == userId && !x.Completed);
                        break;
                    case TaskFilterEnum.sent:
                        query = query.Where(x => x.CreatedBy == userId && !x.Completed);
                        break;
                    case TaskFilterEnum.closed:
                        query = query.Where(x => x.Completed && x.MainAssinedUserId == userId && x.CompleteDate < x.EndDate);
                        break;
                    case TaskFilterEnum.toBeImplemented:
                        query = query.Where(x => !x.Completed && x.MainAssinedUser.UserId == userId);
                        break;
                    case TaskFilterEnum.Helpers:
                        query = query.Where(x => x.AssistantUsers.Any(z => z.UserId == userId) || x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == userId)));

                        break;
                    case TaskFilterEnum.AllClosed:
                        query = query.Where(x => x.Completed && x.MainAssinedUserId == userId);
                        break;
                    case TaskFilterEnum.TasksToView:
                        query = query.Where(x => x.MainAssinedUserId != userId && x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == userId))
                          && x.CreatedBy != userId);
                        break;
                    default:
                        break;
                }
                if (paramsSearchFilterDTO.FromDate.HasValue)
                    query = query.Where(x => x.EndDate >= paramsSearchFilterDTO.FromDate.Value);
                if (paramsSearchFilterDTO.ToDate.HasValue)
                    query = query.Where(x => x.EndDate <= paramsSearchFilterDTO.ToDate.Value);
                if (paramsSearchFilterDTO.MainUserId.HasValue)
                    query = query.Where(x => x.MainAssinedUserId == paramsSearchFilterDTO.MainUserId.Value);
                if (paramsSearchFilterDTO.MainAssinedUserId.HasValue)
                    //query = query.Where(x => x.AssistantUsers.Any(z => z.UserId == paramsSearchFilterDTO.MainAssinedUserId.Value));
                    query = query.Where(x => x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(z => z.UserId == paramsSearchFilterDTO.MainAssinedUserId.Value)));


                //if (paramsSearchFilterDTO.ValidatiyPeriodTo.HasValue)
                //    query = query.Where(x => x.CreatedOn <= paramsSearchFilterDTO.ValidatiyPeriodTo.Value);
                //if (paramsSearchFilterDTO.ValiditayPeriodFrom.HasValue)
                //    query = query.Where(x => x.AssistantUsers.Any(z => z.CreatedOn  >= paramsSearchFilterDTO.ValiditayPeriodFrom.Value));

                if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field == "commiteeId")))
                {
                    UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId =
                        _sessionServices.UserIdAndRoleIdAfterDecrypt((dataSourceRequest.Filter.Filters.Where(z => z.Field == "commiteeId").FirstOrDefault()?.Value), false);

                    var commiteeId = UserIdAndUserRoleId.Id;
                    query = query.Where(x => x.CommiteeId == commiteeId);
                }
                if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field.ToLower() == "comiteetaskcategoryid")))
                {
                    var comiteetaskcategoryid = Convert.ToInt32((dataSourceRequest.Filter.Filters.Where(z => z.Field.ToLower() == "comiteetaskcategoryid").FirstOrDefault()?.Value));
                    query = query.Where(x => x.ComiteeTaskCategoryId == comiteetaskcategoryid);
                }
                if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field.ToLower() == "title")))
                {
                    var title = (dataSourceRequest.Filter.Filters.Where(z => z.Field.ToLower() == "title").FirstOrDefault()?.Value).ToString();
                    query = query.Where(x => x.Title.Contains(title));
                }

                var dta = _Mapper.Map<List<CommiteeTaskDTO>>(query.Skip(dataSourceRequest.Skip).Take(dataSourceRequest.Take).ToList());

                foreach (var itemtask in dta)
                {
                    foreach (var itemMission in itemtask.MultiMission)
                    {
                        itemMission.CommiteeTaskMultiMissionUserDTOs = _unitOfWork.GetRepository<CommiteeTaskMultiMissionUser>().GetAll().Where(x => x.CommiteeTaskMultiMissionId == itemMission.CommiteeTaskMultiMissionId)
                            .Select(x => new CommiteeTaskMultiMissionUserDTO
                            {
                                CommiteeTaskMultiMissionId = x.CommiteeTaskMultiMissionId,
                                CommiteeTaskMultiMissionUserId = x.CommiteeTaskMultiMissionUserId,
                                UserDetailsDto = new UserDetailsDTO()
                                {
                                    UserId = x.UserId,
                                    UserName = x.User.Username,
                                    FullNameAr = x.User.FullNameAr,
                                    FullNameEn = x.User.FullNameEn,
                                    ProfileImage = x.User.ProfileImage
                                },
                                UserId = x.UserId
                            }).ToList();


                    }
                    itemtask.MainAssinedUser = _unitOfWork.GetRepository<User>().GetAll().Where(x => x.UserId == itemtask.MainAssinedUser.UserId)
                            .Select(x => new UserDetailsDTO()
                            {
                                UserId = x.UserId,
                                UserName = x.Username,
                                FullNameAr = x.FullNameAr,
                                FullNameEn = x.FullNameEn,
                                ProfileImage = x.ProfileImage
                            }).FirstOrDefault();
                }

                DataSourceResult<CommiteeTaskDTO> returned = new DataSourceResult<CommiteeTaskDTO>
                {
                    Count = query.Count(),
                    Data = dta

                };
                return returned;
            }
        }
        public List<CommiteeTaskDTO> GetAllForPrint(TaskFilterEnum requiredTasks, int? CommiteeId, int? ComiteeTaskCategoryId, string SearchText, int? userId = null, ParamsSearchFilterDTO paramsSearchFilterDTO = null, int? organizationId = null)
        {
            if (userId != null) organizationId = null;
            if (organizationId != null)
            {
                var returnFromStoredEnumerable = _context.vm_EmpInOrgaHierarchies.FromSqlInterpolated($"exec [dbo].[sp_GetEmployeesByOrganizationHierarchyToReport] {organizationId}").ToList();
                // var res = returnFromStoredEnumerable.AsEnumerable().ToDataSourceResult(dataSourceRequest, true).Data.ToList();

                //var x = returnFromStoredEnumerable.AsQueryable();
                List<LookUpDTO> lookupUser = returnFromStoredEnumerable.AsQueryable().Select(x => new LookUpDTO
                {
                    Id = x.Id,
                    Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
                }).ToList();

                CommiteeTaskDTOAndCount commiteeTaskDTOAndCount = new CommiteeTaskDTOAndCount()
                {

                    tasks = new List<CommiteeTaskDTO>(),
                    CountTasks = 0
                };

                foreach (var item in lookupUser)
                {
                    IQueryable<CommiteeTask> query = this._UnitOfWork.GetRepository<CommiteeTask>().GetAll(true).OrderByDescending(x => x.CreatedOn)
                    .Where(x => (paramsSearchFilterDTO.ValiditayPeriodFrom == null || x.CreatedOn >= paramsSearchFilterDTO.ValiditayPeriodFrom.Value) &&
                                (paramsSearchFilterDTO.ValidatiyPeriodTo == null || x.CreatedOn <= paramsSearchFilterDTO.ValidatiyPeriodTo.Value) &&
                                (x.MeetingId == paramsSearchFilterDTO.MeetingId || paramsSearchFilterDTO.MeetingId == null) &&
                                (x.CommiteeId == paramsSearchFilterDTO.CommiteeId || paramsSearchFilterDTO.CommiteeId == null))
                   .Where(x =>
               ((x.IsShared) || x.MainAssinedUserId == item.Id || x.AssistantUsers.Any(z => z.UserId == item.Id)
               || x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == item.Id))
               || x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == item.Id)) ||
                 // ((paramsSearchFilterDTO.ValiditayPeriodFrom == null || x.CreatedOn >= paramsSearchFilterDTO.ValiditayPeriodFrom) && (paramsSearchFilterDTO.ValidatiyPeriodTo == null || x.CreatedOn <= paramsSearchFilterDTO.ValidatiyPeriodTo)) ||
                 x.CreatedBy == item.Id));
                    switch (requiredTasks)
                    {
                        case TaskFilterEnum.All:
                            break;
                        case TaskFilterEnum.late:
                            query = query.Where(x => x.EndDate <= DateTimeOffset.Now && x.MainAssinedUserId == item.Id && !x.Completed);
                            break;
                        case TaskFilterEnum.closedLate:
                            query = query.Where(x => x.MainAssinedUserId == item.Id && x.Completed && x.CompleteDate >= x.EndDate);
                            break;
                        case TaskFilterEnum.sentlate:
                            query = query.Where(x => x.EndDate < DateTimeOffset.Now && x.CreatedBy == item.Id && !x.Completed);
                            break;
                        case TaskFilterEnum.sent:
                            query = query.Where(x => x.CreatedBy == item.Id && !x.Completed);
                            break;
                        case TaskFilterEnum.closed:
                            query = query.Where(x => x.Completed && x.MainAssinedUserId == item.Id && x.CompleteDate < x.EndDate);
                            break;
                        case TaskFilterEnum.toBeImplemented:
                            query = query.Where(x => !x.Completed && x.MainAssinedUser.UserId == item.Id);
                            break;
                        case TaskFilterEnum.Helpers:
                            query = query.Where(x => x.AssistantUsers.Any(z => z.UserId == item.Id) || x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == item.Id)));

                            break;
                        case TaskFilterEnum.AllClosed:
                            query = query.Where(x => x.Completed && x.MainAssinedUserId == item.Id);
                            break;
                        case TaskFilterEnum.TasksToView:
                            query = query.Where(x => x.MainAssinedUserId != item.Id && x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == item.Id))
                              && x.CreatedBy != item.Id);
                            break;
                        default:
                            break;
                    }
                    if (paramsSearchFilterDTO.FromDate.HasValue)
                        query = query.Where(x => x.EndDate >= paramsSearchFilterDTO.FromDate.Value);
                    if (paramsSearchFilterDTO.ToDate.HasValue)
                        query = query.Where(x => x.EndDate <= paramsSearchFilterDTO.ToDate.Value);
                    if (paramsSearchFilterDTO.MainUserId.HasValue)
                        query = query.Where(x => x.MainAssinedUserId == paramsSearchFilterDTO.MainUserId.Value);
                    if (paramsSearchFilterDTO.MainAssinedUserId.HasValue)
                        //query = query.Where(x => x.AssistantUsers.Any(z => z.UserId == paramsSearchFilterDTO.MainAssinedUserId.Value));
                        query = query.Where(x => x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(z => z.UserId == paramsSearchFilterDTO.MainAssinedUserId.Value)));


                    //if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field.ToLower() == "comiteetaskcategoryid")))
                    //{
                    //    var comiteetaskcategoryid = Convert.ToInt32((dataSourceRequest.Filter.Filters.Where(z => z.Field.ToLower() == "comiteetaskcategoryid").FirstOrDefault()?.Value));
                    //    query = query.Where(x => x.ComiteeTaskCategoryId == comiteetaskcategoryid);
                    //}
                    //if (Convert.ToBoolean(dataSourceRequest.Filter?.Filters?.Any(z => z.Field.ToLower() == "title")))
                    //{
                    //    var title = (dataSourceRequest.Filter.Filters.Where(z => z.Field.ToLower() == "title").FirstOrDefault()?.Value).ToString();
                    //    query = query.Where(x => x.Title.Contains(title));
                    //}


                    var dta = _Mapper.Map<List<CommiteeTaskDTO>>(query.ToList()).ToList();

                    foreach (var itemtask in dta)
                    {
                        foreach (var itemMission in itemtask.MultiMission)
                        {
                            itemMission.CommiteeTaskMultiMissionUserDTOs = _unitOfWork.GetRepository<CommiteeTaskMultiMissionUser>().GetAll().Where(x => x.CommiteeTaskMultiMissionId == itemMission.CommiteeTaskMultiMissionId)
                                .Select(x => new CommiteeTaskMultiMissionUserDTO
                                {
                                    CommiteeTaskMultiMissionId = x.CommiteeTaskMultiMissionId,
                                    CommiteeTaskMultiMissionUserId = x.CommiteeTaskMultiMissionUserId,
                                    UserDetailsDto = new UserDetailsDTO()
                                    {
                                        UserId = x.UserId,
                                        UserName = x.User.Username,
                                        FullNameAr = x.User.FullNameAr,
                                        FullNameEn = x.User.FullNameEn,
                                        ProfileImage = x.User.ProfileImage
                                    },
                                    UserId = x.UserId
                                }).ToList();


                        }
                        itemtask.MainAssinedUser = _unitOfWork.GetRepository<User>().GetAll().Where(x => x.UserId == itemtask.MainAssinedUser.UserId)
                                .Select(x => new UserDetailsDTO()
                                {
                                    UserId = x.UserId,
                                    UserName = x.Username,
                                    FullNameAr = x.FullNameAr,
                                    FullNameEn = x.FullNameEn,
                                    ProfileImage = x.ProfileImage
                                }).FirstOrDefault();
                    }


                    commiteeTaskDTOAndCount.tasks.AddRange(dta);
                    commiteeTaskDTOAndCount.CountTasks += query.Count();

                }
                // return
                //DataSourceResult<CommiteeTaskDTO> returned = new DataSourceResult<CommiteeTaskDTO>
                //{
                //    Count = commiteeTaskDTOAndCount.CountTasks,
                //    Data = commiteeTaskDTOAndCount.tasks
                //    //Data = commiteeTaskDTOAndCount.tasks.Skip(dataSourceRequest.Skip).Take(dataSourceRequest.Take)
                //};
                List<CommiteeTaskDTO> returned = commiteeTaskDTOAndCount.tasks;
                return returned;

            }
            if (userId == null)
            {


                //CommiteeId == null || x.CommiteeId == Convert.ToInt32(CommiteeId) /*&& ComiteeTaskCategoryId == null|| SearchText == "" || x.ComiteeTaskCategoryId == ComiteeTaskCategoryId.Value*/ &&
                var query = this._UnitOfWork.GetRepository<CommiteeTask>().GetAll().Include(x => x.Meeting).OrderByDescending(x => x.CreatedOn).Where(x =>
                (
                 (x.MeetingId == paramsSearchFilterDTO.MeetingId || paramsSearchFilterDTO.MeetingId == null) &&
                 (x.CommiteeId == paramsSearchFilterDTO.CommiteeId || paramsSearchFilterDTO.CommiteeId == null) &&
                 ((x.IsShared && CommiteeId != null) ||
                  x.MainAssinedUserId == _sessionServices.UserId ||
                  x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId) ||
                  x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == _sessionServices.UserId)) ||
                  x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == _sessionServices.UserId)) ||
                  x.CreatedBy == _sessionServices.UserId) &&
                  (SearchText == "" || SearchText == null || x.Title.Contains(SearchText))));

                if (CommiteeId.HasValue)
                {
                    query = query.Where(x => x.CommiteeId == CommiteeId.Value);
                }


                switch (requiredTasks)
                {
                    case TaskFilterEnum.All:
                        break;
                    case TaskFilterEnum.late:
                        query = query.Where(x => x.EndDate < DateTimeOffset.Now && x.MainAssinedUserId == _sessionServices.UserId && !x.Completed);
                        break;
                    case TaskFilterEnum.sentlate:
                        query = query.Where(x => x.EndDate < DateTimeOffset.Now && x.CreatedBy == _sessionServices.UserId && !x.Completed);
                        break;
                    case TaskFilterEnum.sent:
                        query = query.Where(x => x.CreatedBy == _sessionServices.UserId && !x.Completed);
                        break;
                    case TaskFilterEnum.closedLate:
                        query = query.Where(x => x.MainAssinedUserId == _sessionServices.UserId && x.Completed && x.CompleteDate >= x.EndDate);
                        break;
                    case TaskFilterEnum.closed:
                        query = query.Where(x => x.Completed && x.MainAssinedUserId == _sessionServices.UserId && x.CompleteDate <= x.EndDate);
                        break;
                    case TaskFilterEnum.AllClosed:
                        query = query.Where(x => x.Completed && x.MainAssinedUserId == _sessionServices.UserId);
                        break;
                    case TaskFilterEnum.toBeImplemented:
                        query = query.Where(x => !x.Completed && x.MainAssinedUserId == _sessionServices.UserId);
                        break;
                    case TaskFilterEnum.Helpers:
                        query = query.Where(x => x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId) || x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == _sessionServices.UserId)));

                        break;
                    case TaskFilterEnum.TasksToView:
                        query = query.Where(x => x.MainAssinedUserId != _sessionServices.UserId
                        && x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == _sessionServices.UserId)) && x.CreatedBy != _sessionServices.UserId);
                        break;
                    default:
                        break;
                }

                if (paramsSearchFilterDTO.FromDate.HasValue)
                {
                    query = query.Where(x => x.EndDate >= paramsSearchFilterDTO.FromDate.Value);
                }
                if (paramsSearchFilterDTO.ToDate.HasValue)
                {
                    query = query.Where(x => x.EndDate <= paramsSearchFilterDTO.ToDate.Value);
                }

                if (paramsSearchFilterDTO.MainUserId.HasValue)
                {
                    query = query.Where(x => x.MainAssinedUserId == paramsSearchFilterDTO.MainUserId.Value);
                }
                if (paramsSearchFilterDTO.MainAssinedUserId.HasValue)
                {
                    query = query.Where(x => x.AssistantUsers.Any(z => z.UserId == paramsSearchFilterDTO.MainAssinedUserId.Value));
                }
                if (ComiteeTaskCategoryId.HasValue)
                {
                    query = query.Where(x => x.ComiteeTaskCategoryId == ComiteeTaskCategoryId.Value);
                }


                //if (requiredTasks == TaskFilterEnum.late || (requiredTasks == TaskFilterEnum.All && (query.Where(x => x.EndDate < DateTimeOffset.Now && !x.Completed).Any())))
                //{

                //    var result = query.Select(x => new CommiteeTask
                //    {
                //        AssistantUsers = x.AssistantUsers,
                //        ComiteeTaskCategory = x.ComiteeTaskCategory,
                //        ComiteeTaskCategoryId = x.ComiteeTaskCategoryId,
                //        Commitee = x.Commitee,
                //        CommiteeId = x.CommiteeId,
                //        CommiteeTaskId = x.CommiteeTaskId,
                //        Completed = x.Completed,
                //        CompleteDate = x.CompleteDate,
                //        CreatedBy = x.CreatedBy,
                //        CreatedByRole = x.CreatedByRole,
                //        CreatedByRoleId = x.CreatedByRoleId,
                //        CreatedByUser = x.CreatedByUser,
                //        CreatedOn = x.CreatedOn,
                //        DeletedBy = x.DeletedBy,
                //        DeletedOn = x.DeletedOn,
                //        EndDate = x.EndDate,
                //        IsEmail = x.IsEmail,
                //        Islated = x.EndDate < DateTimeOffset.Now && !x.Completed ? true : false,
                //        IsMain = x.IsMain,
                //        IsNotification = x.IsNotification,
                //        IsShared = x.IsShared,
                //        IsSMS = x.IsSMS,
                //        MainAssinedUser = x.MainAssinedUser,
                //        MainAssinedUserId = x.MainAssinedUserId,
                //        MultiMission = x.MultiMission,
                //        TaskAttachments = x.TaskAttachments,
                //        TaskComments = x.TaskComments,
                //        Title = x.Title,
                //        TaskDetails = x.TaskDetails,
                //        UpdatedBy = x.UpdatedBy,
                //        UpdatedOn = x.UpdatedOn,


                //    }).ToList();

                //    return _Mapper.Map<List<CommiteeTaskDTO>>(result);

                //}


                var dta = _Mapper.Map<List<CommiteeTaskDTO>>(query.ToList());

                if (requiredTasks == TaskFilterEnum.late || (requiredTasks == TaskFilterEnum.All && (query.Where(x => x.EndDate < DateTimeOffset.Now && !x.Completed).Any())))
                {
                    dta.ForEach(x => x.Islated = true);
                }
                return dta;
            }
            else
            {
                //(CommiteeId == null || x.CommiteeId == Convert.ToInt32(CommiteeId)) /*&& ComiteeTaskCategoryId == null|| SearchText == "" || x.ComiteeTaskCategoryId == ComiteeTaskCategoryId.Value*/ &&
                var query = this._UnitOfWork.GetRepository<CommiteeTask>().GetAll()/*.Include(x => x.Meeting)*/.OrderByDescending(x => x.CreatedOn).Where(x =>
                (
                 (x.MeetingId == paramsSearchFilterDTO.MeetingId || paramsSearchFilterDTO.MeetingId == null) &&
                  (x.CommiteeId == paramsSearchFilterDTO.CommiteeId || paramsSearchFilterDTO.CommiteeId == null) &&
                (x.IsShared && CommiteeId != null) || x.MainAssinedUserId == userId || x.AssistantUsers.Any(z => z.UserId == userId) ||
                x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == userId)) || x.CreatedBy == userId) ||
                x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == userId)) &&
                    (SearchText == "" || SearchText == null || x.Title.Contains(SearchText)));
                switch (requiredTasks)
                {
                    case TaskFilterEnum.All:
                        break;
                    case TaskFilterEnum.late:
                        query = query.Where(x => x.EndDate < DateTimeOffset.Now && x.MainAssinedUserId == userId && !x.Completed);
                        break;
                    case TaskFilterEnum.sentlate:
                        query = query.Where(x => x.EndDate < DateTimeOffset.Now && x.CreatedBy == userId && !x.Completed);
                        break;
                    case TaskFilterEnum.sent:
                        query = query.Where(x => x.CreatedBy == userId && !x.Completed);
                        break;
                    case TaskFilterEnum.closedLate:
                        query = query.Where(x => x.MainAssinedUserId == userId && x.Completed && x.CompleteDate >= x.EndDate);
                        break;
                    case TaskFilterEnum.closed:
                        query = query.Where(x => x.Completed && x.MainAssinedUserId == userId && x.CompleteDate <= x.EndDate);
                        break;
                    case TaskFilterEnum.AllClosed:
                        query = query.Where(x => x.Completed && x.MainAssinedUserId == userId);
                        break;
                    case TaskFilterEnum.toBeImplemented:
                        query = query.Where(x => !x.Completed && x.MainAssinedUserId == userId);
                        break;
                    case TaskFilterEnum.Helpers:
                        query = query.Where(x => x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId) || x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == _sessionServices.UserId)));

                        break;
                    case TaskFilterEnum.TasksToView:
                        query = query.Where(x => x.MainAssinedUserId != userId && x.AssistantUsers.All(x => x.UserId != userId) && x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == userId)) &&
                                            x.CreatedBy != userId);
                        break;
                    default:
                        break;
                }
                if (paramsSearchFilterDTO.FromDate.HasValue)
                {
                    query = query.Where(x => x.EndDate >= paramsSearchFilterDTO.FromDate.Value);
                }
                if (paramsSearchFilterDTO.ToDate.HasValue)
                {
                    query = query.Where(x => x.EndDate <= paramsSearchFilterDTO.ToDate.Value);
                }
                if (paramsSearchFilterDTO.MainUserId.HasValue)
                {
                    query = query.Where(x => x.MainAssinedUserId == paramsSearchFilterDTO.MainUserId.Value);
                }
                if (paramsSearchFilterDTO.MainAssinedUserId.HasValue)
                {
                    query = query.Where(x => x.AssistantUsers.Any(z => z.UserId == paramsSearchFilterDTO.MainAssinedUserId.Value));
                }
                if (ComiteeTaskCategoryId.HasValue)
                {
                    query = query.Where(x => x.ComiteeTaskCategoryId == ComiteeTaskCategoryId.Value);
                }
                var dta = _Mapper.Map<List<CommiteeTaskDTO>>(query.ToList());
                if (requiredTasks == TaskFilterEnum.late || (requiredTasks == TaskFilterEnum.All && (query.Where(x => x.EndDate < DateTimeOffset.Now && !x.Completed).Any())))
                {
                    dta.ForEach(x => x.Islated = true);
                }
                return dta;
            }
        }
        public byte[] Export(TaskFilterEnum requiredTasks, int? UserIdEncrpted, bool ExportWord = true, int? OrganaizationId = null)
        {
            //count of tasks
            var resultCount = getComitteeTaskStatistics(null, UserIdEncrpted, null, null, null);
            //get all data
            var gridDataResult = GetAllForPrint(requiredTasks, null, null, string.Empty, UserIdEncrpted, new ParamsSearchFilterDTO(), OrganaizationId);

            string filePath = "";
            #region word
            if (ExportWord)
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    // Create Document
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
                    {
                        // Add a main document part. 
                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                        // Create the document structure and add some text.
                        mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                        Body body = mainPart.Document.AppendChild(new Body());

                        #region table count

                        // Create a new table
                        Table tbl1 = new Table();

                        #region Table Style
                        DocumentFormat.OpenXml.Wordprocessing.TableProperties tableProperties11 = new DocumentFormat.OpenXml.Wordprocessing.TableProperties();
                        DocumentFormat.OpenXml.Wordprocessing.TableStyle tableStyle11 = new DocumentFormat.OpenXml.Wordprocessing.TableStyle() { Val = "TableGrid" };
                        DocumentFormat.OpenXml.Wordprocessing.TableWidth tableWidth11 = new DocumentFormat.OpenXml.Wordprocessing.TableWidth() { Width = "0", Type = DocumentFormat.OpenXml.Wordprocessing.TableWidthUnitValues.Auto };
                        DocumentFormat.OpenXml.Wordprocessing.TableBorders tableBorders11 = new DocumentFormat.OpenXml.Wordprocessing.TableBorders();
                        DocumentFormat.OpenXml.Wordprocessing.TopBorder topBorder11 = new DocumentFormat.OpenXml.Wordprocessing.TopBorder() { Val = DocumentFormat.OpenXml.Wordprocessing.BorderValues.Double, Color = "000000", Size = (int)4U, Space = (int)0U };
                        DocumentFormat.OpenXml.Wordprocessing.LeftBorder leftBorder11 = new DocumentFormat.OpenXml.Wordprocessing.LeftBorder() { Val = DocumentFormat.OpenXml.Wordprocessing.BorderValues.Double, Color = "000000", Size = (int)4U, Space = (int)0U };
                        DocumentFormat.OpenXml.Wordprocessing.BottomBorder bottomBorder11 = new DocumentFormat.OpenXml.Wordprocessing.BottomBorder() { Val = DocumentFormat.OpenXml.Wordprocessing.BorderValues.Double, Color = "000000", Size = (int)4U, Space = (int)0U };
                        DocumentFormat.OpenXml.Wordprocessing.RightBorder rightBorder11 = new DocumentFormat.OpenXml.Wordprocessing.RightBorder() { Val = DocumentFormat.OpenXml.Wordprocessing.BorderValues.Double, Color = "000000", Size = (int)4U, Space = (int)0U };
                        DocumentFormat.OpenXml.Wordprocessing.InsideHorizontalBorder insideHorizontalBorder11 = new DocumentFormat.OpenXml.Wordprocessing.InsideHorizontalBorder() { Val = DocumentFormat.OpenXml.Wordprocessing.BorderValues.Double, Color = "000000", Size = (int)4U, Space = (int)0U };
                        DocumentFormat.OpenXml.Wordprocessing.InsideVerticalBorder insideVerticalBorder11 = new DocumentFormat.OpenXml.Wordprocessing.InsideVerticalBorder() { Val = DocumentFormat.OpenXml.Wordprocessing.BorderValues.Double, Color = "000000", Size = (int)4U, Space = (int)0U };
                        BiDiVisual BiDiVisual1 = new BiDiVisual { Val = new EnumValue<OnOffOnlyValues> { Value = OnOffOnlyValues.On } };

                        TableWidth tableWidth10 = new TableWidth();
                        tableWidth10.Width = "5000";
                        tableWidth10.Type = TableWidthUnitValues.Pct;

                        tableBorders11.Append(topBorder11);
                        tableBorders11.Append(leftBorder11);
                        tableBorders11.Append(bottomBorder11);
                        tableBorders11.Append(rightBorder11);
                        tableBorders11.Append(insideHorizontalBorder11);
                        tableBorders11.Append(insideVerticalBorder11);
                        DocumentFormat.OpenXml.Wordprocessing.TableLook tableLook11 = new DocumentFormat.OpenXml.Wordprocessing.TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };
                        tableProperties11.Append(tableStyle11);
                        tableProperties11.Append(tableWidth11);
                        tableProperties11.Append(tableBorders11);
                        tableProperties11.Append(tableLook11);
                        tableProperties11.Append(BiDiVisual1);
                        tableProperties11.Append(tableWidth10);
                        tbl1.Append(tableProperties11);


                        #endregion

                        #region paragraph
                        //Header Paragraph
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph HeaderParagraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesHeaderParagraph = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationHeaderParagraph = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesHeaderParagraph.Append(justificationHeaderParagraph);

                        StyleRunProperties styleRunProperties1 = new StyleRunProperties();
                        Bold bold1 = new Bold();

                        // Specify a 12 point size.
                        DocumentFormat.OpenXml.Wordprocessing.FontSize fontSize1 = new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "24" };
                        styleRunProperties1.Append(bold1);
                        styleRunProperties1.Append(fontSize1);

                        DocumentFormat.OpenXml.Wordprocessing.Run run1HeaderParagraph = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text1HeaderParagraph = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text1HeaderParagraph.Text = "إحصائيات المهام ";// LocalizationRepository.GetByKey("EmployeesReports", culture);
                        run1HeaderParagraph.Append(styleRunProperties1);
                        run1HeaderParagraph.Append(text1HeaderParagraph);
                        BiDi bidi = new BiDi();
                        paraPropertiesHeaderParagraph.Append(bidi);
                        HeaderParagraph.Append(paraPropertiesHeaderParagraph);
                        HeaderParagraph.Append(run1HeaderParagraph);

                        body.AppendChild(HeaderParagraph);
                        #endregion

                        TableRow trHead1 = new TableRow();


                        #region All Tasks  - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraTransactionNumberFormattedHeadCount = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesTransactionNumberFormattedHeadCount = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationTransactionNumberFormattedHeadCount = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesTransactionNumberFormattedHeadCount.Append(justificationTransactionNumberFormattedHeadCount);

                        DocumentFormat.OpenXml.Wordprocessing.Run run1HeadCount = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text1HeadCount = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text1HeadCount.Text = "كل المهام";

                        var run1HeadPropCount = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run1HeadCount.Append(run1HeadPropCount);

                        run1HeadCount.Append(text1HeadCount);

                        paraTransactionNumberFormattedHeadCount.Append(paraPropertiesTransactionNumberFormattedHeadCount);
                        paraTransactionNumberFormattedHeadCount.Append(run1HeadCount);

                        TableCell tcTransactionNumberFormattedHeadCount = new TableCell();
                        tcTransactionNumberFormattedHeadCount.Append(paraTransactionNumberFormattedHeadCount);
                        #endregion

                        #region  Late Tasks - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraSubjectHeadCount = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesSubjectHeadCount = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationSubjectHeadCount = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesSubjectHeadCount.Append(justificationSubjectHeadCount);

                        DocumentFormat.OpenXml.Wordprocessing.Run run2HeadCount = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text2HeadCount = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text2HeadCount.Text = "مهام مُتاخره";
                        var run2HeadPropCount = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run2HeadCount.Append(run2HeadPropCount);
                        run2HeadCount.Append(text2HeadCount);

                        paraSubjectHeadCount.Append(paraPropertiesSubjectHeadCount);
                        paraSubjectHeadCount.Append(run2HeadCount);

                        TableCell tcSubjectHeadCount = new TableCell();
                        tcSubjectHeadCount.Append(paraSubjectHeadCount);
                        #endregion


                        #region Mulitmission - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraFromAnyHeadCount = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesFromAnyHeadCount = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationFromAnyHeadCount = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesFromAnyHeadCount.Append(justificationFromAnyHeadCount);

                        DocumentFormat.OpenXml.Wordprocessing.Run run3HeadCount = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text3HeadCount = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text3HeadCount.Text = "تحت الاجراء";

                        var run3HeadPropCount = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run3HeadCount.Append(run3HeadPropCount);

                        run3HeadCount.Append(text3HeadCount);

                        paraFromAnyHeadCount.Append(paraPropertiesFromAnyHeadCount);
                        paraFromAnyHeadCount.Append(run3HeadCount);

                        TableCell tcFromAnyHeadCount = new TableCell();
                        tcFromAnyHeadCount.Append(paraFromAnyHeadCount);
                        #endregion

                        #region Closed Task - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraToOrganizationHeadCount = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesToOrganizationHeadCount = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationToOrganizationHeadCount = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesToOrganizationHeadCount.Append(justificationToOrganizationHeadCount);

                        DocumentFormat.OpenXml.Wordprocessing.Run run4HeadCount = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text4HeadCount = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text4HeadCount.Text = "كل المغلق";

                        var run4HeadPropCount = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run4HeadCount.Append(run4HeadPropCount);

                        run4HeadCount.Append(text4HeadCount);

                        paraToOrganizationHeadCount.Append(paraPropertiesToOrganizationHeadCount);
                        paraToOrganizationHeadCount.Append(run4HeadCount);

                        TableCell tcToOrganizationNameHeadCount = new TableCell();
                        tcToOrganizationNameHeadCount.Append(paraToOrganizationHeadCount);
                        #endregion

                        #region task - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraStatusNameHeadCount = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesStatusNameHeadCount = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationStatusNameHeadCount = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesStatusNameHeadCount.Append(justificationStatusNameHeadCount);

                        DocumentFormat.OpenXml.Wordprocessing.Run run5HeadCount = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text5HeadCount = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text5HeadCount.Text = "تكليف فرعى";

                        var run5HeadPropCount = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run5HeadCount.Append(run5HeadPropCount);

                        run5HeadCount.Append(text5HeadCount);

                        paraStatusNameHeadCount.Append(paraPropertiesStatusNameHeadCount);
                        paraStatusNameHeadCount.Append(run5HeadCount);

                        TableCell tcStatusNameHeadCount = new TableCell();
                        tcStatusNameHeadCount.Append(paraStatusNameHeadCount);
                        #endregion

                        #region Task View - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraStatusNameHeadAttachmentCount = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesStatusNameHeadAttachmentCount = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationStatusNameHeadAttachmentCount = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesStatusNameHeadAttachmentCount.Append(justificationStatusNameHeadAttachmentCount);

                        DocumentFormat.OpenXml.Wordprocessing.Run run5HeadAttachmentCount = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text5HeadAttachmentCount = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text5HeadAttachmentCount.Text = "مهام للاطلاع";


                        var run5HeadPropAttachmentCount = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run5HeadAttachmentCount.Append(run5HeadPropAttachmentCount);

                        run5HeadAttachmentCount.Append(text5HeadAttachmentCount);

                        paraStatusNameHeadAttachmentCount.Append(paraPropertiesStatusNameHeadAttachmentCount);
                        paraStatusNameHeadAttachmentCount.Append(run5HeadAttachmentCount);

                        TableCell tcStatusNameHeadAttachmentCount = new TableCell();
                        tcStatusNameHeadAttachmentCount.Append(paraStatusNameHeadAttachmentCount);
                        #endregion


                        // Add the cells to the row
                        trHead1.Append(tcTransactionNumberFormattedHeadCount, tcSubjectHeadCount, tcFromAnyHeadCount, tcToOrganizationNameHeadCount, tcStatusNameHeadCount, tcStatusNameHeadAttachmentCount);

                        // Add the rows to the table
                        tbl1.AppendChild(trHead1);

                        #region Second Row count of data 
                        TableRow trHead2 = new TableRow();


                        #region All Tasks Count  - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraTransactionNumberFormattedHeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesTransactionNumberFormattedHeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationTransactionNumberFormattedHeadCountValue = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesTransactionNumberFormattedHeadCountValue.Append(justificationTransactionNumberFormattedHeadCountValue);

                        DocumentFormat.OpenXml.Wordprocessing.Run run1HeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text1HeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text1HeadCountValue.Text = resultCount[0].Count.ToString();

                        var run1HeadPropCountValue = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run1HeadCountValue.Append(run1HeadPropCountValue);

                        run1HeadCountValue.Append(text1HeadCountValue);

                        paraTransactionNumberFormattedHeadCountValue.Append(paraPropertiesTransactionNumberFormattedHeadCountValue);
                        paraTransactionNumberFormattedHeadCountValue.Append(run1HeadCountValue);

                        TableCell tcTransactionNumberFormattedHeadCountValue = new TableCell();
                        tcTransactionNumberFormattedHeadCountValue.Append(paraTransactionNumberFormattedHeadCountValue);
                        #endregion

                        #region  Late Tasks - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraSubjectHeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesSubjectHeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationSubjectHeadCountValue = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesSubjectHeadCountValue.Append(justificationSubjectHeadCountValue);

                        DocumentFormat.OpenXml.Wordprocessing.Run run2HeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text2HeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text2HeadCountValue.Text = resultCount[1].Count.ToString();
                        var run2HeadPropCountValue = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run2HeadCountValue.Append(run2HeadPropCountValue);
                        run2HeadCountValue.Append(text2HeadCountValue);

                        paraSubjectHeadCountValue.Append(paraPropertiesSubjectHeadCountValue);
                        paraSubjectHeadCountValue.Append(run2HeadCountValue);

                        TableCell tcSubjectHeadCountValue = new TableCell();
                        tcSubjectHeadCountValue.Append(paraSubjectHeadCountValue);
                        #endregion


                        #region Mulitmission - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraFromAnyHeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesFromAnyHeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationFromAnyHeadCountValue = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesFromAnyHeadCountValue.Append(justificationFromAnyHeadCountValue);

                        DocumentFormat.OpenXml.Wordprocessing.Run run3HeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text3HeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text3HeadCountValue.Text = resultCount[2].Count.ToString();

                        var run3HeadPropCountValue = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run3HeadCountValue.Append(run3HeadPropCountValue);

                        run3HeadCountValue.Append(text3HeadCountValue);

                        paraFromAnyHeadCountValue.Append(paraPropertiesFromAnyHeadCountValue);
                        paraFromAnyHeadCountValue.Append(run3HeadCountValue);

                        TableCell tcFromAnyHeadCountValue = new TableCell();
                        tcFromAnyHeadCountValue.Append(paraFromAnyHeadCountValue);
                        #endregion

                        #region Closed Task - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraToOrganizationHeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesToOrganizationHeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationToOrganizationHeadCountValue = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesToOrganizationHeadCountValue.Append(justificationToOrganizationHeadCountValue);

                        DocumentFormat.OpenXml.Wordprocessing.Run run4HeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text4HeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text4HeadCountValue.Text = resultCount[3].Count.ToString();

                        var run4HeadPropCountValue = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run4HeadCountValue.Append(run4HeadPropCountValue);

                        run4HeadCountValue.Append(text4HeadCountValue);

                        paraToOrganizationHeadCountValue.Append(paraPropertiesToOrganizationHeadCountValue);
                        paraToOrganizationHeadCountValue.Append(run4HeadCountValue);

                        TableCell tcToOrganizationNameHeadCountValue = new TableCell();
                        tcToOrganizationNameHeadCountValue.Append(paraToOrganizationHeadCountValue);
                        #endregion

                        #region task - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraStatusNameHeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesStatusNameHeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationStatusNameHeadCountValue = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesStatusNameHeadCountValue.Append(justificationStatusNameHeadCountValue);

                        DocumentFormat.OpenXml.Wordprocessing.Run run5HeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text5HeadCountValue = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text5HeadCountValue.Text = resultCount[4].Count.ToString();

                        var run5HeadPropCountValue = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run5HeadCountValue.Append(run5HeadPropCountValue);

                        run5HeadCountValue.Append(text5HeadCountValue);

                        paraStatusNameHeadCountValue.Append(paraPropertiesStatusNameHeadCountValue);
                        paraStatusNameHeadCountValue.Append(run5HeadCountValue);

                        TableCell tcStatusNameHeadCountValue = new TableCell();
                        tcStatusNameHeadCountValue.Append(paraStatusNameHeadCountValue);
                        #endregion

                        #region Task View - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraStatusNameHeadAttachmentCountValue = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesStatusNameHeadAttachmentCountValue = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationStatusNameHeadAttachmentCountValue = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesStatusNameHeadAttachmentCountValue.Append(justificationStatusNameHeadAttachmentCountValue);

                        DocumentFormat.OpenXml.Wordprocessing.Run run5HeadAttachmentCountValue = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text5HeadAttachmentCountValue = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text5HeadAttachmentCountValue.Text = resultCount[5].Count.ToString();


                        var run5HeadPropAttachmentCountValue = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run5HeadAttachmentCountValue.Append(run5HeadPropAttachmentCountValue);

                        run5HeadAttachmentCountValue.Append(text5HeadAttachmentCountValue);

                        paraStatusNameHeadAttachmentCountValue.Append(paraPropertiesStatusNameHeadAttachmentCountValue);
                        paraStatusNameHeadAttachmentCountValue.Append(run5HeadAttachmentCountValue);

                        TableCell tcStatusNameHeadAttachmentCountValue = new TableCell();
                        tcStatusNameHeadAttachmentCountValue.Append(paraStatusNameHeadAttachmentCountValue);
                        #endregion


                        // Add the cells to the row
                        trHead2.Append(tcTransactionNumberFormattedHeadCountValue, tcSubjectHeadCountValue, tcFromAnyHeadCountValue, tcToOrganizationNameHeadCountValue, tcStatusNameHeadCountValue, tcStatusNameHeadAttachmentCountValue);

                        // Add the rows to the table
                        tbl1.AppendChild(trHead2);

                        #endregion

                        #endregion

                        // Create a new table
                        Table tbl = new Table();

                        #region Table Style
                        DocumentFormat.OpenXml.Wordprocessing.TableProperties tableProperties1 = new DocumentFormat.OpenXml.Wordprocessing.TableProperties();
                        DocumentFormat.OpenXml.Wordprocessing.TableStyle tableStyle1 = new DocumentFormat.OpenXml.Wordprocessing.TableStyle() { Val = "TableGrid" };
                        DocumentFormat.OpenXml.Wordprocessing.TableWidth tableWidth1 = new DocumentFormat.OpenXml.Wordprocessing.TableWidth() { Width = "0", Type = DocumentFormat.OpenXml.Wordprocessing.TableWidthUnitValues.Auto };
                        DocumentFormat.OpenXml.Wordprocessing.TableBorders tableBorders1 = new DocumentFormat.OpenXml.Wordprocessing.TableBorders();
                        DocumentFormat.OpenXml.Wordprocessing.TopBorder topBorder1 = new DocumentFormat.OpenXml.Wordprocessing.TopBorder() { Val = DocumentFormat.OpenXml.Wordprocessing.BorderValues.Double, Color = "000000", Size = (int)4U, Space = (int)0U };
                        DocumentFormat.OpenXml.Wordprocessing.LeftBorder leftBorder1 = new DocumentFormat.OpenXml.Wordprocessing.LeftBorder() { Val = DocumentFormat.OpenXml.Wordprocessing.BorderValues.Double, Color = "000000", Size = (int)4U, Space = (int)0U };
                        DocumentFormat.OpenXml.Wordprocessing.BottomBorder bottomBorder1 = new DocumentFormat.OpenXml.Wordprocessing.BottomBorder() { Val = DocumentFormat.OpenXml.Wordprocessing.BorderValues.Double, Color = "000000", Size = (int)4U, Space = (int)0U };
                        DocumentFormat.OpenXml.Wordprocessing.RightBorder rightBorder1 = new DocumentFormat.OpenXml.Wordprocessing.RightBorder() { Val = DocumentFormat.OpenXml.Wordprocessing.BorderValues.Double, Color = "000000", Size = (int)4U, Space = (int)0U };
                        DocumentFormat.OpenXml.Wordprocessing.InsideHorizontalBorder insideHorizontalBorder1 = new DocumentFormat.OpenXml.Wordprocessing.InsideHorizontalBorder() { Val = DocumentFormat.OpenXml.Wordprocessing.BorderValues.Double, Color = "000000", Size = (int)4U, Space = (int)0U };
                        DocumentFormat.OpenXml.Wordprocessing.InsideVerticalBorder insideVerticalBorder1 = new DocumentFormat.OpenXml.Wordprocessing.InsideVerticalBorder() { Val = DocumentFormat.OpenXml.Wordprocessing.BorderValues.Double, Color = "000000", Size = (int)4U, Space = (int)0U };
                        BiDiVisual BiDiVisual = new BiDiVisual { Val = new EnumValue<OnOffOnlyValues> { Value = OnOffOnlyValues.On } };

                        TableWidth tableWidth = new TableWidth();
                        tableWidth.Width = "5000";
                        tableWidth.Type = TableWidthUnitValues.Pct;

                        tableBorders1.Append(topBorder1);
                        tableBorders1.Append(leftBorder1);
                        tableBorders1.Append(bottomBorder1);
                        tableBorders1.Append(rightBorder1);
                        tableBorders1.Append(insideHorizontalBorder1);
                        tableBorders1.Append(insideVerticalBorder1);
                        DocumentFormat.OpenXml.Wordprocessing.TableLook tableLook1 = new DocumentFormat.OpenXml.Wordprocessing.TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };
                        tableProperties1.Append(tableStyle1);
                        tableProperties1.Append(tableWidth1);
                        tableProperties1.Append(tableBorders1);
                        tableProperties1.Append(tableLook1);
                        tableProperties1.Append(BiDiVisual);
                        tableProperties1.Append(tableWidth);
                        tbl.Append(tableProperties1);


                        #endregion



                        TableRow trHead = new TableRow();


                        #region Tasks Title - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraTransactionNumberFormattedHead = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesTransactionNumberFormattedHead = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationTransactionNumberFormattedHead = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesTransactionNumberFormattedHead.Append(justificationTransactionNumberFormattedHead);

                        DocumentFormat.OpenXml.Wordprocessing.Run run1Head = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text1Head = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text1Head.Text = "عنوان التكليف";

                        var run1HeadProp = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run1Head.Append(run1HeadProp);

                        run1Head.Append(text1Head);

                        paraTransactionNumberFormattedHead.Append(paraPropertiesTransactionNumberFormattedHead);
                        paraTransactionNumberFormattedHead.Append(run1Head);

                        TableCell tcTransactionNumberFormattedHead = new TableCell();
                        tcTransactionNumberFormattedHead.Append(paraTransactionNumberFormattedHead);
                        #endregion

                        #region  - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraSubjectHead = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesSubjectHead = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationSubjectHead = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesSubjectHead.Append(justificationSubjectHead);

                        DocumentFormat.OpenXml.Wordprocessing.Run run2Head = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text2Head = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text2Head.Text = "المكلف الرئيسى";
                        var run2HeadProp = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run2Head.Append(run2HeadProp);
                        run2Head.Append(text2Head);

                        paraSubjectHead.Append(paraPropertiesSubjectHead);
                        paraSubjectHead.Append(run2Head);

                        TableCell tcSubjectHead = new TableCell();
                        tcSubjectHead.Append(paraSubjectHead);
                        #endregion


                        #region Mulitmission - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraFromAnyHead = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesFromAnyHead = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationFromAnyHead = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesFromAnyHead.Append(justificationFromAnyHead);

                        DocumentFormat.OpenXml.Wordprocessing.Run run3Head = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text3Head = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text3Head.Text = "مهام فرعية";

                        var run3HeadProp = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run3Head.Append(run3HeadProp);

                        run3Head.Append(text3Head);

                        paraFromAnyHead.Append(paraPropertiesFromAnyHead);
                        paraFromAnyHead.Append(run3Head);

                        TableCell tcFromAnyHead = new TableCell();
                        tcFromAnyHead.Append(paraFromAnyHead);
                        #endregion

                        #region EndData - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraToOrganizationHead = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesToOrganizationHead = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationToOrganizationHead = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesToOrganizationHead.Append(justificationToOrganizationHead);

                        DocumentFormat.OpenXml.Wordprocessing.Run run4Head = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text4Head = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text4Head.Text = "نهاية التكليف";

                        var run4HeadProp = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run4Head.Append(run4HeadProp);

                        run4Head.Append(text4Head);

                        paraToOrganizationHead.Append(paraPropertiesToOrganizationHead);
                        paraToOrganizationHead.Append(run4Head);

                        TableCell tcToOrganizationNameHead = new TableCell();
                        tcToOrganizationNameHead.Append(paraToOrganizationHead);
                        #endregion

                        #region CompeletedTask - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraStatusNameHead = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesStatusNameHead = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationStatusNameHead = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesStatusNameHead.Append(justificationStatusNameHead);

                        DocumentFormat.OpenXml.Wordprocessing.Run run5Head = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text5Head = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text5Head.Text = "تمت المهمة";

                        var run5HeadProp = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run5Head.Append(run5HeadProp);

                        run5Head.Append(text5Head);

                        paraStatusNameHead.Append(paraPropertiesStatusNameHead);
                        paraStatusNameHead.Append(run5Head);

                        TableCell tcStatusNameHead = new TableCell();
                        tcStatusNameHead.Append(paraStatusNameHead);
                        #endregion

                        #region Attachment - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraStatusNameHeadAttachment = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesStatusNameHeadAttachment = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationStatusNameHeadAttachment = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesStatusNameHeadAttachment.Append(justificationStatusNameHeadAttachment);

                        DocumentFormat.OpenXml.Wordprocessing.Run run5HeadAttachment = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text5HeadAttachment = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text5HeadAttachment.Text = "المرفقات";


                        var run5HeadPropAttachment = new RunProperties()
                        {
                            RightToLeftText = new RightToLeftText()
                            {
                                Val = new OnOffValue(true)
                            }
                        };

                        run5HeadAttachment.Append(run5HeadPropAttachment);

                        run5HeadAttachment.Append(text5HeadAttachment);

                        paraStatusNameHeadAttachment.Append(paraPropertiesStatusNameHeadAttachment);
                        paraStatusNameHeadAttachment.Append(run5HeadAttachment);

                        TableCell tcStatusNameHeadAttachment = new TableCell();
                        tcStatusNameHeadAttachment.Append(paraStatusNameHeadAttachment);
                        #endregion
                        #region Number - Head
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph paraNumberHead = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesNumberHead = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationNumberHead = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesNumberHead.Append(justificationNumberHead);

                        DocumentFormat.OpenXml.Wordprocessing.Run run7Head = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text7Head = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text7Head.Text = "#";
                        run7Head.Append(text7Head);

                        paraNumberHead.Append(paraPropertiesNumberHead);
                        paraNumberHead.Append(run7Head);

                        TableCell tcNumberHead = new TableCell();
                        tcNumberHead.Append(paraNumberHead);
                        #endregion

                        // Add the cells to the row
                        trHead.Append(tcNumberHead, tcTransactionNumberFormattedHead, tcSubjectHead, tcFromAnyHead, tcToOrganizationNameHead, tcStatusNameHead, tcStatusNameHeadAttachment);

                        // Add the rows to the table
                        tbl.AppendChild(trHead);


                        #region Append data
                        for (int i = 0; i < gridDataResult.Count; i++)
                        {


                            List<object> objData = new List<object>()
                            {
                                (i + 1).ToString(),
                                gridDataResult[i].Title,
                                gridDataResult[i].MainAssinedUser.FullNameAr,
                                gridDataResult[i].MultiMission.Count == 0 ?"لا يوجد" : "يوجد",
                                gridDataResult[i].EndDate.DateTime.ToString("yyyy-MM-dd"),
                                !gridDataResult[i].Completed ? "لا" : "نعم",
                                gridDataResult[i].TaskAttachments.Count == 0 ? "لا يوجد" : "يوجد",

                            };
                            // Create a new row

                            #region function
                            //int index = 1;
                            TableRow tr = new TableRow();
                            try
                            {

                                for (int ii = 0; ii < objData.Count; ii++)
                                {
                                    DocumentFormat.OpenXml.Wordprocessing.Paragraph paraTransactionNumberFormatted = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                                    DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesTransactionNumberFormatted = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                                    Justification justificationTransactionNumberFormatted = new Justification() { Val = JustificationValues.Center };
                                    paraPropertiesTransactionNumberFormatted.Append(justificationTransactionNumberFormatted);

                                    DocumentFormat.OpenXml.Wordprocessing.Run run1 = new DocumentFormat.OpenXml.Wordprocessing.Run();
                                    DocumentFormat.OpenXml.Wordprocessing.Text text1 = new DocumentFormat.OpenXml.Wordprocessing.Text();
                                    text1.Text = objData[ii] != null ? objData[ii].ToString() : "";

                                    var run1Prop = new RunProperties()
                                    {
                                        RightToLeftText = new RightToLeftText()
                                        {
                                            Val = new OnOffValue(true)
                                        }
                                    };

                                    run1.Append(run1Prop);
                                    run1.Append(text1);

                                    paraTransactionNumberFormatted.Append(paraPropertiesTransactionNumberFormatted);

                                    //if (index == ii)
                                    //{
                                    //    //System.Uri uri = new Uri(URL, UriKind.Absolute);
                                    //   // HyperlinkRelationship rel = mainPart.AddHyperlinkRelationship(uri, true);
                                    //    string relationshipId = rel.Id;

                                    //    Hyperlink hyperlink = new Hyperlink(run1)
                                    //    {
                                    //        History = OnOffValue.FromBoolean(true),
                                    //        Id = relationshipId
                                    //    };
                                    //    paraTransactionNumberFormatted.Append(hyperlink);
                                    //}
                                    //else
                                    // {
                                    paraTransactionNumberFormatted.Append(run1);
                                    // }

                                    TableCell tcTransactionNumberFormatted = new TableCell();
                                    tcTransactionNumberFormatted.Append(paraTransactionNumberFormatted);

                                    // Add the cells to the row
                                    tr.Append(tcTransactionNumberFormatted);

                                }
                                // return tr;
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                            #endregion
                            // TableRow tr = RowData(objData, 1, $"{returnURL}/registrations/viewonly/{gridDataResult[i].TransactionIdEncrypt}", mainPart);
                            // Add the rows to the table
                            tbl.AppendChild(tr);
                        }


                        //Change Page Margin
                        SectionProperties sectionProps = new SectionProperties();
                        PageMargin pageMargin = new PageMargin() { Top = 1008, Right = (UInt32Value)500U, Bottom = 1008, Left = (UInt32Value)500U, Header = (UInt32Value)720U, Footer = (UInt32Value)120U, Gutter = (UInt32Value)0U };
                        sectionProps.Append(pageMargin);
                        body.Append(sectionProps);

                        // Add the table to the body
                        body.AppendChild(tbl1);

                        #region paragraph Second
                        //Header Paragraph
                        DocumentFormat.OpenXml.Wordprocessing.Paragraph HeaderParagraph2 = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraPropertiesHeaderParagraph2 = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        Justification justificationHeaderParagraph2 = new Justification() { Val = JustificationValues.Center };
                        paraPropertiesHeaderParagraph2.Append(justificationHeaderParagraph2);

                        StyleRunProperties styleRunProperties12 = new StyleRunProperties();
                        Bold bold12 = new Bold();

                        // Specify a 12 point size.
                        DocumentFormat.OpenXml.Wordprocessing.FontSize fontSize12 = new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "24" };
                        styleRunProperties12.Append(bold12);
                        styleRunProperties12.Append(fontSize12);

                        DocumentFormat.OpenXml.Wordprocessing.Run run1HeaderParagraph2 = new DocumentFormat.OpenXml.Wordprocessing.Run();
                        DocumentFormat.OpenXml.Wordprocessing.Text text1HeaderParagraph2 = new DocumentFormat.OpenXml.Wordprocessing.Text();
                        text1HeaderParagraph2.Text = "تفاصيل المهام ";// LocalizationRepository.GetByKey("EmployeesReports", culture);
                        run1HeaderParagraph2.Append(styleRunProperties12);
                        run1HeaderParagraph2.Append(text1HeaderParagraph2);
                        BiDi bidi2 = new BiDi();
                        paraPropertiesHeaderParagraph.Append(bidi2);
                        HeaderParagraph2.Append(paraPropertiesHeaderParagraph2);
                        HeaderParagraph2.Append(run1HeaderParagraph2);

                        body.AppendChild(HeaderParagraph2);
                        #endregion
                        body.AppendChild(tbl);

                        mainPart.Document.Save();

                    }
                    return mem.ToArray();
                    #endregion


                }
            }


            #endregion
            else
            {
                #region Excel
                int currentRowNumber = 1;
                var workbook = new XLWorkbook();     //creates the workbook
                var wsDetailedData = workbook.AddWorksheet("expired documents"); //creates the worksheet with sheetname 'data'
                wsDetailedData.Style.Font.FontSize = 11;

                XLBorderStyleValues xlBorderStyleValues = XLBorderStyleValues.Medium;

                for (int i = 3; i < 9; i++)
                {
                    wsDetailedData.Cell(i, 13).Style.Border.OutsideBorder = xlBorderStyleValues;
                    wsDetailedData.Range(wsDetailedData.Cell(i, 14), wsDetailedData.Cell(i, 15)).Style.Border.OutsideBorder = xlBorderStyleValues;
                    wsDetailedData.Range(wsDetailedData.Cell(i, 14), wsDetailedData.Cell(i, 15)).Merge();
                }

                string AllTasks = "كل المهام";//resultCount[0].Name;
                string LateTasks = "مهام مُتاخره";// resultCount[1].Name;
                string ToBeImplementdTasks = "تحت الاجراء";// resultCount[2].Name;
                string ClosedTasks = "كل المغلق"; //resultCount[3].Name;
                string AssistantUserTasks = "تكليف فرعى";// resultCount[4].Name;
                string TaskToView = "مهام للاطلاع";//resultCount[5].Name;

                string AllTasksCount = resultCount[0].Count.ToString();
                string LateTasksCount = resultCount[1].Count.ToString();
                string ToBeImplementdTasksCount = resultCount[2].Count.ToString();
                string ClosedTasksCount = resultCount[3].Count.ToString();
                string AssistantUserTasksCount = resultCount[4].Count.ToString();
                string TaskToViewCount = resultCount[5].Count.ToString();


                wsDetailedData.Cell(3, 14).RichText.AddText(AllTasks);
                wsDetailedData.Cell(3, 13).RichText.AddText(AllTasksCount);
                wsDetailedData.Cell(4, 14).RichText.AddText(LateTasks);
                wsDetailedData.Cell(4, 13).RichText.AddText(LateTasksCount);
                wsDetailedData.Cell(5, 14).RichText.AddText(ToBeImplementdTasks);
                wsDetailedData.Cell(5, 13).RichText.AddText(ToBeImplementdTasksCount);
                wsDetailedData.Cell(6, 14).RichText.AddText(ClosedTasks);
                wsDetailedData.Cell(6, 13).RichText.AddText(ClosedTasksCount);
                wsDetailedData.Cell(7, 14).RichText.AddText(AssistantUserTasks);
                wsDetailedData.Cell(7, 13).RichText.AddText(AssistantUserTasksCount);
                wsDetailedData.Cell(8, 14).RichText.AddText(TaskToView);
                wsDetailedData.Cell(8, 13).RichText.AddText(TaskToViewCount);


                currentRowNumber = currentRowNumber + 10;
                wsDetailedData.Range(wsDetailedData.Cell(currentRowNumber, 14), wsDetailedData.Cell(currentRowNumber, 15)).Merge();
                wsDetailedData.Range(wsDetailedData.Cell(currentRowNumber, 16), wsDetailedData.Cell(currentRowNumber, 17)).Merge();


                wsDetailedData.Cell(currentRowNumber, 16).Value = "عنوان التكليف";
                wsDetailedData.Cell(currentRowNumber, 16).Style.Font.Bold = true;
                wsDetailedData.Cell(currentRowNumber, 16).Style.Fill.BackgroundColor = XLColor.Gray;
                wsDetailedData.Cell(currentRowNumber, 16).Style.Font.FontColor = XLColor.White;
                wsDetailedData.Cell(currentRowNumber, 16).WorksheetColumn().AdjustToContents();

                wsDetailedData.Cell(currentRowNumber, 14).Value = "المكلف الرئيسى";
                wsDetailedData.Cell(currentRowNumber, 14).Style.Font.Bold = true;
                wsDetailedData.Cell(currentRowNumber, 14).Style.Fill.BackgroundColor = XLColor.Gray;
                wsDetailedData.Cell(currentRowNumber, 14).Style.Font.FontColor = XLColor.White;
                wsDetailedData.Cell(currentRowNumber, 14).WorksheetColumn().AdjustToContents();

                wsDetailedData.Cell(currentRowNumber, 13).Value = "مهام فرعية";
                wsDetailedData.Cell(currentRowNumber, 13).Style.Font.Bold = true;
                wsDetailedData.Cell(currentRowNumber, 13).Style.Fill.BackgroundColor = XLColor.Gray;
                wsDetailedData.Cell(currentRowNumber, 13).Style.Font.FontColor = XLColor.White;
                wsDetailedData.Cell(currentRowNumber, 13).WorksheetColumn().AdjustToContents();

                wsDetailedData.Cell(currentRowNumber, 12).Value = "نهاية التكليف";
                wsDetailedData.Cell(currentRowNumber, 12).Style.Font.Bold = true;
                wsDetailedData.Cell(currentRowNumber, 12).Style.Fill.BackgroundColor = XLColor.Gray;
                wsDetailedData.Cell(currentRowNumber, 12).Style.Font.FontColor = XLColor.White;
                wsDetailedData.Cell(currentRowNumber, 12).WorksheetColumn().AdjustToContents();

                wsDetailedData.Cell(currentRowNumber, 11).Value = "تمت المهمة";
                wsDetailedData.Cell(currentRowNumber, 11).Style.Font.Bold = true;
                wsDetailedData.Cell(currentRowNumber, 11).Style.Fill.BackgroundColor = XLColor.Gray;
                wsDetailedData.Cell(currentRowNumber, 11).Style.Font.FontColor = XLColor.White;
                wsDetailedData.Cell(currentRowNumber, 11).WorksheetColumn().AdjustToContents();

                wsDetailedData.Cell(currentRowNumber, 10).Value = "المرفقات";
                wsDetailedData.Cell(currentRowNumber, 10).Style.Font.Bold = true;
                wsDetailedData.Cell(currentRowNumber, 10).Style.Fill.BackgroundColor = XLColor.Gray;
                wsDetailedData.Cell(currentRowNumber, 10).Style.Font.FontColor = XLColor.White;
                wsDetailedData.Cell(currentRowNumber, 10).WorksheetColumn().AdjustToContents();

                currentRowNumber++;

                for (int i = 0; i < gridDataResult.Count; i++)
                {


                    wsDetailedData.Range(wsDetailedData.Cell(i + currentRowNumber, 14), wsDetailedData.Cell(i + currentRowNumber, 15)).Merge();
                    wsDetailedData.Range(wsDetailedData.Cell(i + currentRowNumber, 16), wsDetailedData.Cell(i + currentRowNumber, 17)).Merge();


                    wsDetailedData.Cell(i + currentRowNumber, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    wsDetailedData.Cell(i + currentRowNumber, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    wsDetailedData.Cell(i + currentRowNumber, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    wsDetailedData.Cell(i + currentRowNumber, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    wsDetailedData.Cell(i + currentRowNumber, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


                    wsDetailedData.Cell(i + currentRowNumber, 16).Value = gridDataResult[i].Title;
                    wsDetailedData.Cell(i + currentRowNumber, 14).Value = gridDataResult[i].MainAssinedUser.FullNameAr;
                    wsDetailedData.Cell(i + currentRowNumber, 13).Value = gridDataResult[i].MultiMission.Count == 0 ? "لا يوجد" : "يوجد";
                    wsDetailedData.Cell(i + currentRowNumber, 12).Value = gridDataResult[i].EndDate.ToString("yyyy-MM-dd");//DateTime.ToShortTimeString();
                    wsDetailedData.Cell(i + currentRowNumber, 11).Value = !gridDataResult[i].Completed ? "لا" : "نعم";
                    wsDetailedData.Cell(i + currentRowNumber, 10).Value = gridDataResult[i].TaskAttachments.Count == 0 ? "لا يوجد" : "يوجد";


                    //wsDetailedData.Cell(i, 5).Value = excelSheetParams.StatementRecordList[i].Total;


                }

                filePath = _hostingEnvironment.ContentRootPath + "\\ExportedFiles\\" + DateTime.Now.ToFileTimeUtc() + ".xlsx";

                workbook.SaveAs(filePath); //saves the workbook
                #endregion

                byte[] fileBytes = File.ReadAllBytes(filePath);

                return fileBytes;
            }
        }
        public List<CommiteeTaskDTO> GetAllForCalender(int? CommiteeId, int? ComiteeTaskCategoryId)
        {
            var query = this._UnitOfWork.GetRepository<CommiteeTask>().GetAll().Include(x => x.Meeting).OrderByDescending(x => x.CreatedOn).Where(x =>

           ((CommiteeId == null || x.CommiteeId == Convert.ToInt32(CommiteeId.Value)) && (ComiteeTaskCategoryId == null || x.ComiteeTaskCategoryId == Convert.ToInt32(ComiteeTaskCategoryId)) &&
           /*(x.IsShared && CommiteeId != null) ||*/ x.MainAssinedUserId == _sessionServices.UserId));

            if (CommiteeId.HasValue)
            {
                query = query.Where(x => x.CommiteeId == CommiteeId.Value);
            }

            var dta = _Mapper.Map<List<CommiteeTaskDTO>>(query?.ToList());

            return dta;

        }
        public List<CountResultDTO> getComitteeTaskStatistics(int? organizationId, int? userId, int? committeeId, DateTime? ValiditayPeriodFrom, DateTime? ValidatiyPeriodTo)
        {
            if (organizationId != null)
            {
                // 1- Get All Child Organization 2- Get All User -3 Union All Statistic 
                var returnFromStoredEnumerable = _context.vm_EmpInOrgaHierarchies.FromSqlInterpolated($"exec [dbo].[sp_GetEmployeesByOrganizationHierarchyToReport] {organizationId}").ToList();
                // var res = returnFromStoredEnumerable.AsEnumerable().ToDataSourceResult(dataSourceRequest, true).Data.ToList();

                //var x = returnFromStoredEnumerable.AsQueryable();
                List<LookUpDTO> lookupUser = returnFromStoredEnumerable.AsQueryable().Select(x => new LookUpDTO
                {
                    Id = x.Id,
                    Name = _sessionServices.CultureIsArabic ? x.FullNameAr : x.FullNameEn
                }).ToList();

                // Get Union Statistic
                List<CountResultDTO> countResultFromUnion = new List<CountResultDTO>();
                var TaskOrg = new Dictionary<string, int>();
                TaskOrg["AllTasks"] = 0;
                TaskOrg["LateTasks"] = 0;
                TaskOrg["ToBeImplementdTasks"] = 0;
                TaskOrg["ClosedTasks"] = 0;
                TaskOrg["AssistantUserTasks"] = 0;
                TaskOrg["TaskToView"] = 0;

                foreach (var item in lookupUser)
                {
                    var allTasksOrg = _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                         .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                         .Where(x =>
                     (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&

                     ((x.IsShared && committeeId != null) || x.MainAssinedUserId == item.Id ||
                     x.AssistantUsers.Any(z => z.UserId == item.Id) ||
                     x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == item.Id)) ||
                     x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == item.Id)) ||

                     x.CreatedBy == item.Id)).Count();
                    TaskOrg["AllTasks"] += allTasksOrg;
                    //countResultFromUnion.Add(new CountResultDTO { Name = "AllTasks", Count = allTasksOrg });


                    var lateTasksOrg = _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                         .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                        .Where(x =>
                     x.EndDate <= DateTimeOffset.Now && x.MainAssinedUserId == item.Id && !x.Completed &&
                     (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&

                     ((x.IsShared && committeeId != null) || x.MainAssinedUserId == item.Id)
                     ).Count();
                    //countResultFromUnion.Add(new CountResultDTO { Name = "LateTasks", Count = lateTasksOrg });
                    TaskOrg["LateTasks"] += lateTasksOrg;

                    var ToBeImplementdTasksOrg = _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                    .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                    .Where(x => !x.Completed && (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&

                    (x.MainAssinedUserId == item.Id)).Count();
                    //countResultFromUnion.Add(new CountResultDTO { Name = "ToBeImplementdTasks", Count = ToBeImplementdTasksOrg });
                    TaskOrg["ToBeImplementdTasks"] += ToBeImplementdTasksOrg;

                    var closedTasksOrg = _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                         .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                         .Where(x => x.Completed && x.MainAssinedUserId == item.Id &&
                    (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&

                    ((x.IsShared && committeeId != null) || x.MainAssinedUserId == item.Id)).Count();
                    //countResultFromUnion.Add(new CountResultDTO { Name = "ClosedTasks", Count = closedTasksOrg });
                    TaskOrg["ClosedTasks"] += closedTasksOrg;

                    var assistantTasksOrg = _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)

                         .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))

                        .Where(x =>

                     (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&
                     (x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(a => a.UserId == item.Id)) ||

                     (x.AssistantUsers.Any(z => z.UserId == item.Id)))).Count();
                    //countResultFromUnion.Add(new CountResultDTO { Name = "AssistantUserTasks", Count = assistantTasksOrg });
                    TaskOrg["AssistantUserTasks"] += assistantTasksOrg;

                    var TaskToViewOrg = _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                         .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                         .Where(x =>

                    (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&
                    (x.MainAssinedUserId != item.Id
                    && x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == item.Id)) && x.CreatedBy != item.Id)).Count();
                    //countResultFromUnion.Add(new CountResultDTO { Name = "TaskToView", Count = TaskToViewOrg });
                    TaskOrg["TaskToView"] += TaskToViewOrg;


                }
                foreach (var itemInDictionary in TaskOrg)
                {
                    countResultFromUnion.Add(new CountResultDTO { Name = itemInDictionary.Key, Count = itemInDictionary.Value });
                }
                return countResultFromUnion;

            }
            var result = new List<CountResultDTO>();


            var allTasks = userId == null ? _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                 .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                 .Where(x =>
             (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&
             // (userId == null || x.AssistantUsers.Any(z => z.UserId == userId) || x.MainAssinedUser.UserId == userId) &&
             ((x.IsShared && committeeId != null) || x.MainAssinedUserId == _sessionServices.UserId ||
             x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId) ||
             x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == _sessionServices.UserId)) ||
             x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == _sessionServices.UserId)) ||
             x.CreatedBy == _sessionServices.UserId)).Count()
             : _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                 .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                 .Where(x =>
             (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&
             //(userId == null || x.AssistantUsers.Any(z => z.UserId == userId) || x.MainAssinedUser.UserId == userId) &&
             ((x.IsShared && committeeId != null) || x.MainAssinedUserId == userId ||
             x.AssistantUsers.Any(z => z.UserId == userId) ||
             x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == userId)) ||
             x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(w => w.UserId == userId)) ||

             x.CreatedBy == userId)).Count();

            result.Add(new CountResultDTO { Name = "AllTasks", Count = allTasks });

            var lateTasks = userId == null ? _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                  .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                 .Where(x =>
              x.EndDate <= DateTimeOffset.Now && x.MainAssinedUserId == _sessionServices.UserId && !x.Completed &&
              (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&

              ((x.IsShared && committeeId != null) || x.MainAssinedUserId == _sessionServices.UserId)

              //x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
              ).Count()

              : _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                  .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                 .Where(x =>
              x.EndDate <= DateTimeOffset.Now && x.MainAssinedUserId == userId && !x.Completed &&
              (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&

              ((x.IsShared && committeeId != null) || x.MainAssinedUserId == userId)//&&
                                                                                    //(userId == null || x.AssistantUsers.Any(z => z.UserId == userId) || x.MainAssinedUser.UserId == userId) ||
                                                                                    // x.AssistantUsers.Any(z => z.UserId == userId) ||x.CreatedBy == userId)
              ).Count();
            result.Add(new CountResultDTO { Name = "LateTasks", Count = lateTasks });

            var ToBeImplementdTasks = userId == null ? _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                 .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                .Where(x =>
            !x.Completed &&
            (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&
            // (userId == null || x.AssistantUsers.Any(z => z.UserId == userId) || x.MainAssinedUser.UserId == userId) &&
            (x.MainAssinedUserId == _sessionServices.UserId)).Count()

            : _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                 .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                .Where(x =>
            !x.Completed &&
            (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&
            // (userId == null || x.AssistantUsers.Any(z => z.UserId == userId) || x.MainAssinedUser.UserId == userId) &&
            (x.MainAssinedUserId == userId)).Count();
            result.Add(new CountResultDTO { Name = "ToBeImplementdTasks", Count = ToBeImplementdTasks });

            var closedTasks = userId == null ? _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                 .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                 .Where(x => x.Completed && x.MainAssinedUserId == _sessionServices.UserId &&
            (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&
            //(userId == null || x.AssistantUsers.Any(z => z.UserId == userId) || x.MainAssinedUser.UserId == userId) &&
            ((x.IsShared && committeeId != null) || x.MainAssinedUserId == _sessionServices.UserId)).Count()

            : _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                 .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                 .Where(x =>
            x.Completed && x.MainAssinedUserId == userId &&
            (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&
            //(userId == null || x.AssistantUsers.Any(z => z.UserId == userId) || x.MainAssinedUser.UserId == userId) &&
            ((x.IsShared && committeeId != null) || x.MainAssinedUserId == userId)).Count();
            result.Add(new CountResultDTO { Name = "ClosedTasks", Count = closedTasks });

            var assistantTasks = userId == null ? _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)

                 .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))

                .Where(x => (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&
             (x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(a => a.UserId == _sessionServices.UserId)) ||
             // (userId == null || x.AssistantUsers.Any(z => z.UserId == userId) || x.MainAssinedUser.UserId == userId) &&
             (x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId)))).Count()

             : _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)

                 .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))

                .Where(x => (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&
             (x.MultiMission.Any(a => a.CommiteeTaskMultiMissionUsers.Any(a => a.UserId == userId)) ||

             //(userId == null || x.AssistantUsers.Any(z => z.UserId == userId) || x.MainAssinedUser.UserId == userId) &&
             (x.AssistantUsers.Any(z => z.UserId == userId)))).Count();
            result.Add(new CountResultDTO { Name = "AssistantUserTasks", Count = assistantTasks });

            var TaskToView = userId == null ? _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                 .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                 .Where(x =>

            (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&
            //(userId == null || x.AssistantUsers.Any(z => z.UserId == userId) || x.MainAssinedUser.UserId == userId) &&

            (x.MainAssinedUserId != _sessionServices.UserId
            && x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == _sessionServices.UserId)) && x.CreatedBy != _sessionServices.UserId)).Count()

                       : _UnitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CreatedOn)
                 .Where(x => (ValiditayPeriodFrom == null || x.CreatedOn >= ValiditayPeriodFrom) && (ValidatiyPeriodTo == null || x.CreatedOn <= ValidatiyPeriodTo))
                 .Where(x =>

            (committeeId == null || x.CommiteeId == Convert.ToInt32(committeeId)) &&
            //(userId == null || x.AssistantUsers.Any(z => z.UserId == userId) || x.MainAssinedUser.UserId == userId) &&

            (x.MainAssinedUserId != userId
            && x.TaskGroups.Any(a => a.Group.GroupUsers.Any(w => w.UserId == userId)) && x.CreatedBy != userId)).Count();
            result.Add(new CountResultDTO { Name = "TaskToView", Count = TaskToView });



            return result;
        }
        public override IEnumerable<CommiteeTaskDTO> Insert(IEnumerable<CommiteeTaskDTO> entities)
        {
            IQueryable<CommiteeUsersRole> CreatedByRole = null;
            foreach (var item in entities)
            {

                if (!string.IsNullOrEmpty(item.CommiteeIdEncrypted))
                {

                    item.CommiteeIdEncrypted = _sessionServices.UserIdAndRoleIdAfterDecrypt(item.CommiteeIdEncrypted, false).Id.ToString();
                    item.CommiteeId = int.Parse(item.CommiteeIdEncrypted);
                }
                if (item.CommiteeId.HasValue)
                {

                    CreatedByRole = _unitOfWork.GetRepository<CommiteeUsersRole>().GetAll().Where(x => x.UserId == _sessionServices.UserId && x.CommiteeId == item.CommiteeId);
                    CommiteeRole role = _UnitOfWork.GetRepository<CommiteeRole>().GetAll().Where(x => x.CommiteeRoleId == CreatedByRole.FirstOrDefault().RoleId).FirstOrDefault();
                    item.CreatedByRoleId = CreatedByRole.FirstOrDefault().RoleId;

                    //item.CreatedByRole = CreatedByRole.Select(x => new CommiteeDetailsUsersRoleDTO
                    //{
                    //    CommiteeUsersRoleId = x.CommiteeUsersRoleId,
                    //    Role = new CommiteeDetailsRoleDTO
                    //    {
                    //        CommiteeRoleId = (int)item.CreatedByRoleId,
                    //        CommiteeRolesNameAr = role.CommiteeRolesNameAr,
                    //        CommiteeRolesNameEn = role.CommiteeRolesNameEn
                    //    },
                    //}).FirstOrDefault();
                }

                item.CreatedOn = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero);
                item.CreatedBy = _sessionServices.UserId;


            }

            // create DTo and Inserted into from insertedEntity and entity

            var insertedEntity = base.Insert(entities);
            var x = new List<int>();




            foreach (var item in insertedEntity)
            {
                item.CreatedOn = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero);
                foreach (var itemInMission in item.MultiMission)
                {
                    x.Add(itemInMission.CommiteeTaskMultiMissionId);
                }

            }


            var objeCompositeFromIncomingAndInserted = new List<CommiteetaskMultiMissionDTO>();
            int i = 0;
            foreach (var item in entities)
            {
                foreach (var mission in item.MultiMission)
                {
                    for (; i < x.Count; i++)
                    {
                        mission.CommiteeTaskMultiMissionId = x[i];
                        i++;
                        objeCompositeFromIncomingAndInserted.Add(mission);
                        break;
                    }

                }
            }

            foreach (var item in entities)
            {
                item.MultiMission = objeCompositeFromIncomingAndInserted;
                // item.CommiteeId = _sessionServices.UserIdAndRoleIdAfterDecrypt(item.CommiteeId).Id.ToString();

                item.ComiteeTaskCategory = _Mapper.Map<ComiteeTaskCategoryDTO>(_UnitOfWork.GetRepository<ComiteeTaskCategory>().GetAll()
                    .Where(x => x.ComiteeTaskCategoryId == item.ComiteeTaskCategoryId).FirstOrDefault());


                item.MainAssinedUser = _UnitOfWork.GetRepository<User>().GetAll().Select(x => new UserDetailsDTO
                {
                    UserId = x.UserId,
                    UserName = x.Username,
                    FullNameAr = x.FullNameAr,
                    FullNameEn = x.FullNameEn,
                    ProfileImage = x.ProfileImage,
                }).FirstOrDefault(z => z.UserId == item.MainAssinedUserId);
                //item.CreatedByRole = _UnitOfWork.GetRepository<CommiteeUsersRole>().GetAll().Select(x => new CommiteeDetailsUsersRoleDTO
                //{
                //    CommiteeUsersRoleId = x.CommiteeUsersRoleId,
                //    Role = new CommiteeDetailsRoleDTO
                //    {
                //        CommiteeRoleId = x.Role.CommiteeRoleId,
                //        CommiteeRolesNameAr = x.Role.CommiteeRolesNameAr,
                //        CommiteeRolesNameEn = x.Role.CommiteeRolesNameEn
                //    },
                //}).FirstOrDefault(x => x.CommiteeUsersRoleId == item.CreatedByRoleId);
                //item.CreatedByRole = _UnitOfWork.GetRepository<CommiteeUsersRole>().GetAll().Select(x => new CommiteeDetailsUsersRoleDTO
                //{
                //    CommiteeUsersRoleId = CreatedByRole.CommiteeUsersRoleId,
                //    Role = new CommiteeDetailsRoleDTO
                //    {
                //        CommiteeRoleId = (int)item.CreatedByRoleId,

                //    },
                //}).FirstOrDefault();
                foreach (var item2 in item.AssistantUsers)
                {
                    item2.User = _UnitOfWork.GetRepository<User>().GetAll().Select(x => new UserDetailsDTO
                    {
                        UserId = x.UserId,
                        UserName = x.Username,
                        FullNameAr = x.FullNameAr,
                        FullNameEn = x.FullNameEn,
                        ProfileImage = x.ProfileImage,
                    }).FirstOrDefault(z => z.UserId == item2.UserId);
                }
                // For Group
                foreach (var itemgroup in item.TaskGroups)
                {
                    itemgroup.Group = _UnitOfWork.GetRepository<Group>().GetAll().Select(x => new GroupDto
                    {
                        GroupId = x.GroupId,
                        GroupNameAr = x.GroupNameAr,
                        GroupNameEn = x.GroupNameEn,
                        CreatedBy = x.CreatedBy,
                        GroupUsers = (ICollection<GroupUsersDto>)x.GroupUsers.Select(w => new GroupUsersDto
                        {
                            UserId = w.UserId,
                            GroupId = w.GroupId,
                            GroupUsersId = w.GroupUsersId,
                            userDetailsDTO = new UserDetailsDTO()
                            {
                                UserId = w.User.UserId,
                                UserName = w.User.Username,
                                FullNameAr = w.User.FullNameAr,
                                FullNameEn = w.User.FullNameEn,
                            }
                        })
                    }).FirstOrDefault(z => z.GroupId == itemgroup.GroupId);
                }


                //For CommiteeTaskMultiMission


                // category = (Categoies)AutoMapper.Mapper.Map(viewModel, category, typeof(CategoriesViewModel), typeof(Categoies));
                //_Mapper.Map< CommiteeTaskMultiMissionUserDTO,CommiteeTaskMultiMissionUser>(itemUserInMission);

                foreach (var itemMission in item.MultiMission)
                {
                    foreach (var itemss in itemMission.CommiteeTaskMultiMissionUserDTOs)
                    {
                        itemss.CommiteeTaskMultiMissionId = itemMission.CommiteeTaskMultiMissionId;

                        itemss.UserDetailsDto = _UnitOfWork.GetRepository<User>().GetAll().Select(x => new UserDetailsDTO
                        {
                            UserId = x.UserId,
                            UserName = x.Username,
                            FullNameAr = x.FullNameAr,
                            FullNameEn = x.FullNameEn,
                            ProfileImage = x.ProfileImage,
                        }).FirstOrDefault(z => z.UserId == itemss.UserId);
                        var v = _Mapper.Map(itemss, typeof(CommiteeTaskMultiMissionUserDTO), typeof(CommiteeTaskMultiMissionUser)) as CommiteeTaskMultiMissionUser;
                        this._UnitOfWork.GetRepository<CommiteeTaskMultiMissionUser>().Insert(v);
                        //break;
                    }

                    // _unitOfWork.GetRepository<CommiteeTaskMultiMissionUser>().Insert(itemMission.CommiteeTaskMultiMissionUserDTOs);
                    //var TDbEntities = itemMission.CommiteeTaskMultiMissionUserDTOs.AsQueryable().ProjectTo<CommiteeTaskMultiMissionUser>(_Mapper.ConfigurationProvider, _sessionServices).ToList();
                }

                item.CreatedByUser = _UnitOfWork.GetRepository<User>().GetAll().Select(x => new UserDetailsDTO
                {
                    UserId = x.UserId,
                    UserName = x.Username,
                    FullNameAr = x.FullNameAr,
                    FullNameEn = x.FullNameEn,
                    ProfileImage = x.ProfileImage,
                }).FirstOrDefault(z => z.UserId == _sessionServices.UserId);
                var loc = _unitOfWork.GetRepository<CommiteeLocalization>().GetAll().FirstOrDefault(x => x.Key == "AddNewTaskNotificationText");
                var u = _UnitOfWork.GetRepository<User>().GetAll().Select(x => new UserDetailsDTO
                {
                    UserId = x.UserId,
                    UserName = x.Username,
                    FullNameAr = x.FullNameAr,
                    FullNameEn = x.FullNameEn,
                    ProfileImage = x.ProfileImage,
                    Email = x.Email,
                    Mobile = x.Mobile

                }).FirstOrDefault(z => z.UserId == item.MainAssinedUserId);
                if (item.MainAssinedUserId != _sessionServices.UserId)
                {
                    CommitteeNotificationDTO committeeNotification = new CommitteeNotificationDTO
                    {
                        IsRead = false,
                        UserId = (int)item.MainAssinedUserId,
                        TextAR = loc.CommiteeLocalizationAr + " " + item.Title,
                        TextEn = loc.CommiteeLocalizationEn + " " + item.Title,
                        CommiteeTaskId = insertedEntity.FirstOrDefault()?.CommiteeTaskId,
                        CommiteeId = item.CommiteeId
                    };
                    if (item.IsNotification)
                    {
                        List<CommitteeNotificationDTO> committeeNotifications = new List<CommitteeNotificationDTO> { committeeNotification };
                        _committeeNotificationService.Insert(committeeNotifications);
                    }
                    if (item.IsEmail)
                    {
                        string Message = "";
                        string mailSubject = "";
                        var createdTaskTitle = _commiteeLocalizationService.GetLocaliztionByCode("CreatedTask", _sessionServices.CultureIsArabic);
                        getMailMessage(item, ref Message, ref mailSubject, createdTaskTitle);
                        AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                        Task.Run(() =>
                        {
                            mailServices.SendNotificationEmail(u.Email, mailSubject,
                                null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                                );

                        });
                    }
                    if (item.IsSMS && !string.IsNullOrEmpty(u.Mobile))
                    {
                        smsServices.SendSMS(u.Mobile, new string[0] { }, loc.CommiteeLocalizationAr + " " + item.Title, null);
                    }
                }

                if (item.IsShared)
                {

                    var committemebers = _unitOfWork.GetRepository<CommiteeMember>().GetAll().Where(x => x.CommiteeId == item.CommiteeId && x.UserId != _sessionServices.UserId).ToList();
                    foreach (var item2 in committemebers)
                    {
                        if (item2.UserId != _sessionServices.UserId && item2.UserId != item.MainAssinedUserId)
                        {

                            if (item.IsNotification)
                            {
                                CommitteeNotificationDTO committeeNotification2 = new CommitteeNotificationDTO
                                {
                                    IsRead = false,
                                    UserId = (int)item2.UserId,
                                    TextAR = loc.CommiteeLocalizationAr + " " + item.Title,
                                    TextEn = loc.CommiteeLocalizationEn + " " + item.Title,
                                    CommiteeTaskId = item.CommiteeTaskId,
                                    CommiteeId = item.CommiteeId
                                };
                                List<CommitteeNotificationDTO> committeeNotifications2 = new List<CommitteeNotificationDTO> { committeeNotification2 };
                                _committeeNotificationService.Insert(committeeNotifications2);
                            }
                            if (item.IsEmail)
                            {
                                string Message = "";
                                string mailSubject = "";
                                var createdTaskTitle = _commiteeLocalizationService.GetLocaliztionByCode("CreatedTask", _sessionServices.CultureIsArabic);

                                getMailMessage(item, ref Message, ref mailSubject, createdTaskTitle);
                                AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                                Task.Run(() =>
                                {
                                    mailServices.SendNotificationEmail(item2.User.Email, mailSubject,
                                        null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                                        );

                                });
                            }
                            if (item.IsSMS && !string.IsNullOrEmpty(item2.User.Mobile))
                            {
                                smsServices.SendSMS(item2.User.Mobile, new string[0] { }, loc.CommiteeLocalizationAr + " " + item.Title, null);
                            }
                        }


                    }
                }
                else
                {

                    //if (item.MainAssinedUserId != _sessionServices.UserId)
                    //{

                    //    CommitteeNotificationDTO committeeNotification2 = new CommitteeNotificationDTO
                    //    {
                    //        IsRead = false,
                    //        UserId = (int)item.MainAssinedUserId,
                    //        TextAR = loc.CommiteeLocalizationAr + " " + item.Title,
                    //        TextEn = loc.CommiteeLocalizationEn + " " + item.Title,
                    //        CommiteeTaskId = item.CommiteeTaskId,
                    //        CommiteeId = item.CommiteeId
                    //    };
                    //    List<CommitteeNotificationDTO> committeeNotifications2 = new List<CommitteeNotificationDTO> { committeeNotification2 };
                    //    _committeeNotificationService.Insert(committeeNotifications2);
                    //    if (item.IsEmail)
                    //    {
                    //        mailServices.SendNotificationEmail(u.Email, "اضافه مهمه جديده",
                    //            loc.CommiteeLocalizationAr + " " + item.Title, false, null, null, Hosting.AngularRootPath, null
                    //            );
                    //    }
                    //    if (item.IsSMS)
                    //    {
                    //        smsServices.Send(u.Mobile, new string[0] { }, loc.CommiteeLocalizationAr + " " + item.Title, null);
                    //    }
                    //}
                    //foreach (var item2 in item.AssistantUsers)
                    foreach (var item2 in item.MultiMission)
                    {
                        foreach (var itemUser in item2.CommiteeTaskMultiMissionUserDTOs)

                        {
                            itemUser.UserDetailsDto = _UnitOfWork.GetRepository<User>().GetAll().Select(x => new UserDetailsDTO
                            {
                                UserId = x.UserId,
                                UserName = x.Username,
                                FullNameAr = x.FullNameAr,
                                FullNameEn = x.FullNameEn,
                                ProfileImage = x.ProfileImage,
                                Email = x.Email,
                                Mobile = x.Mobile

                            }).FirstOrDefault(z => z.UserId == itemUser.UserId);

                            //For Group

                            foreach (var itemgroup in item.TaskGroups)
                            {
                                itemgroup.Group = _UnitOfWork.GetRepository<Group>().GetAll().Select(x => new GroupDto
                                {
                                    GroupId = x.GroupId,
                                    GroupNameAr = x.GroupNameAr,
                                    GroupNameEn = x.GroupNameEn,
                                    CreatedBy = x.CreatedBy,
                                    GroupUsers = (ICollection<GroupUsersDto>)x.GroupUsers.Select(w => new GroupUsersDto
                                    {
                                        UserId = w.UserId,
                                        GroupId = w.GroupId,
                                        GroupUsersId = w.GroupUsersId,
                                        userDetailsDTO = new UserDetailsDTO()
                                        {
                                            UserId = w.User.UserId,
                                            UserName = w.User.Username,
                                            FullNameAr = w.User.FullNameAr,
                                            FullNameEn = w.User.FullNameEn,
                                        }
                                    })
                                }).FirstOrDefault(z => z.GroupId == itemgroup.GroupId);
                            }

                            //For CommiteeTaskMultiMission
                            //foreach (var itemMission in item.MultiMission)
                            //{


                            //    itemMission.CommiteeTaskMultiMissionUserDTOs =
                            //        _unitOfWork.GetRepository<CommiteeTaskMultiMissionUser>().GetAll().Select(w => new CommiteeTaskMultiMissionUserDTO
                            //        {
                            //            UserId = w.UserId,
                            //            CommiteeTaskMultiMissionId = w.CommiteeTaskMultiMissionId,
                            //            CommiteeTaskMultiMissionUserId = w.CommiteeTaskMultiMissionUserId,
                            //            UserDetailsDto = new UserDetailsDTO()
                            //            {
                            //                UserId = w.User.UserId,
                            //                UserName = w.User.Username,
                            //                FullNameAr = w.User.FullNameAr,
                            //                FullNameEn = w.User.FullNameEn,
                            //                ProfileImage = w.User.ProfileImage,
                            //            }
                            //        }).ToList();

                            //}


                            if (itemUser.UserId != _sessionServices.UserId)
                            {
                                if (item.IsNotification)
                                {
                                    CommitteeNotificationDTO committeeNotification2 = new CommitteeNotificationDTO
                                    {
                                        IsRead = false,
                                        UserId = (int)itemUser.UserId,
                                        TextAR = loc.CommiteeLocalizationAr + " " + item.Title,
                                        TextEn = loc.CommiteeLocalizationEn + " " + item.Title,
                                        CommiteeTaskId = item.CommiteeTaskId,
                                        CommiteeId = item.CommiteeId
                                    };
                                    List<CommitteeNotificationDTO> committeeNotifications2 = new List<CommitteeNotificationDTO> { committeeNotification2 };
                                    _committeeNotificationService.Insert(committeeNotifications2);
                                }
                                if (item.IsEmail)
                                {
                                    string Message = "";
                                    string mailSubject = "";
                                    var createdTaskTitle = _commiteeLocalizationService.GetLocaliztionByCode("CreatedTask", _sessionServices.CultureIsArabic);

                                    getMailMessage(item, ref Message, ref mailSubject, createdTaskTitle);
                                    AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                                    Task.Run(() =>
                                    {
                                        mailServices.SendNotificationEmail(itemUser.UserDetailsDto.Email, mailSubject,
                                        null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                                        );
                                    });
                                }
                                if (item.IsSMS)
                                {
                                    smsServices.SendSMS(itemUser.UserDetailsDto.Mobile, new string[0] { }, loc.CommiteeLocalizationAr + " " + item.Title, null);
                                }


                            }
                        }
                    }
                }
            }
            return entities;
            //return insertedEntity;
        }
        public CommiteetaskMultiMissionDTO InsertMultiMissionToTask(int commiteeTaskId, CommiteetaskMultiMissionDTO entity)
        {
            // obj Mapping
            var commiteeTaskMultiMissionUsers = entity.CommiteeTaskMultiMissionUserDTOs
            .Select(detail => new CommiteeTaskMultiMissionUser
            {
                UserId = detail.UserId,

            }).ToList();


            var commiteeTaskMultiMission = new CommiteeTaskMultiMission()
            {

                //group.GroupId = entity.GroupId;
                EndDateMultiMission = entity.EndDateMultiMission,
                CommiteeTaskMultiMissionUsers = commiteeTaskMultiMissionUsers,
                CreatedBy = _sessionServices.UserId,
                Name = entity.Name,
                CommiteeTaskId = commiteeTaskId,
                state = entity.state,


            };



            var insertedMission = _unitOfWork.GetRepository<CommiteeTaskMultiMission>().Insert(commiteeTaskMultiMission);
            insertedMission.CommiteeTaskId = commiteeTaskId;


            entity.CommiteeTaskMultiMissionId = commiteeTaskMultiMission.CommiteeTaskMultiMissionId;

            entity.CommiteeTaskId = commiteeTaskId;
            foreach (var item in entity.CommiteeTaskMultiMissionUserDTOs)
            {
                item.CommiteeTaskMultiMissionId = entity.CommiteeTaskMultiMissionId;

            }
            var assistantLog = new List<UpdateTaskLogAssistantUser>();
            foreach (var user in entity.CommiteeTaskMultiMissionUserDTOs)
            {


                var User = _unitOfWork.GetRepository<User>().GetById(user.UserId);
                //var e = _Mapper.Map<UpdateTaskLogAssistantUser>(user);
                var e = new UpdateTaskLogAssistantUser();
                e.Action = "Add";
                e.FullNameAr = User.FullNameAr;
                e.FullNameEn = User.FullNameEn;
                e.CommiteeTaskId = entity.CommiteeTaskId;
                e.AssistantUserId = user.UserId;
                e.CancelDate = DateTimeOffset.Now;
                assistantLog.Add(e);

            }

            if (assistantLog.Count() > 0)
            {
                _unitOfWork.GetRepository<UpdateTaskLogAssistantUser>().Insert(assistantLog);
            }
            _unitOfWork.SaveChanges();
            return entity;


        }
        public CommiteetaskMultiMissionDTO changeState(int missionId, List<int> UserIds,string TaskTitle)
        {
            var mission = _unitOfWork.GetRepository<CommiteeTaskMultiMission>().GetAll().Where(x => x.CommiteeTaskMultiMissionId == missionId).FirstOrDefault();
            mission.state = !mission.state;
            _unitOfWork.GetRepository<CommiteeTaskMultiMission>().Update(mission);
            var mainAssignedUser = _sessionServices;
            var usersEmail = _unitOfWork.GetRepository<User>().GetAll().Where(x => UserIds.Contains(x.UserId) && x.UserId != mainAssignedUser.UserId).Select(x => x.Email );
            foreach (var email in usersEmail)
            {
                //string Message =  " نود إعلامك بأنه تم تغيير حالة المهمة الفرعية  "+" ( "+ TaskTitle +" ) "+ " من " + mainAssignedUser.EmployeeFullNameAr;
                string Message = $"نود إعلامك بإنه تم تغيير حالة المهمة الفرعية ( {TaskTitle} ) من {mainAssignedUser.EmployeeFullNameAr}";
                string mailSubject = "تنبيه بتغيير حالة مهمة فرعية";
                var createdTaskTitle = _commiteeLocalizationService.GetLocaliztionByCode("CreatedTask", _sessionServices.CultureIsArabic);
                //getMailMessage(item, ref Message, ref mailSubject, createdTaskTitle);
                AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                Task.Run(() =>
                {
                    mailServices.SendNotificationEmail(email, mailSubject,
                        null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                        );

                });
            }

            var missionDto = _Mapper.Map<CommiteeTaskMultiMission, CommiteetaskMultiMissionDTO>(mission);

            return missionDto;

        }
        public override IEnumerable<CommiteeTaskDTO> Update(IEnumerable<CommiteeTaskDTO> entities)
        {

            foreach (var task in entities)
            {
                if (!string.IsNullOrEmpty(task.CommiteeIdEncrypted))
                {

                    task.CommiteeIdEncrypted = _sessionServices.UserIdAndRoleIdAfterDecrypt(task.CommiteeIdEncrypted, false).Id.ToString();
                    task.CommiteeId = int.Parse(task.CommiteeIdEncrypted);
                }
                var insertedlog = false;
                var commiteeTask = _unitOfWork.GetRepository<CommiteeTask>().GetById(task.CommiteeTaskId);
                List<UserTask> AllAssistant = _unitOfWork.GetRepository<UserTask>().GetAll().Where(x => x.CommiteeTaskId == commiteeTask.CommiteeTaskId).ToList();
                List<TaskGroups> AllTaskGroups = _unitOfWork.GetRepository<TaskGroups>().GetAll().Where(x => x.TaskId == commiteeTask.CommiteeTaskId).ToList();
                //commiteeTask.MultiMission =new List<CommiteeTaskMultiMission>();
                //var c = _unitOfWork.GetRepository<UserTask>().GetAll().Where(x => x.CommiteeTaskId == task.CommiteeTaskId).ToList();
                List<CommiteeTaskMultiMission> commiteeTaskMultiMissions = _unitOfWork.GetRepository<CommiteeTaskMultiMission>().GetAll().Where(x => x.CommiteeTaskId == commiteeTask.CommiteeTaskId).ToList();

                //For Logs 
                var assistantLog = new List<UpdateTaskLogAssistantUser>();

                foreach (var missionItem in commiteeTaskMultiMissions)
                {
                    foreach (var user in missionItem.CommiteeTaskMultiMissionUsers)
                    {
                        if (!task.MultiMission.Any(x => x.CommiteeTaskMultiMissionUserDTOs.Any(x => x.UserId == user.UserId && x.CommiteeTaskMultiMissionId == user.CommiteeTaskMultiMissionId)))
                        {
                            var e = _Mapper.Map<UpdateTaskLogAssistantUser>(user);
                            var assistantLogRemove = new List<UpdateTaskLogAssistantUser>();
                            e.Action = "Remove";
                            e.FullNameAr = user.User.FullNameAr;
                            e.FullNameEn = user.User.FullNameEn;
                            e.CommiteeTaskId = missionItem.CommiteeTaskId;
                            assistantLogRemove.Add(e);
                            if (assistantLogRemove.Count() > 0)
                            {
                                _unitOfWork.GetRepository<UpdateTaskLogAssistantUser>().Insert(assistantLogRemove);
                                _unitOfWork.SaveChanges();
                            }
                        }
                    }
                }

                foreach (var missionItem in task.MultiMission)
                {
                    foreach (var user in missionItem.CommiteeTaskMultiMissionUserDTOs)
                    {


                        if (!commiteeTaskMultiMissions.Any(x => x.CommiteeTaskMultiMissionUsers.Any(x => x.UserId == user.UserId && x.CommiteeTaskMultiMissionId == user.CommiteeTaskMultiMissionId)))
                        {
                            var User = _unitOfWork.GetRepository<User>().GetById(user.UserId);
                            //var e = _Mapper.Map<UpdateTaskLogAssistantUser>(user);
                            var e = new UpdateTaskLogAssistantUser();
                            var assistantLogAdd = new List<UpdateTaskLogAssistantUser>();
                            e.Action = "Add";
                            e.FullNameAr = User.FullNameAr;
                            e.FullNameEn = User.FullNameEn;
                            e.CommiteeTaskId = task.CommiteeTaskId;
                            e.AssistantUserId = User.UserId;
                            e.CancelDate = DateTimeOffset.Now;
                            assistantLogAdd.Add(e);
                            if (assistantLogAdd.Count() > 0)
                            {
                                _unitOfWork.GetRepository<UpdateTaskLogAssistantUser>().Insert(assistantLogAdd);
                                _unitOfWork.SaveChanges();
                            }
                        }
                    }
                }

                //Get All User In Mission And Delete remove All And Adding New Rows
                foreach (var item in commiteeTaskMultiMissions)
                {
                    var AllItemUser = _unitOfWork.GetRepository<CommiteeTaskMultiMissionUser>().GetAll()
                        .Where(x => x.CommiteeTaskMultiMissionId == item.CommiteeTaskMultiMissionId).ToList();

                    _unitOfWork.GetRepository<CommiteeTaskMultiMissionUser>().Delete(AllItemUser);


                    var AllItemMission = _unitOfWork.GetRepository<CommiteeTaskMultiMission>().GetAll()
                        .Where(x => x.CommiteeTaskMultiMissionId == item.CommiteeTaskMultiMissionId).FirstOrDefault();

                    _unitOfWork.GetRepository<CommiteeTaskMultiMission>().Delete(AllItemMission);

                    //item.CommiteeTaskMultiMissionUsers = new List<CommiteeTaskMultiMissionUser>();

                }
                foreach (var EntityDtoComing in task.MultiMission)
                {
                    // obj Mapping
                    var commiteeTaskMultiMissionUsers = EntityDtoComing.CommiteeTaskMultiMissionUserDTOs
                    .Select(detail => new CommiteeTaskMultiMissionUser
                    {
                        UserId = detail.UserId,
                        //CommiteeTaskMultiMissionId = detail.CommiteeTaskMultiMissionId

                    }).ToList();



                    var commiteeTaskMultiMission = new CommiteeTaskMultiMission()
                    {

                        //group.GroupId = entity.GroupId;
                        EndDateMultiMission = EntityDtoComing.EndDateMultiMission,
                        CommiteeTaskMultiMissionUsers = commiteeTaskMultiMissionUsers,
                        CreatedBy = _sessionServices.UserId,
                        Name = EntityDtoComing.Name,
                        CommiteeTaskId = task.CommiteeTaskId,
                        state = EntityDtoComing.state,


                    };



                    _unitOfWork.GetRepository<CommiteeTaskMultiMission>().Insert(commiteeTaskMultiMission);
                }


                var log = new UpdateTaskLogMainAssignedUser();

                commiteeTask.UpdatedOn = DateTimeOffset.Now;
                commiteeTask.UpdatedBy = _sessionServices.UserId;
                if (task.MainAssinedUserId != commiteeTask.MainAssinedUserId)
                {
                    //bool? e = (commiteeTask.IsShared != task.IsShared) ? commiteeTask.IsShared : null;

                    log.MainAssignedUserId = commiteeTask.MainAssinedUserId;
                    log.FullNameAr = commiteeTask.MainAssinedUser.FullNameAr;
                    log.FullNameEn = commiteeTask.MainAssinedUser.FullNameEn;
                    var userNewAssined = _unitOfWork.GetRepository<User>().GetById(task.MainAssinedUserId);
                    log.NewFullNameAr = userNewAssined.FullNameAr;
                    log.NewFullNameEn = userNewAssined.FullNameEn;
                    log.NewMainAssignedUserId = userNewAssined.UserId;
                    insertedlog = true;


                }
                commiteeTask.MainAssinedUserId = task.MainAssinedUserId;
                commiteeTask.ComiteeTaskCategoryId = task.ComiteeTaskCategoryId;
                commiteeTask.StartDate = task.StartDate;

                if (commiteeTask.IsShared != task.IsShared)
                {
                    log.IsShared = commiteeTask.IsShared;
                    commiteeTask.IsShared = task.IsShared;
                    insertedlog = true;

                }
                if (commiteeTask.EndDate != task.EndDate)
                {
                    log.EndDate = commiteeTask.EndDate;
                    commiteeTask.EndDate = task.EndDate;
                    insertedlog = true;

                }
                if (commiteeTask.Title != task.Title)
                {
                    log.Title = commiteeTask.Title;
                    commiteeTask.Title = task.Title;

                    insertedlog = true;

                }

                if (commiteeTask.TaskDetails != task.TaskDetails)
                {
                    log.TaskDetails = commiteeTask.TaskDetails;
                    commiteeTask.TaskDetails = task.TaskDetails;
                    insertedlog = true;

                }


                //commiteeTask.Title = task.Title;
                // commiteeTask.TaskDetails=task.TaskDetails;
                // commiteeTask.EndDate = task.EndDate;
                commiteeTask.CommiteeId = task.CommiteeId;
                //commiteeTask.IsShared = task.IsShared;
                commiteeTask.AssistantUsers?.RemoveAll(x => x.UserTaskId != null);
                commiteeTask.TaskGroups.ToList()?.RemoveAll(x => x.TaskGroupId != null);
                commiteeTask.TaskAttachments?.RemoveAll(x => x.CommitteeTaskAttachmentId != null);
                //commiteeTask.MultiMission.ToList().RemoveAll(x => x.CommiteeTaskMultiMissionId != null);
                foreach (var user in AllAssistant)
                {
                    if (!task.AssistantUsers.Select(x => x.UserId).ToList().Contains(user.UserId))
                    {
                        var e = _Mapper.Map<UpdateTaskLogAssistantUser>(user);
                        e.Action = "Remove";
                        e.FullNameAr = user.User.FullNameAr;
                        e.FullNameEn = user.User.FullNameEn;
                        assistantLog.Add(e);
                    }
                }


                foreach (var user in _Mapper.Map<List<UserTask>>(task.AssistantUsers))
                {
                    if (!AllAssistant.Select(x => x.UserId).ToList().Contains(user.UserId))
                    {

                        var User = _unitOfWork.GetRepository<User>().GetById(user.UserId);

                        var e = _Mapper.Map<UpdateTaskLogAssistantUser>(user);
                        e.Action = "Add";

                        e.FullNameAr = User.FullNameAr;
                        e.FullNameEn = User.FullNameEn;
                        assistantLog.Add(e);

                    }
                }



                commiteeTask.AssistantUsers = _Mapper.Map<List<UserTask>>(task.AssistantUsers);
                commiteeTask.TaskGroups = _Mapper.Map<List<TaskGroups>>(task.TaskGroups);

                commiteeTask.TaskAttachments = _Mapper.Map<List<CommitteeTaskAttachment>>(task.TaskAttachments);

                //commiteeTask.MultiMission = _Mapper.Map<List<CommiteeTaskMultiMission>>(task.MultiMission);
                // set User amd Mission In CommiteeTaskMultiMissionUserDto


                foreach (var item in commiteeTask.TaskAttachments)
                {
                    item.CommiteeTaskId = task.CommiteeTaskId;
                }
                foreach (var mission in task.MultiMission)
                {
                    mission.CommiteeTaskMultiMissionId = 0;
                    mission.UpdatedOn = DateTimeOffset.Now;
                    mission.UpdatedBy = _sessionServices.UserId;

                }
                //commiteeTask.MultiMission = _Mapper.Map<List<CommiteeTaskMultiMission>>(task.MultiMission);
                log.CancelDate = DateTimeOffset.Now;

                log.CommiteeTaskId = commiteeTask.CommiteeTaskId;

                if (insertedlog)
                {
                    _unitOfWork.GetRepository<UpdateTaskLogMainAssignedUser>().Insert(log);

                }


                _unitOfWork.GetRepository<CommiteeTask>().Update(commiteeTask);

                if (assistantLog.Count() > 0)
                {
                    _unitOfWork.GetRepository<UpdateTaskLogAssistantUser>().Insert(assistantLog);
                }
                var RR = _unitOfWork.SaveChanges();

            }
            var res = new List<CommiteeTaskDTO>();
            var x = _unitOfWork.GetRepository<CommiteeTask>().GetById(entities.FirstOrDefault().CommiteeTaskId);
            var mapped = _Mapper.Map<CommiteeTaskDTO>(x);
            string Message = "";
            string mailSubject = "";
            var EditedTaskTitle = _commiteeLocalizationService.GetLocaliztionByCode("EditedTask", _sessionServices.CultureIsArabic);

            getMailMessage(mapped, ref Message, ref mailSubject, EditedTaskTitle);
            AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
            if (_sessionServices.UserId == x.CreatedByUser.UserId)
            {
                Task.Run(() =>
                {
                    mailServices.SendNotificationEmail(mapped.MainAssinedUser.Email, mailSubject,
                 null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                 );
                });




            }
            else if (_sessionServices.UserId == x.MainAssinedUser.UserId)
            {
                Task.Run(() =>
                {
                    mailServices.SendNotificationEmail(mapped.CreatedByUser.Email, mailSubject,
                  null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                  );
                });

            }

            res.Add(mapped);
            return res;

        }
        public bool Complete(int id, string reason)
        {

            try
            {
                var task = this._UnitOfWork.GetRepository<CommiteeTask>().Find(id);
                var log = new UpdateTaskLogMainAssignedUser();
                task.Completed = !task.Completed;
                task.CloseReopenReason = reason;
                //task.CompleteReasonDate = new DateTimeOffset(DateTime.MinValue, TimeSpan.Zero);
                if (task.Completed)
                {
                    // task.CompleteReasonDate = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero);
                    task.CompleteReasonDate = DateTimeOffset.Now;
                    task.CompleteDate = DateTimeOffset.Now;
                    string Message = "";
                    string mailSubject = "";
                    var ClosedTaskTitle = _commiteeLocalizationService.GetLocaliztionByCode("ClosedTask", _sessionServices.CultureIsArabic);
                    getMailMessage(_Mapper.Map<CommiteeTaskDTO>(task), ref Message, ref mailSubject, ClosedTaskTitle);
                    AlternateView htmlViewForIncoming = CreateAlternateView(Message, null, "text/html");
                    if (_sessionServices.UserId == task.MainAssinedUser.UserId)
                    {

                        Task.Run(() =>
                        {
                            mailServices.SendNotificationEmail(task.CreatedByUser.Email, mailSubject,
                          null, true, htmlViewForIncoming, null, Hosting.AngularRootPath, null
                          );
                        });

                    }
                }
                log.Notes = reason;
                log.Completed = task.Completed;
                log.CommiteeTaskId = task.CommiteeTaskId;
                log.CancelDate = DateTimeOffset.Now;
                //log.CreatedByRoleId = null;
                this._UnitOfWork.GetRepository<CommiteeTask>().Update(task);
                _unitOfWork.GetRepository<UpdateTaskLogMainAssignedUser>().Insert(log);

                return true;
            }
            catch (Exception ex)
            {

                return false;
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
        public void getMailMessage(CommiteeTaskDTO task, ref string mailMessage, ref string mailSubject, string mailTitle)
        {
            try
            {

                var TaskMailSubject = _commiteeLocalizationService.GetLocaliztionByCode("TaskMailSubject", _sessionServices.CultureIsArabic);
                var JobTitleAr = _commiteeLocalizationService.GetLocaliztionByCode("JobTitle", true);
                var JobTitleEn = _commiteeLocalizationService.GetLocaliztionByCode("JobTitle", false);
                var taskCreatorAr = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", true);
                var taskCreatorEn = _commiteeLocalizationService.GetLocaliztionByCode("createdBy", false);
                var taskDetailsLinkAr = _commiteeLocalizationService.GetLocaliztionByCode("taskDetailsLink", true);
                var taskDetailsLinkEn = _commiteeLocalizationService.GetLocaliztionByCode("taskDetailsLink", false);
                mailSubject = TaskMailSubject;
                string systemsettinglink = _systemSettingsService.GetByCode("taskDetailsLink").SystemSettingValue;
                if (task.CommiteeTaskId == 0)
                {
                    var lastInsertedTaskId = _unitOfWork.GetRepository<CommiteeTask>().GetAll().OrderByDescending(x => x.CommiteeTaskId).FirstOrDefault().CommiteeTaskId;
                    task.CommiteeTaskId = lastInsertedTaskId;
                }
                string commiteeTaskEncyption = Encription.EncriptStringAES(task.CommiteeTaskId.ToString());
                string unicodedCommiteeTaskId = HttpUtility.UrlEncode(commiteeTaskEncyption);
                string taskAfterReplace = null;
                var taskDetailsLink = "";
                //if (commiteeTaskEncyption.Contains('='))
                //{
                //    taskAfterReplace = commiteeTaskEncyption.Replace("=", "%3D");
                //}
                //if (commiteeTaskEncyption.Contains('/'))
                //{
                //    taskAfterReplace = taskAfterReplace.Replace("/", "%2F");
                //}
                //if (string.IsNullOrEmpty(taskAfterReplace))
                //{

                //    taskDetailsLink = $"{systemsettinglink}/tasks/{commiteeTaskEncyption}";
                //}
                taskDetailsLink = $"{systemsettinglink}/tasks/{unicodedCommiteeTaskId}";


                string CreateDate = DateTime.Now.ToString("yyyy-MM-dd", new CultureInfo("ar-AE"));
                string CreateDateEn = DateTime.Now.ToString("dd-MM-yyyy");

                string Subject = task.Title;
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
	                                        align-items: center;";
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
                                          <div style='{Email_style}'>
                                           <img style='{Email_image_style}' src='cid:TopHeader'>
                                           <div style='
		                                        width: 100%;
		                                        display: flex;
		                                        flex-direction: column;
		                                        justify-content: center;
		                                        margin: 0;
		                                        padding: 0;
                                                
	                                        '>		
		                                 <table style='width: 100%' border='1'>
			                                <tr style='{tr_style}'>
				                                <td colspan='5' style='{td_style}'>
				                                    <h2 style=' text-align: center; background: #13817E; padding: 9px 0; font-weight: 900; margin-bottom: 0px; color: #fff;'> {mailTitle} </h2>
                                                </td>
			                                </tr>
                                            <tr style='{tr_style}'>
                                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {JobTitleEn} </td>   
				                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'><span style='unicode-bidi: bidi-override;'>{task.Title}</span></td>
				                                <td style='{td_style + ';' + w_20 + rtl}text-align: center;'> {JobTitleAr} </td>
                                                

                                            </tr >  
                                            <tr style='{tr_style}'>
				                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {taskCreatorEn} </td>
				                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'>{_sessionServices.EmployeeFullNameAr}</td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {taskCreatorAr} </td>   
                                            </tr > 
                                            <tr style='{tr_style}'>
				                                <td style='{td_style_En + ';' + w_20}text-align: center;'> {taskDetailsLinkEn} </td>
				                                <td colspan='3' style='{td_style + ';' + rtl}text-align: center;'>{taskDetailsLink}</td>
                                                <td style ='{td_style + ';' + w_20 + rtl}text-align: center;'> {taskDetailsLinkAr} </td>   
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


        //public void CreateEventOnOutlookCalender()
        //{
        //    var scopes = new[] { "User.Read" };

        //    // Multi-tenant apps can use "common",
        //    // single-tenant apps must use the tenant ID from the Azure portal
        //    var tenantId = "common";

        //    // Value from app registration
        //    var clientId = "YOUR_CLIENT_ID";

        //    // using Azure.Identity;
        //    var options = new DeviceCodeCredentialOptions
        //    {
        //        AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
        //        ClientId = clientId,
        //        TenantId = tenantId,
        //        // Callback function that receives the user prompt
        //        // Prompt contains the generated device code that user must
        //        // enter during the auth process in the browser
        //        DeviceCodeCallback = (code, cancellation) =>
        //        {
        //            Console.WriteLine(code.Message);
        //            return Task.FromResult(0);
        //        },
        //    };

        //    // https://learn.microsoft.com/dotnet/api/azure.identity.devicecodecredential
        //    var deviceCodeCredential = new DeviceCodeCredential(options);

        //    var graphClient = new GraphServiceClient(deviceCodeCredential, scopes);

          
        //}

}
}