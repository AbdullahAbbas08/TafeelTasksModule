using CommiteeDatabase.Models.Domains;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    [Table("Themes", Schema = "Reports")]
    public class CommitteeTheme : _BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstColorHex { get; set; }
        public string SecondColorHex { get; set; }
        public string ThirdColorHex { get; set; }
    }
}
