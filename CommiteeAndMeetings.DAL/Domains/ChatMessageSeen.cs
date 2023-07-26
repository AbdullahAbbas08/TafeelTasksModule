using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(ChatMessageId), Name = "IX_ChatMessageSeens_ChatMessageId")]
    [Index(nameof(CreatedBy), Name = "IX_ChatMessageSeens_CreatedBy")]
    public partial class ChatMessageSeen
    {
        [Key]
        public int ChatMessageSeenId { get; set; }
        public int ChatMessageId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        [ForeignKey(nameof(ChatMessageId))]
        [InverseProperty("ChatMessageSeens")]
        public virtual ChatMessage ChatMessage { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.ChatMessageSeens))]
        public virtual User CreatedByNavigation { get; set; }
    }
}
