using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MultiplayerMessagingAdapter {
    private enum MessageType
    {
        JoinGame = 1,
        TowerBuilt = 2,
        EnemyDies = 3,
        HealthUpdate = 4,
        SendEnemy = 5,
        ChatMessage = 6,
        KeepAlive = 7,
        JoinAcknowledge = 8
    };

    public enum GameState
    {
        WaitingForPlayers = 1,
        GameInProgress = 2,
        TeamLoss = 3,
        TeamWin = 4,
        PlayerDisconnect = 5
    }
    private GameState currGameState = GameState.WaitingForPlayers;
    public GameState CurrentGameState { get { return currGameState; } }

    private MessagingNetworkAdapter netAdapter;
    private MonoBehaviour context;

    private string hostId;
    private int teamId;
    private string username;
    private Dictionary<string, Peer> peers = new Dictionary<string, Peer>();

    public MultiplayerMessagingAdapter(MessagingNetworkAdapter netAdapter, MonoBehaviour context, string hostId, int teamId)
    {
        this.netAdapter = netAdapter;
        this.context = context;
        this.hostId = hostId;
    }

    public void ReceiveAndUpdate()
    {
        while (netAdapter.MessageRecvReady()) {
            MessagingNetworkAdapter.Message msg = netAdapter.Recv();
            switch ((MessageType)msg.Type)
            {
                case MessageType.TowerBuilt:
                    ReceiveTowerBuilt(msg.Args[0], intr(msg.Args[1]),
                        intr(msg.Args[2]), intr(msg.Args[3]));
                    break;
                case MessageType.EnemyDies:
                    ReceiveEnemyDeath(intr(msg.Args[0]), intr(msg.Args[1]));
                    break;
                case MessageType.JoinGame:
                    ReceiveJoinGame(msg.Args[0], intr(msg.Args[1]), msg.Args[2]);
                    break;
                case MessageType.HealthUpdate:
                    ReceiveHealthUpdate(msg.Args[0], intr(msg.Args[1]), intr(msg.Args[2]));
                    break;
                case MessageType.SendEnemy:
                    ReceiveEnemyAttack(intr(msg.Args[0]), msg.Args[1], intr(msg.Args[2]));
                    break;
                case MessageType.ChatMessage:
                    ReceiveChatMessage(msg.Args[0], msg.Args[1], intr(msg.Args[2]));
                    break;
                case MessageType.KeepAlive:
                    ReceiveKeepAlive(msg.Args[0]);
                    break;
                case MessageType.JoinAcknowledge:
                    ReceiveJoinAcknowledge(msg.Args[0], intr(msg.Args[1]), msg.Args[2]);
                    break;
            }
        }
        CheckAndKeepAlive();
    }

    private void CheckAndKeepAlive()
    {
        float keepAliveTime = Time.time - 2000;
        float expireTime = Time.time - 4000;
        foreach (KeyValuePair<string, Peer> peerpair in peers)
        {
            if (keepAliveTime > peerpair.Value.lastTimeCommunicated)
            {
                if (expireTime > peerpair.Value.lastTimeCommunicated)
                    currGameState = GameState.PlayerDisconnect;
                else
                    SendKeepAlive();
            }
        }
    }

    private void UpdateLastCommunication(string hostId)
    {
        peers[hostId].lastTimeCommunicated = Time.time;
    }

    private void ReceiveJoinGame(string username, int teamId, string hostId)
    {
        peers[hostId] = new Peer(teamId, hostId, username);
        if (peers.Count == 3)
        {
            currGameState = GameState.GameInProgress;
        }
        SendJoinAcknowledge();
    }

    public void SendJoinGame()
    {
        netAdapter.Send((int)MessageType.JoinGame, username, teamId.ToString(), hostId.ToString());
    }

    private void ReceiveTowerBuilt(string prefabName, int teamId, int x, int y)
    {

    }

    public void SendTowerBuilt(string prefabName, int x, int y)
    {
        
    }

    private void ReceiveEnemyDeath(int enemyId, int teamId)
    {

    }

    public void SendEnemyDeath(int enemyId)
    {

    } 

    private void ReceiveHealthUpdate(string hostId, int teamId, int health)
    {

    }

    public void SendHealthUpdate(int health)
    {

    }

    private void ReceiveEnemyAttack(int enemyId, string prefabName, int teamId)
    {

    }

    public void SendEnemyAttack(int enemyId, string prefabName)
    {

    }

    private void ReceiveChatMessage(string username, string content, int teamId)
    {

    }

    public void SendChatMessage(string content)
    {

    }

    private void ReceiveKeepAlive(string hostId)
    {

    }

    public void SendKeepAlive()
    {

    }

    private void ReceiveJoinAcknowledge(string username, int teamId, string hostId)
    {
        if (peers.ContainsKey(hostId))
            return;
        peers[hostId] = new Peer(teamId, hostId, username);
        if (peers.Count == 3)
        {
            currGameState = GameState.GameInProgress;
        }
    }

    public void SendJoinAcknowledge()
    {
        netAdapter.Send((int)MessageType.JoinAcknowledge, username, teamId.ToString(), hostId.ToString());
    }

    private int intr(string value)
    {
        return Convert.ToInt32(value);
    }

    private class Peer
    {
        public Peer(int teamId, string hostId, string username)
        {
            lastTimeCommunicated = Time.time;
            this.teamId = teamId;
            this.hostId = hostId;
            this.username = username;
        }
        public float lastTimeCommunicated;
        public int teamId;
        public string hostId;
        public string username;
    }
}
