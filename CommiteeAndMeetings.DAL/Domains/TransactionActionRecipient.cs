using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CorrespondentUserId), Name = "IX_TransactionActionRecipients_CorrespondentUserId")]
    [Index(nameof(CreatedBy), Name = "IX_TransactionActionRecipients_CreatedBy")]
    [Index(nameof(DirectedToOrganizationId), Name = "IX_TransactionActionRecipients_DirectedToOrganizationId")]
    [Index(nameof(DirectedToUserId), Name = "IX_TransactionActionRecipients_DirectedToUserId")]
    [Index(nameof(RecipientStatusChangedBy), Name = "IX_TransactionActionRecipients_RecipientStatusChangedBy")]
    [Index(nameof(RecipientStatusId), Name = "IX_TransactionActionRecipients_RecipientStatusId")]
    [Index(nameof(RequiredActionId), Name = "IX_TransactionActionRecipients_RequiredActionId")]
    [Index(nameof(TransactionActionId), Name = "IX_TransactionActionRecipients_TransactionActionId")]
    [Index(nameof(UpdatedBy), Name = "IX_TransactionActionRecipients_UpdatedBy")]
    [Index(nameof(TransactionActionId), nameof(IsCc), nameof(TransactionActionRecipientId), nameof(DirectedToOrganizationId), nameof(DirectedToUserId), nameof(CorrespondentUserId), nameof(RequiredActionId), nameof(IsUrgent), nameof(UrgencyDaysCount), nameof(RecipientStatusId), nameof(SendNotification), Name = "_dta_index_TransactionActionRecipients_11_1390627997__K2_K6_K1_K4_K3_K7_K5_K11_K12_K8_K14_13")]
    [Index(nameof(TransactionActionId), nameof(DirectedToUserId), nameof(DirectedToOrganizationId), nameof(IsCc), nameof(RecipientStatusId), Name = "_dta_index_TransactionActionRecipients_45_1390627997__K2_K3_K4_K6_K8_4149")]
    [Index(nameof(TransactionActionId), nameof(DirectedToUserId), nameof(DirectedToOrganizationId), nameof(IsCc), nameof(RecipientStatusId), Name = "_dta_index_TransactionActionRecipients_45_1390627997__K2_K3_K4_K6_K8_7_16_4364")]
    [Index(nameof(TransactionActionId), nameof(DirectedToOrganizationId), nameof(CorrespondentUserId), nameof(TransactionActionRecipientId), nameof(CreatedOn), nameof(RecipientStatusId), nameof(DirectedToUserId), nameof(RequiredActionId), Name = "_dta_index_TransactionActionRecipients_45_1390627997__K2_K4_K7_K1_K16_K8_K3_K5_6_11")]
    [Index(nameof(DirectedToUserId), nameof(DirectedToOrganizationId), nameof(RecipientStatusId), nameof(CreatedOn), nameof(TransactionActionRecipientId), Name = "_dta_index_TransactionActionRecipients_45_1390627997__K3_K4_K8_K16_K1_6497")]
    [Index(nameof(DirectedToOrganizationId), nameof(TransactionActionRecipientId), nameof(DirectedToUserId), nameof(RecipientStatusId), nameof(CreatedOn), Name = "_dta_index_TransactionActionRecipients_45_1390627997__K4_K1_K3_K8_K16_1040")]
    [Index(nameof(RecipientStatusId), nameof(TransactionActionRecipientId), nameof(CreatedOn), nameof(DirectedToUserId), nameof(DirectedToOrganizationId), nameof(TransactionActionId), Name = "_dta_index_TransactionActionRecipients_45_1390627997__K8_K1_K16_K3_K4_K2")]
    public partial class TransactionActionRecipient
    {
        public TransactionActionRecipient()
        {
            Annotations = new HashSet<Annotation>();
            DeliveryCorrespondentTransactions = new HashSet<DeliveryCorrespondentTransaction>();
            DeliverySheetItems = new HashSet<DeliverySheetItem>();
            FollowUps = new HashSet<FollowUp>();
            Notifications = new HashSet<Notification>();
            TransactionActionRecipientAttachments = new HashSet<TransactionActionRecipientAttachment>();
            TransactionActionRecipientStatuses = new HashSet<TransactionActionRecipientStatus>();
            TransactionActionRecipientUpdateStatuses = new HashSet<TransactionActionRecipientUpdateStatus>();
            TransactionActions = new HashSet<TransactionAction>();
            TransactionDetailLogs = new HashSet<TransactionDetailLog>();
            TransactionWorkFlowProcesses = new HashSet<TransactionWorkFlowProcess>();
        }

        [Key]
        public int TransactionActionRecipientId { get; set; }
        public int TransactionActionId { get; set; }
        public int? DirectedToUserId { get; set; }
        public int? DirectedToOrganizationId { get; set; }
        public int? RequiredActionId { get; set; }
        [Column("IsCC")]
        public bool IsCc { get; set; }
        public int? CorrespondentUserId { get; set; }
        public int? RecipientStatusId { get; set; }
        public int? RecipientStatusChangedBy { get; set; }
        public DateTimeOffset? RecipientStatusChangedOn { get; set; }
        public bool IsUrgent { get; set; }
        public int? UrgencyDaysCount { get; set; }
        public string Notes { get; set; }
        public bool SendNotification { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool IsHidden { get; set; }
        public string NotesEn { get; set; }
        public string NotesFn { get; set; }
        public bool IsNoteHidden { get; set; }
        public int? ToCommitteId { get; set; }

        [ForeignKey(nameof(CorrespondentUserId))]
        [InverseProperty(nameof(User.TransactionActionRecipientCorrespondentUsers))]
        public virtual User CorrespondentUser { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.TransactionActionRecipientCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(DirectedToOrganizationId))]
        [InverseProperty(nameof(Organization.TransactionActionRecipients))]
        public virtual Organization DirectedToOrganization { get; set; }
        [ForeignKey(nameof(DirectedToUserId))]
        [InverseProperty(nameof(User.TransactionActionRecipientDirectedToUsers))]
        public virtual User DirectedToUser { get; set; }
        [ForeignKey(nameof(RecipientStatusId))]
        [InverseProperty("TransactionActionRecipients")]
        public virtual RecipientStatus RecipientStatus { get; set; }
        [ForeignKey(nameof(RecipientStatusChangedBy))]
        [InverseProperty(nameof(User.TransactionActionRecipientRecipientStatusChangedByNavigations))]
        public virtual User RecipientStatusChangedByNavigation { get; set; }
        [ForeignKey(nameof(RequiredActionId))]
        [InverseProperty("TransactionActionRecipients")]
        public virtual RequiredAction RequiredAction { get; set; }
        [ForeignKey(nameof(TransactionActionId))]
        [InverseProperty("TransactionActionRecipients")]
        public virtual TransactionAction TransactionAction { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.TransactionActionRecipientUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(Annotation.ReferrerTransactionActionRecipient))]
        public virtual ICollection<Annotation> Annotations { get; set; }
        [InverseProperty(nameof(DeliveryCorrespondentTransaction.TransactionActionRecipient))]
        public virtual ICollection<DeliveryCorrespondentTransaction> DeliveryCorrespondentTransactions { get; set; }
        [InverseProperty(nameof(DeliverySheetItem.TransactionActionRecipient))]
        public virtual ICollection<DeliverySheetItem> DeliverySheetItems { get; set; }
        [InverseProperty(nameof(FollowUp.TransactionActionRecipient))]
        public virtual ICollection<FollowUp> FollowUps { get; set; }
        [InverseProperty(nameof(Notification.TransactionActionRecipient))]
        public virtual ICollection<Notification> Notifications { get; set; }
        [InverseProperty(nameof(TransactionActionRecipientAttachment.TransactionActionRecipient))]
        public virtual ICollection<TransactionActionRecipientAttachment> TransactionActionRecipientAttachments { get; set; }
        [InverseProperty(nameof(TransactionActionRecipientStatus.TransactionActionRecipient))]
        public virtual ICollection<TransactionActionRecipientStatus> TransactionActionRecipientStatuses { get; set; }
        [InverseProperty(nameof(TransactionActionRecipientUpdateStatus.TransactionActionRecipient))]
        public virtual ICollection<TransactionActionRecipientUpdateStatus> TransactionActionRecipientUpdateStatuses { get; set; }
        [InverseProperty("ReferrerTransactionActionRecipient")]
        public virtual ICollection<TransactionAction> TransactionActions { get; set; }
        [InverseProperty(nameof(TransactionDetailLog.TransactionActionRecipient))]
        public virtual ICollection<TransactionDetailLog> TransactionDetailLogs { get; set; }
        [InverseProperty(nameof(TransactionWorkFlowProcess.TransactionActionRecepient))]
        public virtual ICollection<TransactionWorkFlowProcess> TransactionWorkFlowProcesses { get; set; }
    }
}
