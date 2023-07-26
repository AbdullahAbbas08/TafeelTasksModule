using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("AttachmentUsers", Schema = "Committe")]
    public class AttachmentUser : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
        [Key]
        public int AttachmentUserId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CommiteeAttachmentId { get; set; }
        public virtual CommiteeSavedAttachment CommiteeAttachment { get; set; }
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