using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	Rigidbody rigidB;
	Animator animator;
	CapsuleCollider capsuleCollider;
	Transform cam;

	[HideInInspector]
	public Transform camHolder;

	[SerializeField] float lockSpeed = .5f; // limit locked movement speed modifier
	[SerializeField] float normSpeed = .8f; // norm movement speed modifier
	float speed; 

	[SerializeField] float turnSpeed;

	Vector3 directionPos;
	Vector3 storeDir;

	[HideInInspector]
	public float horizontal;
	[HideInInspector]
	public float vertical;
	[HideInInspector]
	public bool rollInput;

	public bool lockTarget;
	int curTarget;
	bool changeTarget;

	float targetTurnAmount;
	float curTurnAmount;
	public bool canMove;
	public List<Transform> Enemies = new List<Transform>();

	public Transform camTarget;
	public float camTargetSpeed = 5;
	Vector3 targetPos;

	void Start()
	{
		// access refrences 
		rigidB = GetComponent<Rigidbody>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		animator = GetComponent<Animator>();
		camHolder = Camera.main.transform.parent.parent; // access Camera Arm
		GetComponent<PlayerAnimation>().enabled = true;
	}

	void FixedUpdate()
	{
		HandleInput();
		HandleCameraTarget();

		if (canMove)
		{
			if (!lockTarget)
			{
				speed = normSpeed;
				HandleMovementNormal();
			}
			else
			{
				speed = lockSpeed;

				if (Enemies.Count > 0)
				{
					HandleMovementLockOn();
					HandleRotationOnLock();
				}
				else
				{
					lockTarget = false;
				}
			}
		}
	}

	void HandleInput()
	{
		// inputs
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		rollInput = Input.GetButton("Roll");

		// ensure inputs will be relative to camera direction
		storeDir = camHolder.right; ;

		ChangeTargetLogic();
	}

	void ChangeTargetLogic()
	{
		if (Input.GetButtonUp("Fire2"))
			lockTarget = !lockTarget;

		/* // cycles through enemies **currently utilized through other script**
		if (Input.GetKeyUp(KeyCode.Q))
		{
			if (curTarget < Enemies.Count - 1)
			{
				curTarget++;
			}
			else
			{
				curTarget = 0;
			}
		} */
	}

	void HandleMovementNormal()
	{
		canMove = animator.GetBool("canMove");

		Vector3 dirForward = storeDir * horizontal;
		Vector3 dirSides = camHolder.forward * vertical;

		// normalized to prevent moving faster when both axis's are pressed
		if (canMove)
		{
			rigidB.AddForce((dirForward + dirSides).normalized * speed / Time.deltaTime);
		}

		directionPos = transform.position + (storeDir * horizontal) + (cam.forward * vertical);

		// find the direction from that position
		Vector3 dir = directionPos - transform.position;
		dir.y = 0;

		// find the angle
		float angle = Vector3.Angle(transform.forward, dir);

		float animValue = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
		animValue = Mathf.Clamp01(animValue);

		animator.SetFloat("forward", animValue);
		animator.SetBool("lockOn", false);

		// if pressing a button to mvoe
		if (horizontal != 0 || vertical != 0)
		{
			if (angle != 0 && canMove)
			{
				rigidB.rotation = Quaternion.Slerp(
					transform.rotation, 
					Quaternion.LookRotation(dir), 
					turnSpeed * Time.deltaTime
					);
			}
		}
	}

	void HandleMovementLockOn()
	{
		Transform camHolder = cam.parent.parent;
		
		// make sure corret scale
		Vector3 camForward = Vector3.Scale(camHolder.forward, new Vector3(1, 0, 1)).normalized;
		Vector3 camRight = Vector3.Scale(camHolder.right, new Vector3(1, 0, 1).normalized);

		// find move vector based on camera's direction
		Vector3 move = vertical * camForward + horizontal * cam.right;
		
		// forces for rigidbody
		Vector3 moveForward = camForward * vertical;
		Vector3 moveSideways = camRight * horizontal;

		// forces dependent upon move input
		rigidB.AddForce((moveForward + moveSideways).normalized * speed / Time.deltaTime);

		// call function
		SendMoveToAnimator(move);
	}

	void SendMoveToAnimator(Vector3 moveInput)
	{
		// convert move input rom world to local based on where we look
		Vector3 localMove = transform.InverseTransformDirection(moveInput);
		float turnAmount = localMove.x;
		float forwardAmount = localMove.z;

		// makes a smoother turn
		if (turnAmount != 0)
			turnAmount *= 2;

		animator.SetBool("lockOn", true);
		animator.SetFloat("forward", forwardAmount, 0.1f, Time.deltaTime);
		animator.SetFloat("sideways", turnAmount, 0.1f, Time.deltaTime);
	}

	void HandleRotationOnLock()
	{
		Vector3 lookPos = Enemies[curTarget].position;
		Vector3 lookDir = lookPos - transform.position;

		// don't account for height distances
		lookDir.y = 0;

		// rotate to look at target
		Quaternion rot = Quaternion.LookRotation(lookDir);
		rigidB.rotation = Quaternion.Slerp(rigidB.rotation, rot, Time.deltaTime * turnSpeed);
	}

	void HandleCameraTarget()
	{
		
	}

}
