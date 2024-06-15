using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class GameBuilder
{
	// This function will be called from the build process
	[MenuItem("Tools/GameBuilder/TestBuildGame")]
	public static void TestBuildInEditor()
	{
		BuildWindows("../Build");
	}

	public static void Build()
	{
		Debug.Log($"Building Unity");
		var cmdArgs = Environment.GetCommandLineArgs();
		string buildPath = string.Empty;
		for(int i = 0; i < cmdArgs.Length; i++)
		{
			Debug.Log($"Arg : {cmdArgs[i]}");
			if(cmdArgs[i] == $"{nameof(GameBuilder)}.{nameof(Build)}")
			{
				buildPath = cmdArgs[i + 1];

				Debug.Log($"Build path set to {buildPath}");
			}
		}

		BuildWindows(buildPath);
	}

	private static void BuildWindows(string buildPath)
	{
		// Setup build options (e.g. scenes, build output location)
		string[] scenesPath = new string[EditorBuildSettings.scenes.Length];
		for(int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
			EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
			scenesPath[i] = scene.path;
		}

		var options = new BuildPlayerOptions
		{
			// Change to location the output should go
			locationPathName = $"{buildPath}/Game/Game.exe",
			scenes = scenesPath,
			options = BuildOptions.CleanBuildCache | BuildOptions.StrictMode,
			target = BuildTarget.StandaloneWindows64
		};
		var report = BuildPipeline.BuildPlayer(options);

		if(report.summary.result == BuildResult.Succeeded)
		{
			Debug.Log($"Build successful - Build written to {options.locationPathName}");
		}
		else if(report.summary.result == BuildResult.Failed)
		{
			Debug.LogError($"Build failed");
		}
	}
}
