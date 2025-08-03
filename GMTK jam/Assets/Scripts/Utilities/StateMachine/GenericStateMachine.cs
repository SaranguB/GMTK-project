using System;
using System.Collections.Generic;

namespace Utilities.StateMachine
{
    public class GenericStateMachine<T>
    {
        private IState<T> currentState;
        private readonly T owner;
        private Dictionary<Enum, IState<T>> states = new();
        private Enum currentStateKey;
        
        public Enum CurrentStateKey => currentStateKey;
        public IState<T> CurrentState => currentState;
        
        public GenericStateMachine(T owner)
        {
            this.owner = owner;
        }

        protected void AddState(Enum state, IState<T> stateInstance)
        {
            states.TryAdd(state, stateInstance);
        }

        public void ChangeState(Enum key)
        {
            if (states.TryGetValue(key, out IState<T> newState))
            {
                if (currentState == newState) return;

                currentState?.OnExit();
                currentState = newState;
                currentStateKey = key;
                currentState?.OnEnter();
            }
        }

        protected void SetOwner()
        {
            foreach (IState<T> state in states.Values)
            {
                state.Owner = owner;
            }
        }
        
        public void Tick()
        {
            currentState?.Tick();
        }
    }
}