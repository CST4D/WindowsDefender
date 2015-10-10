using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WindowsDefender_WebApp
{
    public class Match
    {
        public string Id { get; set; }
        public ArrayList Users = new ArrayList();
        public string HostId { get; set; } = null;

        public Match()
        {
            Id = Guid.NewGuid().ToString();
        }

        public bool AddUser(User user)
        {
            if (Users.Count >= 4) {
                return false;
            }

            Users.Add(user);

            if (HostId == null) {
                HostId = user.ConnectionId;
            }

            return true;
        }

        public void RemoveUser(User user)
        {
            Users.Remove(user);
        }
    }
}