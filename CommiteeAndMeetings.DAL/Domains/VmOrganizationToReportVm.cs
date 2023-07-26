using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("Vm_OrganizationToReport_vm")]
    public partial class VmOrganizationToReportVm
    {
        [Key]
        public int Id { get; set; }
        public int? ParentOrganizationId { get; set; }
        public bool IsCategory { get; set; }
        [Required]
        public string Code { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string FullPathAr { get; set; }
        public string FullPathEn { get; set; }
        public bool IsAdminOrganization { get; set; }
        public string FullPathFn { get; set; }
        public string OrganizationNameFn { get; set; }
    }
}
