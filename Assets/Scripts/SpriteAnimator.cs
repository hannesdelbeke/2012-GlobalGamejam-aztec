using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteAnimator : MonoBehaviour {
	
	public CharacterController2D characterController = null;
	
	public int frameCount = 8; // number of frames on 1 row (SQUARE!)
	
	public Texture2D humanSprite = null;
	public Texture2D rhinoSprite = null;
	public Texture2D rabbitSprite = null;
	public Texture2D antSprite = null;
	public Texture2D fishSprite = null;
	
	protected class AnimationData
	{
		public int rowIndex;
		public int columnCount;
		public int startColumnIndex;
		
		public int currentFrame;
		public int framesPerSecond;
		
		public AnimationData(int rowIndex, int startColumnIndex, int columnCount)
		{
			this.rowIndex = rowIndex;
			this.columnCount = columnCount;
			this.startColumnIndex = startColumnIndex;
			
			this.currentFrame = startColumnIndex;
			this.framesPerSecond = columnCount;
		}
		
		public AnimationData(int rowIndex, int startColumnIndex, int columnCount, int framesPerSecond)
		{
			this.rowIndex = rowIndex;
			this.columnCount = columnCount;
			this.startColumnIndex = startColumnIndex;
			
			this.currentFrame = startColumnIndex;
			this.framesPerSecond = framesPerSecond;
		}
	}
	
	protected Dictionary<string, AnimationData> animationData = new Dictionary<string, AnimationData>();
	protected AnimationData currentAnimation = null;
	
	// Use this for initialization
	void Start () 
	{
		/*
		Mesh m = GetComponent<MeshFilter>().mesh;
		foreach( Vector2 v in m.uv )
			Debug.Log( v.x + " , " + v.y );
			*/
		
		//m.bounds.
		//Debug.Log(m.uv);
		
		if( characterController == null )
			characterController = GetComponent<CharacterController2D>();
		
		animationData.Add( "idle", new AnimationData(7, 0, 8) );
		animationData.Add( "walk", new AnimationData(6, 0, 8) );
		animationData.Add( "jump", new AnimationData(3, 0, 8) );
		animationData.Add( "pushing", new AnimationData(5, 0, 8) );
		animationData.Add( "water", new AnimationData(4, 0, 8) );
		animationData.Add( "swim", new AnimationData(2, 0, 8) );
		animationData.Add( "falling", new AnimationData(3, 4, 1, 1) );
		
		currentAnimation = animationData[ "idle" ];
	}
	
	public void ChangeTexture(CharacterController2D.CHARACTER_STATE state)
	{
		Debug.Log("Changing texture : " + state.ToString());
		
		Material animMat = GetComponent<MeshRenderer>().material;
		
		switch( state )
		{
		case CharacterController2D.CHARACTER_STATE.HUMAN:
			animMat.mainTexture = humanSprite;
			break;
			
		case CharacterController2D.CHARACTER_STATE.RHINO:
			animMat.mainTexture = rhinoSprite;
			break;
			
		case CharacterController2D.CHARACTER_STATE.RABBIT:
			animMat.mainTexture = rabbitSprite;
			break;
			
		case CharacterController2D.CHARACTER_STATE.ANT:
			animMat.mainTexture = antSprite;
			break;
			
		case CharacterController2D.CHARACTER_STATE.FISH:
			animMat.mainTexture = fishSprite;
			break;
		}
	}
	
	
	//float currentX = 0.0f;
	//float currentY = (1.0f / 8) * 6;
	
	protected float currentFrameTime = 0.0f;
	
	
	protected string previousAnimationName = "idle";
	//protected string 
	
	protected bool animationIsExternallyControlled = false;
	
	public void ForceStartAnimation(string animationName)
	{
		animationIsExternallyControlled = true;
		
		currentAnimation = animationData[ animationName ];
		//currentFrameTime = 5.0f; // force update in Update()
		
		previousAnimationName = animationName;
		
		Debug.Log("Animation Forced " + animationName);
	}
	
	public void StopForcedAnimation()
	{
		animationIsExternallyControlled = false;
		
		Debug.Log("Forced Animation stopped " + StackTraceUtility.ExtractStackTrace());
	}
	
	protected bool wasFacingRight = true;
	
	// Update is called once per frame
	void Update () 
	{
		string currentAnimationName = "";
		
		bool invertUVs = false;
		if( !animationIsExternallyControlled )
		{
			if( characterController.IsRunningLeft() )
			{
				currentAnimationName = "walk";
				//invertUVs = true;
				
				//Debug.Log("WALKING LEFT");
			}
			
			if( characterController.IsRunningRight() )
			{
				currentAnimationName = "walk";
				//invertUVs = false;
				
				//Debug.Log("WALKING RIGHT");
			}
			
			if( characterController.IsIdle() )
			{
				currentAnimationName = "idle";
				//Debug.Log("IDLE");
			}
			
			if( !characterController.IsGrounded() )
			{
				//Debug.Log("FAAAAALLLING");
				currentAnimationName = "falling";
			}
			
			
			currentAnimation = animationData[ currentAnimationName ];
		}
		else
			currentAnimationName = previousAnimationName;
		
		
		invertUVs = !characterController.isFacingRight();
		if( currentAnimationName != previousAnimationName )
		{
			currentFrameTime = 5.0f; // force the animation to start this frame if a new animation has begun
		}
		
		if( wasFacingRight && !characterController.isFacingRight() )
			currentFrameTime = 5.0f; // force the animation to start this frame if a new animation has begun
		
		wasFacingRight = characterController.isFacingRight();
			
		
		previousAnimationName = currentAnimationName;
		
		/*
		if( !characterController.IsGrounded() )
		{
			//Debug.Log("FAAAAALLLING");
			currentAnimation = animationData["falling"];
		}
		*/
		
		
		currentFrameTime += Time.deltaTime;
		if( currentFrameTime > (1.0f / currentAnimation.framesPerSecond) )
		{
			//Debug.Log("FRAMING!" + currentAnimation.framesPerSecond);
			// run is van y 0.8 hoger naar 0.9
			// toekenning voor mesh is 
			// 2     3
			// 0     1
			// dus eerste entry in uv array is linksonder, 2de entry is rechtsonder etc.
			
			//Debug.Log(Time.frameCount + " FRAME : " + currentAnimationName + "  " + currentAnimation.currentFrame + " -> " + currentAnimation.framesPerSecond);
			
			// each x increment is + 0.1
			
			float increment = 1.0f / frameCount;
			
			float yValue = (1.0f / frameCount) * currentAnimation.rowIndex;
			
			currentAnimation.currentFrame = currentAnimation.currentFrame + 1;
			
			if( currentAnimation.currentFrame > (currentAnimation.startColumnIndex + currentAnimation.columnCount - 1) ) // loop back
			{
				//Debug.Log("LOOPOING!");
				currentAnimation.currentFrame = currentAnimation.startColumnIndex;
			}
			
			//Debug.Log("Current frame : " + currentAnimation.currentFrame + " For row " + currentAnimation.rowIndex);
			
			
			float xValue = currentAnimation.currentFrame * increment;
			
				
			
			Mesh m = GetComponent<MeshFilter>().mesh;
			Vector2[] uvs = new Vector2[ 4 ];
				
			uvs[0] = new Vector2( xValue, yValue );
			uvs[1] = new Vector2( xValue + increment, yValue );
			uvs[2] = new Vector2( xValue, yValue + increment);
			uvs[3] = new Vector2( xValue + increment, yValue + increment );
			
			
			if( invertUVs ) // invert the uv coordinates to mirror the texture
			{
				//Debug.Log("RUNNING LEFT!");
				Vector2 temp = uvs[1];
				uvs[1] = uvs[0];
				uvs[0] = temp;
				
				temp = uvs[3];
				uvs[3] = uvs[2];
				uvs[2] = temp;
			}
			
			
			m.uv = uvs;
			
			currentFrameTime = 0.0f;
		}
	}
}
