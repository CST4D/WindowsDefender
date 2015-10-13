using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public LinkedList<Vector2> movementPoints;

    public float movementSpeed;
    public int drainDamage;
    public float drainSpd;
    public float drainDuration;

    public int health;    
    public bool isVisible;
    public bool isGround;
    public int armour;
    public double resistance;
	
	private SpriteRenderer renderer;
	private TowerAI revealingTower;
	private float revealDist;

	float timer;
    // Use this for initialization
	void Start () {
		renderer = this.GetComponent<SpriteRenderer>();
		revealingTower = null;
		revealDist = 0;

	}
	
	// Update is called once per frame
	void Update () {
        enemyAI();
	}

    /// <summary>
    /// The enemies do the following steps:
    /// 1. Check if there is a place to move
    /// 2. If there is then move to that location
    /// 3. Apply any drain damage
    /// </summary>
    protected void enemyAI()
    {
        if (movementPoints != null && movementPoints.Count > 0)
        {
            Vector2 first = movementPoints.First.Value;

            float dist = Vector2.Distance(transform.position, first);

            if (dist <= 0.1)
                movementPoints.RemoveFirst();
            else
                transform.position = Vector2.MoveTowards(transform.position, first, movementSpeed * Time.deltaTime);
        }

        timer += Time.deltaTime;

        if (timer > drainSpd && drainDuration > 0)
            DrainHealth();

		checkIfRevealed ();

		if (!isVisible) 
			renderer.enabled = false;
		else
			renderer.enabled = true;
    }

	/// <summary>
	/// Check if this enemy is revealed, if this enemy is invisible
	/// </summary>
	private void checkIfRevealed()
	{
		if (revealingTower == null) {
			GameObject[] towers = GameObject.FindGameObjectsWithTag ("TOWER");

			foreach (GameObject obj in towers) {
				revealDist = Vector2.Distance (transform.position, obj.transform.position);
				revealingTower = obj.GetComponent<TowerAI> ();
				
				if (revealDist < revealingTower.attackRange && revealingTower.revealsInvisible) {
					isVisible = true;
					break;
				}
				revealingTower = null;
			}
		} else {
			revealDist = Vector2.Distance (transform.position, revealingTower.gameObject.transform.position);
			if (revealDist > revealingTower.attackRange) {
				revealingTower = null;
				isVisible = false;
			}
		}
	}

	//Method to drain enemy's health
    void DrainHealth()
    {
        health -= drainDamage;
        drainDuration -= drainSpd;
        timer -= drainSpd;
    }
	
    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "PROJECTILE")
        {
            projectileAI projectile = obj.gameObject.GetComponent<projectileAI>();

            if (projectile.target == this.gameObject)
            {
                int damage;

                if((damage = projectile.damage - armour) > 0)
                    health -= damage;

                drainDamage = projectile.drainDamage;
                drainSpd += projectile.drainSpd;
                drainDuration += projectile.drainDuration;

                Destroy(obj.gameObject);
            }

        }
    }
}
