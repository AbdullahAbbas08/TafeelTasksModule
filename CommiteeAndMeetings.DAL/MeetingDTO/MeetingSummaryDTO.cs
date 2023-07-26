using System;
using System.Collections.Generic;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingSummaryDTO
    {
        public MeetingSummaryDTO()
        {
            MeetingCommentsPercentage = new();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public DateTimeOffset MeetingFromTime { get; set; }
        public DateTimeOffset MeetingToTime { get; set; }
        public string PhysicalLocation { get; set; }
        public List<MOMSummaryDTO> MOMSummaries { get; set; }
        public virtual List<MeetingAttendeeDTO> MeetingAttendees { get; set; }
        public virtual List<MeetingCoordinatorDTO> MeetingCoordinators { get; set; }
        public virtual List<MOMCommentDTO> MOMComment { get; set; }
        public virtual List<Meeting_Meeting_HeaderAndFooterDTO> MeetingHeaderAndFooters { get; set; }
        public virtual List<MeetingTopicDTO> MeetingTopicDTOs { get; set; }
        public List<CommentsResultPercent> MeetingCommentsPercentage { get; set; }
    } 

    public class CommentsResultPercent
    {
        public string CommentTitle { get; set; }
        public float PercentAccept { get; set; }
        public float PercentReserved { get; set; }
        public float PercentReject { get; set; }

    }
}
