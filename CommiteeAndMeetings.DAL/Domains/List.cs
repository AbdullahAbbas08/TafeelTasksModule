using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("List", Schema = "HangFire")]
    public partial class List
    {
        [Key]
        public long Id { get; set; }
        [StringLength(100)]
        public string Key { get; set; }
        public string Value { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ExpireAt { get; set; }
    }
}
