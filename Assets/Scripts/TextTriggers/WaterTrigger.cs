using UnityEngine;
using System.Collections;

public class WaterTrigger : MonoBehaviour {

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
		
		string text = "Death curse tablets. Always entertaining to bring to a pool party ...";
		
		GameObject.Find("GUIManager").GetComponent<GUIManager>().ShowBalloon(text, CharacterController2D.CHARACTER_STATE.ANT, 10);
	
		
	}
	
}
