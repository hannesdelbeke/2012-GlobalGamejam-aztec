using UnityEngine;
using System.Collections;
using Aniballs.Sifteo;
using Sifteo;

public class CharacterController2D : MonoBehaviour 
{
	
	protected Vector3 originalRotation;
	public float speed = 10;
	public float jumpSpeed = 20;
	public float rabbitJumpSpeed = 40;
	
	protected GameControls mSifteo;
	protected MainControls mSifteoControls;
	protected MainControls.Animal mPrevAnimal = MainControls.Animal.none;
	
	protected float velocityEpsilon = 0.6f;
	
	protected bool facingRight = true;
	
	public bool isFacingRight()
	{
		return facingRight;
	}
	
	public bool IsRunningLeft()
	{
		
		float horizontal = Input.GetAxis("Horizontal"); 
		if( horizontal < -0.1f )
		{
			//if( !GameObject.Find("Background").audio.isPlaying ) GameObject.Find("Background").audio.Play();	
			return true;
		}
		else if( GameObject.Find("ControllerAbstraction").GetComponent<ControllerAbstraction>().Horizontal < -0.1f )
		{
			//if( !GameObject.Find("Background").audio.isPlaying ) GameObject.Find("Background").audio.Play();	
			return true;
		}
		//else{
			//Debug.Log( "SIFTEO GO " +  GameObject.Find("ControllerAbstraction").GetComponent<ControllerAbstraction>().Horizontal );
		//}
		
		return false;
		
		//return rigidbody.velocity.x < -velocityEpsilon;
	}
	
	public bool IsRunningRight()
	{
		float horizontal = Input.GetAxis("Horizontal"); 
		if( horizontal > 0.1f )
		{
		
			//if( !GameObject.Find("Background").audio.isPlaying ) GameObject.Find("Background").audio.Play();	
			return true;
		}
		else if( GameObject.Find("ControllerAbstraction").GetComponent<ControllerAbstraction>().Horizontal > 0.1f )
		{
			//if( !GameObject.Find("Background").audio.isPlaying ) GameObject.Find("Background").audio.Play();	
			return true;
		}
		
		return false;
		//return rigidbody.velocity.x > velocityEpsilon;
	}
	
	public bool IsIdle()
	{
		//Debug.Log("Velocity : " + rigidbody.velocity.x ); 
		
		float horizontal = Input.GetAxis("Horizontal"); 
		if( horizontal < -0.1f || horizontal > 0.1f )
			return false;
		else
		{
			horizontal = GameObject.Find("ControllerAbstraction").GetComponent<ControllerAbstraction>().Horizontal;
			if( horizontal < -0.1f || horizontal > 0.1f )
				return false;
			
		}
		
		//GameObject.Find("Background").audio.Stop();	
		
		return true;
		//return (rigidbody.velocity.x > -velocityEpsilon && rigidbody.velocity.x < velocityEpsilon);// && rigidbody.velocity.y == 0);
	}


	// Use this for initialization
	void Start () 
	{
		originalRotation = transform.eulerAngles;
		originalJumpSpeed = jumpSpeed;
		originalScale = transform.localScale;
		
	}
	
	protected void SifteoInit() {
		mSifteo = GameControls.instance;
		if(mSifteo != null) {
			mSifteoControls = mSifteo.MainControls;
		}
	}
	
	protected float previousVertical = 0;
	//public bool isJumping = false;
	
	protected float jumpingTimeout = 10.0f;
	
	public AudioClip jumpAudio = null;
	public AudioClip riseWaterAudio = null;
	
	public bool stopForcedUpdate = true;
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float horizontal = Input.GetAxis("Horizontal"); 
		float vertical = Input.GetAxis("Vertical");
		
		ProcessStateChanges();
		
		/*
		if( Input.GetKey(KeyCode.LeftArrow) )
			horizontal = -1;
		if( Input.GetKey(KeyCode.RightArrow) )
			horizontal = 1;
		
		if( Input.GetKey(KeyCode.UpArrow) )
			vertical = 1;
		if( Input.GetKey(KeyCode.DownArrow) )
			vertical = -1;
		*/
		
		if(mSifteo == null) { // TODO: this is a terrible hack... when can I be sure GameControls has been constructed?
			SifteoInit();
		} else {
			if(mSifteo.Running) {
				switch(mSifteoControls.VDirection) {
				case Cube.Side.TOP:
					vertical = 1;
					break;
				case Cube.Side.BOTTOM:
					vertical = -1;
					break;
				}
				switch(mSifteoControls.HDirection) {
				case Cube.Side.LEFT:
					horizontal = -1;
					break;
				case Cube.Side.RIGHT:
					horizontal = 1;
					break;
				}
			}
			
			if(mSifteoControls.Action) {
				// TODO: action button pressed
			}
			if(mSifteoControls.Reset) {
				// TODO: reset button hit
			}
			/*
			MainControls.Animal selectedAnimal = mSifteoControls.SelectedAnimal;
			if(selectedAnimal != mPrevAnimal) {
				// TODO: animal changed
			}
			mPrevAnimal = selectedAnimal;
			*/
			
			// TODO: highlight sifteo animal
			// mSifteoControls.HighlightAnimal = MainControls.Animal.ant;
		}
		/*
		if( isJumping && IsGrounded() )
		{
			isJumping = false;
			Debug.Log("JUMPING NO MORE");
		}
		*/

		//Debug.Log(" hor " + horizontal + " + " + vertical );
		
		//transform.position += new Vector3(horizontal * 5 * Time.deltaTime, 0, 0);
		//rigidbody.AddForce(  Vector3.right * horizontal * 5 * Time.deltaTime, ForceMode.Impulse );
		/*if( Input.GetKeyDown(KeyCode.RightArrow) )
		{
			rigidbody.AddForce( Vector3.right * 10, ForceMode.Impulse );
		}
		
		if( Input.GetKeyDown(KeyCode.LeftArrow) )
		{
			rigidbody.AddForce( Vector3.left * 10, ForceMode.Impulse );
		}
		*/
		
		if( freeMovement ) // Ant / Fish mode
		{
			DoFreeMovement(horizontal, vertical);
			return;	
		}
		
		Vector3 targetVelocity = Vector3.zero;
		
		//if( horizontal > 0 )
		//	rigidbody.AddForce (Vector3.right * 10 - rigidbody.velocity, ForceMode.Impulse);
		//else if( horizontal < 0 )
		//	rigidbody.AddForce (Vector3.left * 10 - rigidbody.velocity, ForceMode.Impulse);
		//else // full stop
		//	rigidbody.AddForce ( - rigidbody.velocity, ForceMode.Impulse);
		
		if( horizontal > 0 )
		{
			targetVelocity = Vector3.right * speed;
			facingRight = true;
		}
		else if( horizontal < 0 )
		{
			targetVelocity = Vector3.left * speed;
			facingRight = false; // = facing left
		}
		else
			targetVelocity = Vector3.zero;
		
		targetVelocity -= new Vector3( rigidbody.velocity.x, 0, 0);
		
		rigidbody.AddForce(targetVelocity, ForceMode.Impulse);
			
		
		//Debug.Log( rigidbody.velocity );
		
		//rigidbody.AddForce(  Vector3.right * horizontal * 5 * Time.deltaTime, ForceMode.VelocityChange );
		
		transform.eulerAngles = originalRotation;
		
		//Debug.Log("TIMEPUT " + jumpingTimeout + " GROUNDED " + IsGrounded());
		
		if( jumpingTimeout > 1.0f )
		{
			if( IsGrounded() )
			{
			//Debug.Log("DJUMP!");
				
				if( currentState == CHARACTER_STATE.HUMAN || currentState == CHARACTER_STATE.RABBIT )
				{
			
					if( Input.GetKeyDown(KeyCode.Space) || (vertical != previousVertical && vertical > 0))
					{
						Debug.Log("JUMPING!");
						rigidbody.AddForce( Vector3.up * jumpSpeed, ForceMode.Impulse );
						
						if( currentState == CHARACTER_STATE.RABBIT )
							GameObject.Find("Background").audio.PlayOneShot(jumpAudio);
						
						jumpingTimeout = 0.0f;
					}
				}
			}
		}
		
		if( currentState == CHARACTER_STATE.FISH ) // so this means : fish and freemove is off : not yet in the water
		{
			if( Input.GetKeyDown(KeyCode.Space) || (vertical > 0))
			{
				GetComponent<SpriteAnimator>().ForceStartAnimation("water");
				
				if( !waterRisePlayed )
				{
					Debug.Log("RISE WATER AUDIO");
					GameObject.Find("Background").audio.PlayOneShot(riseWaterAudio);
					
					waterRisePlayed = true;
				}
				
				Transform water = GameObject.Find("Water").transform;
				if( water.position.y < -42 )
				{
					water.position = new Vector3( water.position.x, water.position.y + 0.1f, water.position.z );					
				}
			}
			else
			{
				if( stopForcedUpdate )
					GetComponent<SpriteAnimator>().StopForcedAnimation();
			}
		}
		
		
		/*
		if( jumpingTimeout > 1.0f )
		{
			if( IsGrounded() )
			{
			//Debug.Log("DJUMP!");
			
				if( Input.GetKeyDown(KeyCode.Space) || (vertical != previousVertical && vertical > 0))
				{
					//Debug.Log("JUMPING!");
					//rigidbody.AddForce( Vector3.up * jumpSpeed, ForceMode.Impulse );
					
					jumpingTimeout = 0.0f;
				}
			}
			else
				rigidbody.AddForce( Vector3.down * jumpingTimeout * 4, ForceMode.Impulse);
		}
		else // recently started jumping
		{
			rigidbody.AddForce( Vector3.up * jumpingTimeout * jumpSpeed, ForceMode.Impulse );
		}
		*/
		
		jumpingTimeout += Time.deltaTime;
		
		previousVertical = vertical;
		
		//if( !isGrounded()
		
		
	}
	
	protected bool waterRisePlayed = false;
	
	public bool IsGrounded()
	{
		RaycastHit hit;
		// cast a ray downward: if hit distance is under limit : grounded. Otherwise : jumping
		if( Physics.Raycast(transform.position, Vector3.down, out hit) )
		{
			//Debug.Log("BELOW : " + hit.distance );
			
			if( hit.distance > 4.50 && hit.distance < 7.0f)//6.1) //5.8 ) // between 5.40 and 6.3 (for slopes) we are grounded 
				return true;
			
			if( currentState == CHARACTER_STATE.ANT )
			{
				return true;
				//Debug.Log("BELOW : " + hit.distance );
				//if( hit.distance > 2.30f && hit.distance < 3.0f )
				//	return true;
			}
		}
		
		return false;
	}
	
	protected string pushColliderName = "";
	protected int pushColliderID = -1;
	
	protected int pushingCollisionCount = 0;
	
	public bool freeMovement = false;
	
	void OnCollisionEnter( Collision collision  )
	{
		if( freeMovement )
			return;
		
		//Debug.Log(" COLLIDED " + collision.collider.name);
		
		//if( collision.collider.name == "RhinoPushTrigger" )
		//	Debug.LogError("HERE BE DRAGONS!!!");
		/*
		if( collision.collider.name == "RhinoPushTrigger" )
		{
			Debug.Log("START PUSHING");
			GetComponent<SpriteAnimator>().ForceStartAnimation("pushing");
			
			pushingCollisionCount++;
		}
		*/
		
		
		//return;
		
		//Debug.Log("COLLIDED : " + collision.relativeVelocity);
		
		
		//Debug.Log("NR OF CONTACTS : " + collision.contacts.Length);
		
		//Debug.DrawRay( collision.contacts[0].point, collision.contacts[0].normal );
		
		float angleLeft = Vector3.Angle(Vector3.left, collision.contacts[0].normal);
		float angleRight = Vector3.Angle(Vector3.right, collision.contacts[0].normal);
		
		//Debug.Log("Angles " + angleLeft + " -> " + angleRight );
		
		float pushEpsilon = 1f; // slope angle is about 5 - 10
		if( angleLeft < pushEpsilon || angleRight < pushEpsilon )
		{
			GetComponent<SpriteAnimator>().ForceStartAnimation("pushing");
			//pushColliderName = collision.collider.name;
			pushColliderID = collision.collider.GetInstanceID();
			
			Debug.Log("PUSHCOLLIDER : " + pushColliderID);
		}
		
	}
	
	void OnCollisionStay( Collision collision )
	{
		if( freeMovement )
			return;
		
		//float angleLeft = Vector3.Angle(Vector3.left, collision.contacts[0].normal);
		//float angleRight = Vector3.Angle(Vector3.right, collision.contacts[0].normal);
		float angleUp = Vector3.Angle(Vector3.up, collision.contacts[0].normal);
		
		//Debug.Log("ANGLE UP : " + angleUp);
		
		float slopeEpsilon = 45.0f;
		
		//Debug.DrawRay( collision.contacts[0].point, collision.contacts[0].normal * 10 );
		//Debug.DrawRay( collision.contacts[0].point, collision.relativeVelocity * 10, UnityEngine.Color.red );
		
		if( angleUp > slopeEpsilon )
		{
			//Debug.Log("SLOPING! " + collision.relativeVelocity);
			//rigidbody.AddForce( /*collision.contacts[0].normal * 10*/ collision.relativeVelocity * 10, ForceMode.Impulse );
			
			
			rigidbody.AddForce( new Vector3(0.0f, -9.81f * rigidbody.mass, 0.0f), ForceMode.Impulse );//, ForceMode.Impulse );
			//transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
		}
	}
	
	void OnCollisionExit( Collision collision )
	{
		if( freeMovement )
			return;
		
		//Debug.Log(" COLLIDE STOP " + collision.collider.name);
		
		if( collision.collider.GetInstanceID() == pushColliderID )
		//if( collision.collider.name == "RhinoPushTrigger" ) 
		{
			Debug.Log("STOP PUSHING " + pushingCollisionCount);
			GetComponent<SpriteAnimator>().StopForcedAnimation();
		}
		
	}
	
	
	protected void DoFreeMovement(float horizontal, float vertical)
	{
		//Debug.Log("FreeMove : " + horizontal + " / " + vertical);
		
		Vector3  targetVelocity = Vector3.zero;
		float speedFree = 0.2f;
		
		float x = transform.position.x;
		float y = transform.position.y;
		
		if( horizontal > 0 )
		{
			targetVelocity = Vector3.right * speedFree;
			x += speedFree;
			facingRight = true;
		}
		else if( horizontal < 0 )
		{
			targetVelocity = Vector3.left * speedFree;
			x -= speedFree;
			facingRight = false; // = facing left
		}
		else
			targetVelocity = Vector3.zero;
		
		if( vertical > 0 )
		{
			targetVelocity = Vector3.up * speedFree;
			y += speedFree;
		}
		else if( vertical < 0 )
		{
			targetVelocity = Vector3.down * speedFree;
			y -= speedFree;
		}
		else
			targetVelocity = Vector3.zero;
		
		//targetVelocity -= new Vector3( rigidbody.velocity.x, 0, 0); // constant speed
		
		//rigidbody.velocity = Vector3.zero;
		
		
		//rigidbody.AddForce(targetVelocity, ForceMode.Impulse);
		
		transform.position = new Vector3( x, y, transform.position.z );
		
	}
	
	
	
	public enum CHARACTER_STATE { NONE, HUMAN, RHINO, FISH, RABBIT, ANT }
	
	public CHARACTER_STATE currentState = CHARACTER_STATE.HUMAN;
	
	
	protected float originalJumpSpeed = 0.0f;
	protected Vector3 originalScale = Vector3.zero;
	
	public void SwitchToState( CHARACTER_STATE newState )
	{
		Debug.Log("NEW STATE : " + newState.ToString() + " -> " + currentState); 
		if( newState == currentState )
			return;
		
		GetComponent<SpriteAnimator>().ChangeTexture( newState );
		
		currentState = newState;
		
		if( newState == CHARACTER_STATE.RABBIT )
		{
			originalJumpSpeed = jumpSpeed;
			jumpSpeed = rabbitJumpSpeed;
		}
		else
			jumpSpeed = originalJumpSpeed;
		
		if( newState == CHARACTER_STATE.ANT )
		{
			transform.localScale = new Vector3( transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z / 2 );
			//freeMovement = true;
		}
		else
		{
			//freeMovement = false;
			transform.localScale = originalScale;
		}
			
	}
	
	public void StateSwitchFailed( CHARACTER_STATE type )
	{
		GameObject.Find("GUIManager").GetComponent<GUIManager>().ShowBalloon("YOU TOTALLY FAILED, HAHA", type, 5);
	}
	
	public CHARACTER_STATE currentlyWantedState = CHARACTER_STATE.NONE;
	
	public void ProcessStateChanges()
	{
		//if( currentlyWantedState == CHARACTER_STATE.NONE )
		//	return;
		
		CHARACTER_STATE requestedState = CHARACTER_STATE.NONE;
		
		ControllerAbstraction c = GameObject.Find("ControllerAbstraction").GetComponent<ControllerAbstraction>();
		
		if( Input.GetKeyDown(KeyCode.Alpha1) || c.Rhino ) // RHINO
			requestedState = CHARACTER_STATE.RHINO;
		else if( Input.GetKeyDown(KeyCode.Alpha2) || c.Rabbit ) // RABBIT
			requestedState = CHARACTER_STATE.RABBIT;
		else if( Input.GetKeyDown(KeyCode.Alpha3) || c.Ant ) // ANT
			requestedState = CHARACTER_STATE.ANT;
		else if( Input.GetKeyDown(KeyCode.Alpha4) || c.Fish ) // FISH
			requestedState = CHARACTER_STATE.FISH;
		
		if( requestedState != CHARACTER_STATE.NONE )
		{
		
			// if the requested state is the same as the one we expect: try to get it through simon says	
			if( currentlyWantedState == requestedState)
			{
				//simonsays will call SwitchState if successfull
				GameObject.Find("SimonSays").GetComponent<SimonSaysGraphics>().BeginMinigame(requestedState);
			}
			else
				GameObject.Find("GUIManager").GetComponent<GUIManager>().ShowBalloon("Nope... not this one", requestedState, 5);
			
		}
	
	}
	
	
}
