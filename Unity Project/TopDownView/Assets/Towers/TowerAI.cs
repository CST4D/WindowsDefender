using UnityEngine;
using System.Collections;
using System.Reflection;

public class TowerAI : Building {

    public projectileAI projectileSource;
	
	public bool revealsInvisible;

    protected float timer;
    protected float attackSpd;
    protected bool attacksGround;
    protected bool attacksAir;
    public float attackRange;
    protected int towerDamage;
    protected int drainDamage;
    protected float drainSpd;
    protected float drainDuration;

    // Use this for initialization
    public TowerAI () {
        attackSpd = 0.5f;
        attackRange = 1.0f;
        attacksGround = true;
		attacksAir = true;
		revealsInvisible = false;
        towerDamage = 50;
        drainDamage = 0;
        drainSpd = 0;
        drainDuration = 0;
    }
	
	void Update () {
        towerAI();
    }

    /// <summary>
    /// The tower does the following steps:
    /// 1. Check if the target is able to attack ((timer > attackSpd) - Attack Speed)
    /// 2. Check if they are enemies nearby (detectEnemies() - AttackRange)
    /// 3. Shoot the target if applicable (shootTarget(target.gameObject) - Spawn Projectile)
    /// 4. Decrement timer by Attack Speed
    /// </summary>
    protected void towerAI()
    {
        if (operating)
        {
            EnemyAI target = null;
            timer += Time.deltaTime;

            if (timer > attackSpd)
            {
                target = detectEnemies();

                if (target != null)
                    shootTarget(target.gameObject);

                timer -= attackSpd;
            }
        }
    }

    /// <summary>
    /// Creates a projectile object that will fly towards the target
    /// </summary>
    /// <param name="target"></param>
    public virtual void shootTarget(GameObject target)
    {
        projectileAI temp = (projectileAI) projectileAI.Instantiate(projectileSource, transform.position, transform.rotation);
        temp.transform.parent = transform;
        temp.damage = towerDamage;
        temp.drainDamage = drainDamage;
        temp.drainSpd = drainSpd;
        temp.drainDuration = drainDuration;
        temp.target = target;
    }

    /// <summary>
    /// Scans for the nearest enemy within attack range, and returns the closest enemy as the target
    /// </summary>
    /// <returns></returns>
    public virtual EnemyAI detectEnemies()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("MONSTER");
        EnemyAI target = null;

        float distance = attackRange;

        // Get the nearest target and shoot at it
        foreach (GameObject obj in targets)
        {
            EnemyAI enemy = obj.GetComponent<EnemyAI>();

            if (!((attacksGround && enemy.isGround) || (attacksAir && !enemy.isGround)) || !enemy.isVisible)
                continue;

            float targetDist = Vector2.Distance(transform.position, obj.transform.position);

            if (targetDist < distance)
            {
                distance = targetDist;
                target = enemy;
            }
        }

        return target;
    }

	/// <summary>
	/// Upgrades a property of the tower.
	/// </summary>
	/// <param name="propertyName">Name of property of tower.</param>
	/// <param name="newVal">New value of property.</param>
	public void upgradeTower(string propertyName, object newVal)
	{ 
		FieldInfo info = this.GetType().GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);

		if (info != null)
			info.SetValue (this, newVal);
		else
			Debug.Log("Field does not exist.");
	}
    // End of Function
}
