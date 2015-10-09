using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace WindowsDefender_WebApp
{
    public class ChatHub : Hub
    {
        private ConcurrentDictionary<string, Group> groups = new ConcurrentDictionary<string, Group>();

        public void Send(string name, string message)
        {
            // TODO manually connect to users
            Clients.All.broadcastMessage(name, message);
        }
        
        public override Task OnConnected() {

            /*
            object tempObject;
            Context.Request.Environment.TryGetValue("server.RemoteIpAddress", out tempObject);
            */

            bool found = false;

            foreach (KeyValuePair<string, Group> g in groups)
            {
                Group group = g.Value;

                if(group.ids.Length < 4)
                {
                    found = true;
                    group.Add(Context.ConnectionId);
                    break;
                }
            }

            if (!found)
            {
                Group newGroup = new Group();
                newGroup.Add(Context.ConnectionId);
                groups.TryAdd(groups.Count.ToString(), newGroup);
            }

            var currentName = Context.User.Identity.Name;
            //Context.ConnectionId

            return base.OnConnected();
        }

        private class Group
        {
            public string[] ids = new string[4];
            public string hostId = null;

            public void Add (string id)
            {
                ids[ids.Length - 1] = id;
                if (hostId == null)
                {
                    hostId = id;
                }
            }
        }
    }
}