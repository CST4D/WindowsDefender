using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Collections;

namespace WindowsDefender_WebApp
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, Group> groups = new ConcurrentDictionary<string, Group>();

        public void Send(string name, string message)
        {
            // TODO manually connect to users
            Clients.All.broadcastMessage(name, message);
        }
        
        public override Task OnConnected() {
            Debug.WriteLine("OnConnected");
            /*
            object tempObject;
            Context.Request.Environment.TryGetValue("server.RemoteIpAddress", out tempObject);
            */

            bool found = false;

            foreach (KeyValuePair<string, Group> g in groups)
            {
                Group group = g.Value;

                if(group.Add(Context.ConnectionId))
                {
                    found = true;
                    break;
                }
            }

            Debug.WriteLine("found: " + found);

            if (!found)
            {
                Group newGroup = new Group();
                newGroup.Add(Context.ConnectionId);
                groups.TryAdd(groups.Count.ToString(), newGroup);
                Debug.WriteLine("group created");

            }

            Debug.WriteLine("connection id: " + Context.ConnectionId);

            return base.OnConnected();
        }

        private class Group
        {
            public ArrayList ids = new ArrayList();
            public string hostId = null;

            public bool Add (string id)
            {
                if (ids.Count >= 4) { return false; }

                ids.Add(id);

                if (hostId == null) { hostId = id; }

                return true;
            }
        }
    }
}