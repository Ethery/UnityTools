using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ScenePicker))]
public class ScenePickerDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//if (m_sceneIdProperty == null)
		{
			m_sceneIdProperty = property.FindPropertyRelative(ScenePicker.SCENE_PATH_FIELD_NAME);
			m_sceneIdProperty = property.serializedObject.FindProperty(m_sceneIdProperty.propertyPath);
		}


		Object scene = EditorGUI.ObjectField(position, AssetDatabase.LoadAssetAtPath<Object>(m_sceneIdProperty.stringValue), typeof(SceneAsset), false);
		//currentIndex = EditorGUI.Popup(position, label.text, currentIndex, m_scenesNames.ToArray());
		string path = AssetDatabase.GetAssetPath(scene);

		EnsureSceneIsInBuildSettings(path);

		m_sceneIdProperty.stringValue = path;

		property.serializedObject.ApplyModifiedProperties();
	}

	private void EnsureSceneIsInBuildSettings(string scenePath)
	{
		bool canLoad = false;

		List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
			editorBuildSettingsScenes.Add(EditorBuildSettings.scenes[i]);
			if (!string.IsNullOrEmpty(scenePath) && EditorBuildSettings.scenes[i].path == scenePath)
			{
				canLoad = true;
			}
		}
		if (!canLoad)
		{
			editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
		}

		EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
	}

	SerializedProperty m_sceneIdProperty;
}
