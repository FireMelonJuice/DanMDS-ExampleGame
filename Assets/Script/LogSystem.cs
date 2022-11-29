using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

struct Test
{
    public Test(int a, float b)
    {
        this.a = a;
        this.b = b;
    }

    int a;
    float b;
}

public class LogSystem : MonoBehaviour
{
    public string userName;
    public int map;
    public string desc;
    public float speed = 1;

    private string host = "133.186.208.228";
    private int port = 8080;

    Socket socket = null;

    public void SendData(string rawData)
    {
        byte[] data = Encoding.UTF8.GetBytes(rawData);
        this.socket.Send(data);
    }

    private void Awake()
    {
        Time.timeScale = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress address = IPAddress.Parse(host);
        IPEndPoint endPoint = new IPEndPoint(address, port);

        try
        {
            Debug.Log("Connecting to Server");
            this.socket.Connect(endPoint);
        }
        catch (SocketException e)
        {
            Debug.Log("Connection Failed: " + e.Message);
        }

        string rawData = $"{userName},{map},{desc};";
        SendData(rawData);
    }

    void OnApplicationQuit()
    {
        if (this.socket != null)
        {
            this.socket.Close();
            this.socket = null;
        }
    }
}
