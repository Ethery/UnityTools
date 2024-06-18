namespace UnityTools.AI.BehaviourTree
{
	public class Condition : Task
	{
		public Task SuccessTask = null;
		public Task FailTask = null;

		public delegate bool ConditionCheck(Blackboard blackboard);
		public ConditionCheck ConditionCheckFunction;

		public sealed override ETaskStatus Tick(Blackboard blackboard)
		{
			if (ConditionCheck(blackboard))
			{
				return ETaskStatus.Success;
			}
			return ETaskStatus.Failed;
		}

		private bool ConditionCheck(Blackboard blackboard)
		{
			return ConditionCheckFunction(blackboard);
		}
	}
}
