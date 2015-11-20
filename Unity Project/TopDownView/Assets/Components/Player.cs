using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class Player : MonoBehaviour {
    /// <summary>
    /// The health
    /// </summary>
    public int health;


    // Use this for initialization
    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
        health = 1000;
       
	}

    // Update is called once per frame
    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update () {
        if (Input.GetKey(KeyCode.W))
            gameObject.transform.Translate(Vector2.up * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.S))
            gameObject.transform.Translate(Vector2.down * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.A))
            gameObject.transform.Translate(Vector2.left * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.D))
            gameObject.transform.Translate(Vector2.right * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Called when [trigger enter2 d].
    /// </summary>
    /// <param name="obj">The object.</param>
    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "PROJECTILE")
        {
            projectileAI projectile = obj.gameObject.GetComponent<projectileAI>();

            health -= projectile.damage;
            Destroy(obj.gameObject);

            if (health < 0)
                Destroy(this);
        }
    }
}
