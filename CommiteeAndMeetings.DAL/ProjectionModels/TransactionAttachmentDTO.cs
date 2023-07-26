using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.ProjectionModels
{
    public class TransactionAttachmentDTO
    {
        public int TransactionAttachmentId { get; set; }
        public long TransactionId { get; set; }
        public int AttachmentId { get; set; }
        public bool IsShared { get; set; }
        [NotMapped]
        public int EntryId { get; set; }
        [NotMapped]
        public int AttachmentTypeId { get; set; }
        [NotMapped]
        public string AttachmentName { get; set; }
        [NotMapped]
        public string Notes { get; set; }
        [NotMapped]
        public int? PagesCount { get; set; }
        [NotMapped]
        public string MimeType { get; set; }
        [NotMapped]
        public int? ReferenceAttachmentId { get; set; }
        public int? UserRoleId { get; set; }
        public string OrgnazationNameAr { get; set; }
        public string OrgnazationNameEn { get; set; }
        public string OrgnazationNameFn { get; set; }
        public string attachmentIdEncypted { get; set; }
        public string transactionIdEncypted { get; set; }

    }
}
