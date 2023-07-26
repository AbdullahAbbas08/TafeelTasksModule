using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(AttachmentId), Name = "IX_ChatMessages_AttachmentId")]
    [Index(nameof(ChatRoomId), Name = "IX_ChatMessages_ChatRoomId")]
    [Index(nameof(CreatedBy), Name = "IX_ChatMessages_CreatedBy")]
    [Index(nameof(DeletedBy), Name = "IX_ChatMessages_DeletedBy")]
    public partial class ChatMessage
    {
        public ChatMessage()
        {
            ChatMessageSeens = new HashSet<ChatMessageSeen>();
        }

        [Key]
        public int ChatMessageId { get; set; }
        public string Content { get; set; }
        public int? ChatRoomId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public int? AttachmentId { get; set; }
        public bool AttachmentInculdded { get; set; }

        [ForeignKey(nameof(AttachmentId))]
        [InverseProperty("ChatMessages")]
        public virtual Attachment Attachment { get; set; }
        [ForeignKey(nameof(ChatRoomId))]
        [InverseProperty("ChatMessages")]
        public virtual ChatRoom ChatRoom { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.ChatMessageCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        [InverseProperty(nameof(User.ChatMessageDeletedByNavigations))]
        public virtual User DeletedByNavigation { get; set; }
        [InverseProperty(nameof(ChatMessageSeen.ChatMessage))]
        public virtual ICollection<ChatMessageSeen> ChatMessageSeens { get; set; }
    }
}
