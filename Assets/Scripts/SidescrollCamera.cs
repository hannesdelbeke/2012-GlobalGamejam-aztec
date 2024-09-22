using UnityEngine;
using System.Collections;



public class SidescrollCamera : MonoBehaviour 
{
	public Transform target = null;
	public float distance = 15.0f;
	public float springiness = 4.0f;
	
	public Rect bounds;
	
	protected float originalZpos = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
		if( target == null )
			target = GameObject.Find("Character").transform;
		
		originalZpos = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public bool decrementY = true;
	
	void LateUpdate()
	{
		//Vector3 goal = GetGoalPosition();
		//transform.position = Vector3.Lerp( transform.position, goal, Time.deltaTime * springiness );//Vector3.Lerp( transform.position, goal, Time.deltaTime * springiness );
		transform.position = new Vector3(target.position.x, 0, target.position.z) + new Vector3(0,0, originalZpos);
		//EnforceLevelBounds();
		
		// camera viewport widht is 107 in world coordinates (measure in the level itself!)
		// half of that is 53
		// enforce level bounds:
		
		float offsetRight = 45;
		float offsetLeft = 53;
		if( transform.position.x - offsetLeft < bounds.x )
			transform.position = new Vector3(bounds.x + offsetLeft, transform.position.y, transform.position.z);
		else if( transform.position.x + offsetRight > bounds.xMax )
			transform.position = new Vector3(bounds.xMax - offsetRight, transform.position.y, transform.position.z);
		
		
		
		// make the y positon move down with the x-continuation of the level
		float percent = (transform.position.x - bounds.x) / (bounds.width / 100);
		//Debug.Log("PERCENTAGE " + percent );
		
		// percentage starts at 25 : highest y : 11.59
		// percentage lowest around 75 : lowest y : -7.16
		
		// so... in 50 percent we go from 11.59 to -7.16 : difference of 18.75 in height
		
		// y decrement: 1/50th of 18.75 per percent
		
		float newY = 11.59f - ( (percent - 25) * (18.75f / 50) );
		
		if( decrementY )
			transform.position = new Vector3(transform.position.x, newY, transform.position.z);
		
		
		
		
		
		//Debug.Log("LEVEL BOUNDS MAX " + bounds.xMax);
		
		// also force the target to stay within bounds
		//if( target.position.x - (target.localScale.x / 2) < bounds.x )
		//	target.position = new Vector3( bounds.x + (target.localScale.x / 2), target.position.y, target.position.z );
		
		
		if( target.position.x < bounds.x )
			target.position = new Vector3( bounds.x , target.position.y, target.position.z );
		
		// TODO: add check for bounds on the right side!!!
		
		
		
	}
	
	protected Vector3 lastTargetPosition = Vector3.zero;
	
	protected void EnforceLevelBounds()
	{
			Vector3 clampOffset = Vector3.zero;
	
		// Temporarily set the camera to the goal position so we can test positions for clamping.
		// But first, save the previous position.
		Vector3 cameraPositionSave = transform.position;
		Vector3 goalPosition = transform.position;
	
		// Get the target position in viewport space.  Viewport space is relative to the camera.
		// The bottom left is (0,0) and the upper right is (1,1)
		// @TODO Viewport space changing in Unity 2.0?
		Vector3 targetViewportPosition = camera.WorldToViewportPoint (target.position);
	
		// First clamp to the right and top.  After this we will clamp to the bottom and left, so it will override this
		// clamping if it needs to.  This only occurs if your level is really small so that the camera sees more than
		// the entire level at once.
		
		// What is the world position of the very upper right corner of the camera?
		Vector3 upperRightCameraInWorld = camera.ViewportToWorldPoint (new Vector3 (1.0f, 1.0f, targetViewportPosition.z));
	
		// Find out how far outside the world the camera is right now.
		clampOffset.x = Mathf.Min (bounds.xMax - upperRightCameraInWorld.x, 0.0f);
		clampOffset.y = Mathf.Min ((bounds.yMax - upperRightCameraInWorld.y), 0.0f);
		
		Debug.Log( clampOffset );
	
		// Now we apply our clamping to our goalPosition.  Now our camera won't go past the right and top boundaries of the level!
		goalPosition += clampOffset;
		
		// Now we do basically the same thing, except clamp to the lower left of the level.  This will override any previous clamping
		// if the level is really small.  That way you'll for sure never see past the lower-left of the level, but if the camera is
		// zoomed out too far for the level size, you will see past the right or top of the level.
		
		transform.position = goalPosition;
		Vector3 lowerLeftCameraInWorld = camera.ViewportToWorldPoint (new Vector3 (0.0f, 0.0f, targetViewportPosition.z));
	
		// Find out how far outside the world the camera is right now.
		clampOffset.x = Mathf.Max ((bounds.xMin - lowerLeftCameraInWorld.x), 0.0f);
		clampOffset.y = Mathf.Max ((bounds.yMin - lowerLeftCameraInWorld.y), 0.0f);
	
		// Now we apply our clamping to our goalPosition once again.  Now our camera won't go past the left and bottom boundaries of the level!
		goalPosition += clampOffset;
		
		transform.position = new Vector3(goalPosition.x, 0, transform.position.z);
	}
	
	protected Vector3 GetGoalPosition()
	{
		if( target == null )
			return transform.position;
		
		float heightOffset = 0.0f;
		float distanceModifier = 1.0f;
		float velocityLookAhead = 0.15f;
		Vector2 maxLookAhead = new Vector2(3.0f, 3.0f);
		
		Vector3 goalPosition = target.position + new Vector3( 0 , heightOffset, -distance * distanceModifier);
		
		if( lastTargetPosition != Vector3.zero )
		{
			Vector3 targetVelocity = target.position - lastTargetPosition;//target.GetComponent<Rigidbody>().velocity;
			
			Debug.Log("VELOCITY " + targetVelocity);
			
			Vector3 lookAhead = targetVelocity * velocityLookAhead;
			lookAhead.x = Mathf.Clamp (lookAhead.x, -maxLookAhead.x, maxLookAhead.x);
			lookAhead.y = Mathf.Clamp (lookAhead.y, -maxLookAhead.y, maxLookAhead.y);
			lookAhead.z = 0.0f;
			
			goalPosition += lookAhead;
			
			
			
		}
		
		
		
		
		lastTargetPosition = target.position;
		
		return goalPosition;
	}
}
/*
// This is for setting interpolation on our target, but making sure we don't permanently
// alter the target's interpolation setting.  This is used in the SetTarget () function.
private var savedInterpolationSetting = RigidbodyInterpolation.None;


// Based on the camera attributes and the target's special camera attributes, find out where the
// camera should move to.
function GetGoalPosition () {
	
	
	// To put the icing on the cake, we will make so the positions beyond the level boundaries
	// are never seen.  This gives your level a great contained feeling, with a definite beginning
	// and ending.
	
	var clampOffset = Vector3.zero;
	
	// Temporarily set the camera to the goal position so we can test positions for clamping.
	// But first, save the previous position.
	var cameraPositionSave = transform.position;
	transform.position = goalPosition;
	
	// Get the target position in viewport space.  Viewport space is relative to the camera.
	// The bottom left is (0,0) and the upper right is (1,1)
	// @TODO Viewport space changing in Unity 2.0?
	var targetViewportPosition = camera.WorldToViewportPoint (target.position);
	
	// First clamp to the right and top.  After this we will clamp to the bottom and left, so it will override this
	// clamping if it needs to.  This only occurs if your level is really small so that the camera sees more than
	// the entire level at once.
	
	// What is the world position of the very upper right corner of the camera?
	var upperRightCameraInWorld = camera.ViewportToWorldPoint (Vector3 (1.0, 1.0, targetViewportPosition.z));
	
	// Find out how far outside the world the camera is right now.
	clampOffset.x = Mathf.Min (levelBounds.xMax - upperRightCameraInWorld.x, 0.0);
	clampOffset.y = Mathf.Min ((levelBounds.yMax - upperRightCameraInWorld.y), 0.0);
	
	// Now we apply our clamping to our goalPosition.  Now our camera won't go past the right and top boundaries of the level!
	goalPosition += clampOffset;
	
	// Now we do basically the same thing, except clamp to the lower left of the level.  This will override any previous clamping
	// if the level is really small.  That way you'll for sure never see past the lower-left of the level, but if the camera is
	// zoomed out too far for the level size, you will see past the right or top of the level.
	
	transform.position = goalPosition;
	var lowerLeftCameraInWorld = camera.ViewportToWorldPoint (Vector3 (0.0, 0.0, targetViewportPosition.z));
	
	// Find out how far outside the world the camera is right now.
	clampOffset.x = Mathf.Max ((levelBounds.xMin - lowerLeftCameraInWorld.x), 0.0);
	clampOffset.y = Mathf.Max ((levelBounds.yMin - lowerLeftCameraInWorld.y), 0.0);
	
	// Now we apply our clamping to our goalPosition once again.  Now our camera won't go past the left and bottom boundaries of the level!
	goalPosition += clampOffset;
	
	// Now that we're done calling functions on the camera, we can set the position back to the saved position;
	transform.position = cameraPositionSave;
	
	// Send back our spiffily calculated goalPosition back to the caller!
	return goalPosition;
}
*/

