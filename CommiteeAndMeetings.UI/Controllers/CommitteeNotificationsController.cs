using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using IHelperServices;
using LinqHelper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ISessionServices = IHelperServices.ISessionServices;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommitteeNotificationsController : _BaseController<CommitteeNotification, CommitteeNotificationDTO>
    {
        private readonly ICommitteeNotificationService _commiteeAttachmentService;
        private readonly ISessionServices _sessionServices;
        public CommitteeNotificationsController(ICommitteeNotificationService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._commiteeAttachmentService = businessService;
            this._sessionServices = sessionSevices; 
            
        }
        [HttpGet("GetNotificationCount")]
        public int GetNotificationCount()
        {
            
            return _commiteeAttachmentService.GetNotificationCount((int)_sessionServices.UserId);
        }
        [HttpGet("GetNotificationList")]
        public DataSourceResult<CommitteeNotificationDTO> GetNotificationList( int pageSize = 10, int page = 0)
        {
            return _commiteeAttachmentService.GetNotificationList((int)_sessionServices.UserId, pageSize, page);
        }

        [HttpPut("GetNotificationRead")]
        public CommitteeNotificationDTO GetNotificationRead(string notificationIdEncripted)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _sessionServices.UserIdAndRoleIdAfterDecrypt(notificationIdEncripted,false);

            return _commiteeAttachmentService.GetNotificationRead(UserIdAndUserRoleId.Id);
        }
        [HttpPut("insertNotification")]
        public void insertNotification()
        {
             _commiteeAttachmentService.insertNotification();
        }

    }
}
