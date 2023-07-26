using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("Hash", Schema = "HangFire")]
    public partial class Hash
    {
        [Key]
        [StringLength(100)]
        public string Key { get; set; }

        [StringLength(100)]
        public string Field { get; set; }
        public string Value { get; set; }
        public DateTime? ExpireAt { get; set; }
    }
}
