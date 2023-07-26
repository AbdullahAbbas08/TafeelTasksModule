using System;
using System.Collections.Generic;
using System.Text;

namespace Models.AuditDTOs
{
    public class AuditUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
    }
}
