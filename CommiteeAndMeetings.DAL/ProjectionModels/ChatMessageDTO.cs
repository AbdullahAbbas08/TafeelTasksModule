using System;

namespace Models.ProjectionModels
{
    public class ChatMessageDTO
    {
        public int MessageId { get; set; }
        public int? FromUserId { get; set; }
        public int? ChatRoomId { get; set; }
        public string Message { get; set; }
        public int? AttachmentId { get; set; }
        public byte[] ProfileImage { get; set; }
        public DateTimeOffset? MessageDate { get; set; }
        public AttachmentViewDTO Attachment { get; set; }
    }
}
