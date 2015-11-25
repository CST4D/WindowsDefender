using System;
namespace WDServer
{
    /// <summary>
    /// The main program that creates a new server.
    /// Functionality: Creates a new server.
    /// </summary>
    /// <remarks>Authors: Jeff, Rosanna, Jens (Server Team). Comments by Nadia and Rosanna.</remarks>
    /// <remarks>Updated by: NA</remarks>
    class Program
    {
        /// <summary>
        /// The server.
        /// </summary>
        private static Server server;
        /// <summary>
        /// The main thread that will create the server instance.
        /// </summary>
        /// <param name="args">The commands passed in to main thread. Not used for now.</param>
        static void Main(string[] args)
        {
            server = new Server();
        }
    }
}
