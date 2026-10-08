using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KaisarMMO.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private static MainMenuController instance;
        private bool visible = true;
        private bool loading;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Create()
        {
            if (instance != null) return;
            if (SceneManager.GetActiveScene().name != "MainMenu") return;

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

            if (loading)
            {
                GUI.Label(new Rect(x, y - 45, w, h), "MEMUAT GAMES...", title);
                return;
            }

            if (GUI.Button(new Rect(x, y - 45, w, h), "GAMES", button))
                StartGame();

            if (GUI.Button(new Rect(x, y + 40, w, h), "SETTINGS", button))
                Debug.Log("Settings: existing game settings are unchanged.");

            if (GUI.Button(new Rect(x, y + 125, w, h), "EXIT", button))
                Application.Quit();
        }

        private void StartGame()
        {
            if (loading) return;

            Debug.Log("=== GAMES BUTTON PRESSED ===");
            Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
            Debug.Log("Build scene count: " + SceneManager.sceneCountInBuildSettings);

            // Do not depend on SceneUtility.GetBuildIndexByScenePath here.
            // LoadScene by the scene name is simpler and uses Unity's build settings.
            loading = true;
            visible = false;
            StartCoroutine(LoadGameScene());
        }

        private IEnumerator LoadGameScene()
        {
            const string sceneName = "KaisarWorld";

            AsyncOperation operation = null;

            try
            {
                operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Could not start KaisarWorld load: " + e);
            }

            if (operation == null)
            {
                Debug.LogError(
                    "KaisarWorld could not be loaded. " +
                    "Make sure Assets/KaisarMMO/Art/Scenes/KaisarWorld.unity " +
                    "is included in the Android build.");
                loading = false;
                visible = true;
                yield break;
            }

            operation.allowSceneActivation = true;

            while (!operation.isDone)
                yield return null;

            Debug.Log("=== KAISARWORLD LOADED ===");
        }
    }
}
