namespace Models.ProjectionModels
{
    public class UserPermissionsDTO
    {
        public long Id { get; set; }
        public int PermissionId { get; set; }
        public string PermissionNameAr { get; set; }
        public string PermissionNameEn { get; set; }
        public string PermissionNameFn { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameFn { get; set; }
        public string RoleNameAr { get; set; }
        public string RoleNameEn { get; set; }
        public string RoleNameFn { get; set; }
        public int IsUserPermission { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string OrganizationNameFn { get; set; }
        public int TotalCount { get; set; }
    }
}
