using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmDeliveryAttachmentView
    {
        public int Id { get; set; }
        public int DeliverySheetAttachmentId { get; set; }
        public int DeliverySheetId { get; set; }
        public int? AttachmentId { get; set; }
        [StringLength(500)]
        public string AttachmentName { get; set; }
        public int? AttachmentTypeId { get; set; }
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
    }
}
