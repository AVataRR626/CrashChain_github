using UnityEngine;
using System.Collections;
using InControl; 

[RequireComponent(typeof(CharacterController))]
public class QSICharConn : MonoBehaviour 
{
	public int playerNumber = 0;
	public bool movable = true;
	public bool hasGravity = false;
	public float moveSpeed = 5;
	public float gravity = 9.8f;
	public float jumpForce = 16;
	public float jumpDelay = 0.25f;
	public bool lockX = false;
	public bool lockZ = false;
    public bool invertControls = false;
	public LayerMask terrainMask;
	public LayerMask characterMask;
	public Vector3 horizontalMovement = new Vector3();
	public Transform torsoTransform;
	public bool lockOnAction = true;
	public float [] actionDelay;


	private CharacterController myConn;
	private QSIPlatformCharAnimator myAnim;
	private float verticalSpeed = 0;
	public bool isGrounded = true;
	private bool jumpReady = false;
	private float [] actionClock;

	//public so the animation system can see it.
	public float currentSpeed;


	// Use this for initialization
	void Start () 
	{
		myConn = GetComponent<CharacterController>();
		myAnim = GetComponentInChildren<QSIPlatformCharAnimator>();

		actionClock = new float[actionDelay.Length];

		if(InputManager.Devices.Count - 1 < playerNumber)
			Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{

		UtilityRun();
		FaceButtons ();
        BumperControler();

        //can't move while attacking.
        if (movable)
		{	
			HorizontalMove();
		}
		else
		{
			horizontalMovement = Vector3.zero;
		}

		Gravity ();
		HandleJump();

		isGrounded = OnFloor();

		if(myAnim != null)
		{	myAnim.animationParams.isJumping = !isGrounded;
			if(!isGrounded)
				myAnim.animationParams.jumpTrigger = false;
		}


	}

	void UtilityRun()
	{
		for(int i = 0; i < actionDelay.Length; i++)
		{
			if(actionClock[i] > 0)
				actionClock[i] -= Time.deltaTime;
		}

		if(lockOnAction)
		{
			movable = true;
			for(int i = 0; i < actionDelay.Length; i++)
			{
				if(actionClock[i] > 0)
				{	movable = false;
					break;
				}
			}
		}
		else
		{
			movable =true;
		}
	}

	void Gravity()
	{
		if(!isGrounded)
		{	
			if(hasGravity)
			{	verticalSpeed -= gravity * Time.deltaTime;
				myConn.Move (new Vector3(0,verticalSpeed * Time.deltaTime,0));
			}

			if(verticalSpeed > 3)
			{
				myAnim.animationParams.isJumping = true;
			}
		}

	}

	public bool OnFloor()
	{
		return Physics.CheckCapsule(transform.position,transform.position + new Vector3(0,-(myConn.height/2),0),0.5f,terrainMask);
		
	}

    void BumperControler()
    {
        var inputDevice = InputManager.Devices[playerNumber];
        
        //do bumper stuff here.
    }

	void HorizontalMove()
	{
		if(playerNumber > -1)
		{
			// Use last device which provided input.


			var inputDevice = InputManager.Devices[playerNumber];
			
			// Rotate target object with left stick.
			//transform.Rotate( Vector3.down,  500.0f * Time.deltaTime * inputDevice.LeftStickX, Space.World );
			//transform.Rotate( Vector3.right, 500.0f * Time.deltaTime * inputDevice.LeftStickY, Space.World );


			if(!lockX)
				horizontalMovement.x = inputDevice.LeftStickX;

			if(!lockZ)
				horizontalMovement.z = inputDevice.LeftStickY;
			
			//Debug.Log (moveVector);
			//transform.Translate(Time.deltaTime * moveSpeed * moveVector);

		}

		currentSpeed = horizontalMovement.magnitude*moveSpeed;

		if(myAnim != null)
		{
			myAnim.animationParams.moveSpeed = currentSpeed;
		}

        if (invertControls)
            horizontalMovement *= -1;

        myConn.Move (Time.deltaTime * moveSpeed * horizontalMovement);

	}

	void FaceButtons()
	{
		if(playerNumber > -1)
		{
			// Use last device which provided input.
			var inputDevice = InputManager.Devices[playerNumber];

			if(inputDevice.Action3.WasPressed)
			{
				Action(0);
			}

			if(inputDevice.Action4.WasPressed)
			{
				Action (1);
			}

			if(inputDevice.Action2.WasPressed)
			{
				Action (2);
			}

			if(inputDevice.LeftTrigger.WasPressed || 
			   inputDevice.RightTrigger.WasPressed ||
			   inputDevice.LeftBumper.WasPressed ||
			   inputDevice.RightBumper.WasPressed)
			{
				Action (3);
			}

			if(inputDevice.Action1.WasPressed)
			{
				Debug.Log ("Action 1 Was Pressed");
				if(isGrounded && movable)
				{
					JumpReady();
					movable = false;
				}

			}

			if(inputDevice.Action1)
			{
				movable = false;
				horizontalMovement = Vector3.zero;
			}

			if(inputDevice.Action1.WasReleased)
			{
				movable = true;
				Jump();
			}
		}

	}

	public void Action(int a)
	{
		//Debug.Log ("FaceButtons:Action3");
		if(myAnim != null)
		{
			if(actionClock[a] <= 0)
			{
				myAnim.animationParams.actionTriggers[a] = true;
				actionClock[a] = actionDelay[a];
			}
		}
	}

	public void JumpReady()
	{
		if(myAnim != null)
		{
			myAnim.animationParams.jumpTrigger = true;
			myAnim.animationParams.isJumping = false;
			jumpReady = false;
		}

		movable = false;
	}

	public void Jump()
	{
		if(myAnim != null)
		{
			myAnim.animationParams.isJumping = true;
			myAnim.animationParams.jumpTrigger = false;
			jumpReady = true;
		}
	}

	public void HandleJump()
	{
		if(isGrounded)
		{
			if(jumpReady)
			{
				verticalSpeed = jumpForce;
				isGrounded = false;
				jumpReady = false;
				transform.position += new Vector3(0,1,0);
			}
		}

	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{	
		//isGrounded = true;

		//land on terrain.
		if(myAnim != null)
		{
			if(hit.gameObject.layer == terrainMask)
				myAnim.animationParams.isJumping = false;

		}
	}

	public void Meditate(bool mode)
	{
		if(myAnim != null)
		{
			if(mode)
			{
				myAnim.Meditate();
			}
			else
			{
				myAnim.animationParams.isMeditating = false;
			}
		}

	}


}
