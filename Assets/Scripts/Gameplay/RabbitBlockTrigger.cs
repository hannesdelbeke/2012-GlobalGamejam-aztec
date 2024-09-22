using UnityEngine;
using System.Collections;

public class RabbitBlockTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( !triggeredOnce )
			return;
		/*
		if( Input.GetKeyDown(KeyCode.Alpha1) )
		{
			CharacterController2D c = GameObject.Find("Character").GetComponent<CharacterController2D>();
			c.SwitchToState(CharacterController2D.CHARACTER_STATE.RABBIT);
			c.jumpSpeed = c.jumpSpeed * 2;
		}
		*/
	}
	
	bool triggeredOnce = false;
	
	void OnTriggerEnter( Collider other )
	{
		if( triggeredOnce )
			return;
		
		triggeredOnce = true;
		
		GameObject.Find("Character").GetComponent<CharacterController2D>().currentlyWantedState = CharacterController2D.CHARACTER_STATE.RABBIT;
		
		//Debug.Log("TRIGGAR " + other.gameObject.name);
		
		//GameObject.Find("SimonSays").GetComponent<SimonSaysGraphics>().BeginMinigame( CharacterController2D.CHARACTER_STATE.RHINO );
		
		string rabbit = "Hi there. I'm your Will spirit. Summoning me always drives you to new heights! Hair extensions, (get it?) at no extra cost!";
		
		GameObject.Find("GUIManager").GetComponent<GUIManager>().ShowBalloon(rabbit, CharacterController2D.CHARACTER_STATE.RABBIT, 10);
	}
}
