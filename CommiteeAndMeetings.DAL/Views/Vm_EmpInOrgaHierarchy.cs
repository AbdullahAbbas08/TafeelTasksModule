using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.DAL.Views
{
    public class Vm_EmpInOrgaHierarchy
    {
        
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameFn { get; set; }
    }

    public class Vm_OrgInOrgaHierarchy
    {

        public int Id { get; set; }
        
        public string OrganizationAr { get; set; }
        public string OrganizationEn { get; set; }
        public string OrganizationNameFn { get; set; }

    }
}
