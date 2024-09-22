using UnityEngine;
using System.Collections;

public class FishSwimTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//transform.position = new Vector3( transform.position.x, GameObject.Find("Water").transform.position.y + 5, transform.position.z );
	}
	
	void OnTriggerEnter( Collider other )
	{
		if( other.name != "Character" )
			return;
		
		if( GameObject.Find("Character").GetComponent<CharacterController2D>().currentState == CharacterController2D.CHARACTER_STATE.FISH )
		{
			GameObject.Find("Character").GetComponent<CharacterController2D>().stopForcedUpdate = false;
			Debug.Log("SWIM TRIGGAR");
			GameObject.Find("Character").GetComponent<SpriteAnimator>().ForceStartAnimation("swim");
		}
	}
}
