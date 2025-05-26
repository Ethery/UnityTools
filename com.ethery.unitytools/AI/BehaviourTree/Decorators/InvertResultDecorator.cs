using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace UnityTools.AI.BehaviourTree.Tasks.Decorators
{
	public class InvertResultDecorator : Decorator
	{
		public override ETaskStatus Affect(ETaskStatus taskStatus)
		{
			switch(taskStatus)
			{
				case ETaskStatus.Success:
					return ETaskStatus.Failed;
				case ETaskStatus.Failed:
					return ETaskStatus.Success;
				default:
					return taskStatus;
			}
		}
	}
}
