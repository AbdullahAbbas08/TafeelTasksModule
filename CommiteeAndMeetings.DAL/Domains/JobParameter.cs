using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("JobParameter", Schema = "HangFire")]
    public partial class JobParameter
    {
        [Key]
        public long JobId { get; set; }
        [StringLength(40)]
        public string Name { get; set; }
        public string Value { get; set; }

        [ForeignKey(nameof(JobId))]
        [InverseProperty("JobParameters")]
        public virtual Job Job { get; set; }
    }
}
