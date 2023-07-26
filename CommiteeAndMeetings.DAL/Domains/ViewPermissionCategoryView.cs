using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.DAL.Domains
{
    public class ViewPermissionCategoryView
    {
        public int Id { get; set; }
        public int PermissionId { get; set; }

        public string PermissionCode { get; set; }

        public string PermissionNameAr { get; set; }

        public string PermissionNameEn { get; set; }
        public string PermissionNameFn { get; set; }

        public string URL { get; set; }
        public string Method { get; set; }
        public bool Enabled { get; set; }

        public int PermissionCategoryId { get; set; }
        public bool IsCommittePermission { get; set; }


        public string PermissionCategoryNameAr { get; set; }

        public string PermissionCategoryNameEn { get; set; }
        public string PermissionCategoryNameFn { get; set; }

        public bool IsEmployeeCategory { get; set; }
        public bool ForDelegate { get; set; }
    }
}
