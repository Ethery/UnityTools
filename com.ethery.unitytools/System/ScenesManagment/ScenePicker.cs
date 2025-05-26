using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class ScenePicker
{
#if UNITY_EDITOR
    public const string SCENE_PATH_FIELD_NAME = nameof(m_scenePath);
#endif

    public string ScenePath => m_scenePath;

    [SerializeField]
    private string m_scenePath;
}
