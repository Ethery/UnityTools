using StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityTools.Game
{
	public abstract class GameManager : Singleton<GameManager>
	{
		public string StartScene => m_startScene.ScenePath;

		[SerializeField]
		private ScenePicker m_startScene;

		public virtual void LoadStartScene()
		{
			Debug.Log($"loading {StartScene}");
			SceneManager.LoadScene(StartScene);
		}

		/// <summary>
		/// Called when loading any scene without save.
		/// </summary>
		public abstract void StartGameFromOverridedScene();

		/// <summary>
		/// Called when asking to start a new game.
		/// </summary>
		public abstract void StartANewGame();
	}


	public abstract class GameManager<GAME_MANAGER_TYPE, GAME_DATAS_TYPE, GAME_RULES_TYPE> : GameManager
		where GAME_MANAGER_TYPE : GameManager<GAME_MANAGER_TYPE, GAME_DATAS_TYPE, GAME_RULES_TYPE>
		where GAME_DATAS_TYPE : BaseGameDatas
		where GAME_RULES_TYPE : BaseGameRules
	{
		public new static GAME_MANAGER_TYPE Instance => (GAME_MANAGER_TYPE)GameManager.Instance;

		public StateMachine<GameState> GameStates;

		public GAME_RULES_TYPE GetGameRules()
		{
			return m_gameRules as GAME_RULES_TYPE;
		}

		public GAME_DATAS_TYPE GetGameDatas()
		{
			return m_gameDatas as GAME_DATAS_TYPE;
		}

		protected override void Awake()
		{
			base.Awake();
			GameStates = new StateMachine<GameState>(new DefaultGameState());
			if (m_gameDatas != null)
			{
				DontDestroyOnLoad(m_gameDatas);
			}
		}

		private void Update()
		{
			GameStates.Tick(Time.deltaTime);
		}

		[SerializeField]
		private BaseGameRules m_gameRules;

		[SerializeField]
		private BaseGameDatas m_gameDatas;
	}
}