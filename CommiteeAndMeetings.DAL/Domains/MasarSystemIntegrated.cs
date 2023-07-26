using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("MasarSystemIntegrated")]
    public partial class MasarSystemIntegrated
    {
        public MasarSystemIntegrated()
        {
            MasarSystemIntegratedUsers = new HashSet<MasarSystemIntegratedUser>();
        }

        [Key]
        public int ModuleId { get; set; }
        [StringLength(100)]
        public string ModuleName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsForAll { get; set; }

        [InverseProperty(nameof(MasarSystemIntegratedUser.Module))]
        public virtual ICollection<MasarSystemIntegratedUser> MasarSystemIntegratedUsers { get; set; }
    }
}
