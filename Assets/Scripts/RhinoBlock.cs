using UnityEngine;
using System.Collections;

public class RhinoBlock : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		// the block starts locked
		LockPosition();
	}
	
	protected Vector3 lockedPosition = Vector3.zero;
	
	public void LockPosition()
	{
		//rigidbody.constraints = new RigidbodyConstraints();
		//rigidbody.constraints.
		
		lockedPosition = transform.position;
	}
	
	public void UnlockPosition()
	{
		if( lockedPosition != Vector3.zero )
		{
			rigidbody.velocity = Vector3.zero;
			lockedPosition = Vector3.zero;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if( GameObject.Find("Character").GetComponent<CharacterController2D>().currentState == CharacterController2D.CHARACTER_STATE.RHINO )
		{
			Debug.Log("BLock Unlocked");
			UnlockPosition();
			//GameObject.Find("Character").GetComponent<CharacterController2D>().SwitchToState(CharacterController2D.CHARACTER_STATE.RHINO);
		}
			
		if( transform.position.x > 3.11f )
			transform.position = new Vector3(3.11f, transform.position.y, transform.position.z);
		
		if( lockedPosition != Vector3.zero )
			transform.position = lockedPosition;
	}
	
	bool enteredOnce = false;
	
	void OnCollisionEnter(Collision collisionInfo )
	{
		if( enteredOnce )
			return;
		
		if( GameObject.Find("Character").GetComponent<CharacterController2D>().currentState == CharacterController2D.CHARACTER_STATE.RHINO )
		{
			audio.PlayOneShot( this.audio.clip );
			enteredOnce = true;
		}
	}
}
