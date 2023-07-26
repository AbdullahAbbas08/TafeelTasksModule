using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(ActionId), Name = "IX_TransactionActions_ActionId")]
    [Index(nameof(CreatedBy), Name = "IX_TransactionActions_CreatedBy")]
    [Index(nameof(CreatedByUserRoleId), Name = "IX_TransactionActions_CreatedByUserRoleId")]
    [Index(nameof(DirectedFromOrganizationId), Name = "IX_TransactionActions_DirectedFromOrganizationId")]
    [Index(nameof(DirectedFromUserId), Name = "IX_TransactionActions_DirectedFromUserId")]
    [Index(nameof(ReferrerTransactionActionId), Name = "IX_TransactionActions_ReferrerTransactionActionId")]
    [Index(nameof(ReferrerTransactionActionRecipientId), Name = "IX_TransactionActions_ReferrerTransactionActionRecipientId")]
    [Index(nameof(TransactionId), Name = "IX_TransactionActions_TransactionId")]
    [Index(nameof(UpdatedBy), Name = "IX_TransactionActions_UpdatedBy")]
    [Index(nameof(CreatedOn), Name = "NonClusteredIndex-TrActionCreatedOn")]
    [Index(nameof(TransactionActionId), nameof(ArchieveDate), nameof(ActionId), nameof(TransactionId), nameof(CreatedBy), nameof(DirectedFromUserId), nameof(DirectedFromOrganizationId), Name = "_dta_index_TransactionActions_11_1230627427__K1_K23_K4_K3_K16_K7_K8_5_17")]
    [Index(nameof(TransactionActionId), nameof(TransactionId), nameof(DirectedFromUserId), nameof(DirectedFromOrganizationId), nameof(ArchieveDate), nameof(ActionId), nameof(CreatedBy), Name = "_dta_index_TransactionActions_11_1230627427__K1_K3_K7_K8_K23_K4_K16_5_17")]
    [Index(nameof(TransactionId), nameof(TransactionActionId), nameof(ActionId), nameof(CreatedBy), nameof(DirectedFromUserId), nameof(DirectedFromOrganizationId), Name = "_dta_index_TransactionActions_11_1230627427__K3_K1_K4_K16_K7_K8_5_17_23")]
    [Index(nameof(DirectedFromOrganizationId), nameof(ActionId), nameof(ArchieveDate), nameof(TransactionActionId), nameof(TransactionId), Name = "_dta_index_TransactionActions_11_1230627427__K8_K4_K23_K1_K3")]
    [Index(nameof(TransactionActionId), nameof(TransactionId), nameof(ArchieveDate), nameof(ActionId), nameof(DirectedFromUserId), nameof(DirectedFromOrganizationId), Name = "_dta_index_TransactionActions_45_1230627427__K1_K3_K23_K4_K7_K8_9850")]
    [Index(nameof(TransactionActionId), nameof(TransactionId), nameof(ActionId), Name = "_dta_index_TransactionActions_45_1230627427__K1_K3_K4_8526_8341")]
    [Index(nameof(ArchieveDate), nameof(ActionId), nameof(TransactionActionId), nameof(TransactionId), Name = "_dta_index_TransactionActions_45_1230627427__K23_K4_K1_K3_8066")]
    [Index(nameof(ArchieveDate), nameof(ActionId), nameof(DirectedFromUserId), nameof(DirectedFromOrganizationId), nameof(TransactionId), nameof(TransactionActionId), Name = "_dta_index_TransactionActions_45_1230627427__K23_K4_K7_K8_K3_K1")]
    [Index(nameof(TransactionId), nameof(TransactionActionId), nameof(ActionId), Name = "_dta_index_TransactionActions_45_1230627427__K3_K1_K4_1410")]
    [Index(nameof(ActionId), nameof(TransactionActionId), nameof(CreatedByUserRoleId), nameof(TransactionId), nameof(CreatedBy), nameof(DirectedFromUserId), nameof(DirectedFromOrganizationId), Name = "_dta_index_TransactionActions_45_1230627427__K4_K1_K10_K3_K16_K7_K8_5_17_23_2533")]
    [Index(nameof(ActionId), nameof(ArchieveDate), nameof(TransactionActionId), nameof(TransactionId), Name = "_dta_index_TransactionActions_45_1230627427__K4_K23_K1_K3_8_10")]
    [Index(nameof(ActionId), nameof(ArchieveDate), nameof(TransactionActionId), nameof(TransactionId), nameof(CreatedBy), nameof(DirectedFromUserId), nameof(DirectedFromOrganizationId), Name = "_dta_index_TransactionActions_45_1230627427__K4_K23_K1_K3_K16_K7_K8_5_17_3982")]
    [Index(nameof(ActionId), nameof(DirectedFromOrganizationId), nameof(ArchieveDate), nameof(TransactionActionId), nameof(TransactionId), Name = "_dta_index_TransactionActions_45_1230627427__K4_K8_K23_K1_K3_2533")]
    public partial class TransactionAction
    {
        public TransactionAction()
        {
            Annotations = new HashSet<Annotation>();
            FollowUps = new HashSet<FollowUp>();
            InverseReferrerTransactionAction = new HashSet<TransactionAction>();
            Notifications = new HashSet<Notification>();
            TransactionActionAttachments = new HashSet<TransactionActionAttachment>();
            TransactionActionRecipients = new HashSet<TransactionActionRecipient>();
            TransactionDetailLogs = new HashSet<TransactionDetailLog>();
            TransactionWorkFlowProcesses = new HashSet<TransactionWorkFlowProcess>();
        }

        [Key]
        public int TransactionActionId { get; set; }
        public long TransactionId { get; set; }
        public int ActionId { get; set; }
        public int? ReferrerTransactionActionId { get; set; }
        public int? ReferrerTransactionActionRecipientId { get; set; }
        public int? DirectedFromUserId { get; set; }
        public int? DirectedFromOrganizationId { get; set; }
        public int CreatedByUserRoleId { get; set; }
        public DateTimeOffset? DisabledUntil { get; set; }
        [StringLength(25)]
        public string OutgoingTransactionNumber { get; set; }
        public int? OutgoingTransactionTypeId { get; set; }
        public int? OutgoingImportanceLevelId { get; set; }
        public bool OutgoingIsConfidential { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        [Column("archieveDate")]
        public int ArchieveDate { get; set; }
        public string ActionNumber { get; set; }
        public int? MaxTransactionActionRecipientId { get; set; }
        public int? FromCommitteId { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.TransactionActionCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(CreatedByUserRoleId))]
        [InverseProperty(nameof(UserRole.TransactionActions))]
        public virtual UserRole CreatedByUserRole { get; set; }
        [ForeignKey(nameof(DirectedFromOrganizationId))]
        [InverseProperty(nameof(Organization.TransactionActions))]
        public virtual Organization DirectedFromOrganization { get; set; }
        [ForeignKey(nameof(DirectedFromUserId))]
        [InverseProperty(nameof(User.TransactionActionDirectedFromUsers))]
        public virtual User DirectedFromUser { get; set; }
        [ForeignKey(nameof(ReferrerTransactionActionId))]
        [InverseProperty(nameof(TransactionAction.InverseReferrerTransactionAction))]
        public virtual TransactionAction ReferrerTransactionAction { get; set; }
        [ForeignKey(nameof(ReferrerTransactionActionRecipientId))]
        [InverseProperty(nameof(TransactionActionRecipient.TransactionActions))]
        public virtual TransactionActionRecipient ReferrerTransactionActionRecipient { get; set; }
        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("TransactionActions")]
        public virtual Transaction Transaction { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.TransactionActionUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(Annotation.ReferrerTransactionAction))]
        public virtual ICollection<Annotation> Annotations { get; set; }
        [InverseProperty(nameof(FollowUp.TransactionAction))]
        public virtual ICollection<FollowUp> FollowUps { get; set; }
        [InverseProperty(nameof(TransactionAction.ReferrerTransactionAction))]
        public virtual ICollection<TransactionAction> InverseReferrerTransactionAction { get; set; }
        [InverseProperty(nameof(Notification.TransactionAction))]
        public virtual ICollection<Notification> Notifications { get; set; }
        [InverseProperty(nameof(TransactionActionAttachment.TransactionAction))]
        public virtual ICollection<TransactionActionAttachment> TransactionActionAttachments { get; set; }
        [InverseProperty(nameof(TransactionActionRecipient.TransactionAction))]
        public virtual ICollection<TransactionActionRecipient> TransactionActionRecipients { get; set; }
        [InverseProperty(nameof(TransactionDetailLog.TransactionAction))]
        public virtual ICollection<TransactionDetailLog> TransactionDetailLogs { get; set; }
        [InverseProperty(nameof(TransactionWorkFlowProcess.TransactionAction))]
        public virtual ICollection<TransactionWorkFlowProcess> TransactionWorkFlowProcesses { get; set; }
    }
}
