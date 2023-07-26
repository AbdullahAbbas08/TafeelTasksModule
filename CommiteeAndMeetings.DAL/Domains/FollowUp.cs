using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(ChangeStatusByUserId), Name = "IX_FollowUps_ChangeStatusByUserId")]
    [Index(nameof(CreatedBy), Name = "IX_FollowUps_CreatedBy")]
    [Index(nameof(DeletedBy), Name = "IX_FollowUps_DeletedBy")]
    [Index(nameof(FollowUpStatusTypeId), Name = "IX_FollowUps_FollowUpStatusTypeId")]
    [Index(nameof(OrganizationId), Name = "IX_FollowUps_OrganizationId")]
    [Index(nameof(TransactionActionId), Name = "IX_FollowUps_TransactionActionId")]
    [Index(nameof(TransactionActionRecipientId), Name = "IX_FollowUps_TransactionActionRecipientId")]
    [Index(nameof(TransactionId), Name = "IX_FollowUps_TransactionId")]
    [Index(nameof(UserId), Name = "IX_FollowUps_UserId")]
    public partial class FollowUp
    {
        public FollowUp()
        {
            FollowUpDateModifieds = new HashSet<FollowUpDateModified>();
            FollowUpStatements = new HashSet<FollowUpStatement>();
            FollowUpStatuses = new HashSet<FollowUpStatus>();
        }

        [Key]
        public int FollowUpId { get; set; }
        public long? TransactionId { get; set; }
        public int? TransactionActionId { get; set; }
        public int? TransactionActionRecipientId { get; set; }
        public int? UserId { get; set; }
        public int? OrganizationId { get; set; }
        public int FollowUpStatusTypeId { get; set; }
        public DateTimeOffset? FinishedDate { get; set; }
        public DateTimeOffset FollowUpStatusCreatedOn { get; set; }
        public int? ChangeStatusByUserId { get; set; }
        public DateTimeOffset? ModifiedDateTo { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

        [ForeignKey(nameof(ChangeStatusByUserId))]
        [InverseProperty("FollowUpChangeStatusByUsers")]
        public virtual User ChangeStatusByUser { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty("FollowUpCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        [InverseProperty("FollowUpDeletedByNavigations")]
        public virtual User DeletedByNavigation { get; set; }
        [ForeignKey(nameof(FollowUpStatusTypeId))]
        [InverseProperty("FollowUps")]
        public virtual FollowUpStatusType FollowUpStatusType { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("FollowUps")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("FollowUps")]
        public virtual Transaction Transaction { get; set; }
        [ForeignKey(nameof(TransactionActionId))]
        [InverseProperty("FollowUps")]
        public virtual TransactionAction TransactionAction { get; set; }
        [ForeignKey(nameof(TransactionActionRecipientId))]
        [InverseProperty("FollowUps")]
        public virtual TransactionActionRecipient TransactionActionRecipient { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("FollowUpUsers")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(FollowUpDateModified.FollowUp))]
        public virtual ICollection<FollowUpDateModified> FollowUpDateModifieds { get; set; }
        [InverseProperty(nameof(FollowUpStatement.FollowUp))]
        public virtual ICollection<FollowUpStatement> FollowUpStatements { get; set; }
        [InverseProperty(nameof(FollowUpStatus.FollowUp))]
        public virtual ICollection<FollowUpStatus> FollowUpStatuses { get; set; }
    }
}
