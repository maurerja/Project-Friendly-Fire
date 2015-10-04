using UnityEngine;
using UnityEngine.Networking;

using System.Collections;

public class Player : NetworkBehaviour {

    [SyncVar] public float dx;
    [SyncVar] public float dy;
    public float moveSpeed = 0.02f;

    private const float diag = 0.70710678118f; // sqrt(2)/2
    public GameObject cam;

    private Vector3 cameraOffset = new Vector3(0, 0, -10);

    #region Initialization

    void Start()
    {
        this.transform.Translate(new Vector3(8, 8, 0));
        if (isLocalPlayer) {
            GameObject.Find("MainCamera").SetActive(false);
            cam = new GameObject("PlayerCamera" + GameObject.FindGameObjectsWithTag("Player").Length);
            cam.transform.Translate(0, 0, -10);
            cam.AddComponent<Camera>();
            cam.AddComponent<GUILayer>();
            cam.AddComponent<FlareLayer>();
            //o.AddComponent<AudioListener>();
            cam.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            cam.GetComponent<Camera>().backgroundColor = Color.black;
            cam.GetComponent<Camera>().orthographic = true;
            cam.GetComponent<Camera>().depth = -1;
            cam.GetComponent<Camera>().orthographicSize = 8;
            cam.GetComponent<Camera>().enabled = false;
        }
    }

    public override float GetNetworkSendInterval()
    {
        return 0.03f;
    }

    public override void OnStartClient()
    {
        Debug.Log("Player OnStartClient netId:" + this.netId);
    }

    #endregion
	
	
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }

        float oldDx = dx;
        float oldDy = dy;
        dx = 0;
        dy = 0;
        
        if (Input.GetKey(KeyCode.W))
        {
            dy += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dx -= 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dy -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dx += 1;
        }

        if (dx != 0 && dy != 0)
        {
            dx *= diag;
            dy *= diag;
        }

        Vector3 dirA = (new Vector3(dx, dy, 0).normalized)*0.52f;
        Vector3 dirB = Quaternion.AngleAxis(-45, new Vector3(0, 0, 1)) * dirA;
        Vector3 dirC = Quaternion.AngleAxis(45, new Vector3(0, 0, 1)) * dirA;
        Vector3 collisionA = transform.position + dirA;
        Vector3 collisionB = transform.position + dirB;
        Vector3 collisionC = transform.position + dirC;

        Level lvl = GameObject.Find("LevelManager").GetComponent<Level>();
        Level.Tile ta = lvl.getTile(collisionA.x, collisionA.y);
        Level.Tile tb = lvl.getTile(collisionB.x, collisionB.y);
        Level.Tile tc = lvl.getTile(collisionC.x, collisionC.y);

        bool a = ta != Level.Tile.Empty;
        bool b = tb != Level.Tile.Empty;
        bool c = tc != Level.Tile.Empty;
        if(a && b && c)
        {
            dx = 0;
            dy = 0;
        }
        else if (a && b)
        {
            
        }
        else if (a && c)
        {

        }
        else if (b)
        {

        }
        else if (c)
        {

        }


        if (dx != oldDx || dy != oldDy)
        {
            CmdMove(dx, dy);
        }
	}

    [Command]
    public void CmdMove(float dx, float dy)
    {
        this.dx = dx;
        this.dy = dy;
        transform.Translate(dx * moveSpeed, dy * moveSpeed, 0);
        base.SetDirtyBit(1);
    }

    [ServerCallback]
    public void FixedUpdate()
    {
        float tx = dx * moveSpeed;
        float ty = dy * moveSpeed;
        transform.Translate(tx, ty, 0);
    }

    public void OnGUI()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Event.current.type == EventType.repaint)
        {
            cam.transform.position = transform.position + cameraOffset;
            cam.GetComponent<Camera>().Render();
        }
    }

    #region Utility/Debug

    [ClientRpc]
    void RpcPoke(int value)
    {
        Debug.Log("value:" + value);
    } 

    #endregion
}
