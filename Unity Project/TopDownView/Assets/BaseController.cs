using UnityEngine;
using System.Collections;

/// <summary>
/// @Author Joel
/// </summary>
public class BaseController : MonoBehaviour {

    private float baseMaxHealth;
    private float baseCurrentHealth;

	// Use this for initialization
	void Start () {
        baseMaxHealth = 100;
        baseCurrentHealth = baseMaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetHealth(int offset)
    {
        baseCurrentHealth = baseCurrentHealth - (offset);
        float curr = baseCurrentHealth / baseMaxHealth;
        if (curr > 0)
        {
            transform.localScale = new Vector3(curr, 1, 1);
        }
        if (curr < .99 && curr > 0.5)
        {
            gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
        }
        else if (curr < 0.5)
        {
            gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.red;
        }
    }
}
