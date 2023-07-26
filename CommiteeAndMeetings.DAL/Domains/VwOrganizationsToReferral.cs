using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("Vw_OrganizationsToReferral")]
    public partial class VwOrganizationsToReferral
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string FullPathAr { get; set; }
        public string FullPathEn { get; set; }
    }
}
