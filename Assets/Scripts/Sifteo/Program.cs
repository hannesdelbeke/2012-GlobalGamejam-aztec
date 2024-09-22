using Sifteo;
using Sifteo.Util;
using System;
using System.Collections.Generic;

namespace Aniballs.Sifteo
{
    public class GameControls : BaseApp
    {
        public static GameControls instance;
		
		protected StateMachine2 mStateMachine = new StateMachine2();
		protected MainControls mMainControls = new MainControls();
		
		protected bool mRunning = false;
		
		public MainControls MainControls {
			get {
				return mMainControls;
			}
		}
		
		public bool Running {
			get {
				return mRunning;
			}
		}

        override public int FrameRate
        {
            get { return 20; }
        }
		
		public GameControls() {
            instance = this;
		}

        override public void Setup()
        {
			mStateMachine.State("main", mMainControls);
			
			mStateMachine.SetState("main");
			
			this.StoppedEvent += SifteoStoppedEventHandler;
			this.UnpauseEvent += SifteoUnpauseEventHandler;
			this.PauseEvent   += SifteoPauseEventHandler;
			mRunning = true;
        }
		
		protected void SifteoStoppedEventHandler() {
			mRunning = false;
		}
		
		protected void SifteoUnpauseEventHandler() {
			mRunning = true;
		}
		
		protected void SifteoPauseEventHandler() {
			mRunning = false;
		}

        override public void Tick()
        {
			mStateMachine.Tick(this.DeltaTime);
            if (this.IsIdle)
            {
				mStateMachine.Paint(true);
            }
        }

        static void Main(string[] args)
        {
            new GameControls().Run();
        }
    }
}