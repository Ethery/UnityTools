using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    [Serializable]
    public class StateMachine<STATE_TYPE> where STATE_TYPE : State
    {
        public STATE_TYPE CurrentState => m_currentState;

        public StateMachine(STATE_TYPE defaultState)
        {
            defaultState.OnEnter(null);
            m_currentState = defaultState;
        }

        public bool PushState(STATE_TYPE newState)
        {
            Debug.Log($"Pushing {newState.GetType()} state from {m_currentState}");

			if (m_currentState.CanTransitionTo(newState))
            {
                m_currentState.OnExit(newState);
				newState.OnEnter(m_currentState);
                m_currentState = newState;
                Debug.Log($"Pushed {newState.GetType()} state");
                return true;
            }
            return false;
        }

        public void Tick(float deltaTime)
        {
            m_currentState.Tick(deltaTime);
        }

        [SerializeField]
        private STATE_TYPE m_currentState;
    }
}