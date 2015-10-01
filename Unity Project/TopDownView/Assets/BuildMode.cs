using UnityEngine;
using System.Collections;

public class BuildMode : MonoBehaviour {

    private bool buildMode;

    Building building;

	// Use this for initialization
	void Start () {
        buildMode = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (buildMode && building != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z -= Camera.main.transform.position.z;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            building.transform.position = Vector2.MoveTowards(building.transform.position, mousePos, 10.0f * Time.deltaTime);

            if (Input.GetMouseButtonDown(0))
            {
                building.enabled = true;
                buildMode = false;
                building = null;
            }
        }
	}

    public void BuildTower(Building tower)
    {
        if (!buildMode)
        {
            buildMode = true;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z -= Camera.main.transform.position.z;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            building = (Building) Instantiate(tower, mousePos, transform.rotation);
            building.enabled = false;
        }
    }
}
