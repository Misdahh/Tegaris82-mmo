using UnityEngine;
using UnityEngine.SceneManagement;

namespace KaisarMMO.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private static MainMenuController instance;
        private bool visible = true;
        private const string GameSceneName = "KaisarWorld";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Create()
        {
            if (instance != null) return;

            // Only create the menu controller in the Main Menu.
            if (SceneManager.GetActiveScene().name != "MainMenu")
                return;

            var go = new GameObject("KaisarMMO_MainMenu");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<MainMenuController>();
        }

        private void OnGUI()
        {
            if (!visible) return;

            float w = Mathf.Min(520f, Screen.width * 0.78f);
            float h = 70f;
            float x = (Screen.width - w) * 0.5f;
            float y = Screen.height * 0.50f;

            GUIStyle title = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.Clamp(Screen.width / 18, 28, 56),
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };

            GUIStyle button = new GUIStyle(GUI.skin.button)
            {
                fontSize = Mathf.Clamp(Screen.width / 32, 20, 34)
            };

            GUI.Label(new Rect(x, y - 145, w, 70), "KAISAR MMO", title);

            if (GUI.Button(new Rect(x, y - 45, w, h), "GAMES", button))
                StartGame();

            if (GUI.Button(new Rect(x, y + 40, w, h), "SETTINGS", button))
                Debug.Log("Settings: existing game settings are unchanged.");

            if (GUI.Button(new Rect(x, y + 125, w, h), "EXIT", button))
                Application.Quit();
        }

        private void StartGame()
        {
            Debug.Log("GAMES button pressed. Loading KaisarWorld...");

            int gameSceneIndex = SceneUtility.GetBuildIndexByScenePath(
                "Assets/KaisarMMO/Art/Scenes/KaisarWorld.unity");

            if (gameSceneIndex < 0)
            {
                Debug.LogError(
                    "KaisarWorld is NOT included in the Android build. " +
                    "Expected scene: Assets/KaisarMMO/Art/Scenes/KaisarWorld.unity");
                return;
            }

            visible = false;
            SceneManager.LoadScene(gameSceneIndex, LoadSceneMode.Single);
        }
    }
}
