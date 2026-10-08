using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KaisarMMO.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private static MainMenuController instance;
        private bool loading;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Create()
        {
            if (SceneManager.GetActiveScene().name != "MainMenu") return;
            if (FindObjectOfType<MainMenuController>() != null) return;

            var go = new GameObject("KaisarMMO_MainMenu");
            instance = go.AddComponent<MainMenuController>();
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private void OnGUI()
        {
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

            Rect gamesRect = new Rect(x, y - 45, w, h);
            if (GUI.Button(gamesRect, "GAMES", button))
                StartGame();

            if (GUI.Button(new Rect(x, y + 40, w, h), "SETTINGS", button))
                Debug.Log("Settings: existing game settings are unchanged.");

            if (GUI.Button(new Rect(x, y + 125, w, h), "EXIT", button))
                Application.Quit();

            // Extra Android touch fallback. This does not alter the menu appearance.
            if (Event.current.type == EventType.TouchUp && gamesRect.Contains(Event.current.touch.position))
                StartGame();
        }

        private void StartGame()
        {
            if (loading) return;
            loading = true;
            Debug.Log("=== GAMES BUTTON PRESSED ===");
            StartCoroutine(LoadGameScene());
        }

        private IEnumerator LoadGameScene()
        {
            const string sceneName = "KaisarWorld";
            int index = SceneUtility.GetBuildIndexByScenePath("Assets/KaisarMMO/Art/Scenes/KaisarWorld.unity");
            Debug.Log("KaisarWorld build index = " + index);

            if (index < 0)
            {
                Debug.LogError("KAISARWORLD NOT IN BUILD SETTINGS");
                loading = false;
                yield break;
            }

            AsyncOperation op = null;
            try
            {
                op = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }

            if (op == null)
            {
                loading = false;
                yield break;
            }

            op.allowSceneActivation = true;
            while (!op.isDone)
                yield return null;

            Debug.Log("=== KAISARWORLD LOADED ===");
        }
    }
}
