using UnityEngine;
using System.Collections;

public class JungleTrigger : MonoBehaviour {

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
		
		string text = "Looks like some blocks managed to get themselves trapped here. They even make you look smart...";
		
		GameObject.Find("GUIManager").GetComponent<GUIManager>().ShowBalloon(text, CharacterController2D.CHARACTER_STATE.ANT, 10);
	
		
	}
}
