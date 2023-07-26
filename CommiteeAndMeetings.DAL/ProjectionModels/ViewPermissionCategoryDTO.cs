namespace Models.ProjectionModels
{
    public class ViewPermissionCategoryDTO
    {
        public int Id { get; set; }
        public int PermissionId { get; set; }

        public string PermissionCode { get; set; }

        public string PermissionNameAr { get; set; }

        public string PermissionNameEn { get; set; }

        public string URL { get; set; }
        public string Method { get; set; }
        public bool Enabled { get; set; }

        public int PermissionCategoryId { get; set; }

        public string PermissionCategoryNameAr { get; set; }

        public string PermissionCategoryNameEn { get; set; }
    }
}
