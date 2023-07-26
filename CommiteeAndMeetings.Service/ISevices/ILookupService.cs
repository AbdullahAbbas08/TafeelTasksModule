using CommiteeAndMeetings.Services.ISevices;
using DbContexts.MasarContext.ProjectionModels;
using LinqHelper;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ILookupService : IBusinessService<Lookup, Lookup>
    {
        IEnumerable<Lookup> Get(string Type, string SearchText, DataSourceRequest dataSourceRequest, Dictionary<string, object> args, object[] ids = null);

    }
}
