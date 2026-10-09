# Fix GAMES scene loading (v11)

This patch keeps the existing menu styling and makes scene loading fail visibly instead of remaining on “MEMUAT GAMES...”.

Changed files:
- `Assets/KaisarMMO/Art/Scripts/Ul/MainMenuController.cs`
- `Assets/Editor/BuildScript.cs`

The menu now verifies that `KaisarWorld` is included in the APK before loading it. The build method explicitly enables both `MainMenu` and `KaisarWorld` in `EditorBuildSettings` and in the player build scene list.

Use the whole ZIP to replace the repository contents, then run the Android workflow. If the scene is still missing, the menu should show a clear error instead of an endless loading label. Runtime behavior still needs testing in the actual Android APK.
