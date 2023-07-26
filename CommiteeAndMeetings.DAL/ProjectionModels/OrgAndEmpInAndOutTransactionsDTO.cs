using System;

namespace Models.ProjectionModels
{
    public class OrgAndEmpInAndOutTransactionsDTO
    {
        public int @SKIP { get; set; } = 0;
        public int @SIZE { get; set; } = 50;
        public DateTimeOffset @FROM_DATE { get; set; }
        public DateTimeOffset @TO_DATE { get; set; }
        public string @FROM_ORGS { get; set; }
        public string @TO_EMPS { get; set; }
        public bool IS_INCOMING { get; set; }
        public string @FILTER_BY_ORGS { get; set; }
        public string @FILTER_BY_EMPS { get; set; }

    }
}
