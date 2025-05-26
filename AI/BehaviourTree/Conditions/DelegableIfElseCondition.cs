using System;

namespace UnityTools.AI.BehaviourTree.Tasks
{
	[Serializable]
	public class DelegableIfElseCondition : IfElseCondition
	{
		public delegate bool IfElseDelegate(Blackboard blackboard);

		IfElseDelegate m_ifElseDelegate;

		public DelegableIfElseCondition(IfElseDelegate ifElseDelegate, Task trueTask, Task falseTask) : base(trueTask,falseTask)
		{
			m_ifElseDelegate = ifElseDelegate;
		}

		protected override bool CheckCondition(Blackboard blackboard)
		{
			return m_ifElseDelegate(blackboard);
		}
	}
}
