using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(ClassificationId), Name = "IX_Transactions_ClassificationId")]
    [Index(nameof(ConfidentialityLevelId), Name = "IX_Transactions_ConfidentialityLevelId")]
    [Index(nameof(CreatedBy), Name = "IX_Transactions_CreatedBy")]
    [Index(nameof(DeletedBy), Name = "IX_Transactions_DeletedBy")]
    [Index(nameof(ImportanceLevelId), Name = "IX_Transactions_ImportanceLevelId")]
    [Index(nameof(IncomingOrganizationId), Name = "IX_Transactions_IncomingOrganizationId")]
    [Index(nameof(IncomingTypeId), Name = "IX_Transactions_IncomingTypeId")]
    [Index(nameof(TransactionBasisTypeId), Name = "IX_Transactions_TransactionBasisTypeId")]
    [Index(nameof(TransactionTypeId), Name = "IX_Transactions_TransactionTypeId")]
    [Index(nameof(UpdatedBy), Name = "IX_Transactions_UpdatedBy")]
    [Index(nameof(ConfidentialityLevelId), nameof(TransactionTypeId), nameof(IncomingOrganizationId), nameof(ExecutionPeriod), nameof(TransactionNumberFormatted), nameof(ExecutionDate), nameof(ImportanceLevelId), nameof(TransactionId), Name = "_dta_index_Transactions_11_2121162702__K11_K7_K15_K22_K31_K21_K10_K1_2_4_5_6_14_25")]
    [Index(nameof(TransactionId), nameof(CreatedBy), nameof(CreatedByUserRoleId), nameof(IsCancelled), nameof(TransactionTypeId), Name = "_dta_index_Transactions_11_2121162702__K1_K24_K35_K32_K7")]
    [Index(nameof(TransactionId), nameof(TransactionNumberFormatted), nameof(TransactionNumber), nameof(ImportanceLevelId), nameof(ConfidentialityLevelId), nameof(TransactionTypeId), nameof(IncomingOrganizationId), Name = "_dta_index_Transactions_11_2121162702__K1_K31_K2_K10_K11_K7_K15_4_5_6_14_21_22_25")]
    [Index(nameof(TransactionId), nameof(TransactionTypeId), nameof(ExecutionPeriod), nameof(ExecutionDate), nameof(ImportanceLevelId), nameof(ConfidentialityLevelId), nameof(IncomingOrganizationId), Name = "_dta_index_Transactions_11_2121162702__K1_K7_K22_K21_K10_K11_K15_2_4_5_6_14_25_31")]
    [Index(nameof(TransactionId), nameof(TransactionTypeId), nameof(IsCancelled), nameof(CreatedBy), nameof(CreatedByUserRoleId), Name = "_dta_index_Transactions_11_2121162702__K1_K7_K32_K24_K35_2_3_4_5_6_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_25_26_27_28_")]
    [Index(nameof(TransactionTypeId), nameof(TransactionId), nameof(ConfidentialityLevelId), nameof(ImportanceLevelId), nameof(IncomingOrganizationId), Name = "_dta_index_Transactions_11_2121162702__K7_K1_K11_K10_K15_2_4_5_6_14_21_22_25_31")]
    [Index(nameof(ConfidentialityLevelId), nameof(TransactionId), nameof(TransactionTypeId), nameof(TransactionNumber), nameof(TransactionNumberFormatted), Name = "_dta_index_Transactions_45_2121162702__K11_K1_K7_K2_K31")]
    [Index(nameof(ConfidentialityLevelId), nameof(TransactionTypeId), nameof(IncomingOrganizationId), nameof(ImportanceLevelId), nameof(TransactionId), Name = "_dta_index_Transactions_45_2121162702__K11_K7_K15_K10_K1_25_114")]
    [Index(nameof(TransactionId), nameof(TransactionTypeId), nameof(ConfidentialityLevelId), Name = "_dta_index_Transactions_45_2121162702__K1_K7_K11_8337")]
    [Index(nameof(CreatedBy), nameof(TransactionId), nameof(CreatedByUserRoleId), nameof(IsCancelled), nameof(TransactionTypeId), Name = "_dta_index_Transactions_45_2121162702__K24_K1_K35_K32_K7_1040")]
    [Index(nameof(CreatedBy), nameof(CreatedByUserRoleId), nameof(IsCancelled), nameof(TransactionId), nameof(TransactionTypeId), Name = "_dta_index_Transactions_45_2121162702__K24_K35_K32_K1_K7_2533")]
    [Index(nameof(CreatedBy), nameof(CreatedByUserRoleId), nameof(IsCancelled), nameof(TransactionNumber), nameof(TransactionId), nameof(TransactionTypeId), nameof(TransactionDate), nameof(TransactionBasisTypeId), nameof(ClassificationId), nameof(ImportanceLevelId), nameof(ConfidentialityLevelId), nameof(IncomingTypeId), nameof(IncomingLetterNumber), nameof(TransactionNumberFormatted), Name = "_dta_index_Transactions_45_2121162702__K24_K35_K32_K2_K1_K7_K3_K8_K9_K10_K11_K12_K13_K31_4_5_6_14_15_16_17_18_19_20_21_2_6221")]
    [Index(nameof(TransactionTypeId), nameof(HijriMonth), nameof(HijriYear), Name = "_dta_index_Transactions_45_2121162702__K7_K33_K34_25_5201")]
    public partial class Transaction
    {
        public Transaction()
        {
            Annotations = new HashSet<Annotation>();
            AssignmentComments = new HashSet<AssignmentComment>();
            ChatRooms = new HashSet<ChatRoom>();
            FollowUps = new HashSet<FollowUp>();
            Notifications = new HashSet<Notification>();
            RelatedTransactionChildTransactions = new HashSet<RelatedTransaction>();
            RelatedTransactionParentTransactions = new HashSet<RelatedTransaction>();
            TransactionActions = new HashSet<TransactionAction>();
            TransactionAttachmentIndices = new HashSet<TransactionAttachmentIndex>();
            TransactionAttachments = new HashSet<TransactionAttachment>();
            TransactionDetailLogs = new HashSet<TransactionDetailLog>();
            TransactionIndividuals = new HashSet<TransactionIndividual>();
            TransactionTags = new HashSet<TransactionTag>();
            TransfaredFaxes = new HashSet<TransfaredFaxis>();
        }

        [Key]
        public long TransactionId { get; set; }
        [StringLength(25)]
        public string TransactionNumber { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public string Subject { get; set; }
        public string NormalizedSubject { get; set; }
        public string Notes { get; set; }
        public int TransactionTypeId { get; set; }
        public int? TransactionBasisTypeId { get; set; }
        public int? ClassificationId { get; set; }
        public int? ImportanceLevelId { get; set; }
        public int? ConfidentialityLevelId { get; set; }
        public int? IncomingTypeId { get; set; }
        [StringLength(50)]
        public string IncomingLetterNumber { get; set; }
        public DateTimeOffset? IncomingLetterDate { get; set; }
        public int? IncomingOrganizationId { get; set; }
        [StringLength(50)]
        public string IncomingCorrespondentName { get; set; }
        [StringLength(50)]
        public string IncomingCorrespondentMobileNumber { get; set; }
        [StringLength(50)]
        public string IncomingCorrespondentEmail { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTimeOffset? ExecutionDate { get; set; }
        public int ExecutionPeriod { get; set; }
        public bool IsForAll { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public DateTimeOffset? IncomingOutgoingLetterDate { get; set; }
        [StringLength(35)]
        public string TransactionNumberFormatted { get; set; }
        [Column("isCancelled")]
        public bool? IsCancelled { get; set; }
        public int HijriMonth { get; set; }
        public int HijriYear { get; set; }
        public int? CreatedByUserRoleId { get; set; }
        public string SubjectEn { get; set; }
        public string SubjectFn { get; set; }
        public int? CompanyId { get; set; }
        public bool IsShowAllTime { get; set; }

        [ForeignKey(nameof(ClassificationId))]
        [InverseProperty("Transactions")]
        public virtual Classification Classification { get; set; }
        [ForeignKey(nameof(ConfidentialityLevelId))]
        [InverseProperty("Transactions")]
        public virtual ConfidentialityLevel ConfidentialityLevel { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.TransactionCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        [InverseProperty(nameof(User.TransactionDeletedByNavigations))]
        public virtual User DeletedByNavigation { get; set; }
        [ForeignKey(nameof(ImportanceLevelId))]
        [InverseProperty("Transactions")]
        public virtual ImportanceLevel ImportanceLevel { get; set; }
        [ForeignKey(nameof(IncomingOrganizationId))]
        [InverseProperty(nameof(Organization.Transactions))]
        public virtual Organization IncomingOrganization { get; set; }
        [ForeignKey(nameof(IncomingTypeId))]
        [InverseProperty("Transactions")]
        public virtual IncomingType IncomingType { get; set; }
        [ForeignKey(nameof(TransactionBasisTypeId))]
        [InverseProperty("Transactions")]
        public virtual TransactionBasisType TransactionBasisType { get; set; }
        [ForeignKey(nameof(TransactionTypeId))]
        [InverseProperty("Transactions")]
        public virtual TransactionType TransactionType { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.TransactionUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(Annotation.ReferrerTransaction))]
        public virtual ICollection<Annotation> Annotations { get; set; }
        [InverseProperty(nameof(AssignmentComment.AssignmentTransaction))]
        public virtual ICollection<AssignmentComment> AssignmentComments { get; set; }
        [InverseProperty(nameof(ChatRoom.Transaction))]
        public virtual ICollection<ChatRoom> ChatRooms { get; set; }
        [InverseProperty(nameof(FollowUp.Transaction))]
        public virtual ICollection<FollowUp> FollowUps { get; set; }
        [InverseProperty(nameof(Notification.Transaction))]
        public virtual ICollection<Notification> Notifications { get; set; }
        [InverseProperty(nameof(RelatedTransaction.ChildTransaction))]
        public virtual ICollection<RelatedTransaction> RelatedTransactionChildTransactions { get; set; }
        [InverseProperty(nameof(RelatedTransaction.ParentTransaction))]
        public virtual ICollection<RelatedTransaction> RelatedTransactionParentTransactions { get; set; }
        [InverseProperty(nameof(TransactionAction.Transaction))]
        public virtual ICollection<TransactionAction> TransactionActions { get; set; }
        [InverseProperty(nameof(TransactionAttachmentIndex.Transaction))]
        public virtual ICollection<TransactionAttachmentIndex> TransactionAttachmentIndices { get; set; }
        [InverseProperty(nameof(TransactionAttachment.Transaction))]
        public virtual ICollection<TransactionAttachment> TransactionAttachments { get; set; }
        [InverseProperty(nameof(TransactionDetailLog.Transaction))]
        public virtual ICollection<TransactionDetailLog> TransactionDetailLogs { get; set; }
        [InverseProperty(nameof(TransactionIndividual.Transaction))]
        public virtual ICollection<TransactionIndividual> TransactionIndividuals { get; set; }
        [InverseProperty(nameof(TransactionTag.Transaction))]
        public virtual ICollection<TransactionTag> TransactionTags { get; set; }
        [InverseProperty(nameof(TransfaredFaxis.Transaction))]
        public virtual ICollection<TransfaredFaxis> TransfaredFaxes { get; set; }
    }
}
