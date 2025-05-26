using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// ensure class initializer is called whenever scripts recompile
[InitializeOnLoadAttribute]
public class EntryPointEnforcer : MonoBehaviour
{
	// register an event handler when the class is initialized
	static EntryPointEnforcer()
	{
		EditorApplication.playModeStateChanged += LogPlayModeState;
	}

	private static void LogPlayModeState(PlayModeStateChange state)
	{
		if (state == PlayModeStateChange.EnteredPlayMode)
		{
			bool m_needLoadEntryPoint = true;
			m_scenes = new List<string>();
			for (int i = 0; i < EditorSceneManager.sceneCount; i++)
			{
				Scene scene = EditorSceneManager.GetSceneAt(i);
				m_scenes.Add(scene.path);
				if(EditorSceneManager.GetSceneByBuildIndex(0) == scene)
				{
					m_needLoadEntryPoint = false;
				}
			}

			if(m_needLoadEntryPoint)
			{
				EditorSceneManager.LoadScene(0, LoadSceneMode.Single);
				m_entryPointScene = EditorSceneManager.GetSceneByBuildIndex(0);

				EditorSceneManager.sceneLoaded += OnSceneLoaded;
			}
		}
	}

	private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if(m_entryPointScene==scene)
		{
			foreach (GameObject root in m_entryPointScene.GetRootGameObjects())
			{
				root.GetComponentInChildren<EntryPoint>().OverridedScenesToLoad = m_scenes;
			}
		}
	}

	private static List<string> m_scenes;
	private static Scene m_entryPointScene;
}
