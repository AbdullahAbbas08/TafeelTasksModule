using DbContexts.MasarContext.ProjectionModels;
using LinqHelper;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Services.ISevices
{
    public interface IBusinessService<TDbEntity, TDetailsDTO> : IBusinessService
      where TDbEntity : class
    {
        DataSourceResult<T> GetAll<T>(DataSourceRequest dataSourceRequest, bool WithTracking = true);
        TDetailsDTO GetDetails(object Id, bool WithTracking = true);
        IEnumerable<TDetailsDTO> Insert(IEnumerable<TDetailsDTO> entities);
        IEnumerable<object> Delete(IEnumerable<object> Ids);
        IEnumerable<TDetailsDTO> Update(IEnumerable<TDetailsDTO> Entities);
        bool CheckIfExist(CheckUniqueDTO checkUniqueDTO);
        // bool CheckIfExist(object id);
    }

    public interface IBusinessService
    {
    }
}
