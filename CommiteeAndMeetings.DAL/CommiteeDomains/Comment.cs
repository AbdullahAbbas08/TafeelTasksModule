using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("Comments", Schema = "Committe")]
    public class Comment : _BaseEntity, IAuditableInsert, IAuditableUpdate
    {
        public Comment()
        {
            SavedAttachments= new List<SavedAttachment>();
        }
        [Key]
        public int CommentId { get; set; }
        public string Text { get; set; }
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
        //[NotMapped]
        //public virtual List<CommentAttachmentInTask> commentAttachments { get; set; }
        public virtual List<SavedAttachment> SavedAttachments { get; set; } 


    }
}