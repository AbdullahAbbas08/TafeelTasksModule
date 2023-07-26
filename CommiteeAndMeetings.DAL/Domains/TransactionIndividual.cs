using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_TransactionIndividuals_CreatedBy")]
    [Index(nameof(IndividualRelationshipId), Name = "IX_TransactionIndividuals_IndividualRelationshipId")]
    [Index(nameof(TransactionId), Name = "IX_TransactionIndividuals_TransactionId")]
    [Index(nameof(UpdatedBy), Name = "IX_TransactionIndividuals_UpdatedBy")]
    [Index(nameof(UserId), Name = "IX_TransactionIndividuals_UserId")]
    public partial class TransactionIndividual
    {
        [Key]
        public int TransactionIndividualId { get; set; }
        public long TransactionId { get; set; }
        public int UserId { get; set; }
        public int? IndividualRelationshipId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty("TransactionIndividualCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(IndividualRelationshipId))]
        [InverseProperty("TransactionIndividuals")]
        public virtual IndividualRelationship IndividualRelationship { get; set; }
        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("TransactionIndividuals")]
        public virtual Transaction Transaction { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty("TransactionIndividualUpdatedByNavigations")]
        public virtual User UpdatedByNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("TransactionIndividualUsers")]
        public virtual User User { get; set; }
    }
}
