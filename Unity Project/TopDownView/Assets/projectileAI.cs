using UnityEngine;
using System.Collections;

public class projectileAI : MonoBehaviour {
    public GameObject target;

    public int damage;
	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 2 * Time.deltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
