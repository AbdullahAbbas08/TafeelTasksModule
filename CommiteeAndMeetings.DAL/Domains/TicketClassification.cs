using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("TicketClassification")]
    public partial class TicketClassification
    {
        public TicketClassification()
        {
            ChatRooms = new HashSet<ChatRoom>();
        }

        [Key]
        public int Id { get; set; }
        public string TicketClassificationAr { get; set; }
        public string TicketClassificationEn { get; set; }
        public string TicketClassificationFn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        [InverseProperty(nameof(ChatRoom.TicketClassification))]
        public virtual ICollection<ChatRoom> ChatRooms { get; set; }
    }
}
