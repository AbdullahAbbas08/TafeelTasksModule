using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommiteeAttachmentsController : _BaseController<CommiteeSavedAttachment, CommiteeAttachmentDTO>
    {
        private readonly ICommiteeAttachmentService _commiteeAttachmentService;
        protected readonly IHelperServices.ISessionServices _SessionServices;
        public CommiteeAttachmentsController(ICommiteeAttachmentService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._commiteeAttachmentService = businessService;
            _SessionServices = sessionSevices;
        }
        [HttpGet("GetAllAttchment")]
        public AllCommiteeAttachmentDTO GetAllAttchment(int take, int skip, string CommitteId, DateTime? dateFrom, DateTime? dateTo, string SearchText)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteId,true);

            return _commiteeAttachmentService.GetAllCommiteeAttachment(take, skip, UserIdAndUserRoleId.Id, dateFrom, dateTo, SearchText);
        }
    }
}
