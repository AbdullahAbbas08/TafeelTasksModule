using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("FavoriteList")]
    [Index(nameof(FavoriteOrganizationId), Name = "IX_FavoriteList_FavoriteOrganizationId")]
    [Index(nameof(FavoriteUserId), Name = "IX_FavoriteList_FavoriteUserId")]
    [Index(nameof(UserId), Name = "IX_FavoriteList_UserId")]
    public partial class FavoriteList
    {
        [Key]
        public int FavoriteId { get; set; }
        public int UserId { get; set; }
        public int? FavoriteOrganizationId { get; set; }
        public int? FavoriteUserId { get; set; }
        public int? FavoriteOrderId { get; set; }

        [ForeignKey(nameof(FavoriteOrganizationId))]
        [InverseProperty(nameof(Organization.FavoriteLists))]
        public virtual Organization FavoriteOrganization { get; set; }
        [ForeignKey(nameof(FavoriteUserId))]
        [InverseProperty("FavoriteListFavoriteUsers")]
        public virtual User FavoriteUser { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("FavoriteListUsers")]
        public virtual User User { get; set; }
    }
}
