using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.AuditDTOs
{
    public class AuditFilter
    {
        public int Skip { get; set; }
        public int Count { get; set; }
        public string EntityCode { get; set; }
        public string ActionCode { get; set; }
        public DateTime CreatedOnStartDate { get; set; }
        public DateTime CreatedOnEndDate { get; set; }
        public string CreatedBy { get; set; }
        public string TransactionNumber { get; set; }
    }
}
