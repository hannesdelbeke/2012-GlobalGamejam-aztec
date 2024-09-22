using UnityEngine;
using System.Collections;
using Aniballs.Sifteo;
using Sifteo;

public class ControllerAbstraction : MonoBehaviour {
	protected GameControls mSifteo;
	protected MainControls mSifteoControls;
	protected MainControls.Animal mPrevAnimal = MainControls.Animal.none;
	
	protected float mHorizontal = 0;
	protected float mVertical = 0;
	protected bool mUp = false;
	protected bool mDown = false;
	protected bool mLeft = false;
	protected bool mRight = false;
	protected bool mAction = false;
	protected bool mRhino = false;
	protected bool mRabbit = false;
	protected bool mAnt = false;
	protected bool mFish = false;
	
	public float Horizontal {
		get {
			return mHorizontal;
		}
	}
	
	public float Vertical {
		get {
			return mVertical;
		}
	}
	
	public bool Up {
		get {
			//bool val = mUp;
			//mUp = false;
			//return val;
			
			return mVertical > 0;
		}
	}
	
	public bool Down {
		get {
			//bool val = mDown;
			//mUp = false;
			//return val;
			return mVertical < 0;
		}
	}
	
	public bool Left {
		get {
			//bool val = mLeft;
			//mUp = false;
			//return val;
			
			return mHorizontal < 0;
		}
	}
	
	public bool Right {
		get {
			//bool val = mRight;
			//mUp = false;
			//return val;
			
			return mHorizontal > 0;
		}
	}
	
	public bool Action {
		get {
			bool val = mAction;
			mUp = false;
			return val;
		}
	}
	
	public bool Rhino {
		get {
			bool val = mRhino;
			mUp = false;
			return val;
		}
	}
	
	public bool Rabbit {
		get {
			bool val = mRabbit;
			mUp = false;
			return val;
		}
	}
	
	public bool Ant {
		get {
			bool val = mAnt;
			mUp = false;
			return val;
		}
	}
	
	public bool Fish {
		get {
			bool val = mFish;
			mUp = false;
			return val;
		}
	}

	protected void SifteoInit() {
		mSifteo = GameControls.instance;
		if(mSifteo != null) {
			mSifteoControls = mSifteo.MainControls;
		}
	}
	
	public bool UsingSifteo()
	{
		return mSifteo != null;
	}
	
	void Update () {
		mHorizontal = Input.GetAxis("Horizontal"); 
		mVertical = Input.GetAxis("Vertical");
		/*
		mLeft   = Input.GetKey(KeyCode.LeftArrow);
		mRight  = Input.GetKey(KeyCode.RightArrow);
		mUp     = Input.GetKey(KeyCode.UpArrow);
		mDown   = Input.GetKey(KeyCode.DownArrow);
		mAction = Input.GetKey(KeyCode.Space);
		mRhino  = Input.GetKey(KeyCode.Alpha1);
		mRabbit = Input.GetKey(KeyCode.Alpha2);
		mAnt    = Input.GetKey(KeyCode.Alpha3);
		mFish   = Input.GetKey(KeyCode.Alpha4);
		
		*/
		/*
		if( Input.GetKey(KeyCode.LeftArrow) )
		{
			mLeft = true;
			mRight = false;
		}
		if( Input.GetKey(KeyCode.RightArrow) )
		{
			mLeft = false;
			mRight = true;
		}
		
		if( Input.GetKey(KeyCode.UpArrow) )
		{
			mUp = true;
			mDown = false;
		}
		if( Input.GetKey(KeyCode.DownArrow) )
		{
			mUp = false;
			mDown = true;
		}
		*/
		
		
		if(mSifteo == null) { // TODO: this is a terrible hack... when can I be sure GameControls has been constructed?
			SifteoInit();
		} else {
			if(mSifteo.Running) {
				switch(mSifteoControls.VDirection) {
				case Cube.Side.TOP:
					mVertical = 1;
					break;
				case Cube.Side.BOTTOM:
					mVertical = -1;
					break;
				}
				switch(mSifteoControls.HDirection) {
				case Cube.Side.LEFT:
					mHorizontal = -1;
					break;
				case Cube.Side.RIGHT:
					mHorizontal = 1;
					break;
				}
			}
			
			if(mSifteoControls.Action) {
				mAction = true;
			}
			if(mSifteoControls.Reset) {
				// TODO: reset button hit
			}
			
			MainControls.Animal selectedAnimal = mSifteoControls.SelectedAnimal;
			
			string robinSelected = mSifteoControls.robinSelectedAnimal;
			
			Debug.Log("RobinSelected " + robinSelected);
			
			mRhino = (robinSelected == MainControls.Animal.rhino.ToString() );
			mRabbit = (robinSelected == MainControls.Animal.rabbit.ToString() );
			mAnt = (robinSelected == MainControls.Animal.ant.ToString() );
			mFish = (robinSelected == MainControls.Animal.fish.ToString() );
			
			/*
			Debug.Log("SELECTED ANIMAL : " + selectedAnimal.ToString());
			if(selectedAnimal != mPrevAnimal) 
			{
				Debug.Log("NEW ANIMAL SELECTED! " + selectedAnimal.ToString());
				
				switch(selectedAnimal) {
				case MainControls.Animal.rhino:
					mRhino = true;
					break;
				case MainControls.Animal.rabbit:
					mRabbit = true;
					break;
				case MainControls.Animal.ant:
					mAnt = true;
					break;
				case MainControls.Animal.fish:
					mFish = true;
					break;
				}
			}
			mPrevAnimal = selectedAnimal;
			
			if( selectedAnimal == MainControls.Animal.none )
			{
				mRhino = 
			}
			*/
			
			// TODO: highlight sifteo animal
			// mSifteoControls.HighlightAnimal = MainControls.Animal.ant;
		}
	}
}
