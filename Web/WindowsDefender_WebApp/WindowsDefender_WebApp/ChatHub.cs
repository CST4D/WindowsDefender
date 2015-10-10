using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Collections;
using System.Web.Script.Serialization;

namespace WindowsDefender_WebApp
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>();
        private static ConcurrentDictionary<string, Match> _matches = new ConcurrentDictionary<string, Match>();

        /// <summary>
        /// New user has connected.
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            CreateUser();
            FindMatch();
            return base.OnConnected();
        }

        /// <summary>
        /// Creates a new user in the _users dictionary.
        /// </summary>
        private void CreateUser()
        {
            User user = new User()
            {
                ConnectionId = Context.ConnectionId,
                Name = Context.User.Identity.Name
            };
            _users.TryAdd(Context.ConnectionId, user);
        }

        /// <summary>
        /// Finds a match for the user.
        /// </summary>
        private void FindMatch()
        {
            User user = null;
            _users.TryGetValue(Context.ConnectionId, out user);
            if (user == null) return;

            bool matchFound = false;
            foreach (KeyValuePair<string, Match> g in _matches)
            {
                Match match = g.Value;
                if (matchFound = match.AddUser(user))
                {
                    // Assign match to user
                    user.MatchId = match.Id;

                    // Notify users
                    SendMessageToMatch(user.MatchId, "* " + user.Name + " has joined the game.");
                    SendMessageToUser(user.ConnectionId, "You have joined a game.");

                    // Update match's user list
                    UpdateUserList(match);

                    break;
                }
            }

            if (!matchFound)
            {
                Match newMatch = CreateMatch();
                newMatch.AddUser(user);
                user.MatchId = newMatch.Id;

                // Update match's user list
                UpdateUserList(newMatch);
            }
        }

        /// <summary>
        /// Creates a new match.
        /// </summary>
        /// <returns></returns>
        private Match CreateMatch()
        {
            Match match = new Match();
            _matches.TryAdd(match.Id, match);
            return match;
        }

        /// <summary>
        /// Sends a message to everyone in a match,
        /// including the user who sent it.
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            User user = null;
            _users.TryGetValue(Context.ConnectionId, out user);
            if (user == null)
                return;

            SendMessageToMatch(user.MatchId, message);
            SendMessageToUser(user.ConnectionId, message);
        }
        
        /// <summary>
        /// Sends to everyone in a match except the current user.
        /// </summary>
        /// <param name="message"></param>
        private void SendMessageToMatch(string matchId, string message)
        {
            User user = null;
            _users.TryGetValue(Context.ConnectionId, out user);

            if (user != null)
            {
                Match match = null;
                _matches.TryGetValue(matchId, out match);
                if (match != null)
                {
                    foreach (User matchUser in match.Users)
                    {
                        if (matchUser.ConnectionId != Context.ConnectionId)
                        {
                            Clients.Client(matchUser.ConnectionId).send(message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sends a message to a single user.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="message"></param>
        public void SendMessageToUser(string connectionId, string message)
        {
            if (connectionId != null)
                Clients.Client(connectionId).send(message);
        }

        /// <summary>
        /// Sends a chat message to everyone in the user's match with format: 'Name: message'
        /// </summary>
        /// <param name="message"></param>
        public void SendChatMessage(string message)
        {
            User user = null;
            _users.TryGetValue(Context.ConnectionId, out user);
            if (user == null)
                return;
            Send(user.Name + ": " + message);
        }

        /// <summary>
        /// Sends the current userlist to everyone in a match.
        /// </summary>
        /// <param name="match"></param>
        public void UpdateUserList(Match match)
        {
            // Convert userlist to JSON
            string[] userlist = new string[match.Users.Count];
            for (int i = 0; i < match.Users.Count; i++)
                userlist[i] = ((User)match.Users[i]).Name;
            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(userlist);

            // Send JSON to each user in the match
            foreach (User user in match.Users)
                Clients.Client(user.ConnectionId).updateUserList(json);
        }

        /// <summary>
        /// When the user presses the Ready button.
        /// </summary>
        public void Ready(bool isReady)
        {
            User user = null;
            _users.TryGetValue(Context.ConnectionId, out user);
            if (user == null)
                return;

            if (isReady)
                Send(user.Name + " is ready.");
            else
                Send(user.Name + " is no longer ready.");
        }

        /// <summary>
        /// User has disconnected.
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            User user = null;
            _users.TryGetValue(Context.ConnectionId, out user);
            if (user == null)
                return base.OnDisconnected(stopCalled);

            // Remove user from match
            if (user.MatchId != null)
            {
                // Notify users
                SendMessageToMatch(user.MatchId, "* " + user.Name + " has left the game.");

                Match match = null;
                _matches.TryGetValue(user.MatchId, out match);
                if (match != null)
                    match.RemoveUser(user);
               
                // Update match's user list
                UpdateUserList(match);

                // If match is now empty, close the match entirely
                if (match.Users.Count == 0) {
                    Match dummy = null;
                    _matches.TryRemove(match.Id, out dummy);
                }
            }

            // Remove user
            User userdummy = null;
            _users.TryRemove(user.ConnectionId, out userdummy);

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// User has reconnected.
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            // TODO
            return base.OnReconnected();
        }

    }
}




/*
         object tempObject;
         Context.Request.Environment.TryGetValue("server.RemoteIpAddress", out tempObject);
         */
