using System;

namespace Models.ProjectionModels
{
    public class SavedTransactionDTO
    {
        public int @SKIP { get; set; } = 0;
        public int @SIZE { get; set; } = 50;
        public DateTimeOffset @FROM_DATE { get; set; }
        public DateTimeOffset @TO_DATE { get; set; }
        public string @ORGS_Ids { get; set; }
        public string @EMPS_Ids { get; set; }

    }
}
