using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("UserToken")]
    [Index(nameof(UserId), Name = "IX_UserToken_UserId")]
    public partial class UserToken
    {
        [Key]
        public int Id { get; set; }
        public string AccessTokenHash { get; set; }
        public DateTimeOffset AccessTokenExpiresDateTime { get; set; }
        [Required]
        [StringLength(450)]
        public string RefreshTokenIdHash { get; set; }
        [StringLength(450)]
        public string RefreshTokenIdHashSource { get; set; }
        public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }
        public int UserId { get; set; }
        public int? ApplicationType { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserTokens")]
        public virtual User User { get; set; }

    }
}
