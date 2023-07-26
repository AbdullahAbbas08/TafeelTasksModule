using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmAttachmentView
    {
        public int Id { get; set; }
        public int TransactionAttachmentId { get; set; }
        [StringLength(500)]
        public string AttachmentName { get; set; }
        public int AttachmentTypeId { get; set; }
        [StringLength(500)]
        public string OriginalName { get; set; }
        public int? ReferenceAttachmentId { get; set; }
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
        [StringLength(400)]
        public string PhysicalAttachmentTypeNameAr { get; set; }
        [StringLength(400)]
        public string PhysicalAttachmentTypeNameEn { get; set; }
        public string PhysicalAttachmentTypeNameFn { get; set; }
        [StringLength(400)]
        public string AttachmentTypeCode { get; set; }
        [StringLength(400)]
        public string AttachmentTypeNameAr { get; set; }
        [StringLength(400)]
        public string AttachmentTypeNameEn { get; set; }
        public string AttachmentTypeNameFn { get; set; }
        public long TransactionId { get; set; }
        public int AttachmentId { get; set; }
        public bool IsShared { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        [StringLength(100)]
        public string CreatedByFullNameEn { get; set; }
        public string CreatedByFullNameFn { get; set; }
        [StringLength(100)]
        public string CreatedByFullNameAr { get; set; }
        public string OrgnazationNameAr { get; set; }
        public string OrgnazationNameEn { get; set; }
        public string OrgnazationNameFn { get; set; }
    }
}
