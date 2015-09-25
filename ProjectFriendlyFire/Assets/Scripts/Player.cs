using UnityEngine;
using UnityEngine.Networking;

using System.Collections;

public class Player : NetworkBehaviour {

    [SyncVar] public int dx;
    [SyncVar] public int dy;
    public float moveSpeed = 0.04f;

    #region Initialization

    void Start()
    {
    }

    public override float GetNetworkSendInterval()
    {
        return 0.04f;
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

        int oldDx = dx;
        int oldDy = dy;
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

        if (dx != oldDx || dy != oldDy)
        {
            CmdMove(dx, dy);
        }
	}

    [Command]
    public void CmdMove(int dx, int dy)
    {
        this.dx = dx;
        this.dy = dy;
        transform.Translate(dx * moveSpeed, dy * moveSpeed, 0);
        base.SetDirtyBit(1);
    }

    [ServerCallback]
    public void FixedUpdate()
    {
        transform.Translate(dx * moveSpeed, dy * moveSpeed, 0);
    }

    #region Utility/Debug

    [ClientRpc]
    void RpcPoke(int value)
    {
        Debug.Log("value:" + value);
    } 

    #endregion
}
