using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("State", Schema = "HangFire")]
    public partial class State
    {
        [Key]
        public long Id { get; set; }
        public long JobId { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Reason { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        public string Data { get; set; }

        [ForeignKey(nameof(JobId))]
        [InverseProperty("States")]
        public virtual Job Job { get; set; }
    }
}
