using System;
using System.Collections.Generic;
using Sifteo;
using Sifteo.Util;

namespace Aniballs
{
    public class StateMachine2
    {
        private struct StateTransition
        {
            public string state;
            public string transition;
            public StateTransition(string s, string t)
            {
                this.state = s;
                this.transition = t;
            }
        }
        private class Lock : IDisposable
        {
            private StateMachine2 mMachine;
            private bool mLocked = true;
            public Lock(StateMachine2 sm)
            {
                this.mMachine = sm;
                this.mMachine.LockTransitions();
            }
            public void Dispose()
            {
                if (this.mLocked)
                {
                    Log.Info("Disposing of lock for state: " + this.mMachine.mCurrent);
                    this.mMachine.UnlockTransitions();
                    this.mLocked = false;
                    return;
                }
                Log.Warning("Attempting to Dispose() lock multiple times, ignoring");
            }
        }
        public delegate string StateFunction(string transitionId);
        private string mCurrent = "";
        private Dictionary<string, IStateController> mStates = new Dictionary<string, IStateController>();
        private Dictionary<StateMachine2.StateTransition, string> mTransitions = new Dictionary<StateMachine2.StateTransition, string>();
        private int mTransitionMutex = 1;
        private Queue<string> mTransitionQueue = new Queue<string>();
        public IStateController CurrentState
        {
            get
            {
                return this.mStates[this.mCurrent];
            }
        }
        public string Current
        {
            get
            {
                return this.mCurrent;
            }
        }
        public StateMachine2 State(string name, IStateController scene)
        {
            if (!this.mStates.ContainsKey(name))
            {
                this.mStates.Add(name, scene);
            }
            return this;
        }
       /* public StateMachine2 State(string name, StateMachine2.StateFunction func)
        {
            return this.State(name, new SimpleState(this, func));
        }*/
        public StateMachine2 Transition(string fromState, string transitionId, string toState)
        {
            StateMachine2.StateTransition key = new StateMachine2.StateTransition(fromState, transitionId);
            if (!this.mTransitions.ContainsKey(key))
            {
                this.mTransitions.Add(key, toState);
            }
            return this;
        }
        public StateMachine2 SetState(string name, string transitionId = "")
        {
            if (this.mTransitionMutex > 0 && this.mCurrent.Length > 0)
            {
                Log.Warning("Forcing state to [ {0} ], which may cause inconsistent control flow. Recommend using QueueTransition()", new object[]
				{
					name
				});
            }
            if (this.mStates.ContainsKey(name))
            {
                Log.Info("Setting State: " + name);
                this.LockTransitions();
                if (this.mCurrent.Length > 0)
                {
                    this.mStates[this.mCurrent].OnDispose();
                }
                this.mCurrent = name;
                if (this.mCurrent.Length > 0)
                {
                    this.mStates[this.mCurrent].OnSetup(transitionId);
                }
                this.UnlockTransitions();
                this.CheckTransitionQueue();
            }
            else
            {
                Log.Warning("Attempting to set invalid state [ {0} ], ignoring", new object[]
				{
					name
				});
            }
            return this;
        }
        public void QueueTransition(string name)
        {
            if (this.mTransitionMutex > 0)
            {
                this.mTransitionQueue.Enqueue(name);
                return;
            }
            StateMachine2.StateTransition key = new StateMachine2.StateTransition(this.mCurrent, name);
            Log.Info("Applying Transition: " + name);
            if (this.mTransitions.ContainsKey(key))
            {
                this.SetState(this.mTransitions[key], name);
            }
        }
        public void Tick(float dt)
        {
            if (this.mCurrent.Length > 0)
            {
                //this.CheckTransitionQueue();
                this.mStates[this.mCurrent].OnTick(dt);
                //this.CheckTransitionQueue();
            }
        }
        public void Paint(bool canvasDirty)
        {
            if (this.mCurrent.Length > 0)
            {
                //this.CheckTransitionQueue();
                this.mStates[this.mCurrent].OnPaint(canvasDirty);
                //this.CheckTransitionQueue();
            }
        }
        public IDisposable AquireLock()
        {
            Log.Info("Aquiring Lock for State: " + this.mCurrent);
            return new StateMachine2.Lock(this);
        }
        public void PerformStateChanges()
        {
            CheckTransitionQueue();
        }
        private void LockTransitions()
        {
            this.mTransitionMutex++;
        }
        private void UnlockTransitions()
        {
            this.mTransitionMutex--;
            if (this.mTransitionMutex == 0 && this.mTransitionQueue.Count > 0)
            {
                this.QueueTransition(this.mTransitionQueue.Dequeue());
            }
        }
        private void CheckTransitionQueue()
        {
            this.UnlockTransitions();
            this.LockTransitions();
        }
    }
}
