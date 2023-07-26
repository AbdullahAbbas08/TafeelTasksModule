using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CommiteeLocalizationsController : _BaseController<CommiteeLocalization, CommiteeLocalizationDetailsDTO>
    {
        private readonly ICommiteeLocalizationService _commiteeLocalizationService;
        public CommiteeLocalizationsController(ICommiteeLocalizationService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            this._commiteeLocalizationService = businessService;
        }
        [HttpGet]
        [Route("json/{culture}")]
        [AllowAnonymous]
        public string Json(string culture)
        {
            return this._commiteeLocalizationService.GetJson(culture);
        }
        [HttpGet]
        [Route("GetLastUpDateTime")]
        [AllowAnonymous]
        public DateTime GetLastUpDateTime()
        {
            return this._commiteeLocalizationService.GetLastLocalizationUpdateTime();
        }
    }
}
