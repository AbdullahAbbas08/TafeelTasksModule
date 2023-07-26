using CommiteeAndMeetings.DAL.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces
{
    public interface ILocalizationRepository : IRepository<Localization>
    {
        string GetByKey(string v, string culture);
    }
}
