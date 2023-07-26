using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(ModuleId), Name = "IX_MasarSystemIntegratedUsers_ModuleId")]
    [Index(nameof(UserId), Name = "IX_MasarSystemIntegratedUsers_UserId")]
    public partial class MasarSystemIntegratedUser
    {
        [Key]
        public int SystemIntegratedUserId { get; set; }
        public int ModuleId { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(ModuleId))]
        [InverseProperty(nameof(MasarSystemIntegrated.MasarSystemIntegratedUsers))]
        public virtual MasarSystemIntegrated Module { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("MasarSystemIntegratedUsers")]
        public virtual User User { get; set; }
    }
}
