using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

public class NetworkClient : MonoBehaviour
{
    private Thread clientThread = null;
    private UdpClient server = null;

    void Start()
    {
        clientThread = new Thread(StartClient);
        clientThread.Start();
    }

    private void StartClient()
    {
        byte[] data = new byte[1024];
        string stringData;
        IPEndPoint receiverEndPoint = new IPEndPoint(IPAddress.Any, 0);
        server = new UdpClient("compcst.cloudapp.net", 25001);
        
        string msg = "Testing123";
        data = Encoding.ASCII.GetBytes(msg);
        server.Send(data, data.Length);
        print("Sent message.");

        while (true)
        {
            data = server.Receive(ref receiverEndPoint);
            stringData = Encoding.ASCII.GetString(data, 0, data.Length);
            print(stringData);
        }
    }

    /// <summary>
    /// Must have this otherwise Unity will freeze after running
    /// the application more than once.
    /// </summary>
    public void OnApplicationQuit()
    {
        server.Close();
        if (clientThread != null)
            clientThread.Abort();
    }
}
