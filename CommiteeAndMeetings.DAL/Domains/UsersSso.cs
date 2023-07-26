using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("UsersSSO")]
    public partial class UsersSso
    {
        [Key]
        public int Id { get; set; }
        public string UserNameEncrypted { get; set; }
        public string PasswordEncrypted { get; set; }
        [Column("SSOKey")]
        public string Ssokey { get; set; }
        public DateTimeOffset? RequestedOn { get; set; }
    }
}
