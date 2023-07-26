using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class SearchTemplate
    {
        [Key]
        public int SearchTemplateId { get; set; }
        [StringLength(50)]
        public string SearchTemplateCode { get; set; }
        [StringLength(50)]
        public string SearchTemplateNameAr { get; set; }
        [StringLength(50)]
        public string SearchTemplateNameEn { get; set; }
        public string SearchTemplateNameFn { get; set; }
    }
}
