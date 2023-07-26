using System;

namespace Models.ProjectionModels
{
    public class InboxReceivedAndUnreceivedAnnuallyCountDTO
    {
        public int Id { get; set; }
        public int Received { get; set; }
        public int Unreceived { get; set; }
        public DateTime date { get; set; }
    }
}
