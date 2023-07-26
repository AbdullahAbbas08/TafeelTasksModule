using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;
using CommiteeDatabase.Models.Domains;
using System;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommiteeAttachmentService : IBusinessService<CommiteeSavedAttachment, CommiteeAttachmentDTO>
    {
        AllCommiteeAttachmentDTO GetAllCommiteeAttachment(int take, int skip, int committeId, DateTime? dateFrom, DateTime? dateTo, string SearchText);
    }
}
