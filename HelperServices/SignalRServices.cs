using HelperServices.Hubs;
using IHelperServices;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelperServices
{
    public class SignalRServices : _HelperService, ISignalRServices
    {
        private readonly IHubContext<SignalRHub> _SignalRHub;

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

        public SignalRServices(IHubContext<SignalRHub> signalRHub)
        {
            _SignalRHub = signalRHub;
        }

        public IEnumerable<string> ActiveUsers
        {
            get
            {
                return SignalRHubConnectionHandler.Connections.Select(x => x.Value);
            }
        }

        public void SendMessage(string userName, object message)
        {
            _SignalRHub.Clients.User(userName).SendAsync("receiveMessage", message);
        }

        public void SendMessage(IEnumerable<string> userNames, object message)
        {
            userNames.AsParallel().ForAll(userName =>
            {
                SendMessage(userName, message);
            });

        }

        public void DeleteMessage(string userName, object message)
        {
            _SignalRHub.Clients.User(userName).SendAsync("deleteMessage", message);
        }

        public void DeleteMessage(IEnumerable<string> userNames, object message)
        {
            userNames.AsParallel().ForAll(userName =>
            {
                DeleteMessage(userName, message);
            });
        }

        public void SendNotification(string userName, object notification)
        {
            foreach (var connectionId in SignalRHubConnectionHandler.Connections.Where(c => c.Value == userName))
            {
                _SignalRHub.Clients.Client(connectionId.Key).SendAsync("receiveNotification", notification);
            }
            // _SignalRHub.Clients.User(userName).SendAsync("receiveNotification", notification);
        }

        public void SendNotification(IEnumerable<string> userNames, object notification)
        {
            userNames.AsParallel().ForAll(userName =>
            {
                SendNotification(userName, notification);
            });
        }

        public void DeleteNotification(string userName, object notification)
        {
            foreach (var connectionId in SignalRHubConnectionHandler.Connections.Where(c => c.Value == userName))
            {
                _SignalRHub.Clients.Client(connectionId.Key).SendAsync("deleteNotification", notification);
            }
            //_SignalRHub.Clients.User(userName).SendAsync("deleteNotification", notification);
        }

        public void DeleteNotification(IEnumerable<string> userNames, object notification)
        {
            userNames.AsParallel().ForAll(userName =>
            {
                DeleteNotification(userName, notification);
            });
        }

        public void UserOnlineStatusChanged(string userName, int userId, bool isOnline)
        {
            foreach (var connectionId in SignalRHubConnectionHandler.Connections.Where(c => c.Value == userName))
            {
                _SignalRHub.Clients.Client(connectionId.Key).SendAsync("userOnlineStatusChanged", userId, isOnline);
            }
            //  _SignalRHub.Clients.User(userName).SendAsync("userOnlineStatusChanged", userId, isOnline);
        }

        public void UserOnlineStatusChanged(IEnumerable<string> userNames, int userId, bool isOnline)
        {
            userNames.AsParallel().ForAll(userName =>
            {
                UserOnlineStatusChanged(userName, userId, isOnline);
            });
        }

        public void UserRoleUpdated(IEnumerable<string> userNames, List<int> RoleIds = null)
        {
            var OnlineUsers = SignalRHubConnectionHandler.Connections.Where(x => userNames.Contains(x.Value)).Select(x => x.Key).ToList();
            userNames.AsParallel().ForAll(userName =>
            {
                foreach (var connectionId in SignalRHubConnectionHandler.Connections.Where(c => c.Value == userName))
                {
                    _SignalRHub.Clients.Client(connectionId.Key).SendAsync("UserRoleUpdated", RoleIds);
                }
            });
        }

        public void UserRoleUpdatedByValue(IEnumerable<string> userNames, int UserRoleId)
        {
            var OnlineUsers = SignalRHubConnectionHandler.Connections.Where(x => userNames.Contains(x.Value)).Select(x => x.Key).ToList();
            userNames.AsParallel().ForAll(userName =>
            {
                foreach (var connectionId in SignalRHubConnectionHandler.Connections.Where(c => c.Value == userName))
                {
                    _SignalRHub.Clients.Client(connectionId.Key).SendAsync("UserRoleUpdatedByValue", UserRoleId);
                }
            });
        }

        public void RefreshAfterDelegation(string transactionActionReceipientIdEncrypt, int? OrganizationId, int? UserId)
        {
            _SignalRHub.Clients.All.SendAsync("RefreshAfterDelegation", new { transactionActionReceipientIdEncrypt = transactionActionReceipientIdEncrypt, OrganizationId = OrganizationId, UserId = UserId });
        }

        public void UpdateNotifications(List<int?> OrganizationIds)
        {

            _SignalRHub.Clients.All.SendAsync("UpdateNotifications", OrganizationIds);
        }
    }
}