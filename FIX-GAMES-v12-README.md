# Fix GAMES v12

Changes:
- Removed the second, manually tracked touch path from MainMenuController. The visible IMGUI `GUI.Button` remains the single activation path and supports Android touch.
- Scene validation now trusts the exact scene build index and checks `Application.CanStreamedLevelBeLoaded(buildIndex)` instead of rejecting a valid index because a name/path lookup differs.
- Scene loading uses `SceneManager.LoadScene(buildIndex, LoadSceneMode.Single)` so the menu cannot remain indefinitely in the async-loading label.
- BuildScript checks that both scene files and their `.meta` files exist and logs the Android target and scene count.

Validation limit:
- ZIP structure and source edits were checked, but this environment does not run Unity Editor or an Android device. A successful GitHub build and an on-device test are still required to confirm runtime behavior.
