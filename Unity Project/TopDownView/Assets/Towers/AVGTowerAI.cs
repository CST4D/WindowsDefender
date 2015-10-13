using UnityEngine;
using System.Collections;

public class AVGTowerAI : TowerAI {

    // Use this for initialization
    void Start()
    {
        towerDamage = 50;
        attackSpd = 0.5f;
        attacksGround = true;
        attacksAir = true;
        attackRange = 1.0f;
        drainDamage = 20;
        drainSpd = 0.8f;
        drainDuration = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        towerAI();
    }

    public override void shootTarget(GameObject target)
    {
        base.shootTarget(target);
    }

    // End of Function
}
