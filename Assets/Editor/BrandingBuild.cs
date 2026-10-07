#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public sealed class BrandingBuild : IPreprocessBuildWithReport
{
    public int callbackOrder => -1000;

    public void OnPreprocessBuild(BuildReport report) => Apply();

    [MenuItem("tegaris82/Apply App Branding")]
    public static void Apply()
    {
        PlayerSettings.companyName = "tegaris82";
        PlayerSettings.productName = "tegaris82";
        PlayerSettings.bundleVersion = "1.0.0";
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "com.tegaris82.mmo");
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Standalone, "com.tegaris82.mmo");

        string path = "Assets/Branding/tegaris82-icon-1024.png";
        Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (icon == null) return;

        try
        {
            MethodInfo setIcons = typeof(PlayerSettings).GetMethod("SetIcons", BindingFlags.Public | BindingFlags.Static);
            if (setIcons != null)
            {
                var parameters = setIcons.GetParameters();
                if (parameters.Length == 3)
                {
                    setIcons.Invoke(null, new object[] { NamedBuildTarget.Android, new[] { icon }, Enum.Parse(parameters[2].ParameterType, "Application") });
                    setIcons.Invoke(null, new object[] { NamedBuildTarget.Standalone, new[] { icon }, Enum.Parse(parameters[2].ParameterType, "Application") });
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("tegaris82 icon setup fallback: " + e.Message);
        }
        AssetDatabase.SaveAssets();
    }
}
#endif
