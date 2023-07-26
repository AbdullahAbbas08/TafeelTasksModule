using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Services.ISevices;
using DbContexts.MasarContext.ProjectionModels;
using LinqHelper;
using Models.ProjectionModels;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.ISevices
{
    public interface ITransactionBoxService : IBusinessService<TransactionAction, TransactionBoxDTO>
    {
        TransactionDetailsDTO SaveTransaction(TransactionDetailsDTO transactionDetailsDTO);
        DataSourceResult<TransactionBoxDTO> GetboxDetails(string BoxType, string searchText, int page, int pageSize, bool isCount, int? CommitteId, bool isEmployee = true, int filterId = 0, InboxFilterFieldsDTo inboxFilterFields = null);
        IEnumerable<Lookup> GetEmployeesToReferral(bool isEmployee, bool? isFavourite, string searchText);
        IEnumerable<Lookup> GetOrganizationToReferral(bool isEmployee, bool? isExternalOrg, bool? isFavourite, string searchText,int committeId ,int? transactionActionRecepientId, int? organizationId);
        IEnumerable<TransactionAttachmentDTO> InsertAttachments(List<TransactionAttachmentDTO> transaction);
        bool Delegate(DelegationDTO delegationDTO, IFollowUpService followUpService);
        List<Vw_ReturnGroupReferralDTO> GroupReferral(GroupReferralDTO groupReferralDTO);

    }
}
