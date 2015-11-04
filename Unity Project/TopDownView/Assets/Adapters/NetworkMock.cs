using UnityEngine;
using System.Collections;

public class NetworkMock {
    private MultiplayerNetworkAdapter syncObj;

    public NetworkMock(MultiplayerNetworkAdapter syncObj)
    {
        this.syncObj = syncObj;
    }
     
    public void Send(string msg)
    {
        Debug.Log("send: " + msg);
    }

    public void Recv(string msg)
    {
        Debug.Log("recv: " + msg);
        syncObj.Recv(msg);
    }
}
