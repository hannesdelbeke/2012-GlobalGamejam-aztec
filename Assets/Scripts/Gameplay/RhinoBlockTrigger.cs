using UnityEngine;
using System.Collections;

public class RhinoBlockTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	bool triggeredOnce = false;
	
	void OnTriggerEnter( Collider other )
	{
		if( triggeredOnce )
			return;
		
		triggeredOnce = true;
		
		GameObject.Find("Character").GetComponent<CharacterController2D>().currentlyWantedState = CharacterController2D.CHARACTER_STATE.RHINO;
		
		//Debug.Log("TRIGGAR " + other.gameObject.name);
		
		//GameObject.Find("SimonSays").GetComponent<SimonSaysGraphics>().BeginMinigame( CharacterController2D.CHARACTER_STATE.RHINO );
		
		string rhino = "Greetings... I am your Strength spirit. \nSummoning me will give you great physical power - and looks.";
		
		GameObject.Find("GUIManager").GetComponent<GUIManager>().ShowBalloon(rhino, CharacterController2D.CHARACTER_STATE.RHINO, 8);
	}
}
