using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.DAL.ModelInterfaces;
using CommiteeDatabase.Models.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    [Table("ComiteeTaskCategory", Schema = "Committe")]
    public class ComiteeTaskCategory : _BaseEntity, IAuditableInsert, IAuditableUpdate, IAuditableDelete
    {
        public ComiteeTaskCategory()
        {
            CommiteeTaskEscalations = new HashSet<CommiteeTaskEscalation>();
        }
        public int ComiteeTaskCategoryId { get; set; }
        public string categoryNameAr { get; set; }
        public string categoryNameEn { get; set; }


        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual User CreatedByUser { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        [ForeignKey("CreatedByRole")]
        public int? CreatedByRoleId { get; set; }
        public virtual CommiteeUsersRole CreatedByRole { get; set; }

        [InverseProperty(nameof(CommiteeTaskEscalation.ComiteeTaskCategory))]
        public virtual ICollection<CommiteeTaskEscalation> CommiteeTaskEscalations { get; set; }
    }
}
