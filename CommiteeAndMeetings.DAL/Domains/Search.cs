using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Table("Search")]
    [Index(nameof(CreatedBy), Name = "IX_Search_CreatedBy")]
    [Index(nameof(CreatedByUserRoleId), Name = "IX_Search_CreatedByUserRoleId")]
    [Index(nameof(UpdatedBy), Name = "IX_Search_UpdatedBy")]
    public partial class Search
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Data { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public int? CreatedByUserRoleId { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.SearchCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(CreatedByUserRoleId))]
        [InverseProperty(nameof(UserRole.Searches))]
        public virtual UserRole CreatedByUserRole { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty(nameof(User.SearchUpdatedByNavigations))]
        public virtual User UpdatedByNavigation { get; set; }
    }
}
