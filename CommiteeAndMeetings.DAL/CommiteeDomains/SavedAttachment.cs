using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("SavedAttachments", Schema = "Committe")]
    public class SavedAttachment : _BaseEntity, IAuditableInsert, IAuditableUpdate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int SavedAttachmentId { get; set; }

        [MaxLength(500)]
        public string AttachmentName { get; set; }

        public int AttachmentTypeId { get; set; }

        public bool IsDisabled { get; set; } = false;

        [MaxLength(500)]
        public string OriginalName { get; set; }

        [MaxLength(500)]
        public string MimeType { get; set; }

        public int? Size { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        [MaxLength(100)]
        public string LFEntryId { get; set; }

        [NotMapped]
        public virtual byte[] BinaryContent { get; set; }

        public int? PhysicalAttachmentTypeId { get; set; }

        public int? PagesCount { get; set; }

        public string Notes { get; set; }

        public virtual List<CommiteeSavedAttachment> CommiteeSavedAttachments { get; set; }
        public virtual List<SurveyAttachment> SurveyAttachments { get; set; }
        #region IAuditableInsert
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }


        #endregion IAuditableInsert

        #region IAuditableUpdate

        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }


        #endregion IAuditableUpdate
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }
        [ForeignKey("comment")]
        public int? CommentId { get; set; }

        public virtual Comment comment { get; set; }
    }
}