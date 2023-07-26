using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class Help
    {
        [Key]
        public int HelpId { get; set; }
        [StringLength(400)]
        public string HelpCode { get; set; }
        [StringLength(400)]
        public string HelpNameAr { get; set; }
        [StringLength(400)]
        public string HelpNameEn { get; set; }
        public string HelpContentAr { get; set; }
        public string HelpContentEn { get; set; }
        public string HelpContentFn { get; set; }
        public string HelpNameFn { get; set; }
    }
}
