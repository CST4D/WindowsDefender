using UnityEngine;
using System.Collections;

public class TowerAI : Building {

    public projectileAI projectileSource;
    float timer;
    float attackSpd;
    private projectileAI projectile;

	// Use this for initialization
	void Start () {
        attackSpd = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        if (operating)
        {

            GameObject[] targets = GameObject.FindGameObjectsWithTag("MONSTER");
            GameObject target = null;
            float distance = 2.0f;
            timer += Time.deltaTime;

            if (timer > attackSpd)
            {
                // Get the nearest target and shoot at it
                foreach (GameObject obj in targets)
                {
                    float targetDist = Vector2.Distance(transform.position, obj.transform.position);

                    if (targetDist < distance)
                    {
                        distance = targetDist;
                        target = obj;
                    }
                }

                // Shoot target
                if (target != null)
                {
                    projectile = (projectileAI)projectileAI.Instantiate(projectileSource, transform.position, transform.rotation);
                    projectile.damage = 50;

                    projectile.target = target;
                }

                timer -= attackSpd;
            }
        }
    }
}
