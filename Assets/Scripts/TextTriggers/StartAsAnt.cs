using UnityEngine;
using System.Collections;

public class StartAsAnt : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		GetComponent<CharacterController2D>().SwitchToState( CharacterController2D.CHARACTER_STATE.ANT );
		Debug.Log("StateHasBeenSwitched");
		
		GetComponent<CharacterController2D>().rigidbody.useGravity = false;
		GetComponent<CharacterController2D>().rigidbody.velocity = Vector3.zero;
		GetComponent<CharacterController2D>().freeMovement = true;
		
		GameObject.Find("AntPathDrawer").GetComponent<PathInterface>().enabled = true;
		
		
		string text = "Lucky you have me to save you from dissolving into goo. We can cut through here as long as you avoid the bubbles. The trapped, stale air will kill you instantly – and being inside a mortal when they die is tedious...";
		GameObject.Find("GUIManager").GetComponent<GUIManager>().ShowBalloon(text, CharacterController2D.CHARACTER_STATE.ANT, 10);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
