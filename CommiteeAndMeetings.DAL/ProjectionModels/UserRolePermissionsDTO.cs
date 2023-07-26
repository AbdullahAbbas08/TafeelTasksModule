using System;
using System.Collections.Generic;

namespace DbContexts.MasarContext.ProjectionModels
{
    public class UserRolePermissionsDTO
    {
        public int UserId { get; set; }
        public IEnumerable<UserRoles> UserRoles { get; set; }
    }

    public class UserRoles
    {
        public int userId { get; set; }
        public int UserRoleId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsEmployeeRole { get; set; }
        public bool IsDelegatedEmployeeRole { get; set; }
        public int OrganizationId { get; set; }
        public int? Order { get; set; }

        public string OrganizationName { get; set; }
        public bool? RoleOverridesUserPermissions { get; set; }
        public bool collapse { get; set; }
        public IEnumerable<PermissionCategories> PermissionCategories { get; set; }
        //custom
        public bool IsActive { get; set; }
        public bool IsOrganizationDeleted { get; set; }
        public DateTimeOffset? EnabledSince { get; set; }
        public DateTimeOffset? EnabledUntil { get; set; }
        public string Notes { get; set; }
        public string CreatedByUser { get; set; }
    }

    public class PermissionCategories
    {
        public string PermissionCategoryName { get; set; }
        public IEnumerable<UserRolePermission> UserRolePermissions { get; set; }
    }

    public class UserRolePermission
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool? RolePermissionEnabled { get; set; }
        public bool? UserPermissionEnabled { get; set; }
        public cases cases { get; set; }
    }

    public class cases
    {
        public int Default_case { get; set; }
        public int? new_case { get; set; }
    }
}
