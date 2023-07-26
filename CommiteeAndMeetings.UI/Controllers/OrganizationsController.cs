using CommiteeAndMeetings.DAL.Domains;
using CommiteeAndMeetings.Service.ISevices;
using DbContexts.MasarContext.ProjectionModels;
using Microsoft.AspNetCore.Mvc;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : _BaseController<Organization, OrganizationDetailsDTO>
    {
        private readonly IOrganizationService _organizationService;
        public OrganizationsController(IOrganizationService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._organizationService = businessService;
        }
    }
}
