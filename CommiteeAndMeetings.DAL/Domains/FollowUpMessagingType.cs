using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("FollowUpMessagingType")]
    public partial class FollowUpMessagingType
    {
        public FollowUpMessagingType()
        {
            FollowUpStatements = new HashSet<FollowUpStatement>();
        }

        [Key]
        public int FollowUpMessagingTypeId { get; set; }
        public string FollowUpMessagingTypeCode { get; set; }
        public string FollowUpMessagingTypeNameAr { get; set; }
        public string FollowUpMessagingTypeNameEn { get; set; }
        public string FollowUpMessagingTypeNameFn { get; set; }

        [InverseProperty(nameof(FollowUpStatement.FollowUpMessagingType))]
        public virtual ICollection<FollowUpStatement> FollowUpStatements { get; set; }
    }
}
