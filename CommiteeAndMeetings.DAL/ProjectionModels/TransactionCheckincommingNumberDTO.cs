using System;

namespace DbContexts.MasarContext.ProjectionModels
{
    public class TransactionCheckincommingNumberDTO
    {
        public int Id { get; set; }
        public string TransactionNumberFormatted { get; set; }
        public string IncomingLetterNumber { get; set; }
        public int IncomingOrganizationId { get; set; }
        public DateTimeOffset IncomingLetterDate { get; set; }
    }
}
