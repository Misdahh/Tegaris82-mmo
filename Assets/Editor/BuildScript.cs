using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildScript
{
    static string FindScene()
    {
        string[] paths = AssetDatabase.FindAssets("t:Scene")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(p => !string.IsNullOrEmpty(p))
            .ToArray();

        if (paths.Length == 0)
            throw new BuildFailedException("No Unity scenes were found under Assets.");

        string[] preferred =
        {
            "Assets/KaisarMMO/Art/Scenes/MainMenu.unity",
            "Assets/Scenes/MainMenu.unity",
            "Assets/KaisarMMO/Art/Scenes/KaisarWorld.unity",
            "Assets/Scenes/KaisarWorld.unity"
        };

        foreach (var p in preferred)
            if (paths.Contains(p))
            {
                Debug.Log("BuildScript selected scene: " + p);
                return p;
            }

        var fallback = paths.OrderBy(p => p).First();
        Debug.Log("BuildScript selected fallback scene: " + fallback);
        return fallback;
    }

    static void Build(BuildTarget target, string output)
    {
        string scene = FindScene();
        Directory.CreateDirectory(Path.GetDirectoryName(output));

        var options = new BuildPlayerOptions
        {
            scenes = new[] { scene },
            locationPathName = output,
            target = target,
            options = BuildOptions.None
        };

        Debug.Log("=== BUILD START ===");
        Debug.Log("Target: " + target);
        Debug.Log("Scene: " + scene);
        Debug.Log("Output: " + output);

        BuildReport report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result != BuildResult.Succeeded)
            throw new BuildFailedException("Build failed: " + report.summary.result);

        Debug.Log("=== BUILD SUCCESS ===");
    }

    public static void BuildWebGL()
    {
        Build(BuildTarget.WebGL, "build/WebGL");
    }

    public static void BuildAndroid()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        Build(BuildTarget.Android, "build/tegaris82.apk");
    }
}
