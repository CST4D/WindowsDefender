using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public LinkedList<Vector2> movementPoints;

    public float movementSpeed;
    public int drainDamage;
    public float drainSpd;
    public float drainDuration;
    public AudioClip hitSound;

    public int health;    
    public bool isVisible;
    public bool isGround;
    public int armour;
    public double resistance;

    float timer;
    private AudioSource aSource;
    // Use this for initialization
    void Start () {
        aSource = GetComponent<AudioSource>();
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

        while (timer > drainSpd && drainDuration > 0)
            DrainHealth();
    }

    //Method to drain enemy's health
    void DrainHealth()
    {
        health -= drainDamage;
        drainDuration -= drainSpd;
        timer -= drainSpd;

        if(drainDuration < 0)
        {
            drainDuration = 0;
            drainDamage = 0;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "PROJECTILE")
        {
            projectileAI projectile = obj.gameObject.GetComponent<projectileAI>();

            if (projectile.target == this.gameObject)
            {
                int damage;

                if((damage = projectile.damage - armour) > 0)
                    health -= damage;

                aSource.PlayOneShot(hitSound, 0.9f);

                if (drainDuration <= 0)
                {
                    drainDamage = projectile.drainDamage;
                    drainSpd = projectile.drainSpd;
                    drainDuration += projectile.drainDuration;
                }

                OnDeath();

                Destroy(obj.gameObject);
            }

        }
    }

    protected virtual void OnDeath()
    {

    }
}
