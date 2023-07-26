using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("UserJob")]
    [Index(nameof(UserId), Name = "IX_UserJob_UserId")]
    public partial class UserJob
    {
        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string JobId { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserJobs")]
        public virtual User User { get; set; }
    }
}
