using HelperServices.LinqHelpers;
using System.Collections.Generic;

namespace LinqHelper
{
    public class DataSourceRequest
    {
        public int Take { get; set; }

        public int Skip { get; set; }

        public IEnumerable<Sort> Sort { get; set; }

        public Filter Filter { get; set; }

        public bool Countless { get; set; }

    }
    public class DataSourceRequestDelegation
    {
        public int Take { get; set; }

        public int Skip { get; set; }

        public IEnumerable<Sort> Sort { get; set; }

        public Filter Filter { get; set; }

        public bool Countless { get; set; }
        public int? RoleId { get; set; }

    }

    public class DataSourceRequestForDelegated
    {
        public int? OrganizationID { get; set; }
        public int? RoleID { get; set; }
        public int Take { get; set; }

        public int Skip { get; set; }

        public IEnumerable<Sort> Sort { get; set; }

        public Filter Filter { get; set; }

        public bool Countless { get; set; }

    }
    public class DataSourceRequestForUsersReport
    {
        public int? OrganizationID { get; set; }
        public int? RoleID { get; set; }
        public int Take { get; set; }

        public int Skip { get; set; }

        public IEnumerable<Sort> Sort { get; set; }

        public Filter Filter { get; set; }

        public bool Countless { get; set; }
        public bool? Active { get; set; }
    }
}