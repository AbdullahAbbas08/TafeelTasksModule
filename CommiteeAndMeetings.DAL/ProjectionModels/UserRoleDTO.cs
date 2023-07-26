using System;

namespace DbContexts.MasarContext.ProjectionModels
{
    public class UserRoleDTO
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string OrganizationNameFn { get; set; }
        public int RoleId { get; set; }
        public string RoleNameAr { get; set; }
        public string RoleNameEn { get; set; }
        public string RoleNameFn { get; set; }
        public DateTimeOffset? EnabledSince { get; set; }
        public DateTimeOffset? EnabledUntil { get; set; }
        public string Notes { get; set; }
        public bool RoleOverridesUserPermissions { get; set; }
        public int? Order { get; set; }
        public bool IsDefaultOrganization { get; set; }
    }
    public class EncriptedUserRoleDTO
    {
        public string UserRoleId { get; set; }
        public string UserId { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationNameAr { get; set; }
        public string OrganizationNameEn { get; set; }
        public string OrganizationNameFn { get; set; }
        public string RoleId { get; set; }
        public string RoleNameAr { get; set; }
        public string RoleNameEn { get; set; }
        public string RoleNameFn { get; set; }
        public string EnabledSince { get; set; }
        public string EnabledUntil { get; set; }
        public string Notes { get; set; }
        public string RoleOverridesUserPermissions { get; set; }
        public string Order { get; set; }
        public string IsDefaultOrganization { get; set; }
        public string CountOfUnDelivered { get; set; }
    }
}
