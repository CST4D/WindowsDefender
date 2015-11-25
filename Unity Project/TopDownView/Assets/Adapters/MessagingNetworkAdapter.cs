using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 
/// </summary>
public class MessagingNetworkAdapter {
    /// <summary>
    /// 
    /// </summary>
    public class Message
    {

        /// <summary>
        /// The arguments
        /// </summary>
        private string[] args;
        /// <summary>
        /// The type
        /// </summary>
        private int type;
        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public string[] Args { get { return args; } }
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public int Type { get { return type; } }
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="parts">The parts.</param>
        public Message(string[] parts)
        {
            type = Convert.ToInt32(parts[0]);
            args = new string[parts.Length - 1];
            for (int i = 1; i < parts.Length; i++)
                args[i - 1] = parts[i];
        }
    }

    /// <summary>
    /// The net client
    /// </summary>
    private NetworkClient netClient;
    /// <summary>
    /// The queue
    /// </summary>
    private System.Collections.Generic.Queue<Message> queue;
    /// <summary>
    /// The queue mutex
    /// </summary>
    private System.Threading.Mutex queueMutex;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessagingNetworkAdapter"/> class.
    /// </summary>
    public MessagingNetworkAdapter() : this(null)
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessagingNetworkAdapter"/> class.
    /// </summary>
    /// <param name="netClient">The net client.</param>
    public MessagingNetworkAdapter(NetworkClient netClient)
    {
        this.netClient = netClient;
        queue = new System.Collections.Generic.Queue<Message>();
        queueMutex = new System.Threading.Mutex();
    }

    /// <summary>
    /// Inputs the recv data.
    /// </summary>
    /// <param name="msg">The MSG.</param>
    public void InputRecvData(string msg)
    {
        string[] parts = msg.Split('|');
        try
        {
            int i = Convert.ToInt32(parts[0]);
        }
        catch (Exception)
        {
            return;
        }
        queueMutex.WaitOne();
        queue.Enqueue(new Message(parts));
        queueMutex.ReleaseMutex();
    }

    /// <summary>
    /// Messages the recv ready.
    /// </summary>
    /// <returns></returns>
    public bool MessageRecvReady()
    {
        bool queueCount;
        if (queueMutex.WaitOne(500))
        {
            queueCount = queue.Count > 0;
            queueMutex.ReleaseMutex();
            return queueCount;
        }
        return false;
    }

    /// <summary>
    /// Recvs this instance.
    /// </summary>
    /// <returns></returns>
    public Message Recv()
    {
        Message msg;
        queueMutex.WaitOne();
        msg = queue.Dequeue();
        queueMutex.ReleaseMutex();
        return msg;
    }

    /// <summary>
    /// Sends the specified type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="args">The arguments.</param>
    public void Send(int type, params string[] args)
    {
        string msg = type + "|" + string.Join("|", args);
        netClient.Send(msg);
    }

}
