using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class MultiplayerMessagingAdapter {
    /// <summary>
    /// 
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// The join game
        /// </summary>
        JoinGame = 1,
        /// <summary>
        /// The tower built
        /// </summary>
        TowerBuilt = 2,
        /// <summary>
        /// The enemy dies
        /// </summary>
        EnemyDies = 3,
        /// <summary>
        /// The health update
        /// </summary>
        HealthUpdate = 4,
        /// <summary>
        /// The send enemy
        /// </summary>
        SendEnemy = 5,
        /// <summary>
        /// The chat message
        /// </summary>
        ChatMessage = 6,
        /// <summary>
        /// The keep alive
        /// </summary>
        KeepAlive = 7,
        /// <summary>
        /// The join acknowledge
        /// </summary>
        JoinAcknowledge = 8,
        /// <summary>
        /// The connected
        /// </summary>
        Connected = 9,
        /// <summary>
        /// The disconnected
        /// </summary>
        Disconnected = 10,
    };

    /// <summary>
    /// 
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// The connecting
        /// </summary>
        Connecting = 1,
        /// <summary>
        /// The waiting for players
        /// </summary>
        WaitingForPlayers = 2,
        /// <summary>
        /// The game in progress
        /// </summary>
        GameInProgress = 3,
        /// <summary>
        /// The team loss
        /// </summary>
        TeamLoss = 4,
        /// <summary>
        /// The team win
        /// </summary>
        TeamWin = 5,
        /// <summary>
        /// The player disconnect
        /// </summary>
        PlayerDisconnect = 6
    }
    /// <summary>
    /// The curr game state
    /// </summary>
    private GameState currGameState = GameState.Connecting;
    /// <summary>
    /// Gets the state of the current game.
    /// </summary>
    /// <value>
    /// The state of the current game.
    /// </value>
    public GameState CurrentGameState { get { return currGameState; } }

    /// <summary>
    /// The net adapter
    /// </summary>
    private MessagingNetworkAdapter netAdapter;
    /// <summary>
    /// The context
    /// </summary>
    private MonoBehaviour context;

    /// <summary>
    /// The team identifier
    /// </summary>
    private int teamId;
    /// <summary>
    /// The username
    /// </summary>
    private string username;
    /// <summary>
    /// The peers
    /// </summary>
    private Dictionary<string, Peer> peers = new Dictionary<string, Peer>();
    /// <summary>
    /// The enemies
    /// </summary>
    private Dictionary<int, EnemyAI> enemies = new Dictionary<int, EnemyAI>();
    /// <summary>
    /// The team spawners
    /// </summary>
    private ArrayList[] teamSpawners;
    /// <summary>
    /// The gc enemies
    /// </summary>
    private ArrayList gcEnemies;
    /// <summary>
    /// The last time communication
    /// </summary>
    private float lastTimeCommunication = Time.time;
    /// <summary>
    /// The chat instance
    /// </summary>
    private Chat chatInstance;

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiplayerMessagingAdapter"/> class.
    /// </summary>
    /// <param name="netAdapter">The net adapter.</param>
    /// <param name="context">The context.</param>
    /// <param name="username">The username.</param>
    /// <param name="teamId">The team identifier.</param>
    /// <param name="teamSpawners">The team spawners.</param>
    /// <param name="enemies">The enemies.</param>
    /// <param name="chat">The chat.</param>
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

    /// <summary>
    /// Receives the and update.
    /// </summary>
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

    /// <summary>
    /// Checks the and keep alive.
    /// </summary>
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

    /// <summary>
    /// Updates the last communication.
    /// </summary>
    /// <param name="username">The username.</param>
    private void UpdateLastCommunication(string username)
    {
        peers[username].lastTimeCommunicated = Time.time;
    }

    /// <summary>
    /// Updates the last communication with server.
    /// </summary>
    private void UpdateLastCommunicationWithServer()
    {
        lastTimeCommunication = Time.time;
    }

    /// <summary>
    /// Receives the connected.
    /// </summary>
    private void ReceiveConnected()
    {
        currGameState = GameState.WaitingForPlayers;
        SendJoinGame();
    }

    /// <summary>
    /// Receives the disconnected.
    /// </summary>
    private void ReceiveDisconnected()
    {
        if (currGameState <= GameState.GameInProgress)
            currGameState = GameState.PlayerDisconnect;
    }

    /// <summary>
    /// Receives the join game.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="teamId">The team identifier.</param>
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

    /// <summary>
    /// Sends the join game.
    /// </summary>
    public void SendJoinGame()
    {
        netAdapter.Send((int)MessageType.JoinGame, username, teamId.ToString());
        UpdateLastCommunicationWithServer();
    }

    /// <summary>
    /// Receives the tower built.
    /// </summary>
    /// <param name="prefabName">Name of the prefab.</param>
    /// <param name="teamId">The team identifier.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="username">The username.</param>
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

    /// <summary>
    /// Sends the tower built.
    /// </summary>
    /// <param name="prefabName">Name of the prefab.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    public void SendTowerBuilt(string prefabName, double x, double y)
    {
        netAdapter.Send((int)MessageType.TowerBuilt, prefabName, teamId.ToString(), x.ToString(), y.ToString(), username);
        UpdateLastCommunicationWithServer();
    }

    /// <summary>
    /// Receives the enemy death.
    /// </summary>
    /// <param name="enemyId">The enemy identifier.</param>
    /// <param name="teamId">The team identifier.</param>
    /// <param name="username">The username.</param>
    private void ReceiveEnemyDeath(int enemyId, int teamId, string username)
    {
        if (enemies.ContainsKey(enemyId))
        {
            enemies[enemyId].health = 0;
            enemies.Remove(enemyId);
        }
        UpdateLastCommunication(username);
    }

    /// <summary>
    /// Sends the enemy death.
    /// </summary>
    /// <param name="enemyId">The enemy identifier.</param>
    public void SendEnemyDeath(int enemyId)
    {
        netAdapter.Send((int)MessageType.EnemyDies, enemyId.ToString(), teamId.ToString(), username);
        UpdateLastCommunicationWithServer();
    }

    /// <summary>
    /// Receives the health update.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="teamId">The team identifier.</param>
    /// <param name="health">The health.</param>
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

    /// <summary>
    /// Sends the health update.
    /// </summary>
    /// <param name="health">The health.</param>
    public void SendHealthUpdate(int health)
    {
        netAdapter.Send((int)MessageType.HealthUpdate, username, teamId.ToString(), health.ToString());
        UpdateLastCommunicationWithServer();
    }

    /// <summary>
    /// Receives the enemy attack.
    /// </summary>
    /// <param name="enemyId">The enemy identifier.</param>
    /// <param name="prefabName">Name of the prefab.</param>
    /// <param name="teamId">The team identifier.</param>
    /// <param name="spawnerId">The spawner identifier.</param>
    /// <param name="username">The username.</param>
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

    /// <summary>
    /// Sends the enemy attack.
    /// </summary>
    /// <param name="enemyId">The enemy identifier.</param>
    /// <param name="prefabName">Name of the prefab.</param>
    /// <param name="teamId">The team identifier.</param>
    /// <param name="spawnerId">The spawner identifier.</param>
    /// <param name="enemy">The enemy.</param>
    public void SendEnemyAttack(int enemyId, string prefabName, int teamId, int spawnerId, EnemyAI enemy)
    {
        enemies.Add(enemyId, enemy);
        netAdapter.Send((int)MessageType.SendEnemy, enemyId.ToString(), prefabName, teamId.ToString(), spawnerId.ToString(), username);
        UpdateLastCommunicationWithServer();
    }

    /// <summary>
    /// Receives the chat message.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="content">The content.</param>
    /// <param name="teamId">The team identifier.</param>
    private void ReceiveChatMessage(string username, string content, int teamId)
    {
        chatInstance.ReceiveChatMessage(username, content);
        if (username != this.username)
            UpdateLastCommunication(username);
    }

    /// <summary>
    /// Sends the chat message.
    /// </summary>
    /// <param name="content">The content.</param>
    public void SendChatMessage(string content)
    {
        netAdapter.Send((int)MessageType.ChatMessage, username, content, teamId.ToString());
        UpdateLastCommunicationWithServer();
    }

    /// <summary>
    /// Receives the keep alive.
    /// </summary>
    /// <param name="username">The username.</param>
    private void ReceiveKeepAlive(string username)
    {
        if (username == this.username)
            return;
        peers[username].lastTimeCommunicated = Time.time;
    }

    /// <summary>
    /// Sends the keep alive.
    /// </summary>
    public void SendKeepAlive()
    {
        netAdapter.Send((int)MessageType.KeepAlive, username);
        UpdateLastCommunicationWithServer();
    }

    /// <summary>
    /// Receives the join acknowledge.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="teamId">The team identifier.</param>
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

    /// <summary>
    /// Sends the join acknowledge.
    /// </summary>
    public void SendJoinAcknowledge()
    {
        netAdapter.Send((int)MessageType.JoinAcknowledge, username, teamId.ToString());
        UpdateLastCommunicationWithServer();
    }

    /// <summary>
    /// Intrs the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    private int intr(string value)
    {
        return Convert.ToInt32(value);
    }

    /// <summary>
    /// 
    /// </summary>
    private class Peer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Peer"/> class.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="username">The username.</param>
        public Peer(int teamId, string username)
        {
            lastTimeCommunicated = Time.time;
            this.teamId = teamId;
            this.username = username;
        }
        /// <summary>
        /// The last time communicated
        /// </summary>
        public float lastTimeCommunicated;
        /// <summary>
        /// The team identifier
        /// </summary>
        public int teamId;
        /// <summary>
        /// The username
        /// </summary>
        public string username;
        /// <summary>
        /// The health
        /// </summary>
        public int health = 100;
    }
}
