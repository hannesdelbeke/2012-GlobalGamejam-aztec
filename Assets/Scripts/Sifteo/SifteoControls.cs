using Sifteo;
using Sifteo.Util;
using System;
using System.Collections.Generic;
using Aniballs.Sifteo.Util;
using UnityEngine;
using Color = Sifteo.Color;

namespace Aniballs.Sifteo
{
	public class MainControls : IStateController
	{
		protected Cube mControlCube;
		protected Cube[] mAnimalCube = new Cube[(int)Animal.max];
		protected Cube mResetCube;
		
		protected bool mControlDirty = true;
		protected bool[] mAnimalDirty = new bool[(int)Animal.max];
		protected bool mResetDirty   = true;
		
		protected const float mHighlightTime = 5;
		
		protected Cube.Side mHDirection = Cube.Side.NONE;
		protected Cube.Side mVDirection = Cube.Side.NONE;
		protected Animal mSelectedAnimal = Animal.none;
		protected bool mReset = false;
		protected bool mAction = false;
		protected Animal mHighlightAnimal = Animal.max;
		protected float mHighlightCounter = 0.0f;
		
		public MainControls()
		{
		}
		
		/**
		 * Interface to main game
		 * */
		public bool Reset {
			get {
				bool reset = mReset;
				mReset = false;
				return reset;
			}
		}
		
		public bool Action {
			get {
				bool action = mAction;
				mAction = false;
				return action;
			}
		}
		
		public Cube.Side HDirection {
			get {
				return mHDirection;
			}
		}
		
		public Cube.Side VDirection {
			get {
				return mVDirection;
			}
		}
		
		public Animal SelectedAnimal {
			get {
				//Debug.Log("IEMAND HEEFT MIJ GE-SELECT!");
				Animal selectedAnimal = mSelectedAnimal;
				mSelectedAnimal = Animal.none;
				HighlightAnimal = selectedAnimal;
				return selectedAnimal;
			}
		}
		
		/*
		public void AnimalSelected()
		{
			selectedAnimal = Animal.none;
			HighlightAnimal = selectedAnimal;
		}
		*/
		
		public Animal HighlightAnimal {
			set {
				mHighlightAnimal = value;
				mHighlightCounter = mHighlightTime;
			}
		}
		
		/**
		 * 
		 * */
		public void OnSetup (string transitionId) {
			mControlCube = GameControls.instance.CubeSet[0];
			
			for(int animal = 0; animal < (int)Animal.max; animal++) {
				mAnimalCube[animal] = GameControls.instance.CubeSet[animal + 1];
			}
			mResetCube   = GameControls.instance.CubeSet[5];
			
			mControlCube.TiltEvent        += HandleControlCubeTiltEvent;
			mControlCube.NeighborAddEvent += HandleControlCubeNeighborAddEvent;
			mControlCube.ButtonEvent      += HandleControlCubeButtonEvent;
			mResetCube.ButtonEvent        += HandleResetCubeButtonEvent;
			mControlCube.NeighborRemoveEvent += HandleNeighborRemoveEventHandler;
			
			mControlDirty     = true;
			mResetDirty       = true;
			mHighlightAnimal  = Animal.none;
			mHighlightCounter = 0;
			
			for(int animal = 0; animal < (int)Animal.max; animal++) {
				mAnimalDirty[animal] = true;
			}
		}
		
		public void OnTick (float dt) {
			float prevCounter = mHighlightCounter;
			if(mHighlightCounter > 0) {
				mHighlightCounter -= dt;
			}
			if(Math.Floor(mHighlightCounter) != Math.Floor(prevCounter) && mHighlightAnimal != Animal.none) {
				mAnimalDirty[(int)mHighlightAnimal] = true;
			}
			if(mHighlightCounter < 0) {
				mHighlightCounter = 0;
				mHighlightAnimal = Animal.none;
			}
		}
		
		public void OnPaint (bool canvasDirty) {
			if(mControlDirty) {
				mControlDirty = false;
				mControlCube.Image("arrowsinterface", 0, 0, 0, 128, 128, 128, 0, 0);
				if(mHDirection != Cube.Side.NONE) {
					mControlCube.Image("arrowsinterface", 0, 0, 0, 0, 128, 128, 0, (int)mHDirection);
				}
				if(mVDirection != Cube.Side.NONE) {
					mControlCube.Image("arrowsinterface", 0, 0, 0, 0, 128, 128, 0, (int)mVDirection);
				}
				mControlCube.Paint();
			}
			
			
			for(int animal = 0; animal < (int)Animal.max; animal++) {
				if(mAnimalDirty[animal]) {
					mAnimalDirty[animal] = false;
					mAnimalCube[animal].Image("icoonanimal", 0, 0, 0, animal * 128, 128, 128, 0, 0);
					if((int)mSelectedAnimal == animal || mHighlightCounter > 0 && (int)mHighlightAnimal == animal) {
						Color color = Util.Graphics.Red;
						if(Math.Floor(mHighlightCounter) % 2 == 1) {
							color = Util.Graphics.Green;
						}
						
						//Util.Graphics.Border(mAnimalCube[animal], color, 2);
						//mAnimalCube[animal]
					}
					mAnimalCube[animal].Paint();
				}
			}
			
			
			if(mResetDirty) {
				mResetDirty = false;
				mResetCube.Image("ResetButton", 0, 0, 0, 0, 128, 128, 0, 0);
				mResetCube.Paint();
			}
		}
		
		public void OnDispose () {
			mControlCube.TiltEvent        -= HandleControlCubeTiltEvent;
			mControlCube.NeighborAddEvent -= HandleControlCubeNeighborAddEvent;
			mControlCube.ButtonEvent      -= HandleControlCubeButtonEvent;
			mResetCube.ButtonEvent        -= HandleResetCubeButtonEvent;
			
			//mControlCube.Nei
			
			mControlCube = null;
			for(int animal = 0; animal < (int)Animal.max; animal++) {
				mAnimalCube[animal] = null;
			}
			mResetCube   = null;
		}
		
		
		/*
		 * Use animal's special function
		 * */
		protected void HandleControlCubeButtonEvent (Cube c, bool pressed)
		{
			if(pressed) {
				mAction = true;
			}
		}
		
		/*
		 * Reset the game
		 * */
		protected void HandleResetCubeButtonEvent (Cube c, bool pressed)
		{
			if(pressed) {
				mReset = true;
			}
		}
		
		public string robinSelectedAnimal = "none";
		
		/*
		 * Load animal in control cube and on screen
		 * */
		protected void HandleControlCubeNeighborAddEvent (Cube c, Cube.Side side, Cube neighbor, Cube.Side neighborSide)
		{
			Animal prevAnimal = mSelectedAnimal;
			for(int animal = 0; animal < (int)Animal.max; animal++) {
				if(mAnimalCube[animal] == neighbor) {
					mSelectedAnimal = (Animal)animal;
					
					robinSelectedAnimal = ( (Animal) animal).ToString();
					
					Debug.Log("ANIMAL SELECTED : " + mSelectedAnimal.ToString());
				}
			}
			if(prevAnimal != mSelectedAnimal) {
				if(prevAnimal != Animal.none) {
					mAnimalDirty[(int)prevAnimal] = true;
				}
				mAnimalDirty[(int)mSelectedAnimal] = true;
			}
		}
		
		protected void HandleNeighborRemoveEventHandler(Cube c, Cube.Side side, Cube neighbor, Cube.Side neighborSide)
		{
			robinSelectedAnimal = "none";
		}
		
		/*
		 * Controls the movement direction of the animal
		 * */
		protected void HandleControlCubeTiltEvent (Cube c, int x, int y, int z)
		{
			Cube.Side prevVDirection = mVDirection;
			Cube.Side prevHDirection = mHDirection;
			if(x < 1) {
				mHDirection = Cube.Side.LEFT;
			} else if(x > 1) {
				mHDirection = Cube.Side.RIGHT;
			} else {
				mHDirection = Cube.Side.NONE;
			}
			if(y > 1) {
				mVDirection = Cube.Side.TOP;
			} else if(y < 1) {
				mVDirection = Cube.Side.BOTTOM;
			} else {
				mVDirection = Cube.Side.NONE;
			}
			if(prevHDirection != mHDirection || prevVDirection != mVDirection) {
				mControlDirty = true;
			}
		}
		
		public enum Animal {
			rhino,
			rabbit,
			ant,
			fish,
			max,
			none = max,
		}
	}
}

