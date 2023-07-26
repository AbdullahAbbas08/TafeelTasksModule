using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmMigratedAttachmentView
    {
        public long Id { get; set; }
        public long MigratedTransactionAttachmentId { get; set; }
        [StringLength(500)]
        public string AttachmentName { get; set; }
        public int AttachmentTypeId { get; set; }
        [StringLength(500)]
        public string OriginalName { get; set; }
        [StringLength(500)]
        public string MimeType { get; set; }
        public int? Size { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        [Column("LFEntryId")]
        [StringLength(100)]
        public string LfentryId { get; set; }
        public int? PagesCount { get; set; }
        public string Notes { get; set; }
        [StringLength(50)]
        public string PhysicalAttachmentTypeNameAr { get; set; }
        [StringLength(50)]
        public string PhysicalAttachmentTypeNameEn { get; set; }
        public string PhysicalAttachmentTypeNameFn { get; set; }
        [StringLength(50)]
        public string AttachmentTypeCode { get; set; }
        [StringLength(50)]
        public string AttachmentTypeNameAr { get; set; }
        [StringLength(50)]
        public string AttachmentTypeNameEn { get; set; }
        public string AttachmentTypeNameFn { get; set; }
        public long? MigratedTransactionId { get; set; }
        public int AttachmentId { get; set; }
    }
}
