using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.Enums;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeAndMeetings.Service.Sevices;
using CommiteeDatabase.Models.Domains;
using LinqHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Web;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommiteeTasksController : _BaseController<CommiteeTask, CommiteeTaskDTO>
    {
        private readonly ICommiteeTaskService _commiteeTaskService;
        protected readonly IHelperServices.ISessionServices _SessionServices;
        public CommiteeTasksController(ICommiteeTaskService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._commiteeTaskService = businessService;
            _SessionServices = sessionSevices;
        }
        [HttpGet("Complete")]
        public bool Complete(string id, string reason)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(id, true);

            return _commiteeTaskService.Complete(UserIdAndUserRoleId.Id, reason);
        }
        [HttpPost("InsertMultiMissionToTask")]
        public CommiteetaskMultiMissionDTO InsertMultiMissionToTask(string CommitteTaskIdEncrpted, CommiteetaskMultiMissionDTO entity)
        {
            int commiteeTaskId =  _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteTaskIdEncrpted, false).Id;

            return _commiteeTaskService.InsertMultiMissionToTask(commiteeTaskId, entity);
           
        }
        [HttpPut("ChangeStateForMission")]
        public CommiteetaskMultiMissionDTO ChangeStateForMission(string missionEncrypted)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(missionEncrypted, false);
            return _commiteeTaskService.changeState(UserIdAndUserRoleId.Id);

        }
        [HttpGet("GetAllwithFilters")]
        public DataSourceResult<CommiteeTaskDTO> GetAllwithFilters([FromQuery] DataSourceRequest dataSourceRequest, [FromQuery] TaskFilterEnum requiredTasks, string UserIdEncrpted,string OrganizationId, [FromQuery] ParamsSearchFilterDTO paramsSearchFilterDTO = null)
        {
            int userId;
            int? userIdOrNull = null;
            int.TryParse(UserIdEncrpted, out userId);
            if (userId == 0)
                userIdOrNull = null;
            else
                userIdOrNull = userId;
            int organizationId;
            int? organizationIdOrNull = null;
            int.TryParse(OrganizationId, out organizationId);
            if (organizationId == 0)
                organizationIdOrNull = null;
            else
                organizationIdOrNull = organizationId;
            return _commiteeTaskService.GetAllwithfilters(dataSourceRequest, requiredTasks, paramsSearchFilterDTO, userIdOrNull, organizationIdOrNull);
        }

        [HttpGet("GetStatistics")]
        public List<CountResultDTO> GetCommitteeTaskStatistics(string OrganizationId,string UserId,string committeeId, DateTime? ValiditayPeriodFrom, DateTime? ValidatiyPeriodTo)
        {
            int userId = 0;
            int? userIdOrNull = null;
            int organizationId = 0;
            int? organizationIdOrNull = null;
            if (!string.IsNullOrEmpty(committeeId))
            {
                if (!string.IsNullOrEmpty(UserId))
                {
                    UserIdAndRoleIdAfterDecryptDTO userIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(UserId, false) ;
                    userId = userIdAndUserRoleId.Id;
                }

                if (userId == 0)
                    userIdOrNull = null;
                else
                    userIdOrNull = userId;

                if (!string.IsNullOrEmpty(OrganizationId))
                {
                    UserIdAndRoleIdAfterDecryptDTO userIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(OrganizationId, false);
                    organizationId = userIdAndUserRoleId.Id;
                }

                if (organizationId == 0)
                    organizationIdOrNull = null;
                else
                    organizationIdOrNull = organizationId;


                UserIdAndRoleIdAfterDecryptDTO committeeIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(committeeId, true);
            return _commiteeTaskService.getComitteeTaskStatistics(organizationIdOrNull, userIdOrNull, committeeIdAndUserRoleId.Id, ValiditayPeriodFrom, ValidatiyPeriodTo);
            }
            else
            {
                int commiteId;
                int? CommitIdNull = null;
                int.TryParse(committeeId, out commiteId);
                if (commiteId == 0)
                    CommitIdNull = null;

                if (!string.IsNullOrEmpty(UserId))
                {
                    UserIdAndRoleIdAfterDecryptDTO userIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(UserId, false);
                    userId = userIdAndUserRoleId.Id;
                }

                if (userId == 0)
                    userIdOrNull = null;
                else
                    userIdOrNull = userId;

                if (!string.IsNullOrEmpty(OrganizationId))
                {
                    UserIdAndRoleIdAfterDecryptDTO userIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(OrganizationId, false);
                    organizationId = userIdAndUserRoleId.Id;
                }

                if (organizationId == 0)
                    organizationIdOrNull = null;
                else
                    organizationIdOrNull = organizationId;

                return _commiteeTaskService.getComitteeTaskStatistics(organizationIdOrNull, userIdOrNull, CommitIdNull, ValiditayPeriodFrom, ValidatiyPeriodTo);
                

            }



        }

        [HttpGet("GetAllForPrint")]
        public List<CommiteeTaskDTO> GetAllForPrint([FromQuery] TaskFilterEnum requiredTasks, string CommiteeId, int? ComiteeTaskCategoryId, string SearchText,string UserIdEncrpted, [FromQuery] ParamsSearchFilterDTO paramsSearchFilterDTO = null, int? organizationId = null)
        {
            int userId;
            int? userIdOrNull = null;
            int.TryParse(UserIdEncrpted, out userId);
            if (userId == 0)
                userIdOrNull = null;
            else 
            userIdOrNull = userId;
            if (!string.IsNullOrEmpty(CommiteeId))
            {

                UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommiteeId,true);

                return _commiteeTaskService.GetAllForPrint(requiredTasks, UserIdAndUserRoleId.Id, ComiteeTaskCategoryId, SearchText, userIdOrNull, paramsSearchFilterDTO, organizationId);
            }
            int commiteId;
            int? CommitIdNull = null;
            int.TryParse(CommiteeId, out commiteId);
            if (commiteId == 0)
                CommitIdNull = null;
            return _commiteeTaskService.GetAllForPrint(requiredTasks, CommitIdNull, ComiteeTaskCategoryId, SearchText, userIdOrNull, paramsSearchFilterDTO, organizationId);

        }

        [HttpPost("Export")]
        public FileResult Export([FromQuery] TaskFilterEnum requiredTasks , string UserIdEncrpted, ExportType exportType,int? OrganizationId=null)
        {
            int? userId = null;
            if (int.TryParse(UserIdEncrpted, out int parsedUserId))
            {
                userId = parsedUserId;
            }
            string mimeType = "";
            switch (exportType)
            {
                case ExportType.Excel:
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                
                case ExportType.Word:
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                default:
                    break;
            }
            byte[] fileBytes = _commiteeTaskService.Export(requiredTasks , userId, mimeType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" ? true : false, OrganizationId);

            if (mimeType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {

                return this.File(fileBytes, mimeType, "Report-" + DateTime.Now.ToShortDateString() + ".xlsx");
            }
            else
            {
                return this.File(fileBytes, mimeType, "Report-" + DateTime.Now.ToShortDateString() + ".doc");

            }
        }


        [HttpGet("GetAllCalender")]
        public List<CommiteeTaskDTO> GetAllCalender(string commiteeId, int? ComiteeTaskCategoryId)
        {
            if (!string.IsNullOrEmpty(commiteeId))
            {

                UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(commiteeId,true);

                return _commiteeTaskService.GetAllForCalender(UserIdAndUserRoleId.Id, ComiteeTaskCategoryId);
            }
            int commiteId;
            int? CommitIdNull = null;
            int.TryParse(commiteeId, out commiteId);
            if (commiteId == 0)
                CommitIdNull = null;

            return _commiteeTaskService.GetAllForCalender(CommitIdNull, ComiteeTaskCategoryId);

        }
        [AllowAnonymous]
        [HttpGet("GetDetailsById")]
        public CommiteeTaskDTO GetDetailsById(string ComiteeTaskId)
        {
            string decodeduserid = HttpUtility.UrlDecode(ComiteeTaskId);
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(decodeduserid, false);
            var commiteeTask =_commiteeTaskService.GetDetailsById(UserIdAndUserRoleId.Id);
            return commiteeTask;
        }


    }
}
