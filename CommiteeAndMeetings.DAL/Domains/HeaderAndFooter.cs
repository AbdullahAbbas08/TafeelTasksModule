using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("HeaderAndFooter")]
    [Index(nameof(HeaderAndFooterId), nameof(ReportName), Name = "IX_Id_ReportName")]
    public partial class HeaderAndFooter
    {
        [Key]
        public int HeaderAndFooterId { get; set; }
        public int TypeId { get; set; }
        public string ReportName { get; set; }
        public string ReportNameAr { get; set; }
        [Column("HTML", TypeName = "ntext")]
        public string Html { get; set; }
        public string ReportNameEn { get; set; }
        public string ReportNameFn { get; set; }
    }
}
