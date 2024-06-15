using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityTools.AI.BehaviourTree;

namespace UnityTools.AI.BehaviourTree.Tasks
{
	[Serializable]
	public abstract class Decorator : Task
	{
		public Task DecoratedTask;

		public sealed override ETaskStatus Tick(Blackboard blackboard)
		{
			ETaskStatus taskStatus = DecoratedTask.Tick(blackboard);
			taskStatus = Affect(taskStatus);
			return taskStatus;
		}

		public abstract ETaskStatus Affect(ETaskStatus taskStatus);
	}
}
