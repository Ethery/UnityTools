using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityTools.AI.BehaviourTree;

namespace UnityTools.AI.BehaviourTree.Tasks
{
	[Serializable]
	public class Selector : Task
	{
		public List<Task> SubTasks = new List<Task>();

		public sealed override ETaskStatus Tick(Blackboard blackboard)
		{
			for(int i = 0; i < SubTasks.Count; ++i)
			{
				ETaskStatus subTaskStatus = SubTasks[i].Tick(blackboard);
				Debug.Log($"Task{SubTasks[i].GetType().Name} finished with status {subTaskStatus}");
				if(subTaskStatus == ETaskStatus.Running)
				{
					return ETaskStatus.Running;
				}
				else if(subTaskStatus == ETaskStatus.Success)
				{
					return ETaskStatus.Success;
				}
			}
			return ETaskStatus.Failed;
		}
	}
}
