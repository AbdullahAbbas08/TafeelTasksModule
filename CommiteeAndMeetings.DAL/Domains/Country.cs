using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class Country
    {
        public Country()
        {
            AllowedCountries = new HashSet<AllowedCountry>();
        }

        [Key]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        [InverseProperty(nameof(AllowedCountry.Country))]
        public virtual ICollection<AllowedCountry> AllowedCountries { get; set; }
    }
}
