using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		GameObject.Find("Character").GetComponent<CharacterController2D>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if( Input.GetMouseButtonDown(0) )
		{
			GetComponent<MeshRenderer>().enabled = false;
			GameObject.Find("Character").GetComponent<CharacterController2D>().enabled = true;
		}
		
	}
}
