using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Services.ISevices;
using Models.ProjectionModels;
using System.Collections.Generic;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ITransactionService : IBusinessService<Transaction, TransactionDTO> 
    {
        //bool Delegate(DelegationDTO delegationDTO, IFollowUpService followUpService);

    }
}
