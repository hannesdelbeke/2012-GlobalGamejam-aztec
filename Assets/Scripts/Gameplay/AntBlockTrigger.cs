using UnityEngine;
using System.Collections;

public class AntBlockTrigger : MonoBehaviour {

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
		
		GameObject.Find("Character").GetComponent<CharacterController2D>().currentlyWantedState = CharacterController2D.CHARACTER_STATE.ANT;
		
		//Debug.Log("TRIGGAR " + other.gameObject.name);
		
		//GameObject.Find("SimonSays").GetComponent<SimonSaysGraphics>().BeginMinigame( CharacterController2D.CHARACTER_STATE.RHINO );
		
		string ant = "Hey ugly! I'm your Intelligence spirit, which is why I'm so damn small! I can forge ahead when those other chumps can't, if you ever manage to summon me..";
		
		GameObject.Find("GUIManager").GetComponent<GUIManager>().ShowBalloon(ant, CharacterController2D.CHARACTER_STATE.ANT, 10);
	}
}
