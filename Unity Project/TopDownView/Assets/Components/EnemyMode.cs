using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class EnemyMode : MonoBehaviour {
    
    public bool ReadyToSend { get; set; }
    /// <summary>
    /// The resource text
    /// </summary>
    public UnityEngine.UI.Text resourceText;
    /// <summary>
    /// The flash count
    /// </summary>
    public int flashCount;

    /// <summary>
    /// The message adapter
    /// </summary>
    MultiplayerMessagingAdapter messageAdapter;
    /// <summary>
    /// The opposing team
    /// </summary>
    int opposingTeam;
    /// <summary>
    /// The team spawners
    /// </summary>
    ArrayList[] teamSpawners;
    /// <summary>
    /// The enemies
    /// </summary>
    ArrayList enemies;
    /// <summary>
    /// The current enemy identifier
    /// </summary>
    int currentEnemyId;

    // Use this for initialization
    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        ReadyToSend = false;
        currentEnemyId = new System.Random().Next() % 1000000000;
	}

    /// <summary>
    /// Initializes the specified MSG adapter.
    /// </summary>
    /// <param name="msgAdapter">The MSG adapter.</param>
    /// <param name="opposingTeam">The opposing team.</param>
    /// <param name="teamSpawners">The team spawners.</param>
    /// <param name="enemies">The enemies.</param>
    public void Initialize(MultiplayerMessagingAdapter msgAdapter, int opposingTeam, ArrayList[] teamSpawners, ArrayList enemies)
    {
        messageAdapter = msgAdapter;
        this.opposingTeam = opposingTeam;
        this.teamSpawners = teamSpawners;
        this.enemies = enemies;
    }

    // Update is called once per frame
    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update () {
        
	}

    /// <summary>
    /// Flashes this instance.
    /// </summary>
    public void flash()
    {
        flashCount--;
        if (resourceText.color == UnityEngine.Color.white)
        {
            resourceText.color = UnityEngine.Color.red;
        }
        else
        {
            resourceText.color = UnityEngine.Color.white;
        }
        if (flashCount < 1) CancelInvoke("flash");
    }

    /// <summary>
    /// Build Tower Function which allows the player to build the specified tower
    /// </summary>
    /// <param name="enemy">The enemy.</param>
    public void SendEnemy(EnemyAI enemy)
    {
        if (!ReadyToSend)
            return;
        int money = int.Parse(resourceText.text);
        if (!((money - enemy.cost) >= 0))
        {
            flashCount = 6;
            InvokeRepeating("flash", 0, 0.15f);
            return;
        }
        int spawnerId = new System.Random().Next() % teamSpawners[opposingTeam - 1].Count;
        SpawnerAI spai = ((SpawnerAI)teamSpawners[opposingTeam - 1][spawnerId]);
        EnemyAI temp = (EnemyAI)GameObject.Instantiate(enemy, spai.transform.position, transform.rotation);
        temp.transform.parent = transform.Find("Enemies").transform;
        LinkedList<Vector2> copyWaypoints = new LinkedList<Vector2>();

        if (temp.isGround)
            foreach (Vector2 v in spai.wayPoints)
                copyWaypoints.AddLast(v);
        else
            foreach (Vector2 v in spai.flyPoints)
                copyWaypoints.AddLast(v);

        temp.movementPoints = copyWaypoints;
        temp.targetWaypoint = spai.targetWaypoint;
        temp.enemyId = currentEnemyId;
        enemies.Add(temp);
        messageAdapter.SendEnemyAttack(currentEnemyId++, enemy.name, opposingTeam, spawnerId, temp);
        money -= temp.cost;
        resourceText.text = money.ToString();
    }
}
