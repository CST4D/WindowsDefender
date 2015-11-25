using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WDClient;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        string SERVER_IP = "compcst.cloudapp.net";
        string username = "Jeff";
        string matchID = "4fg7-38g3-d922-f75g-48g6";

        /// <summary>
        /// Tests that the JOINED command gets received after sending JOIN
        /// </summary>
        [TestMethod]
        public void TestOnJoin()
        {
            NetworkClient testClient = new NetworkClient(matchID, username, SERVER_IP);
            testClient.Start();

            // Test this function
            testClient.SendInstruction(WDServer.Instruction.Type.JOIN, username, matchID);

            // Get response
            WDServer.Instruction receivedInstruction = WaitForResponse(testClient);

            // Expect to receive the JOINED command as a response
            Assert.AreEqual(receivedInstruction.Command, WDServer.Instruction.Type.JOINED);
        }


        /// <summary>
        /// Tests that the user gets disconnected after 15 seconds after not sending any heartbeats
        /// </summary>
        [TestMethod]
        public void TestHeartBeat()
        {
            NetworkClient testClient = new NetworkClient(matchID, username, SERVER_IP);
            testClient.Start();

            // Test this function
            testClient.SendInstruction(WDServer.Instruction.Type.JOIN, username, matchID);

            // Get response
            WDServer.Instruction receivedInstruction = WaitForResponse(testClient);

            // Get response
            WDServer.Instruction receivedInstruction2 = WaitForResponse(testClient);

            // Expect to receive the JOINED command as a response
            Assert.AreEqual(receivedInstruction.Command, WDServer.Instruction.Type.LEAVE);
        }

        private static WDServer.Instruction WaitForResponse(NetworkClient testClient)
        {
            byte[] receivedData = new byte[512];
            IPEndPoint receiverEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // Block and wait
            receivedData = testClient.socket.Receive(ref receiverEndPoint);

            // Data received, remove trailing nulls
            int i = receivedData.Length - 1;
            while (receivedData[i] == 0)
                --i;
            byte[] data = new byte[i + 1];
            Array.Copy(receivedData, data, i + 1);

            // Deserialize recieved instruction
            WDServer.Instruction receivedInstruction = Serializer.DeSerialize(data);
            return receivedInstruction;
        }
    }
}
