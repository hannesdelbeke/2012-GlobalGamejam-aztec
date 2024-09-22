using UnityEngine;
using System.Collections;

public class FishDeathTrigger : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	
	public Transform respawnPoint = null;
	
	void OnTriggerEnter( Collider other )
	{
		if( other.name == "Character" )
		{
			GameObject.Find("Character").GetComponent<SpriteAnimator>().StopForcedAnimation();
			GameObject.Find("Character").GetComponent<CharacterController2D>().stopForcedUpdate = true;
		}
		
		if( GameObject.Find("Water").transform.position.y < -43 )
		{
			string text = "Why is the rum always gone...";
			GameObject.Find("GUIManager").GetComponent<GUIManager>().ShowBalloon(text, CharacterController2D.CHARACTER_STATE.FISH, 5);
			
			// RESPAWN
			GameObject.Find("Character").transform.position = respawnPoint.position;
		}
		else
		{
			// PROCEED TO NEXT LEVEL
			string text = "Party on Wayne!";
			GameObject.Find("GUIManager").GetComponent<GUIManager>().ShowBalloon(text, CharacterController2D.CHARACTER_STATE.FISH, 5);
			
			Application.LoadLevel("antlevel");
		}
	
	}
}