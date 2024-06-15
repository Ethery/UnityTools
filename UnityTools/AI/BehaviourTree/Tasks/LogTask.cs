using System;
using UnityEngine;

namespace UnityTools.AI.BehaviourTree.Tasks
{
	[Serializable]
	public class LogTask : Task
	{
		public LogTask(string logMessage)
		{
			m_logMessage = logMessage;
		}

		public override ETaskStatus Tick(Blackboard blackboard)
		{
			Debug.Log($"Log : {m_logMessage}");
			return ETaskStatus.Success;
		}

		[SerializeField]
		private string m_logMessage;
	}
}
