using UnityEngine;
using System.Collections;

public class FishBlockTrigger : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( !triggeredOnce )
			return;
	}
	
	bool triggeredOnce = false;
	
	void OnTriggerEnter( Collider other )
	{
		if( triggeredOnce )
			return;
		
		triggeredOnce = true;
		
		GameObject.Find("Character").GetComponent<CharacterController2D>().currentlyWantedState = CharacterController2D.CHARACTER_STATE.FISH;
		
		//Debug.Log("TRIGGAR " + other.gameObject.name);
		
		//GameObject.Find("SimonSays").GetComponent<SimonSaysGraphics>().BeginMinigame( CharacterController2D.CHARACTER_STATE.RHINO );
		
		string fish = "Hello my dear.. I'm your Life spirit. You can call on me when you're looking down at certain death. Just don't squeeze too hard!";
		
		GameObject.Find("GUIManager").GetComponent<GUIManager>().ShowBalloon(fish, CharacterController2D.CHARACTER_STATE.FISH, 10);
	}
}