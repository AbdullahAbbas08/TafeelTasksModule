using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.AuditDTOs
{
    public class AuditTrail
    {

        public int AuditTrailId { get; set; }
        public string EntityCode { get; set; }
        public string ActionCode { get; set; }
        public string UserJsonString { get; set; }
        public string UserRoleJsonString { get; set; }
        public string OrganizationJsonString { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PreviousState { get; set; }
        public string CurrentState { get; set; }
        public string IP { get; set; }
        public string ApplicationType { get; set; }
        public string CreatedBy { get; set; }
    }
}
