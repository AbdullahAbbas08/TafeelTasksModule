using CommiteeAndMeetings.Service.ISevices;
using DbContexts.MasarContext.ProjectionModels;
using LinqHelper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : _BaseController<Lookup, Lookup>
    {
        private readonly ILookupService _LookupService;
        public LookupController(ILookupService businessService, IHelperServices.ISessionServices sessionSevices) : base(businessService, sessionSevices)
        {
            _LookupService = businessService;
        }
        [HttpGet]
        public IEnumerable<Lookup> Get([FromQuery] string type, [FromQuery] string searchText, [FromQuery] DataSourceRequest dataSourceRequest, [FromQuery] string args = null)
        {
            Dictionary<string, dynamic> argsDictionary = args == null ? null : JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(args);
            return _LookupService.Get(type, searchText, dataSourceRequest, argsDictionary, null);
        }
    }
}
