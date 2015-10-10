using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WindowsDefender_WebApp
{
    public class Match
    {
        public string Id { get; set; }
        public string HostId { get; set; } = null;

        public ArrayList Users = new ArrayList();
        private object _usersLock = new object();

        public Match()
        {
            Id = Guid.NewGuid().ToString();
        }

        public bool AddUser(User user)
        {
            lock (_usersLock)
            {
                if (Users.Count >= 4)
                {
                    return false;
                }

                Users.Add(user);

                if (HostId == null)
                {
                    HostId = user.ConnectionId;
                }

                return true;
            }
        }

        public void RemoveUser(User user)
        {
            lock (_usersLock)
                Users.Remove(user);
        }
    }
}