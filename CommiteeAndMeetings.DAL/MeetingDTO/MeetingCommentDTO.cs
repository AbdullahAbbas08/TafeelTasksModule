using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.MeetingDomains;
using Models;
using System;
using System.Collections.Generic;

namespace CommiteeAndMeetings.DAL.MeetingDTO
{
    public class MeetingCommentDTO
    {
        public MeetingCommentDTO()
        {
            SurveyAnswers = new List<SurveyAnswerDTO>();
        }
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public virtual MeetingDTO Meeting { get; set; } 
        public int CommentId { get; set; }
        public virtual CommentDTO Comment { get; set; }
        public CommentType CommentType { get; set; }
        public int? CreatedBy { get; set; }
        public UserDetailsDTO CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        
        public List<SurveyAnswerDTO> SurveyAnswers { get; set; }

       
    }
}