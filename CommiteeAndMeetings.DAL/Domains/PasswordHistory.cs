using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    public partial class PasswordHistory
    {
        [Key]
        public int Id { get; set; }
        public string Password { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool IsFromAdmin { get; set; }
        public string UserName { get; set; }
    }
}
