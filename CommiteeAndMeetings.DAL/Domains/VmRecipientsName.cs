using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    public partial class VmRecipientsName
    {
        public int Id { get; set; }
        public int TransactionActionRecipientId { get; set; }
        public int TransactionActionId { get; set; }
        [Column("IsCC")]
        public bool IsCc { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string NameFn { get; set; }
        public int? DirectedToUserId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public byte[] ProfileImage { get; set; }
        public int? CorrespondentUserId { get; set; }
        public bool IsUrgent { get; set; }
        public int? UrgencyDaysCount { get; set; }
        public string Notes { get; set; }
        public int? RecipientStatusId { get; set; }
        public int? RequiredActionId { get; set; }
        public bool SendNotification { get; set; }
        [StringLength(100)]
        public string CorrespondentUserNameAr { get; set; }
        [StringLength(100)]
        public string CorrespondentUserNameEn { get; set; }
        public string CorrespondentUserNameFn { get; set; }
        public bool? IsOuterOrganization { get; set; }
        [StringLength(50)]
        public string RequiredActionNameAr { get; set; }
        [StringLength(50)]
        public string RequiredActionNameEn { get; set; }
        public string RequiredActionNameFn { get; set; }
    }
}
