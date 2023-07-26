using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("AttachmentComments", Schema = "Committe")]
    public class AttachmentComment : _BaseEntity, IAuditableInsert, IAuditableUpdate
    {
        [Key]
        public int AttachmentCommentId { get; set; }
        public int CommentId { get; set; }
        public virtual Comment Comment { get; set; }
        public int AttachmentId { get; set; }
        public virtual CommiteeSavedAttachment Attachment { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
    }
}