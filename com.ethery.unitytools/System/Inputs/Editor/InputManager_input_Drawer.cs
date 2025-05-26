using System.Collections.Generic;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityTools.Systems.Inputs;

[CustomPropertyDrawer(typeof(InputManager.Input))]
public class InputManager_input_Drawer : PropertyDrawer
{
	public override VisualElement CreatePropertyGUI(SerializedProperty property)
	{
		m_property = property;
		VisualElement ve = new VisualElement();
		{
			m_dropdownField = new DropdownField(m_property.displayName);
			m_dropdownField.RegisterValueChangedCallback(OnDropdownValueChanged);
			ComputeDropDown();
			if(m_property.boxedValue is InputManager.Input input && input.IsValid())
			{
				m_dropdownField.SetValueWithoutNotify(ToDropdownValue(input));
			}
			else
			{
				m_dropdownField.SetValueWithoutNotify("NONE");
			}

			ve.Add(m_dropdownField);
		}
		return ve;
	}

	private void OnDropdownValueChanged(ChangeEvent<string> evt)
	{
		m_property.boxedValue = m_dropdownValuesForInput[evt.newValue];
		m_property.serializedObject.ApplyModifiedProperties();
	}

	public void ComputeDropDown()
	{
		m_dropdownValuesForInput.Clear();

		m_dropdownValuesForInput.Add("None",InputManager.Input.NONE);

		List<string> values = new List<string>();
		values.Add("None");
		InputActionAsset asset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"t:{nameof(InputActionAsset)}")[0]));

		foreach(InputActionMap map in asset.actionMaps)
		{
			foreach(InputAction action in map.actions)
			{
				InputManager.Input input = new InputManager.Input(action.name, map.name);

				string dropdownValue = "NONE";
				if(input != InputManager.Input.NONE)
				{
					dropdownValue = ToDropdownValue(input);
				}
				m_dropdownValuesForInput.Add(dropdownValue, input);
				values.Add(dropdownValue);
			}
		}

		m_dropdownField.choices = values;
	}

	private static string ToDropdownValue(InputManager.Input input)
	{
		return $"{input.MapName}/{input.Name}";
	}

	private Dictionary<string, InputManager.Input> m_dropdownValuesForInput = new Dictionary<string, InputManager.Input>();
	private DropdownField m_dropdownField;
	private SerializedProperty m_property;
}
