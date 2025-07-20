using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityTools.Game
{

public abstract class BaseGameRules : ScriptableObject
{ }

[Serializable]
public abstract class BaseGameDatas : MonoBehaviour
{ }

public class GameManager : Singleton<GameManager>
{
	public StateMachine<GameState> GameStates;

    public string StartScene => m_startScene.ScenePath;

	public T GetGameRules<T>() where T : BaseGameRules
	{
		return m_gameRules as T;
	}

	public T GetGameDatas<T>() where T : BaseGameDatas
	{
		return m_gameDatas as T;
	}

	protected override void Awake()
	{
		base.Awake();
		GameStates = new StateMachine<GameState>(new DefaultGameState());
	}

	private void Update()
	{
		GameStates.Tick(Time.deltaTime);
	}
	
	[SerializeField]
	private ScenePicker m_startScene;

	[SerializeField]
	private BaseGameRules m_gameRules;

	[SerializeField]
	private BaseGameDatas m_gameDatas;
}


public class DefaultGameState : GameState
{
	public override bool CanTransitionTo(State requestedNewState)
	{
		return true;
	}

	public override void OnEnter(State previousState)
	{
	}

	public override void OnExit(State nextState)
	{
	}

	public override void Tick(float deltaTime)
	{
	}
}

}