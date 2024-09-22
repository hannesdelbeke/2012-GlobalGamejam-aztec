using UnityEngine;
using System.Collections;

public class CharacterState : MonoBehaviour 
{
	/*
	public enum CHARACTER_STATE { NONE, HUMAN, RHINO, FISH, RABBIT, ANT }
	
	protected CHARACTER_STATE currentState = CHARACTER_STATE.HUMAN;
	
	*/
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if( Input.GetKeyDown(KeyCode.L) )
		//	SwitchToState(CHARACTER_STATE.RHINO);
	}
	
	/*
	public void SwitchToState( CHARACTER_STATE newState )
	{
		if( newState == currentState )
			return;
		
		GetComponent<SpriteAnimator>().ChangeTexture( newState );
	}
	*/
}
