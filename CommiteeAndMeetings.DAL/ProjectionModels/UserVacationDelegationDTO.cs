using DbContexts.MasarContext.ProjectionModels;

namespace Models.ProjectionModels
{
    public class UserVacationDelegationDTO
    {
        public bool Available { get; set; }
        public Lookup User { get; set; }
        public Lookup StandByUser { get; set; }
        public bool NoUser { get; set; } = false;
    }
}
