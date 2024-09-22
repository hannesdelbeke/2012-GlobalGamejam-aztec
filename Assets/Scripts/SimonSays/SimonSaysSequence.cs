using UnityEngine;
using System;
using System.Collections.Generic;


public class SimonSaysSequence : MonoBehaviour  
{
	protected SimonSays[] mSequence;
	protected int mCurrentIndex = 0;
	protected float mTimer = 0;
	protected bool mRunning = false;
	protected static System.Random random = new System.Random();
	
	public ControllerAbstraction Controller;
	public SimonSaysGraphics graphics;
	public bool  IncludePress  = false;
	public bool  IncludeAnt    = false;
	public bool  IncludeFish   = false;
	public bool  IncludeRhino  = false;
	public bool  IncludeRabbit = false;
	public int   Length = 3;
	public float SecondsToRespond = 5.0f;
	
	public SimonSays CurrentSimon 
	{
		get {
			if(mCurrentIndex >= mSequence.Length) {
				return SimonSays.none; // this is actually an error...
			}
			return mSequence[mCurrentIndex];
		}
	}
	
	public bool SimonRunning 
	{
		get 
		{
			return mRunning;
		}
	}
	
	public float TimeLeft 
	{
		get {
			return SecondsToRespond - mTimer;
		}
	}
	
	public SimonSaysSequence() 
	{
		
	}
	
	void Start () 
	{
		
	}
	
	public AudioClip down = null;
	public AudioClip up = null;
	public AudioClip left = null;
	public AudioClip right = null;
	
	public void StartNewRound()
	{
		List<SimonSays> options = new List<SimonSays>();
		options.Add(SimonSays.up);
		options.Add(SimonSays.left);
		options.Add(SimonSays.down);
		options.Add(SimonSays.right);
		if(IncludePress)  options.Add(SimonSays.press);
		if(IncludeAnt)    options.Add(SimonSays.ant);
		if(IncludeFish)   options.Add(SimonSays.fish);
		if(IncludeRhino)  options.Add(SimonSays.rhino);
		if(IncludeRabbit) options.Add(SimonSays.rabbit);
		
		mSequence = new SimonSays[Length];
		for(int i = 0; i < Length; i++) 
		{
			mSequence[i] = options[random.Next(options.Count)];
			
			// make sure we don;t have 2 the same options after another
			if( i != 0 && mSequence[i] == mSequence[ i - 1 ] )
				i--;
		}
		
		DoStartSequence();
	}
	
	void DoStartSequence() 
	{
		mCurrentIndex = 0;
		mTimer = 0;
		mRunning = true;
		
		foreach( SimonSays s in mSequence )
			Debug.Log( s.ToString() );
		
		graphics.ShowDirection( CurrentSimon.ToString() );
	}
	
	protected float detectionTimeout = 5.0f;
	
	void Update() 
	{
		if(!mRunning)
		{
			// If Simon is silent... don't do anything
			return;
		}
		
		if(Controller == null) {
			// ERROR - should be set
			Debug.Log("Error: SimonSaysSequence is missing a ControllerAbstraction!");
			return;
		}
		
		if( Controller.UsingSifteo() )
		{
			Debug.Log("USING SIFTEO");
			
			detectionTimeout += Time.deltaTime;
			
			if( detectionTimeout < 1.0f ) // wait 1 second between answers (to account for sifteo double-axis-edness)
				return; 
			
			detectionTimeout = 0.0f;
		}
		
		//Debug.Log("Current : " + CurrentSimon.ToString() );
		
		if(CurrentSimon == SimonSays.up && Controller.Up
			|| CurrentSimon == SimonSays.left && Controller.Left
			|| CurrentSimon == SimonSays.down && Controller.Down
			|| CurrentSimon == SimonSays.right && Controller.Right
			|| CurrentSimon == SimonSays.press && Controller.Action
			|| CurrentSimon == SimonSays.rhino && Controller.Rhino
			|| CurrentSimon == SimonSays.rabbit && Controller.Rabbit
			|| CurrentSimon == SimonSays.ant && Controller.Ant
			|| CurrentSimon == SimonSays.fish && Controller.Fish || Input.GetKeyDown(KeyCode.S) ) 
		{
			if( CurrentSimon == SimonSays.left )
				audio.PlayOneShot(left, 5);
			else if( CurrentSimon == SimonSays.right )
				audio.PlayOneShot(right, 5);
			else if( CurrentSimon == SimonSays.up )
				audio.PlayOneShot(up, 5);
			else if( CurrentSimon == SimonSays.down )
				audio.PlayOneShot(down, 5);
			
			// Correct: move to next item in sequence
			mCurrentIndex++;
			mTimer = 0.0f;
			
			
			if(mCurrentIndex >= mSequence.Length || Input.GetKeyDown(KeyCode.S)) 
			{
				// WIN!!
				mCurrentIndex = 0;
				mRunning = false;
				
				Debug.Log("YOU WIN FROM SIMON!");
				
				graphics.EndMinigame(true);
				
				return;
			}
			else
			{
				graphics.ShowDirection( CurrentSimon.ToString() );
			}
		}
		
		float frameTime = Time.deltaTime;//1.0f / 50.0f; // TODO: fill in actual frame time from Unity
		
		mTimer += frameTime;
		if(mTimer > SecondsToRespond) 
		{
			// failed to respond correctly
			// TODO: close SimonSays - let player restart
			mCurrentIndex = 0;
			mRunning = false;
			
			Debug.Log("YOU LOSE, SIMON WINS!");
			graphics.EndMinigame(false);
		}
	}
	
	public enum SimonSays {
		up = 0,
		left,
		down,
		right,
		press,
		rhino,
		rabbit,
		ant,
		fish,
		max,
		animals = ant,
		none = max
	}
}