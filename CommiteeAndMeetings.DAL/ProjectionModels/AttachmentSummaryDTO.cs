namespace Models
{
    public class AttachmentSummaryDTO
    {
        public int AttachmentId { get; set; }
        public int AttachmentTypeId { get; set; }
        public bool IsDisabled { get; set; } = false;
        public string LFEntryId { get; set; }

        /// <summary>
        /// إسم المرفق أو الخطاب أو نوع الملف الفيزيكال
        /// </summary>
        public string AttachmentName { get; set; }

        public int? PagesCount { get; set; }
        public string Notes { get; set; }

        public bool IsShared { get; set; }
        public int? ReferenceAttachmentId { get; set; }

    }
}