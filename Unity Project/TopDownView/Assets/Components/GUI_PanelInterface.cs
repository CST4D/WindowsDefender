using UnityEngine;
using System.Collections;

/// <summary>
/// @Author Calvin Truong
/// @Version 1.0
/// 
/// @Description: When the button ("btn_Towers) is clicked, call the onClick() function
/// which will proceed to create an array of tower buttons based on the towers in the
/// towers array.
/// 
/// </summary>
public class GUI_PanelInterface : MonoBehaviour {

    public ButtonScript buttonTemplate;
    public GameObject panel;
    public GameObject[] antiViruses;
    public GameObject[] viruses;

    private ArrayList buttons;
    private bool showButtons;

    private int buttonStatus; // 0 = None, 1 = AntiVirus, 2 = Virus

	// Use this for initialization
	void Start () {
        buttons = new ArrayList();
        showButtons = false;
        buttonStatus = 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Instantiates tower buttons based on the number of elements in the towers array.
    /// The tower buttons's text is based on the element's name.
    /// </summary>
    public void onClick(int status)
    {
        if (status != 1 && status != 2)
            return;

        if (buttonStatus != status && showButtons)
            removeButtons();

        if (!showButtons)
        {
            buttonStatus = status;

            // Hard-coded value
            int offsetX = 80;

            // Get the Panel's Position
            Vector3 panelPosition = panel.transform.position;

            // Get the Button's Rect Transformation
            Rect buttonRect = buttonTemplate.gameObject.GetComponent<RectTransform>().rect;

            // Get the Panel's Rect Transformation
            Rect panelRect = panel.gameObject.GetComponent<RectTransform>().rect;

            GameObject[] buttonObjects;

            switch (status)
            {
                case 1: buttonObjects = antiViruses; break;
                case 2: buttonObjects = viruses; break;
                default: buttonObjects = null; break;
            }

            for (int i = 0; i < buttonObjects.Length; i++)
            {
                Vector3 newPosition = panelPosition;
                Vector3 spawnPosition = panelPosition;
                // We set the x position of the new button
                newPosition.x += offsetX + buttonRect.width * i;
                spawnPosition.x += offsetX + buttonRect.width * 0;

                // Create the new button
                ButtonScript newButton = (ButtonScript)Instantiate(buttonTemplate, spawnPosition, transform.rotation);

                // Name of the object on Unity Editor
                newButton.name = buttonObjects[i].name;

                // Text of the object to display in the game
                newButton.gameObject.GetComponentInChildren<UnityEngine.UI.Text>().text = buttonObjects[i].name;

                // Set the parent of the object to this object (GUI_Towers)
                // If you don't then the object isn't part of the canvas / child of canvas
                newButton.transform.SetParent(panel.transform);

                // Set the position for the button to slide to
                newButton.MoveLocation = newPosition;

                // Assign the button to the game controller
                newButton.GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BuildMode>();

                // Assign the tower for the button that should be built
                newButton.SourceObject = buttonObjects[i];

                // Add button to array list
                buttons.Add(newButton);
            }

            showButtons = true;
        }
    }

    private void removeButtons()
    {
        Vector3 moveLocation = ((ButtonScript)buttons[0]).MoveLocation;

        foreach (ButtonScript btn in buttons)
        {
            btn.MoveLocation = moveLocation;
            btn.Hide = true;
        }


        showButtons = false;
    }

}
