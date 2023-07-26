namespace DbContexts.MasarContext.ProjectionModels
{
    public class OrganizationSummaryDTO
    {
        public int OrganizationId { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string OrganizationNameFn { get; set; }

        public bool IsAdminOrganization { get; set; }
        public bool IsOuterOrganization { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public bool DelegateToAllChildrenExceptChildrenOfMain { get; set; }
        public bool DelegateToItSelf { get; set; }
        public bool? IsGeneral { get; set; } = false;
        public bool? IsRelatedNeed { get; set; } = false;
        public int? DepCode { get; set; }
        public bool AdminCanEnableDisableUsers { get; set; } = true;
    }
}
