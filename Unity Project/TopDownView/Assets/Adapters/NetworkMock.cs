using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class NetworkMock {
    /// <summary>
    /// The synchronize object
    /// </summary>
    private MessagingNetworkAdapter syncObj;

    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkMock"/> class.
    /// </summary>
    /// <param name="syncObj">The synchronize object.</param>
    public NetworkMock(MessagingNetworkAdapter syncObj)
    {
        this.syncObj = syncObj;
    }

    /// <summary>
    /// Sends the specified MSG.
    /// </summary>
    /// <param name="msg">The MSG.</param>
    public void Send(string msg)
    {
        Debug.Log("send: " + msg);
    }

    /// <summary>
    /// Recvs the specified MSG.
    /// </summary>
    /// <param name="msg">The MSG.</param>
    public void Recv(string msg)
    {
        Debug.Log("recv: " + msg);
        syncObj.InputRecvData(msg);
    }
}
