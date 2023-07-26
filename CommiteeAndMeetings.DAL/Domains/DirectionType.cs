using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class DirectionType
    {
        [Key]
        public int DirectionTypeId { get; set; }
        [StringLength(400)]
        public string DirectionTypeCode { get; set; }
        [StringLength(400)]
        public string DirectionTypeNameAr { get; set; }
        [StringLength(400)]
        public string DirectionTypeNameEn { get; set; }
        public string DirectionTypeNameFn { get; set; }
    }
}
