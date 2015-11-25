using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WindowsDefender_WebApp
{
    /// <summary>
    /// This class that handles the information about a match.
    /// Functionality: We can add a user or remove a user to the match.
    /// </summary>
    /// <remarks>Authors: Jeff, Rosanna, Jens (Server Team). Comments by Nadia and Rosanna.</remarks>
    /// <remarks>Updated by: NA</remarks>
    public class Match
    {
        /// <summary>
        /// The ID of the match.
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// The list of users that are currently connected to the match.
        /// </summary>
        public ArrayList Users = new ArrayList();
        /// <summary>
        /// A monitor lock that makes it so that multiple users can write to the list at the same time when writing.
        /// </summary>
        private object _usersLock = new object();

        /// <summary>
        /// Adds a user to the match. Uses a monitor lock to ensure multiple users are not writing to the list at the same time.
        /// </summary>
        /// <param name="user">The user to be added.</param>
        /// <returns>Returns true if added, and false if not added.</returns>
        public bool AddUser(User user)
        {
            lock (_usersLock)
            {
                if (Users.Count >= 4)
                    return false;

                Users.Add(user);
                return true;
            }
        }

        /// <summary>
        /// Remove user from match and re-assign the match host if needed. Uses a monitor lock to ensure multiple users are not writing to the list at the same time.
        /// </summary>
        /// <param name="user">The user to remove from the match.</param>
        public void RemoveUser(User user)
        {
            lock (_usersLock)
            {
                Users.Remove(user);
            }
        }
    }
}