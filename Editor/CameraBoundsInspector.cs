using UnityEditor;

[CustomEditor(typeof(CameraBounds))]
public class CameraBoundsInspector : Editor
{
	public void OnSceneGUI()
	{
		var t = target as CameraBounds;
		Handles.DrawWireCube(t.Bounds.center, t.Bounds.size);

	}
}