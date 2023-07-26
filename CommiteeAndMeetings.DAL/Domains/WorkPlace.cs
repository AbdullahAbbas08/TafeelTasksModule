using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_WorkPlaces_CreatedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_WorkPlaces_UpdatedBy")]
    public partial class WorkPlace
    {
        [Key]
        public int WorkPlaceId { get; set; }
        [StringLength(100)]
        public string WorkPlaceNameAr { get; set; }
        [StringLength(100)]
        public string WorkPlaceNameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string WorkPlaceNameFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.WorkPlaceCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.WorkPlaceUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
