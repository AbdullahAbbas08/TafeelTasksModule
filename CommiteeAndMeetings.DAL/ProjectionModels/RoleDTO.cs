using System.Collections.Generic;

namespace DbContexts.MasarContext.ProjectionModels
{
    public class RoleDTO
    {
        public RoleDTO()
        {
            RolePermissions = new List<RolePermissionDTO>();
        }
        public int RoleId { get; set; }
        public string RoleNameAr { get; set; }
        public string RoleNameFn { get; set; }

        public string RoleNameEn { get; set; }
        public bool IsEmployeeRole { get; set; }
        public bool IsDelegatedEmployeeRole { get; set; }
        public List<RolePermissionDTO> RolePermissions { get; set; }

    }
}