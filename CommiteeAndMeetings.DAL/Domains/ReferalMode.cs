using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class ReferalMode
    {
        [Key]
        public int ReferalModeId { get; set; }
        public string ReferalModeNameAr { get; set; }
        public string ReferalModeNameEn { get; set; }
        public string ReferalModeName { get; set; }
        public bool? IsDefaultForTransaction { get; set; }
        public bool? IsDefaultForCircular { get; set; }
        public string ReferalModeNameFn { get; set; }
    }
}
