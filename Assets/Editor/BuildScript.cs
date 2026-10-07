using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public static class BuildScript
{
    public static void BuildAndroid()
    {
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });
        var scenes = sceneGuids.Select(AssetDatabase.GUIDToAssetPath)
            .Where(p => p.EndsWith(".unity"))
            .OrderBy(p => p == "Assets/Scenes/MainMenu.unity" ? 0 : 1)
            .ThenBy(p => p)
            .ToArray();

        if (scenes.Length == 0)
            throw new BuildFailedException("No Unity scenes found under Assets/Scenes.");

        Directory.CreateDirectory("build");
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions {
            scenes = scenes,
            locationPathName = Path.Combine("build", "tegaris82.apk"),
            target = BuildTarget.Android,
            options = BuildOptions.None
        });

        if (report.summary.result != BuildResult.Succeeded)
            throw new BuildFailedException("Android build failed: " + report.summary.result);
    }
}
