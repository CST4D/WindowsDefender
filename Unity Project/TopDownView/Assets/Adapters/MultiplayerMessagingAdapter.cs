using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class MultiplayerMessagingAdapter {
    public enum MessageType
    {
        JoinGame = 1,
        TowerBuilt = 2,
        EnemyDies = 3,
        HealthUpdate = 4,
        SendEnemy = 5,
        ChatMessage = 6,
        KeepAlive = 7,
        JoinAcknowledge = 8,
        Connected = 9,
        Disconnected = 10,
    };

    public enum GameState
    {
        Connecting = 1,
        WaitingForPlayers = 2,
        GameInProgress = 3,
        TeamLoss = 4,
        TeamWin = 5,
        PlayerDisconnect = 6
    }
    private GameState currGameState = GameState.Connecting;
    public GameState CurrentGameState { get { return currGameState; } }

    private MessagingNetworkAdapter netAdapter;
    private MonoBehaviour context;

    private int teamId;
    private string username;
    private Dictionary<string, Peer> peers = new Dictionary<string, Peer>();
    private Dictionary<int, EnemyAI> enemies = new Dictionary<int, EnemyAI>();
    private ArrayList[] teamSpawners;
    private ArrayList gcEnemies;
    private float lastTimeCommunication = Time.time;
    private Chat chatInstance;

    public MultiplayerMessagingAdapter(MessagingNetworkAdapter netAdapter, MonoBehaviour context, string username, int teamId, ArrayList[] teamSpawners, ArrayList enemies, Chat chat)
    {
        this.netAdapter = netAdapter;
        this.context = context;
        this.teamSpawners = teamSpawners;
        gcEnemies = enemies;
        this.username = username;
        this.teamId = teamId;
        this.chatInstance = chat;
    }

    public void ReceiveAndUpdate()
    {
        while (netAdapter.MessageRecvReady()) {
            MessagingNetworkAdapter.Message msg = netAdapter.Recv();
            switch ((MessageType)msg.Type)
            {
                case MessageType.TowerBuilt:
                    ReceiveTowerBuilt(msg.Args[0], intr(msg.Args[1]),
                        Convert.ToDouble(msg.Args[2]), Convert.ToDouble(msg.Args[3]), msg.Args[4]);
                    break;
                case MessageType.EnemyDies:
                    ReceiveEnemyDeath(intr(msg.Args[0]), intr(msg.Args[1]), msg.Args[2]);
                    break;
                case MessageType.JoinGame:
                    ReceiveJoinGame(msg.Args[0], intr(msg.Args[1]));
                    break;
                case MessageType.HealthUpdate:
                    ReceiveHealthUpdate(msg.Args[0], intr(msg.Args[1]), intr(msg.Args[2]));
                    break;
                case MessageType.SendEnemy:
                    ReceiveEnemyAttack(intr(msg.Args[0]), msg.Args[1], intr(msg.Args[2]), intr(msg.Args[3]), msg.Args[4]);
                    break;
                case MessageType.ChatMessage:
                    ReceiveChatMessage(msg.Args[0], msg.Args[1], intr(msg.Args[2]));
                    break;
                case MessageType.KeepAlive:
                    ReceiveKeepAlive(msg.Args[0]);
                    break;
                case MessageType.JoinAcknowledge:
                    ReceiveJoinAcknowledge(msg.Args[0], intr(msg.Args[1]));
                    break;
                case MessageType.Connected:
                    ReceiveConnected();
                    break;
                case MessageType.Disconnected:
                    ReceiveDisconnected();
                    break;
            }
        }
        if (currGameState <= GameState.GameInProgress)
            CheckAndKeepAlive();
    }

    private void CheckAndKeepAlive()
    {
        float keepAliveTime = Time.time - 1.5f;
        float expireTime = Time.time - 4.0f;
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
        if (Time.time - lastTimeCommunication >= 1.0f)
        {
            SendKeepAlive();
        }
    }

    private void UpdateLastCommunication(string username)
    {
        peers[username].lastTimeCommunicated = Time.time;
    }

    private void UpdateLastCommunicationWithServer()
    {
        lastTimeCommunication = Time.time;
    }

    private void ReceiveConnected()
    {
        currGameState = GameState.WaitingForPlayers;
        SendJoinGame();
    }

    private void ReceiveDisconnected()
    {
        if (currGameState <= GameState.GameInProgress)
            currGameState = GameState.PlayerDisconnect;
    }

    private void ReceiveJoinGame(string username, int teamId)
    {
        if (username == this.username)
            return;
        peers[username] = new Peer(teamId, username);
        if (peers.Count == 1)
        {
            currGameState = GameState.GameInProgress;
        }
        SendJoinAcknowledge();
        
    }

    public void SendJoinGame()
    {
        netAdapter.Send((int)MessageType.JoinGame, username, teamId.ToString());
        UpdateLastCommunicationWithServer();
    }

    private void ReceiveTowerBuilt(string prefabName, int teamId, double x, double y, string username)
    {
        if (username == this.username)
            return;
        Tile[] tiles = UnityEngine.Object.FindObjectsOfType<Tile>();
        Tile closestTile = null;
        float closestDist = 1.0f;
        Vector2 pos = new Vector2((float)x, (float)y);
        foreach (Tile tile in tiles)
        {
            float dist = Vector2.Distance(tile.transform.position, pos);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestTile = tile;
            }
        }
        closestTile.Buildable = false;
        closestTile.Walkable = false;

        GameObject building = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Towers/" + prefabName), pos, context.transform.rotation);
        building.GetComponent<Building>().transform.parent = context.transform.Find("Towers").transform;
        building.GetComponent<Building>().operating = true;

        UpdateLastCommunication(username);
    }

    public void SendTowerBuilt(string prefabName, double x, double y)
    {
        netAdapter.Send((int)MessageType.TowerBuilt, prefabName, teamId.ToString(), x.ToString(), y.ToString(), username);
        UpdateLastCommunicationWithServer();
    }

    private void ReceiveEnemyDeath(int enemyId, int teamId, string username)
    {
        if (enemies.ContainsKey(enemyId))
        {
            enemies[enemyId].health = 0;
            enemies.Remove(enemyId);
        }
        UpdateLastCommunication(username);
    }

    public void SendEnemyDeath(int enemyId)
    {
        netAdapter.Send((int)MessageType.EnemyDies, enemyId.ToString(), teamId.ToString(), username);
        UpdateLastCommunicationWithServer();
    } 

    private void ReceiveHealthUpdate(string username, int teamId, int health)
    {
        if (health <= 0 && currGameState == GameState.GameInProgress)
        {
            if (teamId == this.teamId)
            {
                currGameState = GameState.TeamLoss;
            } else
            {
                currGameState = GameState.TeamWin;
            }
        }
        if (username != this.username)
            UpdateLastCommunication(username);
    }

    public void SendHealthUpdate(int health)
    {
        netAdapter.Send((int)MessageType.HealthUpdate, username, teamId.ToString(), health.ToString());
        UpdateLastCommunicationWithServer();
    }

    private void ReceiveEnemyAttack(int enemyId, string prefabName, int teamId, int spawnerId, string username)
    {
        if (enemies.ContainsKey(enemyId))
            return;
        GameObject temp;
        SpawnerAI spai = ((SpawnerAI)teamSpawners[teamId - 1][spawnerId]);
        GameObject prefab = (GameObject)Resources.Load("Enemies/" + prefabName);
        temp = (GameObject)UnityEngine.Object.Instantiate(prefab, spai.transform.position, context.transform.rotation);
        temp.transform.parent = context.transform.Find("Enemies").transform;
        temp.GetComponent<EnemyAI>().enemyId = enemyId;
        LinkedList<Vector2> copyWaypoints = new LinkedList<Vector2>();

        if (temp.GetComponent<EnemyAI>().isGround)
            foreach (Vector2 v in spai.wayPoints)
                copyWaypoints.AddLast(v);
        else
            foreach (Vector2 v in spai.flyPoints)
                copyWaypoints.AddLast(v);

        temp.GetComponent<EnemyAI>().movementPoints = copyWaypoints;
        temp.GetComponent<EnemyAI>().targetWaypoint = spai.targetWaypoint;
        gcEnemies.Add(temp.GetComponent<EnemyAI>());
        enemies.Add(enemyId, temp.GetComponent<EnemyAI>());

        UpdateLastCommunication(username);
    }

    public void SendEnemyAttack(int enemyId, string prefabName, int teamId, int spawnerId, EnemyAI enemy)
    {
        enemies.Add(enemyId, enemy);
        netAdapter.Send((int)MessageType.SendEnemy, enemyId.ToString(), prefabName, teamId.ToString(), spawnerId.ToString(), username);
        UpdateLastCommunicationWithServer();
    }

    private void ReceiveChatMessage(string username, string content, int teamId)
    {
        chatInstance.ReceiveChatMessage(username, content);
        if (username != this.username)
            UpdateLastCommunication(username);
    }

    public void SendChatMessage(string content)
    {
        netAdapter.Send((int)MessageType.ChatMessage, username, content, teamId.ToString());
        UpdateLastCommunicationWithServer();
    }

    private void ReceiveKeepAlive(string username)
    {
        if (username == this.username)
            return;
        peers[username].lastTimeCommunicated = Time.time;
    }

    public void SendKeepAlive()
    {
        netAdapter.Send((int)MessageType.KeepAlive, username);
        UpdateLastCommunicationWithServer();
    }

    private void ReceiveJoinAcknowledge(string username, int teamId)
    {
        if (username == this.username)
            return;
        if (peers.ContainsKey(username))
            return;
        peers[username] = new Peer(teamId, username);
        if (peers.Count == 1)
        {
            currGameState = GameState.GameInProgress;
        }
    }

    public void SendJoinAcknowledge()
    {
        netAdapter.Send((int)MessageType.JoinAcknowledge, username, teamId.ToString());
        UpdateLastCommunicationWithServer();
    }

    private int intr(string value)
    {
        return Convert.ToInt32(value);
    }

    private class Peer
    {
        public Peer(int teamId, string username)
        {
            lastTimeCommunicated = Time.time;
            this.teamId = teamId;
            this.username = username;
        }
        public float lastTimeCommunicated;
        public int teamId;
        public string username;
        public int health = 100;
    }
}
