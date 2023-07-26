using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("FollowUpStatementType")]
    public partial class FollowUpStatementType
    {
        public FollowUpStatementType()
        {
            FollowUpStatements = new HashSet<FollowUpStatement>();
        }

        [Key]
        public int FollowUpStatementTypeId { get; set; }
        public string FollowUpStatementCode { get; set; }
        public string FollowUpStatementNameAr { get; set; }
        public string FollowUpStatementNameEn { get; set; }
        public string FollowUpStatementNameFn { get; set; }

        [InverseProperty(nameof(FollowUpStatement.FollowUpStatementType))]
        public virtual ICollection<FollowUpStatement> FollowUpStatements { get; set; }
    }
}
