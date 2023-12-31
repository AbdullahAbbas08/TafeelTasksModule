﻿namespace DbContexts.MasarContext.ProjectionModels
{
    public class PermissionDTO
    {
        public int PermissionId { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionNameAr { get; set; }
        public string PermissionNameEn { get; set; }
        public string PermissionNameFn { get; set; }

        public string PermissionName { get; set; }
        public string URL { get; set; }
        public string Method { get; set; }
        public bool Enabled { get; set; }
        public int PermissionCategoryId { get; set; }
        public bool ForDelegate { get; set; }
        public bool IsCommittePermission { get; set; } = false;
    }
}
