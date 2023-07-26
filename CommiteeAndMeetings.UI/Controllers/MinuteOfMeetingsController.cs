using CommiteeAndMeetings.DAL.MeetingDomains;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using IHelperServices.Models;
using Microsoft.Extensions.Options;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class MinuteOfMeetingsController : _BaseController<MinuteOfMeeting, MinuteOfMeetingDTO>
    {
        private readonly IMinuteOfMeetingService _minuteOfMeetingService;
        private readonly IMeetingService _meetingService;
        public AppSettings _AppSettings;

        IHttpContextAccessor _contextAccessor;

        public MinuteOfMeetingsController(IMinuteOfMeetingService businessService, IHelperServices.ISessionServices sessionSevices, IHttpContextAccessor contextAccessor, IOptions<AppSettings> appSettings) : base(businessService, sessionSevices)
        {
            this._minuteOfMeetingService = businessService;
            this._contextAccessor = contextAccessor;
            _AppSettings = new AppSettings();
            _AppSettings = appSettings.Value;


        }
        [HttpGet("MOMApproval")]
        public async Task<bool> MOMApprovalAsync(int meetingId)
        {
            var tokenvalues = new StringValues();
            string masarURL = _AppSettings.MasarURL;
            _contextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out tokenvalues);

            return await _minuteOfMeetingService.MOMApproval(meetingId,tokenvalues, masarURL);
        }
        public override IEnumerable<object> Delete(int id)
        {
            _minuteOfMeetingService.DeleteMomComents(id);
            return base.Delete(id);
        }


        [HttpGet("SendMailMoMInvitation")]
        public bool SendMailMoMInvitation(int MeetingId)
        {
            return _minuteOfMeetingService.SendMailMoMInvitation(MeetingId);
        }



    }
}