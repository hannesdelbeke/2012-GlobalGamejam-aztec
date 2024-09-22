using UnityEngine;
using System.Collections;

public class EvilTrigger : MonoBehaviour {

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
		
		string text = "This guy.. whatever you do don't look into its eyes. Unless you want your skin to melf off of your face.";
		
		GameObject.Find("GUIManager").GetComponent<GUIManager>().ShowBalloon(text, CharacterController2D.CHARACTER_STATE.ANT, 10);
	
		
	}
}
