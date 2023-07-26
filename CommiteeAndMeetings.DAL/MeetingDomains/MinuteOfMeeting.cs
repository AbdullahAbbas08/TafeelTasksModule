using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.MeetingDomains
{
    [Table("MinutesOfMeetings", Schema = "Meeting")]
    public class MinuteOfMeeting : _BaseEntity, IAuditableInsertNoRole, IAuditableUpdate, IAuditableDelete
    {
        public MinuteOfMeeting()
        {
            Topics = new List<MinuteOfMeetingTopic>();
            MOMComment = new List<MOMComment>();
        }
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public virtual Meeting Meeting { get; set; }
        public bool FromTopic { get; set; }
        public virtual List<MinuteOfMeetingTopic> Topics { get; set; }
        public virtual List<MOMComment> MOMComment { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public bool Colsed { get; set; } = false;
        public bool EmailSentInvitation { get; set; }=false;
    }
}