using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	string registeredGameName = "dvlDrummer_17_2016";
	bool isRefreshing = false;
	float refreshRequestLength = 3.0f;
	HostData[] hostData;

	private void startServer(){
		Network.InitializeServer (16, 25002, false);
		MasterServer.RegisterHost (registeredGameName, "to be named");

	}

	void OnServerInitialized(){
		Debug.Log ("Server has been initialized");
	}

	void OnMasterServerEvent (MasterServerEvent mse){
		if (mse == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log ("Registration successful");
		}
	}

	public IEnumerator RefreshHostList(){
		Debug.Log ("Refreshing...");
		MasterServer.RequestHostList (registeredGameName);
		float timeStarted = Time.time;
		float timeEnd = Time.time + refreshRequestLength;
		while(Time.time < timeEnd){
			hostData = MasterServer.PollHostList ();
			yield return new WaitForEndOfFrame();
		}

		if (hostData == null || hostData.Length == 0) {
			Debug.Log ("No active servers have been found.");
		} else {
			Debug.Log (hostData.Length + " have been found.");
		}
	}

	public void OnGUI(){
		if (Network.isClient || Network.isServer) {
			return;
		}
		if(GUI.Button(new Rect(25f,25f,150f, 30f), "Start New Server")){
			//start Server Function Here
			startServer();
		}
		if(GUI.Button(new Rect(25f,65f,150f, 30f), "Refresh Server List")){
			//Refresh Server list Function Here
			StartCoroutine("RefreshHostList");
		}
		if (hostData != null) {
			for(int i = 0; i<hostData.Length; i++){
				if(GUI.Button(new Rect(Screen.width/2, 65f +(30f*i), 300f,30f),hostData[i].gameName)){
					Network.Connect(hostData[i]);
				}
			}
		}
	}
}
