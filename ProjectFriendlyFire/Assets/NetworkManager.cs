using UnityEngine;

using System;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;

public class NetworkManager : MonoBehaviour {

    string ip = "127.0.0.1";
    int port = 25000;
    int maxConnections = 4;

	// Use this for initialization
	void Start () 
    {
		print ("in start");
		ip = GetIP ();
		print (ip);
	}

	// Update is called once per frame
	void Update () 
    {
	    
	}

	string GetIP()
	{
		string strHostName = "";
		strHostName = System.Net.Dns.GetHostName();
		IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
		IPAddress[] addr = ipEntry.AddressList;
		return addr[addr.Length-1].ToString();
	}

    void OnGUI()
    {
		if (Network.isServer) {
			NetworkPlayer[] players = Network.connections;
			foreach(NetworkPlayer p in players) {
				GUILayout.BeginHorizontal();
				GUILayout.Label(p.ipAddress.ToString());
				GUILayout.EndHorizontal();
			}
			return;
		}
		if (Network.isClient) {

			return;
		}

        GUILayout.BeginHorizontal();
        ip = GUILayout.TextField(ip);
        GUILayout.Label("IP ADDRESS");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        port = int.Parse(GUILayout.TextField(port.ToString()));
        GUILayout.Label("PORT");
        GUILayout.EndHorizontal();

        if (GUILayout.Button("CONNECT"))
        {
            print("connecting...");
            Network.Connect(ip, port);
        }

        if (GUILayout.Button("START SERVER"))
        {
            bool useNat = !Network.HavePublicAddress();
            print(string.Format("starting server on {0}:{1}", ip, port));
            Network.InitializeServer(maxConnections, port, useNat);
        }
    }

	void OnServerInitialized()
	{
		print("server started");
	}

    void OnConnectedToServer()
    {
        print("connected");
    }
}
