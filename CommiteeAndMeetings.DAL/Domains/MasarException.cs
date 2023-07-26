using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class MasarException
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public int UserRoleId { get; set; }
        public DateTime Time { get; set; }

        [ForeignKey(nameof(UserRoleId))]
        [InverseProperty("MasarExceptions")]
        public virtual UserRole UserRole { get; set; }
    }
}
