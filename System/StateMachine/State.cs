namespace StateMachine
{
	public abstract class State
	{
		public abstract void OnEnter(State previousState);
		public abstract void OnExit(State nextState);
		public abstract void Tick(float deltaTime);
	}
}