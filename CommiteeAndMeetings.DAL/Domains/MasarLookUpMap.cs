using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class MasarLookUpMap
    {
        [Key]
        public int Id { get; set; }
        public string TableName { get; set; }
        public string Value { get; set; }
        public string MapValue { get; set; }
    }
}
