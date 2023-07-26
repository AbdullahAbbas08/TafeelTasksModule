using System.Collections.Generic;

namespace DbContexts.MasarContext.ProjectionModels
{
    public class RolePermissionsDTO
    {
        public int RoleId { get; set; }
        public IEnumerable<PermissionCategoryDTO> PermissionCategories { get; set; }
    }

    public class RolePermissionDTO
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public string PermissionNameAr { get; set; }
        public string PermissionNameEn { get; set; }
        public bool? PermissionStatus { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public class PermissionCategoryDTO
    {
        public string PermissionCategoryName { get; set; }
        public IEnumerable<RolePermissionDTO> Permissions { get; set; }
    }
}
