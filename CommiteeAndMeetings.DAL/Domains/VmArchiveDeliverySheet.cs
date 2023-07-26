using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmArchiveDeliverySheet
    {
        public int Id { get; set; }
        public int DeliverySheetId { get; set; }
        [StringLength(25)]
        public string DeliverySheetNumber { get; set; }
        public int CorrespondentUserId { get; set; }
        public int? DeliverySheetStatusId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [StringLength(100)]
        public string CorrespondUserNameAr { get; set; }
        [StringLength(100)]
        public string CorrespondUserNameEn { get; set; }
        [StringLength(100)]
        public string DoneByFullNameAr { get; set; }
        [StringLength(100)]
        public string DoneByFullNameEn { get; set; }
        public int? DeliverySheetAttachmentsCount { get; set; }
        public bool IsExternal { get; set; }
    }
}
