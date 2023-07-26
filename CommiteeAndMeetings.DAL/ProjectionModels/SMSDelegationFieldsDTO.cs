using System;

namespace Models.ProjectionModels
{
    public class SMSDelegationFieldsDTO
    {
        public string TransactionNumberFormatted { get; set; }
        public string From { get; set; }
        public DateTimeOffset Date { get; set; }
    }

}
