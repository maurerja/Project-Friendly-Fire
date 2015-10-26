using UnityEngine;
using UnityEngine.Networking;

using System.Collections;

public class Player : NetworkBehaviour {

    [SyncVar] public float dx;
    [SyncVar] public float dy;
	[SyncVar] public float angle;
    private float moveSpeed = 5;
	private float accel = 5;
	private float friction = 0.69f;

    private const float diag = 0.70710678118f; // sqrt(2)/2
    public GameObject cam;

    private Vector3 cameraOffset = new Vector3(0, 0, -10);


	private Rigidbody2D rb;

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

		rb = this.GetComponent<Rigidbody2D>();
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
	}

    [Command]
    public void CmdMove(float dx, float dy, float angle)
    {
		this.dx = dx;
		this.dy = dy;
		float ax = dx * accel;
		float ay = dy * accel;
		this.rb.AddForce(new Vector2 (ax, ay));
		this.rb.velocity = Mathf.Clamp(this.rb.velocity.magnitude, 0, this.moveSpeed) * this.rb.velocity.normalized;
		if (dx == 0 && dy == 0)
		{
			this.rb.velocity *= friction;
			if (this.rb.velocity.magnitude < 0.001f)
			{
				this.rb.velocity = new Vector2(0, 0);
			}
		}

		this.angle = angle;
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
        
		base.SetDirtyBit(1);
    }
	
    void FixedUpdate()
    {
		if (isLocalPlayer)
		{
			float oldDx = dx;
			float oldDy = dy;
			float oldAngle = angle;
			dx = 0;
			dy = 0;
			angle = -1;
			
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

			//Get the Screen positions of the object
			Vector2 positionOnScreen = cam.GetComponent<Camera>().WorldToViewportPoint (transform.position);
			
			//Get the Screen position of the mouse
			Vector2 mouseOnScreen = (Vector2)cam.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
			
			//Get the angle between the points
			angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen) + 180;
			
			if (dx != oldDx || dy != oldDy || angle != oldAngle)
			{
				CmdMove(dx, dy, angle);
			}
		}
		ServerFixedUpdate();
    }

	[ServerCallback]
	private void ServerFixedUpdate() {
		float ax = dx * accel;
		float ay = dy * accel;
		this.rb.AddForce(new Vector2 (ax, ay));
		this.rb.velocity = Mathf.Clamp(this.rb.velocity.magnitude, 0, this.moveSpeed) * this.rb.velocity.normalized;
		if (dx == 0 && dy == 0)
		{
			this.rb.velocity *= friction;
			if (this.rb.velocity.magnitude < 0.001f)
			{
				this.rb.velocity = new Vector2(0, 0);
			}
		}

		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
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

	float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}
    #endregion
}
