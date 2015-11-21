
using System.Net.Sockets;
using System.Net;
using System.Threading;
using WDClient;
using System;
using WDServer;

/// <summary>
/// This class contains the component that is responsible for sending and receiving messages from the Client side.
/// Functionality: Starts the client, requests to join, can send instructions, receive instructions, and quit.
/// </summary>
/// <remarks>Authors: Duy, Nadia, Joel. Comments added by Rosanna and Nadia.</remarks>
/// <remarks>Update by: NA</remarks>
namespace UnitTests
{
    public class NetworkClient
    {
        /// <summary>
        /// The thread the client runs on.
        /// </summary>
        private Thread clientThread = null;
        /// <summary>
        /// The socket for UDP.
        /// </summary>
        public UdpClient socket = null;
        /// <summary>
        /// The monitor lock that makes it so you cannot send more than one message at the same time. So buffer will not be overwritten.
        /// </summary>
        private object sendLock = new object();
        /// <summary>
        /// The variable that keep the client running, which is checking for instructions.
        /// Will run until app quit.
        /// </summary>
        bool running = true;
        /// <summary>
        /// The IP of the server.
        /// TODO: Change it eventually. (Should not be hardcoded to local).
        /// </summary>
        public string SERVER_IP = "127.0.0.1";
        /// <summary>
        /// The Port that the server is using.
        /// </summary>
        private const int SERVER_PORT = 25001;
        /// <summary>
        /// The client's username for the game. It is created in the web component of the application.
        /// TODO: Change it eventually. (Should not be hardcoded).
        /// </summary>
        public string username = "Jeff";
        /// <summary>
        /// The client's MatchID for the game. It is created in the web component of the application.
        /// TODO: Change it eventually. (Should not be hardcoded).
        /// </summary>
        public string matchID = "4fg7-38g3-d922-f75g-48g6";

        /// <summary>
        /// The component that the game logic uses to creates the instance of our class.
        /// </summary>
        /// <param name="matchID">The MatchID of the client. It is 20 characters separated with a dash every 4 characters.</param>
        /// <param name="username">The username of the client.</param>
        public NetworkClient(string matchID, string username, string ip)
        {
            this.matchID = matchID;
            this.username = username;
            this.SERVER_IP = ip;
        }
        /// <summary>
        /// Starts the new thread that will call the client to start.
        /// </summary>
        public void Start()
        {
            socket = new UdpClient(SERVER_IP, SERVER_PORT);
        }

        /// <summary>
        /// Sends an instruction to the server. It's in a lock (using monitor lock) to prevent multiple sending instructions at the same time.
        /// </summary>
        /// <param name="command">The type of instruction to send.</param>
        /// <param name="arg1">An argument to pass along with the instruction.</param>
        /// <param name="arg2">An argument to pass along with the instruction.</param>
        /// <param name="arg3">An argument to pass along with the instruction.</param>
        /// <param name="arg4">An argument to pass along with the instruction.</param>
        public void SendInstruction(Instruction.Type command, string arg1 = "", string arg2 = "", string arg3 = "", string arg4 = "")
        {
            lock (sendLock)
            {
                try
                {
                    Instruction newInstruction = new Instruction()
                    {
                        Command = command,
                        Arg1 = arg1,
                        Arg2 = arg2,
                        Arg3 = arg3,
                        Arg4 = arg4
                    };

                    // Send instruction
                    byte[] dataToSend = Serializer.Serialize(newInstruction);
                    socket.Send(dataToSend, dataToSend.Length);
                }
                catch (Exception ex)
                {
                }
            }
        }

        /// <summary>
        /// Quits the client and closes the socket and stops the threads.
        /// Must have this otherwise Unity will freeze after running
        /// the application more than once.
        /// </summary>
        public void OnApplicationQuit()
        {
            running = false;
            socket.Close();
            clientThread.Abort();
        }
    }
}