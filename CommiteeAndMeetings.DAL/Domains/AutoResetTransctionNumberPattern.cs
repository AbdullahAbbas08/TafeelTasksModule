using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class AutoResetTransctionNumberPattern
    {
        [Key]
        public int AutoResetTransctionNumberPatternId { get; set; }
        public string AutoResetTransctionNumberPatternNameAr { get; set; }
        public string AutoResetTransctionNumberPatternNameEn { get; set; }
        public string AutoResetTransctionNumberPatternNameFn { get; set; }
    }
}
