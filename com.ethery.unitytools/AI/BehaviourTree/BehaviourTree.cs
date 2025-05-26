using System;
using UnityEngine;

namespace UnityTools.AI.BehaviourTree
{
	[Serializable]
	public class BehaviourTree
    {
        public virtual Task Root { get; }

		public ETaskStatus Execute(Blackboard blackboard)
		{
			if (Root == null)
			{
				Debug.LogError($"{nameof(Root)} is not initialized. Make sure to override {this.GetType()}.{nameof(Root)}.");
				return ETaskStatus.Failed;
			}
			return Root.Tick(blackboard);
		}

#if UNITY_EDITOR
		public void OnInspectorGUI()
		{
			if (Root != null)
			{
				Root.OnInspectorGUI();
			}
		}
#endif
	}

}
