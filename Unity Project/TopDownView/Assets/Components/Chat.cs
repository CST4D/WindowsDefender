using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Chat : MonoBehaviour {

    MultiplayerMessagingAdapter multiMessageAdapter;
    public InputField input;
    public Text text;
    public Scrollbar verticalScroll;
	// Use this for initialization
	void Start () {
	
	}

    public void Initialize (MultiplayerMessagingAdapter adapt)
    {
        multiMessageAdapter = adapt;
    }
	
    public void ReceiveChatMessage(string username, string content)
    {
        text.text = "[" + username + "]: " + content + "\n" + text.text;
    }

	// Update is called once per frame
	void Update () {
        
    }
    
    void OnGUI()
    {
        if (input.isFocused && input.text != "" && Input.GetKey(KeyCode.Return))
        {
            multiMessageAdapter.SendChatMessage(input.text.Replace("|", ""));
            input.text = "";
        }
    }
}
