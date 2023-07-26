using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_Nationalities_CreatedBy")]
    [Index(nameof(UpdatedBy), Name = "IX_Nationalities_UpdatedBy")]
    public partial class Nationality
    {
        public Nationality()
        {
            Users = new HashSet<User>();
        }

        [Key]
        public int NationalityId { get; set; }
        [StringLength(400)]
        public string NationalityNameAr { get; set; }
        [StringLength(400)]
        public string NationalityNameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string NationalityNameFn { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.NationalityCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.NationalityUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
        [InverseProperty(nameof(User.Nationality))]
        public virtual ICollection<User> Users { get; set; }
    }
}
