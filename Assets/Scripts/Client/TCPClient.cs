using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Client;
using UnityEngine;

public sealed class TCPClient : Publisher
{
    // The port number for the remote device.  
    private const int port = 9001;
    // ManualResetEvent instances signal completion.  
    private ManualResetEvent connectDone =
        new ManualResetEvent(false);
    private ManualResetEvent sendDone =
        new ManualResetEvent(false);
    private ManualResetEvent receiveDone =
        new ManualResetEvent(false);

    private List<Subscriber> Subscribers;
    byte[] bytes = new byte[1024];


    // The response from the remote device.  
    private String response = String.Empty;
    private static readonly TCPClient instance = new TCPClient();

    public bool IsConnected { get ; set; }
    private bool IsWaitingData = false;

    private Socket server;

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static TCPClient()
    {
    }

    public ManualResetEvent GetSendDone()
    {
        return sendDone;
    }

    private TCPClient()
    {
        IsConnected = false;
        Subscribers = new List<Subscriber>();
    }

    public static TCPClient Instance
    {
        get
        {
            return instance;
        }
    }
    
    

    public void StartClient(IPAddress ipAddress)
    {
        // Connect to a remote device.  
        try
        {
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
            
            // Debug.Log(String.Format("It will point to IP -> {0} and the port -> {1}",remoteEP.Address,remoteEP.Port));

            // Create a TCP/IP socket.  
            server = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.  
            server.Connect(remoteEP);
            if (server.Connected)
            {
                IsConnected = true;
                var thread = new Thread(CallReceive);
                thread.Start();
                Debug.Log("Server is connected!!!");
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void Close()
    {
        // Release the socket.
        try
        {
            Debug.Log("Trying to close the socket!");
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            IsConnected = false;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.  
            client.EndConnect(ar);

            Debug.Log(String.Format("Socket connected to {0}",client.RemoteEndPoint));

            IsConnected = true;

            // Signal that the connection has been made.  
            connectDone.Set();
            
            var thread = new Thread(CallReceive);
            thread.Start();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void CallReceive()
    {
        while (IsConnected)
        {
//            if (!IsWaitingData)
//            {
                Receive();
                Debug.Log("Waiting data from socket!");
                IsWaitingData = true;
//            }
        }
    }

    public void Receive()
    {
        try
        {
            int bytesRec = server.Receive(bytes);
            response = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            Publish();
            IsWaitingData = false;
//            // Create the state object.  
//            StateObject state = new StateObject();
//            state.workSocket = client;
//
//            // Begin receiving the data from the remote device.  
//            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
//                new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the state object and the client socket   
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;
            client.NoDelay = true;

            // Read data from the remote device.  
            int bytesRead = client.EndReceive(ar);

            response = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);

            Debug.Log("From phone:" + response);
            // Signal that all bytes have been received.  
            receiveDone.Set();
            Publish();
            IsWaitingData = false;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void Send(String data)
    {
        // Convert the string data to byte data using ASCII encoding.  
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        Debug.Log("Sending the data to the phone!");
        server.Send(byteData);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = client.EndSend(ar);
            // Debug.Log(String.Format("Sent {0} bytes to server.", bytesSent));

            // Signal that all bytes have been sent.  
            sendDone.Set();
            
            Debug.Log("Data sended to the phone!");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }


    public void AddSubscriber(Subscriber subscriber)
    {
        Subscribers.Add(subscriber);
    }

    public void RemoveSubscriber(Subscriber subscriber)
    {
        Subscribers.Remove(subscriber);
    }

    public void Publish()
    {
        foreach (var subscriber in Subscribers)
        {
            subscriber.Update(response);
        }
    }
}

public class StateObject
{
    // Client socket.  
    public Socket workSocket = null;
    // Size of receive buffer.  
    public const int BufferSize = 256;
    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];
    // Received data string.  
    public StringBuilder sb = new StringBuilder();
}