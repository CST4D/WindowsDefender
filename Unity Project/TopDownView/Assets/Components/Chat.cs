using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class Chat : MonoBehaviour {

    /// <summary>
    /// The multi message adapter
    /// </summary>
    MultiplayerMessagingAdapter multiMessageAdapter;
    /// <summary>
    /// The input
    /// </summary>
    public InputField input;
    /// <summary>
    /// The text
    /// </summary>
    public Text text;
    /// <summary>
    /// The vertical scroll
    /// </summary>
    public Scrollbar verticalScroll;
    // Use this for initialization
    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
	
	}

    /// <summary>
    /// Initializes the specified adapt.
    /// </summary>
    /// <param name="adapt">The adapt.</param>
    public void Initialize (MultiplayerMessagingAdapter adapt)
    {
        multiMessageAdapter = adapt;
    }

    /// <summary>
    /// Receives the chat message.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="content">The content.</param>
    public void ReceiveChatMessage(string username, string content)
    {
        text.text = "[" + username + "]: " + content + "\n" + text.text;
    }

    // Update is called once per frame
    /// <summary>
    /// Updates this instance.
    /// </summary>
    void Update () {
        
    }

    /// <summary>
    /// Called when [GUI].
    /// </summary>
    void OnGUI()
    {
        if (input.isFocused && input.text != "" && Input.GetKey(KeyCode.Return))
        {
            multiMessageAdapter.SendChatMessage(input.text.Replace("|", ""));
            input.text = "";
        }
    }
}
