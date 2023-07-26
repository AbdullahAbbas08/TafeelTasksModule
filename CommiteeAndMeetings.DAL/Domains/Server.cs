using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("Server", Schema = "HangFire")]
    [Index(nameof(LastHeartbeat), Name = "IX_HangFire_Server_LastHeartbeat")]
    public partial class Server
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; }
        public string Data { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastHeartbeat { get; set; }
    }
}
