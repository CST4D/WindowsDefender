using UnityEngine;
using System.Collections;
using System;

public class MessagingNetworkAdapter {
    public class Message
    {
        
        private string[] args;
        private int type;
        public string[] Args { get { return args; } }
        public int Type { get { return type; } }
        public Message(string[] parts)
        {
            type = Convert.ToInt32(parts[0]);
            args = new string[parts.Length - 1];
            for (int i = 1; i < parts.Length; i++)
                args[i - 1] = parts[i];
        }
    }

    private NetworkClient netClient;
    private System.Collections.Generic.Queue<Message> queue;
    private System.Threading.Mutex queueMutex;

    public MessagingNetworkAdapter() : this(null)
    {
        
    }

    public MessagingNetworkAdapter(NetworkClient netClient)
    {
        this.netClient = netClient;
        queue = new System.Collections.Generic.Queue<Message>();
        queueMutex = new System.Threading.Mutex();
    }

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

    public Message Recv()
    {
        Message msg;
        queueMutex.WaitOne();
        msg = queue.Dequeue();
        queueMutex.ReleaseMutex();
        return msg;
    }

    public void Send(int type, params string[] args)
    {
        string msg = type + "|" + string.Join("|", args);
        netClient.Send(msg);
    }

}
