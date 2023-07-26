using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_Genders_CreatedBy")]
    public partial class Gender
    {
        [Key]
        public int GenderId { get; set; }
        [StringLength(400)]
        public string GenderNameAr { get; set; }
        [StringLength(400)]
        public string GenderNameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string GenderNameFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.Genders))]
        public virtual User CreatedByNavigation { get; set; }
    }
}
