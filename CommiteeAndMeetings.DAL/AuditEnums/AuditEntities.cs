using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.AuditEnums
{
    public enum AuditEntities
    {
        Users,
        UserRoles,
        Organization,
        Roles,
        Transaction,
        Document,
        Delegation
    }
    public enum NotificationType
    {
        Email=1,
        SMS=2
    }
}
