using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.ModelInterfaces;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("CommiteeTasks", Schema = "Committe")]
    public class CommiteeTask : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
        public CommiteeTask()
        {
            // AssistantUsers = new List<UserTask>();
            // TaskComments = new List<TaskComment>();
            // TaskAttachments = new List<CommitteeTaskAttachment>();
            //MultiMission = new List<CommiteeTaskMultiMission>();

        }
        [Key]
        public int CommiteeTaskId { get; set; }
        public string Title { get; set; }
        public string TaskDetails { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public int? CommiteeId { get; set; }
        public int? MeetingId { get; set;}
        [ForeignKey("ComiteeTaskCategory")]
        public int? ComiteeTaskCategoryId { get; set; }
        public virtual ComiteeTaskCategory ComiteeTaskCategory { get; set; }
        public virtual Commitee Commitee { get; set; }
        public virtual Meeting Meeting { get; set; }
        public int MainAssinedUserId { get; set; }
        public bool IsMain { get; set; }
        public bool TaskToView { get; set; } = false;
        public bool IsShared { get; set; } = true;
        public bool IsEmail { get; set; }
        public string CloseReopenReason { get; set; }
        public DateTimeOffset? CompleteReasonDate { get; set; }
        public bool IsNotification { get; set; }
        public bool IsSMS { get; set; }
        public virtual User MainAssinedUser { get; set; }
        public virtual List<UserTask> AssistantUsers { get; set; }
        public virtual List<CommitteeTaskAttachment> TaskAttachments { get; set; }
        public virtual ICollection<CommiteeTaskMultiMission> MultiMission { get; set; }
        public virtual List<TaskComment> TaskComments { get; set; }
        public virtual ICollection<TaskGroups> TaskGroups { get; set; }
        public bool Completed { get; set; }
        public bool Islated { get; set; } = false;
        public DateTimeOffset? CompleteDate { get; set; }

        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }

    }
}