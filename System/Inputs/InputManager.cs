using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace UnityTools.Systems.Inputs
{
	[RequireComponent(typeof(BaseInputModule))]
	public class InputManager : Singleton<InputManager>
	{
		#region Classes

		[Serializable]
		public class Input
		{
			/*
			public class Comparer : IEqualityComparer<Input>
			{
				public bool Equals(Input x, Input y)
				{
					return x.Equals(y);
				}

				public int GetHashCode(Input obj)
				{
					return obj.GetHashCode();
				}
			}
			*/

			public const string NAME_FIELD_NAME = nameof(m_name);
			public const string GROUP_FIELD_NAME = nameof(m_mapName);

			public static Input NONE = new Input(nameof(NONE), string.Empty);

			public string Name => m_name;

			public string MapName => m_mapName;

			public Input(string name, string mapName)
			{
				this.m_name = name;
				this.m_mapName = mapName;
			}

			public bool IsValid()
			{
				return !string.IsNullOrEmpty(m_name) && !string.IsNullOrEmpty(m_mapName) && this != NONE;
			}

			public override bool Equals(object obj)
			{
				return obj is Input input && m_mapName == input.m_mapName && m_name == input.m_name;
			}
			public override int GetHashCode()
			{
				return m_mapName.GetHashCode() + m_name.GetHashCode();
			}
			public override string ToString()
			{
				return $"{m_mapName}.{m_name}";
			}

			[SerializeField]
			private string m_name;

			[SerializeField]
			private string m_mapName;
		}

		#endregion

		public class InputEvent
		{
			public Action<InputAction> Callback => m_callback;

			public InputActionPhase EventType => m_eventType;

			public InputEvent(Action<InputAction> callback, InputActionPhase eventType)
			{
				this.m_callback = callback;
				this.m_eventType = eventType;
			}

			private Action<InputAction> m_callback;
			private InputActionPhase m_eventType;
		}

		public static void RegisterInput(Input input, InputEvent eventToCall, bool register)
		{
			if (Instance == null)
			{
				//Debug.LogError($"Can't register Input because InputManager Instance is null");
				return;
			}
			if(register)
			{
				if(Instance.m_events.ContainsKey(input))
				{
					if(!Instance.m_events[input].Add(eventToCall))
					{
						UnityEngine.Debug.LogError($"An event with the same parameters already exists for {input}");
					}
				}
				else
				{
					Instance.m_events.Add(input, new HashSet<InputEvent> { eventToCall });
					Debug.Log($"Registered event on {input}");
				}
			}
			else
			{
				if(Instance.m_events.ContainsKey(input))
				{
					if(Instance.m_events[input].Contains(eventToCall))
					{
						Instance.m_events[input].Remove(eventToCall);
						if(Instance.m_events[input].Count <= 0)
						{
							Instance.m_events.Remove(input);
							Debug.Log($"Unregistered event on {input}");
						}
					}
				}
			}
		}

		public Dictionary<string, Input> Inputs => m_inputs;

		#region Private

		protected override void Awake()
		{
			base.Awake();
			m_inputs.Clear();
			m_events.Clear();

			foreach(InputActionMap map in m_inputsAsset.actionMaps)
			{
				map.actionTriggered += OnActionTriggered;
				foreach (InputAction action in map.actions)
				{
					Input input = new Input(action.name, map.name);
					m_inputs.Add(input.ToString(), input);
					Debug.Log($"Added Input {action.name} to {nameof(InputManager)}.{nameof(m_inputs)}");
				}
			}
		}

		private void OnActionTriggered(InputAction.CallbackContext obj)
		{
			string inputString = $"{obj.action.actionMap.name}.{obj.action.name}";
			
			if(IsMouseOnUI())
				return;

            if (m_inputs.ContainsKey(inputString))
			{
				Input input = m_inputs[inputString];
				if(m_events.ContainsKey(input))
				{
					foreach(InputEvent eventToCall in m_events[input])
					{
						if(eventToCall.EventType == obj.phase)
						{
							eventToCall.Callback(obj.action);
						}
					}
				}
			}
		}

		private bool IsMouseOnUI()
		{
			GameObject gameObject = EventSystem.current?.currentSelectedGameObject;
			return gameObject != null && gameObject.GetComponentInParent<Canvas>() != null;
		}

		protected override void OnDestroy()
		{
			foreach (InputActionMap map in m_inputsAsset.actionMaps)
			{
				map.actionTriggered -= OnActionTriggered;
			}

			m_inputs.Clear();
			m_events.Clear();
			base.OnDestroy();
		}

		private static Dictionary<string, Input> m_inputs = new Dictionary<string, Input>();

		private Dictionary<Input, HashSet<InputEvent>> m_events = new Dictionary<Input, HashSet<InputEvent>>();

		[SerializeField] private InputActionAsset m_inputsAsset;
		#endregion
	}
}
