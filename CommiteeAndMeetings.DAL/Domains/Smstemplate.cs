using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("SMSTemplate")]
    public partial class Smstemplate
    {
        [Key]
        [Column("SMSTemplateId")]
        public int SmstemplateId { get; set; }
        [Column("SMScode")]
        public string Smscode { get; set; }
        public string TemplateCode { get; set; }
        public string Parameters { get; set; }
        public string TextMessage { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
    }
}
