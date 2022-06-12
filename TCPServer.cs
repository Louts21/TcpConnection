using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
    #region private members
    private TcpListener tcpListener;
    private Thread tcpListenerThread;
    private TcpClient connectedTcpClient;
    #endregion

    public string[] informationArray;

    void Start()
    {
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequest));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
    }

    void Update()
    {

    }

    private void ListenForIncommingRequest()
    {
        try
        {
            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 11100);
            tcpListener.Start();
            Debug.Log("Server is listening!");

            ReadStream();
        }
        catch (SocketException se)
        {
            Debug.Log("Socket exception - " + se.ToString());
        }
    }

    private void ReadStream()
    {
        while (true)
        {
            using (connectedTcpClient = tcpListener.AcceptTcpClient())
            {
                using (NetworkStream stream = connectedTcpClient.GetStream())
                {
                    int length;
                    Byte[] bytes = new byte[1024];
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        string clientMessage = Encoding.ASCII.GetString(incommingData);

                        TextDevider(clientMessage);
                    }
                }
            }
        }
    }

    private void TextDevider(string text)
    {
        if (text.IndexOf('\n') >= 0)
        {
            informationArray = text.Split('\n');
            Debug.Log(informationArray[0] + " and " + informationArray[1]);
            SendMessageToClient(informationArray[0]);
        }
        else
        {
            SendMessageToClient(text);
        }
    }

    private void SendMessageToClient(string sendableText)
    {
        if (connectedTcpClient == null)
        {
            return;
        }

        try
        {
            NetworkStream stream = connectedTcpClient.GetStream();
            if (stream.CanWrite)
            {
                byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(sendableText);
                stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                Debug.Log("Server sent his message!");
            }
        }
        catch (SocketException se)
        {
            Debug.Log("Socket exception - " + se.ToString());
        }
    }
}