using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using WDClient;
using System;

/// <summary>
/// 
/// </summary>
public class NetworkClient : MonoBehaviour
{
    /// <summary>
    /// The client thread
    /// </summary>
    private Thread clientThread = null;
    /// <summary>
    /// The socket
    /// </summary>
    private UdpClient socket = null;
    /// <summary>
    /// The send lock
    /// </summary>
    private object sendLock = new object();
    /// <summary>
    /// The running
    /// </summary>
    bool running = true;
    /// <summary>
    /// The ip
    /// </summary>
    private string ip = "127.0.0.1";
    /// <summary>
    /// The serve r_ port
    /// </summary>
    private const int SERVER_PORT = 25001;
    /// <summary>
    /// The username
    /// </summary>
    string username;
    /// <summary>
    /// The match identifier
    /// </summary>
    string matchID;
    /// <summary>
    /// The net adapter
    /// </summary>
    MessagingNetworkAdapter netAdapter;

    /// <summary>
    /// Initializes the specified ip.
    /// </summary>
    /// <param name="ip">The ip.</param>
    /// <param name="matchID">The match identifier.</param>
    /// <param name="username">The username.</param>
    /// <param name="netAdapter">The net adapter.</param>
    public void Initialize(string ip, string matchID, string username, MessagingNetworkAdapter netAdapter)
    {
        this.matchID = matchID;
        this.username = username;
        this.netAdapter = netAdapter;
        this.ip = ip;
        clientThread = new Thread(StartClient);
        clientThread.Start();
    }

    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Starts the client.
    /// </summary>
    private void StartClient()
    {
       
        // Setup
        byte[] receivedData = new byte[512];
        IPEndPoint receiverEndPoint = new IPEndPoint(IPAddress.Any, 0);
        socket = new UdpClient(ip, SERVER_PORT);

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
    /// Sends the specified MSG.
    /// </summary>
    /// <param name="msg">The MSG.</param>
    public void Send(string msg)
    {
        SendInstruction(Instruction.Type.CMD, msg);
    }

    /// <summary>
    /// Sends an instruction to the server
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="arg1">The arg1.</param>
    /// <param name="arg2">The arg2.</param>
    /// <param name="arg3">The arg3.</param>
    /// <param name="arg4">The arg4.</param>
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
    /// When a command is received from the server
    /// </summary>
    /// <param name="instruction">The instruction.</param>
    private void ReceiveInstruction(Instruction instruction)
    {
        // If user was disconnected
        if (instruction.Command == Instruction.Type.LEAVE)
        {
            print("Disconnected from the server.");
            netAdapter.InputRecvData("" + (int)MultiplayerMessagingAdapter.MessageType.Disconnected);
        }

        // If user joined match successfully
        if (instruction.Command == Instruction.Type.JOINED)
        {
            print("You have joined the match successfully.");
            netAdapter.InputRecvData("" + (int)MultiplayerMessagingAdapter.MessageType.Connected);
        }

        if (instruction.Command == Instruction.Type.CMD)
        {
            // This is the actual data from a received instruction for use with game logic
            string arg1 = instruction.Arg1;
            string arg2 = instruction.Arg2;
            string arg3 = instruction.Arg3;
            string arg4 = instruction.Arg4;
            print("Received command with arguments: " + arg1 + "/" + arg2 + "/" + arg3 + "/" + arg4);
            netAdapter.InputRecvData(arg1);
        }
    }

    /// <summary>
    /// Must have this otherwise Unity will freeze after running
    /// the application more than once.
    /// </summary>
    public void OnApplicationQuit()
    {
        running = false;
        Instruction newInstruction = new Instruction()
        {
            Command = Instruction.Type.LEAVE,
            Arg1 = "leaving aapp",
            Arg2 = "",
            Arg3 = "",
            Arg4 = ""
        };
        // Send instruction
        byte[] dataToSend = Serializer.Serialize(newInstruction);
        socket.Send(dataToSend, dataToSend.Length);
        socket.Close();
        clientThread.Abort();
    }
}
