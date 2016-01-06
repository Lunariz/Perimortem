using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	//Movement
	public float maxSpeed;
	public float accelerationSpeed;
	public float speedSoftCap;
	float capAcceleration;
	public float jumpForce;
	public float gravity;
	public float maxFallSpeed;
	public float fallSpeedSoftCap;
	public float wallJumpDelay;

	Rigidbody2D rigidbody;

	//Colliders and collisions
	public ColliderCounter rightCol;
	public ColliderCounter leftCol;
	public ColliderCounter bottomCol;
	public ColliderCounter topCol;

	int touchingBottom = 0;
	int touchingRight = 0;
	int touchingLeft = 0;

	[HideInInspector] public float horSpeed = 0;
	[HideInInspector] public float verSpeed = 0;

	bool doubleJumped = false;
	bool canDoubleJump = false;
	bool wallJumped = false;

	float rightJumpDelay = 0;
	float leftJumpDelay = 0;

	private float m_curGravityMult = 1.0f;
	public float Friction = 0.95f;

	SkeletonAnimation ani;
	string currentAnimation;

	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
		ani = GetComponentInChildren<SkeletonAnimation> ();
		rightCol.triggerEnter = RightColEnter;
		rightCol.triggerExit = RightColExit;
		leftCol.triggerEnter = LeftColEnter;
		leftCol.triggerExit = LeftColExit;
		bottomCol.triggerEnter = BottomColEnter;
		bottomCol.triggerExit = BottomColExit;
		topCol.triggerEnter = TopColEnter;
		topCol.triggerExit = TopColExit;

		capAcceleration = accelerationSpeed / 20f;
	}

	private float footstepTimer;

	void FixedUpdate () {
		var velocity = rigidbody.velocity;
		horSpeed = velocity.x;
		verSpeed = velocity.y;

		leftJumpDelay -= Time.deltaTime;
		rightJumpDelay -= Time.deltaTime;

		if(!Grab ())
		{
			CalculateHorizontalSpeed();
			CalculateVerticalSpeed();
		}


		//Factor gravity
		verSpeed = Mathf.Max(-maxFallSpeed, verSpeed - gravity * m_curGravityMult * Time.deltaTime * 60.0f);

		var delta = new Vector2(horSpeed - velocity.x, verSpeed - velocity.y);

		rigidbody.AddForce(delta, ForceMode2D.Impulse);
		//rigidbody.velocity = new Vector2(horSpeed, verSpeed);

		//rigidbody.velocity = new Vector2(horSpeed, verSpeed);
		
		if (horSpeed >= 2f) {
			if (footstepTimer <= 0) {
				if (Random.value > 0.5)
					AudioManager.instance.PlaySound(Audio.Footstep1);
				else
					AudioManager.instance.PlaySound(Audio.Footstep3);
				footstepTimer = 0.3f;
			} else {
				footstepTimer -= Time.deltaTime;
			}
		}
	}

	private void CalculateHorizontalSpeed() {
		float input = Input.GetAxis("Horizontal");

		//Accelerate
		if (Mathf.Abs(input) > 0.5f) 
		{
			if(!Airborne)
			{
				if(input > 0.5f)
					FaceAniRight();
				else
					FaceAniLeft();
				SetAnimation("Run" , true);
			}
			float curAccel;

			if (Mathf.Abs(horSpeed) > speedSoftCap) {
				curAccel = capAcceleration;
			} else {
				curAccel = accelerationSpeed;
			}

			horSpeed += input * curAccel * Time.deltaTime * 60.0f;
		}
		//Decelerate 
		else {
			horSpeed *= Mathf.Pow(Friction, Time.deltaTime);
			if(!Airborne)// && ((horSpeed < 0.5f && horSpeed > 0) || (horSpeed > -0.5f && horSpeed < 0)))
				SetAnimation("Idle" , true);
		}
	}

	void CalculateVerticalSpeed()
	{
		//Handle jumping
		if(Input.GetAxis("Jump") > 0.1f  && !Airborne) //First jump
		{
			verSpeed = jumpForce;
            AudioManager.instance.PlaySound(Audio.Jump);
			SetAnimation("JumpStart" , false);
		}
		else if(Input.GetAxis("Jump") > 0.1f && canDoubleJump)//Double jump in air
		{
			verSpeed = jumpForce;
			doubleJumped = true;
			canDoubleJump = false;
            AudioManager.instance.PlaySound(Audio.Jump);
			SetAnimation("Idle" , true);
			SetAnimation("JumpStart" , false);
		}
		else if(Airborne && Input.GetAxis("Jump") == 0 && !doubleJumped) //Check if double jump should be available
			canDoubleJump = true;
	}

	bool Grab()
	{
		float input = Input.GetAxis("Horizontal");

		//Walljump
		if (Airborne && CanWJRight && Input.GetButtonDown ("Jump")) {
			verSpeed = jumpForce;
			horSpeed = maxSpeed * 0.6f;
			wallJumped = true;
			doubleJumped = false;
			AudioManager.instance.PlaySound (Audio.Jump);
			m_curGravityMult = 1.0f;
			SetAnimation("WallJump" , false);
			FaceAniRight();
		} else if (Airborne && CanWJLeft && Input.GetButtonDown ("Jump")) {
			verSpeed = jumpForce;
			horSpeed = maxSpeed * -0.6f;
			wallJumped = true;
			doubleJumped = false;
			AudioManager.instance.PlaySound (Audio.Jump);
			m_curGravityMult = 1.0f;
			SetAnimation ("WallJump", false);
			FaceAniLeft ();
		}

		//Grab
		else if (Airborne && input > 0.5f && touchingRight > 0) {
			verSpeed *= 0.3f;
			m_curGravityMult = 0.1f;
			horSpeed = 0;
			doubleJumped = true;
			SetAnimation("Idle" , true);
			SetAnimation ("WallJump", false, 0);
			FaceAniLeft ();
			return true;
		} else if (Airborne && input < -0.5f && touchingLeft > 0) {
			verSpeed *= 0.3f;
			m_curGravityMult = 0.1f;
			horSpeed = 0;
			doubleJumped = true;
			SetAnimation("Idle" , true);
			SetAnimation ("WallJump", false, 0);
			FaceAniRight ();
			return true;
		}
		else {
			m_curGravityMult = 1.0f;
		}

		return false;
	}

	bool Airborne	{ get {return touchingBottom == 0;}	}
	bool CanWJRight { get{ return touchingLeft > 0 || rightJumpDelay > 0 && !wallJumped; } }
	bool CanWJLeft	{ get{ return touchingRight > 0 || leftJumpDelay > 0 && !wallJumped; } }

	void FaceAniRight()
	{
		if (ani.transform.localScale.x < 0)
			ani.transform.localScale = new Vector3 (-ani.transform.localScale.x, ani.transform.localScale.y, ani.transform.localScale.y);
	}
	void FaceAniLeft()
	{
		if (ani.transform.localScale.x > 0)
			ani.transform.localScale = new Vector3 (-ani.transform.localScale.x, ani.transform.localScale.y, ani.transform.localScale.y);
	}
	void SetAnimation (string anim, bool loop, float speed = 1f) 
	{
		if (currentAnimation != anim) {
			ani.state.SetAnimation(0, anim, loop);
			currentAnimation = anim;
		}
		ani.timeScale = speed;
	}

	#region CollisionManagement
	public void BottomColEnter()
	{
		touchingBottom++;
		doubleJumped = false;
		canDoubleJump = false;
	}
	public void BottomColExit()
	{
		touchingBottom--;
	}
	public void RightColEnter()
	{
		touchingRight++;
		wallJumped = false;
	}
	public void RightColExit()
	{
		touchingRight--;
		if (touchingRight == 0) 
			leftJumpDelay = wallJumpDelay;
	}
	public void LeftColEnter()
	{
		touchingLeft++;
		wallJumped = false;
	}
	public void LeftColExit()
	{
		touchingLeft--;
		if (touchingLeft == 0)
			rightJumpDelay = wallJumpDelay;
	}
	public void TopColEnter()
	{
	}
	public void TopColExit()
	{
	}
	#endregion

}
