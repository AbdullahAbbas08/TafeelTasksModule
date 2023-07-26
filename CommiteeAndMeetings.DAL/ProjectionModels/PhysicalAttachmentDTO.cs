﻿namespace Models
{
    public class PhysicalAttachmentDTO
    {
        public int AttachmentId { get; set; }
        public int? PhysicalAttachmentTypeId { get; set; }
        public string PhysicalAttachmentTypeName { get; set; }
        public int? PagesCount { get; set; }
        public string Notes { get; set; }
        public bool IsShared { get; set; }
    }
}