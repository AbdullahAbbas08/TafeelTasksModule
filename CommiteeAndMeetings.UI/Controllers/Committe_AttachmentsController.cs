using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ProjectionModels;
using System.Collections.Generic;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Committe_AttachmentsController : _BaseController<SavedAttachment, SavedAttachmentDTO>
    {
        private readonly ICommitte_AttachmentService attachmentService;
        public Committe_AttachmentsController(ICommitte_AttachmentService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this.attachmentService = businessService;
        }
        [HttpPost, Route("InsertPhysicalAttachments")]
        public IEnumerable<PhysicalAttachmentDTO> InsertPhysicalAttachments([FromBody] IEnumerable<PhysicalAttachmentDTO> physicalAttachments)
        {
            return this.attachmentService.InsertPhysicalAttachments(physicalAttachments);
        }
        [HttpGet, Route("GetCommiteeAttachmentsCount")]
        public int GetCommiteeAttachmentsCount(int TransactionId)
        {
            return this.attachmentService.GetCommiteeAttachmentsCount(TransactionId);
        }
        [HttpPost, Route("AddLettersAttachment")]
        public LettersDTO AddLettersAttachment([FromBody] LettersDTO lettersDTO)
        {
            return this.attachmentService.AddLetterAttachment(lettersDTO);
        }
        [HttpPut, Route("UpdateLetter")]
        public bool UpdateLetter([FromBody] IEnumerable<AttachmentDetailsDTO> letter)
        {
            return this.attachmentService.UpdateLetter(letter);
        }
        [HttpPut, Route("DeleteIndividualUserAttachment")]
        public bool DeleteIndividualUserAttachment(int UserId, int attachementId)
        {
            return this.attachmentService.DeleteIndividualUserAttachment(UserId, attachementId);
        }
    }
}
