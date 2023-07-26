using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("Vm_IncomingOrganization_report_View")]
    public partial class VmIncomingOrganizationReportView
    {
        [Key]
        public long Id { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string Subject { get; set; }
        public int? IncomingOrganizationId { get; set; }
        public string IncomingLetterNumber { get; set; }
        public DateTimeOffset? IncomingLetterDate { get; set; }
        [Column("TCreatedOn")]
        public DateTimeOffset TcreatedOn { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
    }
}
