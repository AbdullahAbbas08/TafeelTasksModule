using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    [Table("CommitteeNotifications", Schema = "Committe")]
    public class CommitteeNotification : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
        [Key]
        public int CommitteeNotificationId { get; set; }
        public string TextAR { get; set; }
        public string TextEn { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }
        public int? CommiteeTaskId { get; set; }
        public virtual CommiteeTask CommiteeTask { get; set; }
        public int? CommiteeSavedAttachmentId { get; set; }
        public virtual CommiteeSavedAttachment CommiteeSavedAttachment { get; set; }
        public int? SurveyId { get; set; }
        public virtual Survey Survey { get; set; }
        public int? CommentId { get; set; }
        public virtual Comment Comment { get; set; }
        public int? CommiteeId { get; set; }
        public virtual Commitee Commitee { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
        public int? MeetingId { get; set; }
        public virtual Meeting Meeting { get; set; }
        public int? MeetingTopicId { get; set; }
        public virtual MeetingTopic MeetingTopic { get; set; }
        public int? MinuteOfMeetingId { get; set; }
        public virtual MinuteOfMeeting MinuteOfMeeting { get; set; }
    }
}
