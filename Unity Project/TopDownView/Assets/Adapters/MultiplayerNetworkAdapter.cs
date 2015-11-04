using UnityEngine;
using System.Collections;
using System;

public class MultiplayerNetworkAdapter {
    public class Message
    {
        public enum MessageType
        {
            GameStart = 1,
            TowerBuilt = 2,
            EnemyDies = 3,
            HealthUpdate = 4,
            SendEnemy = 5
        };
        private string[] args;
        private MessageType type;
        public string[] Args { get { return args; } }
        public MessageType Type { get { return type; } }
        public Message(string[] parts)
        {
            int msgType = Convert.ToInt32(parts[0]);
            type = (MessageType)msgType;
            args = new string[parts.Length - 1];
            for (int i = 1; i < parts.Length; i++)
                args[i - 1] = parts[i];
        }
    }

    public delegate void send_msg_delegate(string msg);

    private send_msg_delegate send;
    private System.Collections.Generic.Queue<Message> queue;

    public MultiplayerNetworkAdapter() : this(null)
    {
        NetworkMock mock = new NetworkMock(this);
        this.send = mock.Send;
    }

    public MultiplayerNetworkAdapter(send_msg_delegate send)
    {
        this.send = send;
        queue = new System.Collections.Generic.Queue<Message>();
    }

    public void RecvData(string msg)
    {
        string[] parts = msg.Split('|');
        queue.Enqueue(new Message(parts));
    }

    public Message ConsumeMessage()
    {
        return queue.Dequeue();
    }

    public void SendMessage(Message.MessageType type, params string[] args)
    {
        string msg = string.Join("|", args);
        send(msg);
    }

}
