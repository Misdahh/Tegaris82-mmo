using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public static class BuildScript
{
    public static void BuildAndroid()
    {
        string[] guids = AssetDatabase.FindAssets("t:Scene");
        string[] scenes = guids
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(p => p.EndsWith(".unity"))
            .Where(p => !p.Contains("/MainMenu.unity"))
            .OrderBy(p => p == "Assets/Scenes/KaisarWorld.unity" ? 0 : 1)
            .ThenBy(p => p)
            .ToArray();

        if (scenes.Length == 0)
            throw new BuildFailedException("No Unity scene was found.");

        Directory.CreateDirectory("build");
        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = Path.Combine("build", "tegaris82.apk"),
            target = BuildTarget.Android,
            options = BuildOptions.None
        });

        if (report.summary.result != BuildResult.Succeeded)
            throw new BuildFailedException("Android build failed: " + report.summary.result);
    }
}
