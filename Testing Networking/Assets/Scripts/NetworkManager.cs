using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// creates a new network manager class
[RequireComponent (typeof (NetworkView))] // because we will use rpc calls
public class NetworkManager : MonoBehaviour {

    string registeredGameName = "Rosanna_Test_Serverblahblbbhlaalddd";
    bool isRefreshing = false;
    float refreshRequestLength = 3.0f;
    HostData[] hostData;

    MessageWindow messageWindow;
    public string chatInput = "";
    float messageDisplayTime = 60;
    public GUISkin chatterSkin;
    Vector2 chatOutputSize = new Vector2(350, 175);
    Vector2 chatOutputPosition = new Vector2(0, Screen.height - 200);
    Vector2 chatInputSize = new Vector2(350, 25);
    Vector2 chatInputPosition = new Vector2(0, Screen.height - 25);
    Rect chatInputRect;
    Rect chatOutputRect;
    Rect entireChatArea;
    string myName = "NoName";
    bool nameSet = false;
    bool connected = false;
    Dictionary<NetworkPlayer, string> usersByID = new Dictionary<NetworkPlayer, string>();
    
    // Use this for initialization
    void Start()
    {

        // we cannot create a message window this way: therefore using .CreateInstance method
        //new MessageWindow(chatOutputSize, chatterSkin);

        messageWindow = (MessageWindow)ScriptableObject.CreateInstance("MessageWindow");
        messageWindow.createParameters(chatOutputSize, chatterSkin);
    }



    // Update is called once per frame
   /* void Update()
    {

        if(!networkView.isMine)
        return;
        if (!GUIUtils.MouseOverRect(entireChatArea))
        {
            messageWindow.pauseAutoScroll = false;
            messageWindow.CountDownTimers();
        }
        else
        {
            messageWindow.pauseAutoScroll = true;
        }
    }*/

    // Sets the name of the person connecting or hosting the game
    void SetName(string newName)
    {
        //Only after setting a name will the player be joined to the chat
        nameSet = true;
        if (Network.isServer)
        {
            JoinUser(newName, Network.player);
        }
        else
        {
            NetworkView a = GetComponent<NetworkView>();
            NetworkView b = GetComponent<NetworkView>();
            a.RPC("JoinUser", RPCMode.Server, myName, Network.player);
            b.RPC("Server_SendCurrentUsers", RPCMode.Server, Network.player);
        }
    }

    // Sends a message to everyone when one player has updated the message log
    void ProcessInput()
    {
        if (chatInput.Length > 0)
        {
            NetworkView a = GetComponent<NetworkView>();
            a.RPC("LogMessage", RPCMode.All, chatInput, Network.player);
        }
        chatInput = "";
    }

    // When someone has connected it will set their network connected state to true
    void OnConnectedToServer()
    {
        connected = true;
    }

    // when someone has disconnected from the server it will set their network connected state to false
    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        if (Network.isServer)
            SystemMessage("Server connection lost!", Network.player);
        else
            if (info == NetworkDisconnection.LostConnection)
            SystemMessage("Lost connection to server!", Network.player);
        else
            SystemMessage("Successfully diconnected from server.", Network.player);
    }


    // when someone has disconnected fromt he server it will set their network connected state to false
    void OnPlayerDisconnected(NetworkPlayer player)
    {
        RemoveUser(player);
    }


    // when a message is sent, should send to current users
    [RPC]
    void Server_SendCurrentUsers(NetworkPlayer recipient)
    {
        //In case someone else gets this message, only reply if we're the server
        if (Network.isServer)
        {
            foreach (NetworkPlayer user in usersByID.Keys)
            {
                NetworkView a = GetComponent<NetworkView>();
                a.RPC("JoinUser", recipient, usersByID[user], user);
            }
        }
    }

    // when someone joins the game, this method is called
    [RPC]
    void JoinUser(string name, NetworkPlayer player)
    {
        if (!this.usersByID.ContainsKey(player))
        {
            this.usersByID.Add(player, name);

            if (Network.isServer)
            {
                //Since we're server, let everyone know when someone joins.
                //This includes the someone who joined, so they can add themselves and get the message
                NetworkView a = GetComponent<NetworkView>();
                NetworkView b = GetComponent<NetworkView>();
                a.RPC("JoinUser", RPCMode.Others, name, player);
                b.RPC("SystemMessage", RPCMode.All, "Joined chat.", player);
            }
        }
    }

    // when someone is removed, this method is called
    [RPC]
    void RemoveUser(NetworkPlayer player)
    {
        if (this.usersByID.ContainsKey(player))
        {
            SystemMessage("Has left chat", player);
            //If we're the server, let everyone know when someone leaves.
            if (Network.isServer)
            {
                NetworkView a = GetComponent<NetworkView>();
                a.RPC("RemoveUser", RPCMode.Others, player);
            }
            this.usersByID.Remove(player);
        }
    }

    // handling sending system messages to players
    [RPC]
    void SystemMessage(string message, NetworkPlayer player)
    {
        if (usersByID.ContainsKey(player))
        {
            message = usersByID[player] + ": " + message;
            messageWindow.AddMessage(messageDisplayTime, message);

        }
        else
        {
            //We don't know that user? Why not? Get the user list again, maybe we'll know for next time.
            NetworkView a = GetComponent<NetworkView>();
            a.RPC("Server_SendCurrentUsers", RPCMode.Server, Network.player);
        }
    }

    // this method is called when someone sends a message
    [RPC]
    void LogMessage(string message, NetworkPlayer player)
    {
        if (usersByID.ContainsKey(player))
        {
            //If we didn't say it, enter add some info about who did
            if (player != Network.player)
            {
                message = usersByID[player] + " said: " + message;
            }

            messageWindow.AddMessage(messageDisplayTime, message);
        }
        else
        {
            //We don't know that user? Why not? Get the user list again, maybe we'll know for next time.
            NetworkView a = GetComponent<NetworkView>();
            a.RPC("Server_SendCurrentUsers", RPCMode.Server, Network.player);
        }
    }


    //--------------------------------------------------------
    // starts the server
    private void StartServer()
    {
        Network.InitializeServer(4, 25002, false);
        MasterServer.RegisterHost(registeredGameName, "Rosanna Tower Testing", "Testing Networking");
    }

    // when the server is initialized/connected and ready to start game
    void OnServerInitialized()
    {
        Debug.Log("Server has been initialized!");
        connected = true;//from new
    }

    // this method is fired when a new server hosts a game
    void OnMasterServerEvent(MasterServerEvent masterServerEvent)
    {
        if(masterServerEvent == MasterServerEvent.RegistrationSucceeded)
        Debug.Log("Registration successful");

    }

    // refreshes the list of active hosts (the servers connected)
    public IEnumerator RefreshHostList()
    {
        Debug.Log("Refreshing...");
        MasterServer.RequestHostList(registeredGameName);
        float timeStarted = Time.time;
        float timeEnd = Time.time + refreshRequestLength;

        while (Time.time < timeEnd)
        {
            hostData = MasterServer.PollHostList();
            yield return new WaitForEndOfFrame();
        }

        if (hostData == null || hostData.Length == 0) { 
        Debug.Log("No active servers have been found.");
        } else
        {
            Debug.Log(hostData.Length + " have been found.");
        }
    }

    // this method is called when frame is generated
    public void OnGUI()
    {

        if (Network.isClient || Network.isServer)
        {
            GUI.Label(new Rect(25f, 25f, 250f,30f), "Connections:" + Network.connections.Length);

            chatInputRect = new Rect(chatInputPosition.x, chatInputPosition.y, chatInputSize.x, chatInputSize.y);
            chatOutputRect = new Rect(chatOutputPosition.x, chatOutputPosition.y, chatOutputSize.x, chatOutputSize.y);
            entireChatArea = new Rect(chatInputPosition.x, chatInputPosition.y, Mathf.Max(chatInputSize.x, chatOutputSize.x), chatInputSize.y + chatOutputSize.y);

           
           
            if (connected)
            {
                if (nameSet)
                {

                    messageWindow.Draw(chatOutputPosition);

                    if (Event.current.Equals(Event.KeyboardEvent("return")) && GUI.GetNameOfFocusedControl().Equals("TextInput"))
                    {
                        ProcessInput();
                    }
                    GUI.SetNextControlName("TextInput");
                    chatInput = GUI.TextField(chatInputRect, chatInput);

                }
                else
                {
                    //Require the user set their name before they're fully joined to the chat server.
                    GUI.Label(chatOutputRect, "Please enter your name");
                    if (Event.current.Equals(Event.KeyboardEvent("return")) && GUI.GetNameOfFocusedControl().Equals("NameInput") && myName.Length > 0)
                    {
                        SetName(myName);
                    }

                    GUI.SetNextControlName("NameInput");
                    myName = GUI.TextField(chatInputRect, myName);

                    GUI.FocusControl("NameInput");
                }
            }
            return;


        }

        if (GUI.Button(new Rect(25f, 25f, 150f, 30f), "Start New Server"))
        {
            // Start server function here
            StartServer();
        }

        if (GUI.Button(new Rect(25f, 65f, 150f, 30f), "Refresh Server List"))
        {
            // Refresh Server List Function here
            StartCoroutine("RefreshHostList");
        }

        if(hostData != null)
        {



            if (GUI.Button(new Rect(Screen.width / 2 + 300f, 65f, 100f, 30f), "connect to Nadia's Computer"))
            {
                Network.Connect("142.232.18.95", 25002);
            }

            for (int i = 0; i < hostData.Length; i++)
            {
                if(GUI.Button(new Rect(Screen.width/2, 65f+ (30f * i), 300f, 30f), hostData[i].gameName))
                {
                    // for peer to peer connection, network.connect(ipAddress, remotePort)
                    Debug.Log("Created client. " + hostData[i]);
                    Network.Connect(hostData[i]);
                }

            }
        }
    }
    
    //


}
