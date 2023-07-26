using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("UserVacation")]
    [Index(nameof(StandByUserId), Name = "IX_UserVacation_StandByUserId")]
    [Index(nameof(UserId), Name = "IX_UserVacation_UserId")]
    public partial class UserVacation
    {
        [Key]
        public int UserVacationId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StandByUserId { get; set; }
        public string Description { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }

        [ForeignKey(nameof(StandByUserId))]
        [InverseProperty("UserVacationStandByUsers")]
        public virtual User StandByUser { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserVacationUsers")]
        public virtual User User { get; set; }
    }
}
