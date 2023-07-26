using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_ChatRooms_CreatedBy")]
    [Index(nameof(TicketClassificationId), Name = "IX_ChatRooms_TicketClassificationId")]
    [Index(nameof(TransactionId), Name = "IX_ChatRooms_TransactionId")]
    public partial class ChatRoom
    {
        public ChatRoom()
        {
            ChatMessages = new HashSet<ChatMessage>();
            ChatRoomUsers = new HashSet<ChatRoomUser>();
        }

        [Key]
        public int ChatRoomId { get; set; }
        public string ChatRoomName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public bool Active { get; set; }
        public long? TransactionId { get; set; }
        public int RoomType { get; set; }
        public int? TicketStatus { get; set; }
        public string TicketReferenceNumber { get; set; }
        public int? TicketClassificationId { get; set; }
        public string TicketTitle { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.ChatRooms))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(TicketClassificationId))]
        [InverseProperty("ChatRooms")]
        public virtual TicketClassification TicketClassification { get; set; }
        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("ChatRooms")]
        public virtual Transaction Transaction { get; set; }
        [InverseProperty(nameof(ChatMessage.ChatRoom))]
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
        [InverseProperty(nameof(ChatRoomUser.ChatRoom))]
        public virtual ICollection<ChatRoomUser> ChatRoomUsers { get; set; }
    }
}
