using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommiteeAndMeetings.DAL.Domains
{
    [Index(nameof(CreatedBy), Name = "IX_UserSpecificOrganizationsAndEmployees_CreatedBy")]
    [Index(nameof(OrganizationId), Name = "IX_UserSpecificOrganizationsAndEmployees_OrganizationId")]
    [Index(nameof(SpecificOrganizationId), Name = "IX_UserSpecificOrganizationsAndEmployees_SpecificOrganizationId")]
    [Index(nameof(SpecificUserId), Name = "IX_UserSpecificOrganizationsAndEmployees_SpecificUserId")]
    [Index(nameof(UpdatedBy), Name = "IX_UserSpecificOrganizationsAndEmployees_UpdatedBy")]
    [Index(nameof(UserId), Name = "IX_UserSpecificOrganizationsAndEmployees_UserId")]
    public partial class UserSpecificOrganizationsAndEmployee
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? SpecificOrganizationId { get; set; }
        public int? SpecificUserId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public bool IsFromSync { get; set; }
        public int? GroupId { get; set; }
        public int? OrganizationId { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty("UserSpecificOrganizationsAndEmployeeCreatedByNavigations")]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty("UserSpecificOrganizationsAndEmployeeOrganizations")]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(SpecificOrganizationId))]
        [InverseProperty("UserSpecificOrganizationsAndEmployeeSpecificOrganizations")]
        public virtual Organization SpecificOrganization { get; set; }
        [ForeignKey(nameof(SpecificUserId))]
        [InverseProperty("UserSpecificOrganizationsAndEmployeeSpecificUsers")]
        public virtual User SpecificUser { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        [InverseProperty("UserSpecificOrganizationsAndEmployeeUpdatedByNavigations")]
        public virtual User UpdatedByNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserSpecificOrganizationsAndEmployeeUsers")]
        public virtual User User { get; set; }
    }
}
