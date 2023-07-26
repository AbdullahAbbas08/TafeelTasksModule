using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("JobQueue", Schema = "HangFire")]
    public partial class JobQueue
    {
        [Key]
        public int Id { get; set; }
        public long JobId { get; set; }
        [StringLength(50)]
        public string Queue { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FetchedAt { get; set; }
    }
}
