using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    public float speed;
    public Vector3 MoveLocation { get; set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector2.Distance(transform.position, MoveLocation) != 0)
            transform.position = Vector2.MoveTowards(transform.position, MoveLocation, speed * Time.deltaTime); 
	}
}
