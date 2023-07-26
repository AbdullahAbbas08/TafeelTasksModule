using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class RecipientStatus
    {
        public RecipientStatus()
        {
            TransactionActionRecipientStatuses = new HashSet<TransactionActionRecipientStatus>();
            TransactionActionRecipientUpdateStatusCurrentRecipientStatuses = new HashSet<TransactionActionRecipientUpdateStatus>();
            TransactionActionRecipientUpdateStatusUpdateRecipientStatuses = new HashSet<TransactionActionRecipientUpdateStatus>();
            TransactionActionRecipients = new HashSet<TransactionActionRecipient>();
        }

        [Key]
        public int RecipientStatusId { get; set; }
        [StringLength(50)]
        public string RecipientStatusCode { get; set; }
        [StringLength(50)]
        public string RecipientStatusNameAr { get; set; }
        [StringLength(50)]
        public string RecipientStatusNameEn { get; set; }
        [StringLength(50)]
        public string RecipientStatusLocalizeNameAr { get; set; }
        [StringLength(50)]
        public string RecipientStatusLocalizeNameEn { get; set; }
        public string RecipientStatusLocalizeNameFn { get; set; }
        public string RecipientStatusNameFn { get; set; }

        [InverseProperty(nameof(TransactionActionRecipientStatus.RecipientStatus))]
        public virtual ICollection<TransactionActionRecipientStatus> TransactionActionRecipientStatuses { get; set; }
        [InverseProperty(nameof(TransactionActionRecipientUpdateStatus.CurrentRecipientStatus))]
        public virtual ICollection<TransactionActionRecipientUpdateStatus> TransactionActionRecipientUpdateStatusCurrentRecipientStatuses { get; set; }
        [InverseProperty(nameof(TransactionActionRecipientUpdateStatus.UpdateRecipientStatus))]
        public virtual ICollection<TransactionActionRecipientUpdateStatus> TransactionActionRecipientUpdateStatusUpdateRecipientStatuses { get; set; }
        [InverseProperty(nameof(TransactionActionRecipient.RecipientStatus))]
        public virtual ICollection<TransactionActionRecipient> TransactionActionRecipients { get; set; }
    }
}
