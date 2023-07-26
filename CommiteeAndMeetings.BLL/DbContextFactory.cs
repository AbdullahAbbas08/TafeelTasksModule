using CommiteeAndMeetings.BLL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CommiteeAndMeetings.BLL
{
    public class DbContextFactory
    {
        public static string connectionString;
        public MasarContext Create()
        {
            var options = new DbContextOptionsBuilder<MasarContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new MasarContext(options);
        }
    }
}
