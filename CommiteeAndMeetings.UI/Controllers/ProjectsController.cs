using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : _BaseController<Project, ProjectDTO>
    {
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._projectService = businessService;
        }
        [HttpGet("GetProjectsLookup")]
        public List<LookUpDTO> GetProjectsLookup(int take, int skip, string searchText)
        {
            return _projectService.GetListOfProjectsLookup(take, skip, searchText);
        }
    }
}