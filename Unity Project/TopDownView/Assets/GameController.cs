using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public Waypoint wayPoint;
    public EnemyAI enemy;

    private ArrayList enemies;
    private float timer;
	// Use this for initialization
	void Start () {
        enemies = new ArrayList();
        timer = 0;   
    }

    // Update is called once per frame
    void Update () {
        // Check health of monsters
        CheckEnemy();

        // Create Monsters
        timer += Time.deltaTime;
        if(timer > 0.5f)
        {
            EnemyAI temp;
            timer -= 0.5f;
            temp = (EnemyAI) GameObject.Instantiate(enemy, transform.position, transform.rotation);
            temp.health = 300;
            temp.next = wayPoint;
            enemies.Add(temp);
        }
	}

    void CheckEnemy()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            EnemyAI temp = (EnemyAI) enemies[i];
            if (temp.health < 0)
            {
                Destroy(temp.gameObject);
                enemies.Remove(temp);
            }
        }
    }
}
