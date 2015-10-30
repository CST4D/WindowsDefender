using UnityEngine;
using System.Collections;

public class CameraAI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    Vector3 KeyboardNavigation(Vector3 position)
    {
        Vector3 direction = position;
        if (Input.GetKey(KeyCode.W))
            direction.y += 2;
        if (Input.GetKey(KeyCode.A))
            direction.x -= 2;
        if (Input.GetKey(KeyCode.S))
            direction.y -= 2;
        if (Input.GetKey(KeyCode.D))
            direction.x += 2;
        return direction;
    }

    Vector3 MouseNavigation(Vector3 position)
    {
        Vector3 direction = position;
        if (Input.mousePosition.y < 30)
            direction.y -= 5;
        if (Input.mousePosition.x < 30)
            direction.x -= 5;
        if (Input.mousePosition.y > (Display.main.renderingHeight - 30))
            direction.y += 5;
        if (Input.mousePosition.x > (Display.main.renderingWidth - 30))
            direction.x += 5;
        return direction;
    }
	
	// Update is called once per frame
	void Update () {
        //Vector3 direction = KeyboardNavigation(transform.position);
        Vector3 direction = MouseNavigation(transform.position);
        transform.position = Vector3.MoveTowards(transform.position, direction, 1.0f * Time.deltaTime);
    }
}
