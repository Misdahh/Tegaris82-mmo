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
        string[] scenes =
        {
            "Assets/KaisarMMO/Art/Scenes/MainMenu.unity",
            "Assets/KaisarMMO/Art/Scenes/KaisarWorld.unity"
        };

        // Verify both required scenes exist before building.
        foreach (var scenePath in scenes)
        {
            if (!File.Exists(scenePath))
                throw new BuildFailedException("Required scene not found: " + scenePath);
        }

        Directory.CreateDirectory(Path.GetDirectoryName(output));

        var options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = output,
            target = target,
            options = BuildOptions.None
        };

        Debug.Log("=== BUILD START ===");
        Debug.Log("Target: " + target);
        Debug.Log("Scenes: MainMenu + KaisarWorld");
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
