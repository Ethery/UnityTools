using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace StateMachine
{
    public abstract class StateWithParameters<PARAM_TYPE> : State, IInitializableByParameters<PARAM_TYPE>
    {
        public PARAM_TYPE Parameters => m_parameters;

        public void Initialize(PARAM_TYPE parameters)
        {
            m_parameters = parameters;
        }

        public override sealed void OnEnter(State previousState)
        {
            Assert.IsTrue(m_parameters != null, $"State {this.GetType()} must be initialized with a {typeof(PARAM_TYPE)} BEFORE Enter.");
        }

        public abstract void OnEnter(State previousState, PARAM_TYPE parameters);

        public PARAM_TYPE m_parameters;
    }
}