using CommiteeAndMeetings.BLL.BaseObjects;
using CommiteeAndMeetings.BLL.BaseObjects.RepositoriesInterfaces;
using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.Service.ISevices;
using CommiteeDatabase.Models.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using static AutoMapper.Internal.ExpressionFactory;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyAnswerUsersController : _BaseController<SurveyAnswerUser, SurveyAnswerUserDTO>
    {
        private readonly ISurveyAnswerUserService _surveyAnswerUserService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMeetingService meetingService;

        public SurveyAnswerUsersController(ISurveyAnswerUserService businessService,
                                           IHelperServices.ISessionServices sessionSevices,
                                           IUnitOfWork unitOfWork,
                                           IMeetingService meetingService) : base(businessService, sessionSevices)
        {
            this._surveyAnswerUserService = businessService;
            this.unitOfWork = unitOfWork;
            this.meetingService = meetingService;
        }

        public override IEnumerable<SurveyAnswerUserDTO> Post([FromBody] IEnumerable<SurveyAnswerUserDTO> entities)
        {
            var member = base.Post(entities);
            member = member.Select(c => new SurveyAnswerUserDTO
            {
                SurveyAnswerId = c.SurveyAnswerId,
                SurveyAnswerUserId = c.SurveyAnswerUserId,
                UserId = c.UserId,
                User = _surveyAnswerUserService.GetCommitteUser(c.UserId),
            });
            _surveyAnswerUserService.SurveyAnswerUserSignalR(member);
            return member;
        }

        [HttpPost("InsertCustome")]
        public virtual IEnumerable<SurveyAnswerUserDTO> InsertCustome([FromBody] IEnumerable<SurveyAnswerUserDTO> entities)
        {
            if (entities.ToList().Count > 0)
            {
                var surveyId = unitOfWork.GetRepository<SurveyAnswer>().Find(entities.FirstOrDefault().SurveyAnswerId).SurveyId;
                var survey = unitOfWork.GetRepository<Survey>().Find(surveyId);
                var meetingId = entities.FirstOrDefault().MeetingId;
                var CommiteeId = survey.CommiteeId;
                if (meetingId != null)
                {
                    var UsersNotVoting = meetingService.UsersNotVoting(int.Parse(meetingId.ToString()));
                    if (UsersNotVoting.Select(x => x.UserId).Contains(entities.FirstOrDefault().UserId))
                        return _BusinessService.Insert(entities);
                }
                else if (CommiteeId != null || survey.MeetingTopicId != null)
                    return _BusinessService.Insert(entities);
                else return null;
            }
                return null;
        }
        
        [AllowAnonymous] 
        [HttpPost("InsertCustomeFormEmail")]
        public virtual IEnumerable<SurveyAnswerUserDTO> InsertCustomeFormEmail(int userId, int SurveyAnswerId)
        {
            IEnumerable<SurveyAnswerUserDTO> entities = new List<SurveyAnswerUserDTO>() {
            new SurveyAnswerUserDTO { UserId = userId, SurveyAnswerId = SurveyAnswerId }
            }; 

            if (entities.ToList().Count >0)
            {
                var surveyId = unitOfWork.GetRepository<SurveyAnswer>().Find(entities.FirstOrDefault().SurveyAnswerId).SurveyId;
                var survey = unitOfWork.GetRepository<Survey>().Find(surveyId);
                var meetingId = survey.MeetingId;

                var UsersNotVoting = meetingService.UsersNotVoting(int.Parse(meetingId.ToString()));

                if (UsersNotVoting.Select(x => x.UserId).Contains(entities.FirstOrDefault().UserId))
                    return _BusinessService.Insert(entities);
            }
            return null;
        }
    }
}
