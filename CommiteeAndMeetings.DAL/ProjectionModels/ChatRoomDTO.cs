using Models.Enums;
using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class ChatRoomDTO
    {
        public ChatRoomDTO()
        {
            Messages = new List<ChatMessageDTO>();
        }
        public int ChatRoomId { get; set; }
        public string ChatRoomName { get; set; }
        public RoomType RoomType { get; set; }
        public long? TransactionId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        public int? TicketStatus { get; set; }
        public string TicketReferenceNumber { get; set; }
        public string TicketTitle { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public List<ChatMessageDTO> Messages { get; set; }
        public int? TicketClassificationId { get; set; }
        public string TicketClassificationName { get; set; }
        public string UserOrganization { get; set; }
        public string UserFullName { get; set; }
    }
}
