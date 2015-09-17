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
	    
	}

	// Update is called once per frame
	void Update () 
    {
	    
	}

    void OnGui()
    {
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

    void OnConnectedToServer()
    {
        print("connected");
    }
}
