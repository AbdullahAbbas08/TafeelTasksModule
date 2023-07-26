using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Services.ISevices;
using LinqHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ICommiteeTaskEscalationService : IBusinessService<CommiteeTaskEscalation, CommiteeTaskEscalationDTO>
    {
         DataSourceResult<CommiteeTaskEscalationDTO> GetAllWithCategory(DataSourceRequest dataSourceRequest);
    }
}
