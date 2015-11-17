using UnityEngine;

public class BaseHealth : MonoBehaviour {
    private float BaseMaxHealth;
    private float BaseCurrentHealth;


	// Use this for initialization
	void Start () {
        BaseMaxHealth = 100;
        BaseCurrentHealth = BaseMaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	}

    void SetHealth(int offset)
    {
        BaseCurrentHealth = BaseCurrentHealth - (offset);
        float curr = BaseCurrentHealth / BaseMaxHealth;
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
