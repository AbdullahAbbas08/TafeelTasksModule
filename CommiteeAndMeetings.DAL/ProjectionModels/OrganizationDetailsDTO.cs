namespace DbContexts.MasarContext.ProjectionModels
{
    public class OrganizationDetailsDTO
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string OrganizationNameFn { get; set; }

        public int? ParentOrganizationId { get; set; }
        public string ParentOrganizationName { get; set; }
        public int? RootOrganizationId { get; set; }
        public int? AdminOrganizationId { get; set; }
        public string AdminOrganizationName { get; set; }

        public bool IsAdminOrganization { get; set; }
        public bool IsCategory { get; set; }
        public bool IsOuterOrganization { get; set; }
        public bool IsMainOrganization { get; set; }
        public bool DelegateOnlyToSiblingsAndChildren { get; set; }
        public bool DelegateToAllChildrenExceptChildrenOfMain { get; set; }
        public bool DelegateToItSelf { get; set; }

        public bool SMSAllowed { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public int? DisplayOrder { get; set; }
        public string Code { get; set; }
        public int? ManagerUserId { get; set; }
        public byte[] StampFile { get; set; }
        public string Color { get; set; }
        public string FullPathAr { get; set; }
        public string FullPathEn { get; set; }
        public string FullPathFn { get; set; }

        public string FullPath { get; set; }
        public string ArchFolderEntryId { get; set; }
        public bool IsActive { get; set; }
        public int? FollowUpOrganizationId { get; set; }
        public int? OrganizationWeight { get; set; }
        //custom
        public bool AllowAdminOrganization { get; set; }
        public string FollowUpOrganizationName { get; set; }
        public bool? IsBlackBox { get; set; } = false;
        public bool? IsGeneral { get; set; } = false;
        public bool? IsRelatedNeed { get; set; } = false;
        public int? DepCode { get; set; }
        public bool AdminCanEnableDisableUsers { get; set; }
        public int? GroupId { get; set; }
        public Lookup manager { get; set; }

        #region ShallowCopy

        public OrganizationDetailsDTO ShallowCopy()
        {
            return (OrganizationDetailsDTO)this.MemberwiseClone();
        }

        #endregion

    }
}
