using UnityEngine;
using System.Collections;

public class McAfeeTowerAI : TowerAI
{

    McAfeeTowerAI() : base()
    {
        towerDamage = 50;
        attackSpd = 0.5f;
        attacksGround = true;
        attacksAir = false;
        drainDamage = 20;
        drainSpd = 1.6f;
        drainDuration = 2.0f;
        attackRange = 1.25f;
    }

    public override projectileAI shootTarget(GameObject target)
    {
        projectileAI temp = base.shootTarget(target);

        temp.aoeTarget = target.transform.position;

        return temp;
    }



}
