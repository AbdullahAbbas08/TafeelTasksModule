using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("CommiteeSavedAttachments", Schema = "Committe")]
    public class CommiteeSavedAttachment : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
        public CommiteeSavedAttachment()
        {
            Attachments = new List<SavedAttachment>();
            AttachmentUsers = new List<AttachmentUser>();
            AttachmentComments = new List<AttachmentComment>();
        }
        [Key]
        public int CommiteeAttachmentId { get; set; }
        public int CommiteeId { get; set; }
        public virtual Commitee Commitee { get; set; }
        // public int AttachmentId { get; set; }
        public virtual List<SavedAttachment> Attachments { get; set; }
        public bool AllUsers { get; set; }
        public string Description { get; set; }
        public virtual List<AttachmentUser> AttachmentUsers { get; set; }
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public virtual List<AttachmentComment> AttachmentComments { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
    }
}