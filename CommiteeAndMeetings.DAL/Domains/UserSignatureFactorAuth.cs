using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(UserId), Name = "IX_UserSignatureFactorAuths_UserId")]
    public partial class UserSignatureFactorAuth
    {
        [Key]
        public int Id { get; set; }
        public string FactorAuthCode { get; set; }
        public DateTime FactorAuthDate { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserSignatureFactorAuths")]
        public virtual User User { get; set; }
    }
}
