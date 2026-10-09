using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public static class BuildScript
{
    public static void BuildAndroid()
    {
        string[] scenes =
        {
            "Assets/KaisarMMO/Art/Scenes/MainMenu.unity",
            "Assets/KaisarMMO/Art/Scenes/KaisarWorld.unity"
        };

        foreach (string scene in scenes)
        {
            if (!File.Exists(scene))
                throw new BuildFailedException("Required scene not found: " + scene);

            string meta = scene + ".meta";
            if (!File.Exists(meta))
                throw new BuildFailedException("Scene .meta file not found: " + meta);
        }

        // Keep the Editor build scene list aligned with the exact scene list passed to BuildPlayer.
        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(scenes[0], true),
            new EditorBuildSettingsScene(scenes[1], true)
        };

        Directory.CreateDirectory("build");
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "build/tegaris82.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        UnityEngine.Debug.Log("=== ANDROID BUILD START ===");
        UnityEngine.Debug.Log("Target: Android");
        UnityEngine.Debug.Log("Scene count: " + options.scenes.Length);
        UnityEngine.Debug.Log("Build scene 0: " + scenes[0]);
        UnityEngine.Debug.Log("Build scene 1: " + scenes[1]);
        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
            throw new BuildFailedException("Android build failed: " + report.summary.result);

        UnityEngine.Debug.Log("=== ANDROID BUILD SUCCESS ===");
        UnityEngine.Debug.Log("APK: build/tegaris82.apk");
    }
}
