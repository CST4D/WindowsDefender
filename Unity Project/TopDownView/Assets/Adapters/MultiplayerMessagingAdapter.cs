using UnityEngine;
using System.Collections;
using System;

public class MultiplayerMessagingAdapter {
    private enum MessageType
    {
        JoinGame = 1,
        TowerBuilt = 2,
        EnemyDies = 3,
        HealthUpdate = 4,
        SendEnemy = 5,
        ChatMessage = 6
    };
    private MessagingNetworkAdapter netAdapter;
    private MonoBehaviour context;

    public MultiplayerMessagingAdapter(MessagingNetworkAdapter nadapter, GameController context)
    {
        this.netAdapter = nadapter;
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
                    ReceiveJoinGame(msg.Args[0], intr(msg.Args[1]), intr(msg.Args[2]));
                    break;
                case MessageType.HealthUpdate:
                    ReceiveHealthUpdate(intr(msg.Args[0]), intr(msg.Args[1]), intr(msg.Args[2]));
                    break;
                case MessageType.SendEnemy:
                    ReceiveEnemyAttack(intr(msg.Args[0]), msg.Args[1], intr(msg.Args[2]));
                    break;
                case MessageType.ChatMessage:
                    ReceiveChatMessage(msg.Args[0], msg.Args[1], intr(msg.Args[2]));
                    break;
            }
        }
    }

    private void ReceiveJoinGame(string username, int teamId, int userId)
    {

    }

    public void SendJoinGame(string username, int teamId, int generatedUserId)
    {

    }

    private void ReceiveTowerBuilt(string prefabName, int teamId, int x, int y)
    {

    }

    public void SendTowerBuilt(string prefabName, int teamId, int x, int y)
    {
        
    }

    private void ReceiveEnemyDeath(int enemyId, int teamId)
    {

    }

    public void SendEnemyDeath(int enemyId, int teamId)
    {

    } 

    private void ReceiveHealthUpdate(int userId, int teamId, int health)
    {

    }

    public void SendHealthUpdate(int userId, int teamId, int health)
    {

    }

    private void ReceiveEnemyAttack(int enemyId, string prefabName, int teamId)
    {

    }

    public void SendEnemyAttack(int enemyId, string prefabName, int teamId)
    {

    }

    private void ReceiveChatMessage(string username, string content, int teamId)
    {

    }

    public void SendChatMessage(string username, string content, int teamId)
    {

    }

    private int intr(string value)
    {
        return Convert.ToInt32(value);
    }
}
