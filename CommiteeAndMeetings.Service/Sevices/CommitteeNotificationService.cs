using AutoMapper;
using AutoMapper.QueryableExtensions;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Services.Sevices;
using CommiteeDatabase.Models.Domains;
using HelperServices.LinqHelpers;
using IHelperServices.Models;
using LFSO102Lib;
using LinqHelper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CommiteeAndMeetings.Service.Sevices
{
    public class CommitteeNotificationService : BusinessService<CommitteeNotification, CommitteeNotificationDTO>, ICommitteeNotificationService
    {
        IHelperServices.ISessionServices _sessionServices;
        public readonly MasarContext _context;
        protected readonly IMapper _Mapper;
        public CommitteeNotificationService(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer stringLocalizer, ISecurityService securityService, IHelperServices.ISessionServices sessionServices, IOptions<AppSettings> appSettings)
             : base(unitOfWork, mapper, stringLocalizer, securityService, sessionServices, appSettings)
        {
            _sessionServices = sessionServices;
            _context = new MasarContext();
            _Mapper = mapper;
        }

        public int GetNotificationCount(int userId)
        {
            return this._UnitOfWork.GetRepository<CommitteeNotification>().GetAll().Where(x => !x.IsRead && x.UserId == userId).Count();
        }

        public DataSourceResult<CommitteeNotificationDTO> GetNotificationList(int userId, int take, int skip)
        {
            DataSourceRequest dataSourceRequest = new DataSourceRequest
            {
                Countless = false,
                Skip = skip*take,
                Take = take,
                Filter = null,
                Sort = null
            };
            IQueryable<CommitteeNotification> Notifications = this._UnitOfWork.GetRepository<CommitteeNotification>().GetAll().OrderByDescending(x => x.CreatedOn).Where(x => x.UserId == userId);
            
            var result = Notifications.ProjectTo<CommitteeNotificationDTO>(_Mapper.ConfigurationProvider).ToDataSourceResult(dataSourceRequest);
            
            return result;
            

            
        }

        public CommitteeNotificationDTO GetNotificationRead(int committeeNotificationId)
        {
            var Notifications = this._UnitOfWork.GetRepository<CommitteeNotification>().GetAll().Where(x => x.CommitteeNotificationId == committeeNotificationId).FirstOrDefault();
            Notifications.IsRead = true;
            _UnitOfWork.GetRepository<CommitteeNotification>().Update(Notifications);
            
           var notificationDto = _Mapper.Map<CommitteeNotification, CommitteeNotificationDTO >(Notifications);
            
            return notificationDto;
        }
        public void insertNotification()
        {
            var AddNotificationCount = _UnitOfWork.GetRepository<CommitteeNotification>().GetAll().Where(x => x.TextEn.Contains("tasks late") && x.CreatedBy == _sessionServices.UserId && x.CreatedOn.Value.Date == DateTime.Now.Date).Count() == 0;

            if (AddNotificationCount)
            {
                var count = _UnitOfWork.GetRepository<CommiteeTask>().GetAll().Where(x =>
             x.EndDate <= DateTimeOffset.Now && x.MainAssinedUserId == _sessionServices.UserId && !x.Completed &&

             (x.IsShared || x.MainAssinedUserId == _sessionServices.UserId)

             //x.AssistantUsers.Any(z => z.UserId == _sessionServices.UserId) || x.CreatedBy == _sessionServices.UserId)
             ).Count();
                if (count > 0)
                {
                    CommitteeNotification committeeNotification2 = new CommitteeNotification
                    {
                        IsRead = false,
                        UserId = (int)_sessionServices.UserId,
                        TextAR = $"لديك {count} مهمات متاخره",
                        TextEn = $"You have {count} tasks late",
                        CreatedBy = _sessionServices.UserId,
                        CreatedOn = DateTimeOffset.Now



                    };
                    List<CommitteeNotification> committeeNotifications2 = new List<CommitteeNotification> { committeeNotification2 };

                    _context.CommitteeNotifications.AddRange(committeeNotifications2);
                    _context.SaveChanges();
                }
            }
        }
    }
}