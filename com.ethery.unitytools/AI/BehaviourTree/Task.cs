using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace UnityTools.AI.BehaviourTree
{
	public enum ETaskStatus
	{
		Running,    //Is running and not finished yet (can't know if it's a success or a fail).
		Success,    //Has run, and finished successfully
		Failed,     //Has run, and failed.
	}

	[Serializable]
	public abstract class Task
	{
		public abstract ETaskStatus Tick(Blackboard blackboard);

#if UNITY_EDITOR
		public virtual void OnInspectorGUI()
		{
			UnityEditor.EditorGUILayout.LabelField(this.GetType().Name);
		}
#endif
	}
}
