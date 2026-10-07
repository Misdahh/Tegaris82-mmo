using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildScript
{
    public static void BuildAndroid()
    {
        // Find all Unity scenes in the project.
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene");

        if (sceneGuids == null || sceneGuids.Length == 0)
        {
            throw new BuildFailedException(
                "No Unity scene was found. Create and save a scene under Assets/Scenes/ first.");
        }

        // Prefer Assets/Scenes/MainScene.unity, then the first scene found.
        string preferred = "Assets/Scenes/MainScene.unity";
        string scenePath = sceneGuids
            .Select(AssetDatabase.GUIDToAssetPath)
            .FirstOrDefault(p => p == preferred);

        if (string.IsNullOrEmpty(scenePath))
            scenePath = sceneGuids
                .Select(AssetDatabase.GUIDToAssetPath)
                .OrderBy(p => p)
                .First();

        string outputDir = "build";
        Directory.CreateDirectory(outputDir);
        string outputPath = Path.Combine(outputDir, "tegaris82.apk");

        Debug.Log("=== Android build scene ===");
        Debug.Log("Using scene: " + scenePath);
        Debug.Log("Output: " + outputPath);

        EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.Android,
            BuildTarget.Android
        );

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = new[] { scenePath },
            locationPathName = outputPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new BuildFailedException(
                "Android build failed: " + report.summary.result);
        }

        Debug.Log("=== Android APK build SUCCESS ===");
        Debug.Log("APK: " + outputPath);
    }
}
