using UnityEngine;
using System.Collections;

public class EmailVirusAI : EnemyAI {

    public int hitCount;
	// Use this for initialization
	void Start () {
        hitCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
        enemyAI();
	}

    public override void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "PROJECTILE")
        {
            projectileAI projectile = obj.gameObject.GetComponent<projectileAI>();

            if (projectile.target == this.gameObject)
            {
                hitCount++;

                if (hitCount == 2)
                    health = 0;


                Destroy(obj.gameObject);
            }

        }
    }
}
