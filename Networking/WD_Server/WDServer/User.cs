using System;
using System.Net;
using WDServer;

namespace WindowsDefender_WebApp
{
    /// <summary>
    /// This class handles information about the user.
    /// Functionality: increments a timeout count that will be used with the heartbeat function.
    /// Also has the ability to check if the connection has expired, and the ability to reset the timeout counter.
    /// </summary>
    /// <remarks>Authors: Jeff, Rosanna, Jens (Server Team). Comments by Nadia and Rosanna.</remarks>
    /// <remarks>Updated by: NA</remarks>
    public class User
    {
        /// <summary>
        /// A monitor lock that makes it so multiple users cannot access the timeout at the same time.
        /// </summary>
        private object  _lock = new object();
        /// <summary>
        /// The component that handles the information about the IP and Port and other.
        /// </summary>
        public IPEndPoint  EndPoint { get; set; }
        /// <summary>
        /// The username of the user.
        /// </summary>
        public string    Username { get; set; }
        /// <summary>
        /// The match that the user is connected to or connecting to.
        /// </summary>
        public string    MatchId { get; set; }
        /// <summary>
        /// A timeout counter for the heartbeat function.
        /// </summary>
        private int     _timeout = 0;

        /// <summary>
        /// Increments the timeout. Uses a monitor lock to control access of the counter.
        /// </summary>
        public void IncrementTimeout()
        {
            lock (_lock)
                _timeout++;
        }

        /// <summary>
        /// Checks if the connection has currently expired.
        /// </summary>
        /// <returns>True if the connection has expired, false if it is still conected.</returns>
        public bool IsConnectionExpired()
        {
            lock (_lock)
            {
                if (_timeout > Server.CLIENT_TIMEOUT)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Resets the timer count to 0.
        /// </summary>
        public void ResetTimeout()
        {
            lock (_lock)
                _timeout = 0;
        }
    }
}