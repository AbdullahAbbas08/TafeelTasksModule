using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_IndividualRelationships_CreatedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_IndividualRelationships_UpdatedBy")]
    public partial class IndividualRelationship
    {
        public IndividualRelationship()
        {
            TransactionIndividuals = new HashSet<TransactionIndividual>();
        }

        [Key]
        public int IndividualRelationshipId { get; set; }
        [StringLength(400)]
        public string IndividualRelationshipNameAr { get; set; }
        [StringLength(400)]
        public string IndividualRelationshipNameEn { get; set; }
        public int DisplayOrder { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string IndividualRelationshipNameFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.IndividualRelationshipCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.IndividualRelationshipUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(TransactionIndividual.IndividualRelationship))]
        public virtual ICollection<TransactionIndividual> TransactionIndividuals { get; set; }
    }
}
