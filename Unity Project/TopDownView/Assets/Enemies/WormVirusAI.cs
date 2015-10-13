using UnityEngine;
using System.Collections;

public class WormVirusAI : EnemyAI {

	// Use this for initialization
	void Start () {
        duplicates = true;
	}
	
	// Update is called once per frame
	void Update () {
        enemyAI();
	}

    
}
