using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public LinkedList<Vector2> movementPoints;

    public float movementSpeed;
    public int health;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (movementPoints != null && movementPoints.Count > 0)
        {
            Vector2 first = movementPoints.First.Value;

            float dist = Vector2.Distance(transform.position, first);

            if (dist <= 0.1)
                movementPoints.RemoveFirst();
            else
                transform.position = Vector2.MoveTowards(transform.position, first, movementSpeed * Time.deltaTime);
        }
	}

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "PROJECTILE")
        {
            projectileAI projectile = obj.gameObject.GetComponent<projectileAI>();

            health -= projectile.damage;
            Destroy(obj.gameObject);
        }
    }
}
