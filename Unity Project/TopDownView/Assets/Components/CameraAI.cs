using UnityEngine;
using System.Collections;

public class CameraAI : MonoBehaviour {

    private float mapWidth;
    private float mapHeight;

	// Use this for initialization
	void Start () {
	    
	}

    public void SetMapSize(float width, float height)
    {
        mapWidth = width;
        mapHeight = height;
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
        if (Input.GetKey(KeyCode.X))
            direction.z += 2;
        if (Input.GetKey(KeyCode.Z))
            direction.z -= 2;
        return direction;
    }

    Vector3 MouseNavigation(Vector3 position)
    {
        Vector3 direction = position;
        if (Input.mousePosition.y < 30 && direction.y > -0.01f)
            direction.y -= 20;
        if (Input.mousePosition.x < 30 && direction.x > -0.01f)
            direction.x -= 20;
        if (Input.mousePosition.y > (Screen.height - 30) && direction.y < mapHeight + 0.01f)
            direction.y += 20;
        if (Input.mousePosition.x > (Screen.width - 30) && direction.x < mapWidth + 0.01f)
            direction.x += 20;
        if (Input.GetAxis("Mouse ScrollWheel") > 0.01f && direction.z < -0.5f)
            direction.z += Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetAxis("Mouse ScrollWheel") < -0.01f)
            direction.z += Input.GetAxis("Mouse ScrollWheel");
        return direction;
    }
	
	// Update is called once per frame
	void Update () {
        //Vector3 direction = KeyboardNavigation(transform.position);
        Vector3 direction = MouseNavigation(transform.position);
        transform.position = Vector3.MoveTowards(transform.position, direction, 1.5f * Time.deltaTime);
    }
}
