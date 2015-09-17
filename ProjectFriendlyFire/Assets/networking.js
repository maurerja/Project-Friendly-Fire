//****** Donations are greatly appreciated.  ******
//****** You can donate directly to Jesse through paypal at  jesse_etzler@yahoo.com   ******
 
#pragma strict
 
var ipAddress : String = "127.0.0.1";
var port : int = 25000;
var maxConnections : int = 10;
 
function OnGUI () {
 
    GUILayout.BeginHorizontal ();
    ipAddress = GUILayout.TextField (ipAddress);
    GUILayout.Label ("IP ADDRESS");
    GUILayout.EndHorizontal ();
 
    GUILayout.BeginHorizontal ();
    var tempPort : String;
    tempPort = GUILayout.TextField (port.ToString());
    port = parseInt(tempPort);
    GUILayout.Label ("PORT");
    GUILayout.EndHorizontal();
 
    if(GUILayout.Button ("CONNECT")) {
        print("connecting... ");
        Network.Connect (ipAddress, port);
    }
 
    if(GUILayout.Button ("START SERVER")) {
        print("starting server on " + ipAddress + ":" + port);
        Network.InitializeServer (maxConnections, port);
    }
}
 
function OnConnectedToServer () {
    print("connected");
}