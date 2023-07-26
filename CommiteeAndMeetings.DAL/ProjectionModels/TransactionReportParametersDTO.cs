using System;
using System.Collections.Generic;

namespace Models.ProjectionModels
{
    public class TransactionsReportsParametersDTO
    {
        public int @SKIP { get; set; } = 0;
        public int @SIZE { get; set; } = 50;
        public DateTimeOffset @FROM_DATE { get; set; }
        public DateTimeOffset @TO_DATE { get; set; }
        public string @FROM_ORGS { get; set; }
        public string @TO_ORGS { get; set; }
        public string @FROM_EMPS { get; set; }
        public string @TO_EMPS { get; set; }
        public bool @DRAFTS { get; set; }
        public bool @OUTBOX { get; set; }
        public bool @RECIEVED { get; set; }
        public bool @UNRECIEVED { get; set; }
        public bool @LATE { get; set; }
        public bool @DECISION { get; set; }
        public bool @IsCC { get; set; }
        public bool @IsSeen { get; set; }
        public bool @LateFromDelegation { get; set; }
    }
    public class TransactionByTypeReportParametersDTO
    {
        public DateTimeOffset? FROM_DATE { get; set; }
        public DateTimeOffset? TO_DATE { get; set; }
    }
    public class WithdrawTransactioReporParametersDTO
    {
        public int @SKIP { get; set; } = 0;
        public int @SIZE { get; set; } = 50;
        public DateTimeOffset @FROM_DATE { get; set; }
        public DateTimeOffset @TO_DATE { get; set; }
        public int? @TO_ORGS { get; set; }

        public int? @TO_EMPS { get; set; }
        public int filterID { get; set; }
    }
    public class TransactionsReportsMultiParametersDTO
    {
        public int @SKIP { get; set; } = 0;
        public int @SIZE { get; set; } = 50;
        public DateTimeOffset @FROM_DATE { get; set; }
        public DateTimeOffset @TO_DATE { get; set; }
        public List<int> @FROM_ORGS { get; set; }
        public List<int> @TO_ORGS { get; set; }
        public List<int> @FROM_EMPS { get; set; }
        public List<int> @TO_EMPS { get; set; }
    }
}
