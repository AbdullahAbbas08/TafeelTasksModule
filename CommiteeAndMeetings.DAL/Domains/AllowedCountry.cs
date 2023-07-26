using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CountryId), Name = "IX_AllowedCountries_CountryId")]
    public partial class AllowedCountry
    {
        [Key]
        public int AllowedCountryId { get; set; }
        public int CountryId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [ForeignKey(nameof(CountryId))]
        [InverseProperty("AllowedCountries")]
        public virtual Country Country { get; set; }
    }
}
