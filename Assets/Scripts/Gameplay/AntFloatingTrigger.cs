using UnityEngine;
using System.Collections;

public class AntFloatingTrigger : MonoBehaviour 
{
	
	//public bool disableGravity = true;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	bool usedOnce = false;
	
	void OnTriggerEnter( Collider other )
	{
		if( usedOnce )
			return;
		
		usedOnce = true;
		
		Rigidbody rb = other.collider.attachedRigidbody;
		/*
		if( disableGravity )
		{
			rb.useGravity = false;
			//rb.isKinematic = true;
		}
		else
		{
			rb.useGravity = true;
			//rb.isKinematic = false;
		}
		*/
		
		rb.useGravity = !rb.useGravity;
		//rb.isKinematic = !rb.isKinematic;
		
		if( !rb.useGravity )
		{
			GameObject.Find("Character").GetComponent<CharacterController2D>().rigidbody.velocity = Vector3.zero;
			GameObject.Find("Character").GetComponent<CharacterController2D>().freeMovement = true;
			GameObject.Find("AntPathDrawer").GetComponent<PathInterface>().enabled = true;
			
			//Camera.main.orthographicSize = 20;
			StartCoroutine( zoomCamera(25) );
			
		}
		else
		{
			GameObject.Find("AntPathDrawer").GetComponent<PathInterface>().enabled = false;
			//Camera.main.orthographicSize = 40;
			StartCoroutine( zoomCamera(40) );
		}
			
	
		Debug.Log("DOING GRAVITY");
	}
	
	IEnumerator zoomCamera( float size )
	{
		while( Mathf.Abs(Camera.main.orthographicSize - size) > 0.5f )
		{
			Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, size, Time.deltaTime);
			yield return null;
		}
		
		yield return null;
	}
	
	void OnTriggerExit( Collider other )
	{
		GetComponent<BoxCollider>().isTrigger = false;
		
		if( other.collider.attachedRigidbody.useGravity )
		{
			GameObject.Find("Character").GetComponent<CharacterController2D>().freeMovement = false;
		}
	}
}
