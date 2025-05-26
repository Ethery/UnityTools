using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomPropertyDrawer(typeof(ScenePicker))]
public class ScenePickerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (m_sceneIdProperty == null)
        {
            m_sceneIdProperty = property.FindPropertyRelative(ScenePicker.SCENE_PATH_FIELD_NAME);
        }
        
        if (m_scenesNames == null)
        {
            m_scenesNames = new List<string>();
        }
        else
        {
            m_scenesNames.Clear();
		}

		if (m_scenesIDs == null)
		{
			m_scenesIDs = AssetDatabase.FindAssets("t:scene");
		}

		int currentIndex = 0;
        
        for (int i = 0; i < m_scenesIDs.Length; i++)
        {
            string sceneID = m_scenesIDs[i];
            string sceneName = AssetDatabase.GUIDToAssetPath(sceneID);
            m_scenesNames.Add(sceneName);

            if (sceneName == m_sceneIdProperty.stringValue)
            {
                currentIndex = i;
            }
        }

        currentIndex = EditorGUI.Popup(position, currentIndex, m_scenesNames.ToArray());

        EnsureSceneIsInBuildSettings(m_scenesNames[currentIndex]);
        
		m_sceneIdProperty.stringValue = m_scenesNames[currentIndex];

        property.serializedObject.ApplyModifiedProperties();
    }

    private void EnsureSceneIsInBuildSettings(string scenePath)
    {
		bool canLoad = false;

		List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
			if (!string.IsNullOrEmpty(scenePath))
                editorBuildSettingsScenes.Add(EditorBuildSettings.scenes[i]);
			if (EditorBuildSettings.scenes[i].path == scenePath)
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

	string[] m_scenesIDs = null;
    List<string> m_scenesNames;
    SerializedProperty m_sceneIdProperty;
}
