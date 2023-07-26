using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class ArchivedeliverySheetDto
    {
        public int Id { get; set; }
        public string DeliverySheetNumber { get; set; }
        public int DeliverySheetId { get; set; }
        public int CorrespondentUserId { get; set; }
        public int? DeliverySheetStatusId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int DeliverySheetAttachmentsCount { get; set; }
        public string CorrespondUserNameAr { get; set; }
        public string CorrespondUserNameEn { get; set; }
        public string DoneByFullNameAr { get; set; }
        public string DoneByFullNameEn { get; set; }

        public bool IsExternal { get; set; }

        public List<DeliveryAttachmentViewDTO> deliveySheetAttachment { get; set; }
    }
}
