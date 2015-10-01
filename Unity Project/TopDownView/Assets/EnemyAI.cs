using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public Waypoint next;
    public int health;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (this.next != null)
        {
            float distance = Vector2.Distance(transform.position, this.next.transform.position);

            if (distance > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, this.next.transform.position, 1 * Time.deltaTime);
            }
            else
            {
                if (next.next != null)
                    next = next.next;
            }
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
