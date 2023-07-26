using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Keyless]
    [Table("Counter", Schema = "HangFire")]
    public partial class Counter
    {
        [Required]
        [StringLength(100)]
        public string Key { get; set; }
        public int Value { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ExpireAt { get; set; }
    }
}
