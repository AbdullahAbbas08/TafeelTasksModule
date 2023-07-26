using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("Job", Schema = "HangFire")]
    public partial class Job
    {
        public Job()
        {
            JobParameters = new HashSet<JobParameter>();
            States = new HashSet<State>();
        }

        [Key]
        public long Id { get; set; }
        public long? StateId { get; set; }
        [StringLength(20)]
        public string StateName { get; set; }
        [Required]
        public string InvocationData { get; set; }
        [Required]
        public string Arguments { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ExpireAt { get; set; }

        [InverseProperty(nameof(JobParameter.Job))]
        public virtual ICollection<JobParameter> JobParameters { get; set; }
        [InverseProperty(nameof(State.Job))]
        public virtual ICollection<State> States { get; set; }
    }
}
