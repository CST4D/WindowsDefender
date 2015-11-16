using UnityEngine;
using System.Collections;

public class AvastTowerAI : TowerAI {
	
AvastTowerAI() : base()
	{
		towerDamage = 50;
		attackSpd = 0.5f;
		attacksGround = true;
		attacksAir = false;
		drainDamage = 20;
		drainSpd = 0.8f;
		drainDuration = 2.0f;
	}
	
	// End of Function
}

