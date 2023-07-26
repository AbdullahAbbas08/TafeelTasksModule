using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(ChatRoomId), Name = "IX_ChatRoomUsers_ChatRoomId")]
    [Index(nameof(CreatedBy), Name = "IX_ChatRoomUsers_CreatedBy")]
    [Index(nameof(DeletedBy), Name = "IX_ChatRoomUsers_DeletedBy")]
    [Index(nameof(UserId), Name = "IX_ChatRoomUsers_UserId")]
    public partial class ChatRoomUser
    {
        [Key]
        public int ChatRoomUserId { get; set; }
        public int ChatRoomId { get; set; }
        public int UserId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public bool Active { get; set; }

        [ForeignKey(nameof(ChatRoomId))]
        [InverseProperty("ChatRoomUsers")]
        public virtual ChatRoom ChatRoom { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty("ChatRoomUserCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(DeletedBy))]
        [InverseProperty("ChatRoomUserDeletedByNavigations")]
        public virtual User DeletedByNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("ChatRoomUserUsers")]
        public virtual User User { get; set; }
    }
}
