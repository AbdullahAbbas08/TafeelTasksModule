using CommiteeAndMeetings.DAL.CommiteeDomains;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using LinqHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommiteeTaskEscalationController : _BaseController<CommiteeTaskEscalation, CommiteeTaskEscalationDTO>
    {

        ICommiteeTaskEscalationService _BusinessService;
        
        public CommiteeTaskEscalationController(ICommiteeTaskEscalationService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            _BusinessService=businessService;
        }
        [HttpGet("GetAllWithCategory")]
        public DataSourceResult<CommiteeTaskEscalationDTO> GetAllWithCategory([FromQuery] DataSourceRequest dataSourceRequest)
        {
            DataSourceResult<CommiteeTaskEscalationDTO> CommiteeTaskEscalationDtos = _BusinessService.GetAllWithCategory(dataSourceRequest);
            return CommiteeTaskEscalationDtos;
        }
    }
}
