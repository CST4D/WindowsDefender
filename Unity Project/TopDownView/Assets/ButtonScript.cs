using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    public float speed;
    public Vector3 MoveLocation { get; set; }

    public Building Tower { get; set; }
    public BuildMode GameController { get; set; }
    public Texture2D backgroundTexture;

    private string toolTipText = null;
    private GUIStyle guiStyleFore;
    private GUIStyle guiStyleBack;
    

    // Use this for initialization
    void Start () {
        guiStyleFore = new GUIStyle();
        guiStyleFore.normal.textColor = Color.white;
        guiStyleFore.wordWrap = true;
        guiStyleFore.alignment = TextAnchor.MiddleCenter;
        guiStyleBack = new GUIStyle();
        guiStyleBack.normal.textColor = Color.black;
        guiStyleBack.alignment = TextAnchor.MiddleCenter;
        guiStyleBack.normal.background = backgroundTexture;
        guiStyleBack.wordWrap = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector2.Distance(transform.position, MoveLocation) != 0)
            transform.position = Vector2.MoveTowards(transform.position, MoveLocation, speed * Time.deltaTime); 
	}

    /// <summary>
    /// When the button is clicked, trigger the Game Controller's Build Mode Function
    /// </summary>
    public void OnClick()
    {
        GameController.BuildTower(Tower);
    }

    public void OnMouseEnter()
    {
        toolTipText = Tower.GetComponent<TowerAI>().toolTip();
    }

    public void OnMouseExit()
    {
        toolTipText = null;
    }

    public void OnGUI()
    {
        if(toolTipText != null)
        {
            var x = Event.current.mousePosition.x;
            var y = Event.current.mousePosition.y;
            int width = 135 ;
            int height = 135;

            GUI.Label(new Rect(x - 1 - width/2, y - height, width, height), toolTipText, guiStyleBack);
            GUI.Label(new Rect(x - width/2, y - height, width, height), toolTipText, guiStyleFore);
        }
    }
}
