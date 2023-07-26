using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;
using Models;
using Models.ProjectionModels;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommitte_AttachmentService : IBusinessService<SavedAttachment, SavedAttachmentDTO>
    {
        IEnumerable<PhysicalAttachmentDTO> InsertPhysicalAttachments(IEnumerable<PhysicalAttachmentDTO> physicalAttachments);
        int GetCommiteeAttachmentsCount(int CommiteeId);
        LettersDTO AddLetterAttachment(LettersDTO lettersDTO);
        bool UpdateLetter(IEnumerable<AttachmentDetailsDTO> letter);
        bool DeleteIndividualUserAttachment(int UserId, int attachementId);

    }
}
