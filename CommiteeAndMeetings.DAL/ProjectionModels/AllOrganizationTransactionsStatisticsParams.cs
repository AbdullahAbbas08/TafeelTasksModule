using System;

namespace Models.ProjectionModels
{
    public class AllOrganizationTransactionsStatisticsParams
    {
        public int[] OrgIdsList { get; set; }
        public DateTimeOffset To { get; set; }
        public int SKIP { get; set; } = 0;
        public int SIZE { get; set; } = 50;
    }
    public class OrganizationChildTransactionsStatisticsParams
    {
        public int OrgId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public int SKIP { get; set; } = 0;
        public int SIZE { get; set; } = 50;

    }
    public class OrganizationChildTransactionsParams
    {
        public int OrgId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public int SKIP { get; set; } = 0;
        public int SIZE { get; set; } = 50;
        public int Filter { get; set; } = 1;
    }
}
