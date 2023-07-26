using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("Vm_functions_IncomingOrganization_report_View")]
    public partial class VmFunctionsIncomingOrganizationReportView
    {
        [Key]
        public int Id { get; set; }
        public int? StatusId { get; set; }
        public string StatusNameAr { get; set; }
        public string StatusNameEn { get; set; }
        public int? DelegatedOrganizationId { get; set; }
        public string DelegatedOrganizationNameAr { get; set; }
        public string DelegatedOrganizationNameEn { get; set; }
    }
}
