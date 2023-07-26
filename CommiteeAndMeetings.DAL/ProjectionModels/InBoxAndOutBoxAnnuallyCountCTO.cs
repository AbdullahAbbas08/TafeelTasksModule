using System;

namespace Models.ProjectionModels
{
    public class InBoxAndOutBoxAnnuallyCountCTO
    {
        public int Id { get; set; }
        public int inbox { get; set; }
        public int outbox { get; set; }
        public DateTime dateTime { get; set; }
    }
}
