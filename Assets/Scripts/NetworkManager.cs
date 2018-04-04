using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public sealed class NetworkManager {

    private const int messageInterval = 200;
    private const int updateTime = 20;

    private static NetworkManager instance;

	public static NetworkManager Instance {
        get 
        {
            if (instance == null)
                instance = new NetworkManager();
            return instance;
        } 
    }

    private readonly string host = "10.0.0.2";
    private readonly int port = 10002;

    readonly Thread thread;

    private Queue<string> messageQueue;

    private NetworkManager()
    {
        messageQueue = new Queue<string>();
        thread = new Thread(Update);
        thread.Start();
    }

    public void AddMessage(string message)
    {
        messageQueue.Enqueue(message);
    }

	private void Send(string message)
	{
		byte[] bytes = Encoding.UTF8.GetBytes (message + "\r\n");
		var ip = IPAddress.Parse (host);

		var endPoint = new IPEndPoint (ip, port);
		var socket = new Socket (ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		socket.Connect (endPoint);

		socket.Send (bytes);

		var recMessage = new byte[256];
		socket.Receive (recMessage);

		socket.Close ();
	}

    private void Update()
    {
        int currentMessageInterval = 0;
        
        while(true)
        {
            if(messageQueue.Count > 0 && currentMessageInterval > messageInterval)
            {
                Send(messageQueue.Dequeue());
                currentMessageInterval = 0;
            }

            currentMessageInterval += updateTime;
            Thread.Sleep(updateTime);
        }
    }

	private Socket connectSocket(string server, int port)
	{
		Socket s = null;
		IPHostEntry hostEntry = null;

		// Get host related information.
		hostEntry = Dns.GetHostEntry(server);

		// Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
		// an exception that occurs when the host IP Address is not compatible with the address family
		// (typical in the IPv6 case).
		foreach(IPAddress address in hostEntry.AddressList)
		{
			IPEndPoint ipe = new IPEndPoint(address, port);
			Socket tempSocket = 
				new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

			tempSocket.Connect(ipe);

			if(tempSocket.Connected)
			{
				s = tempSocket;
				break;
			}
			else
			{
				continue;
			}
		}
		return s;
	}
}
