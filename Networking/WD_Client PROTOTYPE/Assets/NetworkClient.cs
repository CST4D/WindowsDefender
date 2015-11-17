using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using WDClient;
using System;

/// <summary>
/// This class contains the component that is responsible for sending and receiving messages from the Client side.
/// Functionality: Starts the client, requests to join, can send instructions, receive instructions, and quit.
/// </summary>
/// <remarks>Authors: Duy, Nadia, Joel. Comments added by Rosanna and Nadia.</remarks>
/// <remarks>Update by: NA</remarks>
public class NetworkClient : MonoBehaviour
{
    /// <summary>
    /// The thread the client runs on.
    /// </summary>
    private Thread clientThread = null;
    /// <summary>
    /// The socket for UDP.
    /// </summary>
    private UdpClient socket = null;
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
    private const string SERVER_IP = "127.0.0.1";
    /// <summary>
    /// The Port that the server is using.
    /// </summary>
    private const int SERVER_PORT = 25001;
    /// <summary>
    /// The client's username for the game. It is created in the web component of the application.
    /// TODO: Change it eventually. (Should not be hardcoded).
    /// </summary>
    string username = "Jeff";
    /// <summary>
    /// The client's MatchID for the game. It is created in the web component of the application.
    /// TODO: Change it eventually. (Should not be hardcoded).
    /// </summary>
    string matchID = "4fg7-38g3-d922-f75g-48g6";

    /// <summary>
    /// The component that the game logic uses to creates the instance of our class.
    /// </summary>
    /// <param name="matchID">The MatchID of the client. It is 20 characters separated with a dash every 4 characters.</param>
    /// <param name="username">The username of the client.</param>
    public NetworkClient(string matchID = "4fg7-38g3-d922-f75g-48g6", string username = "Jeff")
    {
        this.matchID = matchID;
        this.username = username;
    }
    /// <summary>
    /// Starts the new thread that will call the client to start.
    /// </summary>
    void Start()
    {
        clientThread = new Thread(StartClient);
        clientThread.Start();
    }

    /// <summary>
    /// Starts the client. Asks the server for permission to join. Then runs in a loop, constantly looking for new instructions, 
    /// such as Leave, Joined, and Command. It deserializes the instruction and passes it on to ReceiveInstruction method.
    /// </summary>
    private void StartClient()
    {
       
        // Setup
        byte[] receivedData = new byte[512];
        IPEndPoint receiverEndPoint = new IPEndPoint(IPAddress.Any, 0);
        socket = new UdpClient(SERVER_IP, SERVER_PORT);

        // Send JOIN command (This adds the user to the server's user 
        // list and inserts him/her into the correct match based on matchID)
        print("Sending JOIN command to server.");
        SendInstruction(Instruction.Type.JOIN, username, matchID);

        // Wait for incoming instructions (This blocks until something is received)
        while (running)
        {
            try {
                // Block and wait
                receivedData = socket.Receive(ref receiverEndPoint);

                // Data received, remove trailing nulls
                int i = receivedData.Length - 1;
                while (receivedData[i] == 0)
                    --i;
                byte[] data = new byte[i + 1];
                Array.Copy(receivedData, data, i + 1);

                // Deserialize recieved instruction
                Instruction receivedInstruction = Serializer.DeSerialize(data);

                // Handle the instruction
                ReceiveInstruction(receivedInstruction);
            }
            catch (Exception ex)
            {
                print(ex.StackTrace);
            }
        }
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
        lock(sendLock) {
            try {
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
                print("Size of data being sent: " + dataToSend.Length);
                print("Data sent: " + arg1 + "/" + arg2 + "/" + arg3 + "/" + arg4);
            } catch (Exception ex)
            {
                print(ex.StackTrace);
            }
        }
    }

    /// <summary>
    /// When a command is received from the server. It specifies what to do with the different type of instruction received.
    /// TODO: A 'leave' command, 'joined' command and 'command' command just print for now. Need to add function.
    /// </summary>
    private void ReceiveInstruction(Instruction instruction)
    {
        // If user was disconnected
        if (instruction.Command == Instruction.Type.LEAVE)
        {
            print("Disconnected from the server.");
        }

        // If user joined match successfully
        if (instruction.Command == Instruction.Type.JOINED)
        {
            print("You have joined the match successfully.");
        }

        if (instruction.Command == Instruction.Type.CMD)
        {
            // This is the actual data from a received instruction for use with game logic
            string arg1 = instruction.Arg1;
            string arg2 = instruction.Arg2;
            string arg3 = instruction.Arg3;
            string arg4 = instruction.Arg4;
            print("Received command with arguments: " + arg1 + "/" + arg2 + "/" + arg3 + "/" + arg4);
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
