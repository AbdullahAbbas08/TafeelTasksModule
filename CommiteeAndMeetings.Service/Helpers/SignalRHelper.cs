using CommiteeAndMeetings.DAL.CommiteeDTO;
using CommiteeAndMeetings.DAL.MeetingDTO;
using CommiteeAndMeetings.Service.ISevices;
using HelperServices.Hubs;
using Microsoft.AspNetCore.SignalR;
using Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.UI.Helpers
{
    public class SignalRHelper
    {
        private readonly IHubContext<SignalRHub> _signalR;
        private readonly IDataProtectService _dataProtectService;
        public SignalRHelper(IHubContext<SignalRHub> signalR, IDataProtectService dataProtectService)
        {
            _signalR = signalR;
            _dataProtectService = dataProtectService;
        }
        public Action<string> UserConnectedTask
        {
            get
            {
                return SignalRHubConnectionHandler.UserConnectedTask;
            }
            set
            {
                if (SignalRHubConnectionHandler.UserConnectedTask == null)
                    SignalRHubConnectionHandler.UserConnectedTask = value;
            }
        }
        public Action<string> UserDisconnectedTask { get { return SignalRHubConnectionHandler.UserDisconnectedTask; } set { if (SignalRHubConnectionHandler.UserDisconnectedTask == null) SignalRHubConnectionHandler.UserDisconnectedTask = value; } }
        public void SignOut(string Username)
        {
            var ConnectionsToRemove = new List<string>();
            ConnectionsToRemove.AddRange(SignalRHubConnectionHandler.Connections.Where(x => x.Value.ToUpper() == Username.ToUpper()).Select(x => x.Key));
            foreach (var Id in ConnectionsToRemove)
            {
                SignalRHubConnectionHandler.RemoveConnection(Id);
            }
        }
        public IEnumerable<string> ActiveUsers
        {
            get
            {
                return SignalRHubConnectionHandler.Connections.Select(x => x.Value);
            }
        }
        public void NextTopicSender(UserChatDTO user, TopicTimeLineDTO topicTimeLineDTO, int fromUserId)
        {
            foreach (var connectionId in SignalRHubConnectionHandler.Connections.Where(c => c.Value == _dataProtectService.Encrypt(user.UserName)))
            {
                _signalR.Clients.Client(connectionId.Key).SendAsync("NextTopicListener", topicTimeLineDTO, fromUserId);
            }
        }
        public void BeginTopicSender(UserChatDTO user, int topicId, int fromUserId)
        {
            foreach (var connectionId in SignalRHubConnectionHandler.Connections.Where(c => c.Value == _dataProtectService.Encrypt(user.UserName)))
            {
                _signalR.Clients.Client(connectionId.Key).SendAsync("BeginTopicListener", topicId, fromUserId);
            }
        }
        public void EndTopicSender(UserChatDTO user, int topicId, int fromUserId)
        {
            foreach (var connectionId in SignalRHubConnectionHandler.Connections.Where(c => c.Value == _dataProtectService.Encrypt(user.UserName)))
            {
                _signalR.Clients.Client(connectionId.Key).SendAsync("EndTopicListener", topicId, fromUserId);
            }
        }
        public void SurveyAnswerUserSender(UserChatDTO user, int topicId, IEnumerable<SurveyAnswerUserDTO> item, int fromUserId)
        {
            foreach (var connectionId in SignalRHubConnectionHandler.Connections.Where(c => c.Value == _dataProtectService.Encrypt(user.UserName)))
            {
                _signalR.Clients.Client(connectionId.Key).SendAsync("SurveyAnswerUserListener", item, topicId, fromUserId);
            }
        }
        public void PauseTopicSender(UserChatDTO user, int topicId, int fromUserId)
        {
            foreach (var connectionId in SignalRHubConnectionHandler.Connections.Where(c => c.Value == _dataProtectService.Encrypt(user.UserName)))
            {
                _signalR.Clients.Client(connectionId.Key).SendAsync("PauseTopicListener", topicId, fromUserId);
            }
        }
        public void ResumeTopicSender(UserChatDTO user, int topicId, int fromUserId)
        {
            foreach (var connectionId in SignalRHubConnectionHandler.Connections.Where(c => c.Value == _dataProtectService.Encrypt(user.UserName)))
            {
                _signalR.Clients.Client(connectionId.Key).SendAsync("ResumeTopicListener", topicId, fromUserId);
            }
        }
        public void InsertTopicCommentSender(UserChatDTO user, IEnumerable<TopicCommentDTO> comments, int fromUserId)
        {
            foreach (var connectionId in SignalRHubConnectionHandler.Connections.Where(c => c.Value == _dataProtectService.Encrypt(user.UserName)))
            {
                _signalR.Clients.Client(connectionId.Key).SendAsync("InsertTopicCommentListener", comments, fromUserId);
            }
        }
        public void InsertSurveySender(UserChatDTO user, SurveyDTO survey, int fromUserId)
        {
            foreach (var connectionId in SignalRHubConnectionHandler.Connections.Where(c => c.Value == _dataProtectService.Encrypt(user.UserName)))
            {
                _signalR.Clients.Client(connectionId.Key).SendAsync("InsertSurveyListener", survey, fromUserId);
            }
        }
    }
}
