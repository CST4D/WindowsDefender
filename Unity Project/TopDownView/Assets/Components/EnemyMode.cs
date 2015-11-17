using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMode : MonoBehaviour {

    public UnityEngine.UI.Text resourceText;
    public int flashCount;

    MultiplayerMessagingAdapter messageAdapter;
    int opposingTeam;
    ArrayList[] teamSpawners;
    ArrayList enemies;
    int currentEnemyId;

	// Use this for initialization
	void Start () {
        currentEnemyId = new System.Random().Next() % 1000000000;
	}

    public void Initialize(MultiplayerMessagingAdapter msgAdapter, int opposingTeam, ArrayList[] teamSpawners, ArrayList enemies)
    {
        messageAdapter = msgAdapter;
        this.opposingTeam = opposingTeam;
        this.teamSpawners = teamSpawners;
        this.enemies = enemies;
    }
	
	// Update is called once per frame
	void Update () {
        
	}

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
    /// <param name="tower"></param>
    public void SendEnemy(EnemyAI enemy)
    {
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
