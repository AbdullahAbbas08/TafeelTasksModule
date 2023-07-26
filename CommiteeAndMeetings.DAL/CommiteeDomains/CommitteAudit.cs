using CommiteeDatabase.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.DAL.CommiteeDomains
{
    public class CommitteAudit: _BaseEntity
    {
        public CommitteAudit()
        {
        }

        public int Id { get; set; }
        public string ActionName { get; set; }
        public string TableName { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string RoleName { get; set; }
        public string IP { get; set; }
        public string ApplicationType { get; set; }
        public string OrganizationName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ForignKeys { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public long? TransactionId { get; set; }
    }
}
