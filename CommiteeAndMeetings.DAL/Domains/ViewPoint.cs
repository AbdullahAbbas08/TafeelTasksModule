using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class ViewPoint
    {
        [Key]
        public int ViewPointId { get; set; }
        [StringLength(50)]
        public string ViewPointCode { get; set; }
        [StringLength(50)]
        public string ViewPointNameAr { get; set; }
        [StringLength(50)]
        public string ViewPointNameEn { get; set; }
        public string ViewPointNameFn { get; set; }
    }
}
