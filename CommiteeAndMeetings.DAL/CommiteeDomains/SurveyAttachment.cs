using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeDatabase.Models.Domains
{
    [Table("SurveyAttachments", Schema = "Committe")]
    public class SurveyAttachment
    {
        [Key]
        public int SurveyAttachmentId { get; set; }
        public int SurveyId { get; set; }
        public virtual Survey Survey { get; set; }
        public int AttachmentId { get; set; }
        public virtual SavedAttachment Attachment { get; set; }
        public bool Active { get; set; }
    }
}