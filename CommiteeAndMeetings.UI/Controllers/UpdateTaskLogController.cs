using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using static iTextSharp.text.pdf.AcroFields;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateTaskLogController : ControllerBase
    {
        private readonly IHelperServices.ISessionServices _SessionServices;
        private readonly IUpdateTaskLogService updateTaskLogService;
        public UpdateTaskLogController(IUpdateTaskLogService _updateTaskLogService,IHelperServices.ISessionServices sessionSevices)
        {
            _SessionServices = sessionSevices;
            updateTaskLogService = _updateTaskLogService;
        }
        [HttpGet("GetTaskUpdateslog")]
        public List<UpdateTaskLogDTO> GetTaskUpdateslog(string CommiteeTaskId)
        {
            UserIdAndRoleIdAfterDecryptDTO IdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommiteeTaskId,true);

            return updateTaskLogService.GetTaskUpdateslog(IdAndUserRoleId.Id);
        }
    }
}
