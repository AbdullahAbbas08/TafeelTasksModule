using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.ProjectionModels;
using CommiteeAndMeetings.Service.ISevices;
using DbContexts.MasarContext.ProjectionModels;
using IHelperServices.Models;
using iTextSharp.text;
using LinqHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Models.ProjectionModels;
using System.Collections.Generic;
using System.Linq;
using static iTextSharp.text.pdf.AcroFields;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {

        public ITransactionBoxService _TransactionBoxService;
        public AppSettings _AppSettings;
        private readonly IFollowUpService FollowUpService;
        protected readonly IHelperServices.ISessionServices _SessionServices;
        public TransactionsController(ITransactionBoxService TransactionBoxService, IFollowUpService FollowUpService, IOptions<AppSettings> appSettings, IHelperServices.ISessionServices sessionSevices)
        {
            _AppSettings = new AppSettings();
            _AppSettings = appSettings.Value;
            _TransactionBoxService = TransactionBoxService;
            _SessionServices = sessionSevices;
        }

        [HttpPost]
        [Route("GroupReferral")]
        public List<Vw_ReturnGroupReferralDTO> GroupReferral([FromBody] GroupReferralDTO groupReferralDTO)
        {
            return this._TransactionBoxService.GroupReferral(groupReferralDTO);
        }

        [HttpGet("GetboxType")]
        public DataSourceResult<TransactionBoxDTO> GetboxType(string boxType, string searchText, int page, int pageSize, string CommitteId, bool isCount = true, bool isEmployee = true, int filterId = 0, int filterCase = 1)
        {
            UserIdAndRoleIdAfterDecryptDTO UserIdAndUserRoleId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteId,true);

            InboxFilterFieldsDTo inboxFilterFields = new InboxFilterFieldsDTo
            {
                FilterCase = filterCase
            };
            var result = _TransactionBoxService.GetboxDetails(boxType, searchText, page, pageSize, isCount, UserIdAndUserRoleId.Id, isEmployee, filterId, inboxFilterFields);
            return result;
        }
        [HttpPost("SaveTransaction")]
        public TransactionDetailsDTO SaveTransaction([FromBody] TransactionDetailsDTO transaction)
        {
            //var absoluteUrl = _AppSettings.AbsoluteUrl;
            //HttpClientFactory http = new HttpClientFactory();
            //return Ok(http.SaveTransaction(transaction, accessToken, absoluteUrl));
            return _TransactionBoxService.SaveTransaction(transaction);
        }
        [HttpPost("InsertAttachments")]
        public IEnumerable<TransactionAttachmentDTO> InsertAttachments([FromBody] List<TransactionAttachmentDTO> transaction)
        {
            //var absoluteUrl = _AppSettings.AbsoluteUrl;
            //HttpClientFactory http = new HttpClientFactory();
            //return Ok(http.InsertAttachments(transaction, accessToken,absoluteUrl));

            foreach (var item in transaction)
            {
              item.TransactionId =  _SessionServices.UserIdAndRoleIdAfterDecrypt(item.transactionIdEncypted,false).Id;
              item.AttachmentId = _SessionServices.UserIdAndRoleIdAfterDecrypt(item.attachmentIdEncypted,false).Id;

                //item.attachmentId = int.Parse(item.AttachmentId);

            }
            return _TransactionBoxService.InsertAttachments(transaction);
        }

        [HttpPost("InsertDelegation")]
        public bool InsertDelegation([FromBody] DelegationDTO delegationDTO)
        {
            delegationDTO.EmailDetailsUrlsDTO.detailsUrl = $"{Request.Scheme}://{Request.Host.Value}";
            bool result = _TransactionBoxService.Delegate(delegationDTO, FollowUpService);
            return result;
        }

        [HttpGet, Route("GetEmployeesToReferral")]
        public IEnumerable<Lookup> GetEmployeesToReferral(bool isEmployee, bool? isFavourite, string searchText)
        {
            return _TransactionBoxService.GetEmployeesToReferral(isEmployee, isFavourite, searchText);
        }
        [HttpGet, Route("GetOrganizationToReferral")]
        public IEnumerable<Lookup> GetOrganizationToReferral(bool isEmployee, bool? isExternalOrg, bool? isFavourite, string searchText,string CommitteIdEncripted, int? transactionActionRecepientId, int? OrganizationId = null)
        {
            var committeId = _SessionServices.UserIdAndRoleIdAfterDecrypt(CommitteIdEncripted, false).Id;
            return _TransactionBoxService.GetOrganizationToReferral(isEmployee, isExternalOrg, isFavourite, searchText, committeId, transactionActionRecepientId, OrganizationId);
        }
    }
}
