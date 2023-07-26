namespace DbContexts.MasarContext.ProjectionModels
{
    public class PermissionCategorySummaryDTO
    {
        public int PermissionCategoryId { get; set; }
        public string PermissionCategoryNameAr { get; set; }
        public string PermissionCategoryNameEn { get; set; }
        public string PermissionCategoryNameFn { get; set; }

        public string PermissionCategoryName { get; set; }
        public bool? IsEmployeeCategory { get; set; }
        public PermissionCategorySummaryDTO EmployeePermissionCategory { get; set; }
    }
}
