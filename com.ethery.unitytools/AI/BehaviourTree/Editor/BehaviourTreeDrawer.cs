using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityTools.AI.BehaviourTree
{
	[CustomPropertyDrawer(typeof(BehaviourTreePicker))]
	public class BehaviourTreePickerDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginDisabledGroup(Application.isPlaying);
			{
				SerializedProperty typeProperty = property.FindPropertyRelative(BehaviourTreePicker.TYPENAME_FIELDNAME);

				if (m_types == null)
				{
					m_types = new List<Type>();
					foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
					{
						m_types.AddRange(assembly.GetTypes()
						.Where(Filter));
					}
				}
				string[] displayedOptions = new string[m_types.Count];
				for (int i = 0; i < m_types.Count; i++)
				{
					displayedOptions[i] = m_types[i].FullName;
				}

				int currentTypeIndex = string.IsNullOrEmpty(typeProperty.stringValue) ? -1 : m_types.FindIndex((t) => t.FullName == typeProperty.stringValue);
				if (currentTypeIndex != -1)
				{
					int newTypeIndex = EditorGUI.Popup(position, "Behaviour", currentTypeIndex, displayedOptions);

					if (currentTypeIndex != newTypeIndex)
					{
						typeProperty.stringValue = displayedOptions[newTypeIndex];
						Debug.Log($"Changing behaviour from {displayedOptions[currentTypeIndex]} to {displayedOptions[newTypeIndex]}");
					}
				}
				else
				{
					EditorGUI.LabelField(position, $"current type not found ({typeProperty.stringValue})");
					if (displayedOptions.Length > 0)
						typeProperty.stringValue = displayedOptions[0];
					else
						typeProperty.stringValue = string.Empty;
				}
			}
		}

		private bool Filter(Type myType)
		{
			return myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(BehaviourTree));
		}

		private List<Type> m_types = null;
	}
}
