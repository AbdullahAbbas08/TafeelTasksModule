using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class MasarLookUpExternalOrganization
    {
        [Key]
        public int Id { get; set; }
        public string MainId { get; set; }
        public string SubId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string FrenchName { get; set; }
    }
}
