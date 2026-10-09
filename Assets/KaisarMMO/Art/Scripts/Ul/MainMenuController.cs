using UnityEngine;
using UnityEngine.SceneManagement;

namespace KaisarMMO.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private static MainMenuController instance;
        private bool loading;
        private string errorMessage = "";
        private string statusMessage = "";

        private const string MainMenuScene = "MainMenu";
        private const string GameSceneName = "KaisarWorld";
        private const string GameScenePath = "Assets/KaisarMMO/Art/Scenes/KaisarWorld.unity";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Create()
        {
            if (SceneManager.GetActiveScene().name != MainMenuScene) return;
            if (FindObjectOfType<MainMenuController>() != null) return;
            new GameObject("KaisarMMO_MainMenu").AddComponent<MainMenuController>();
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

        private Rect GamesButtonRect()
        {
            float w = Mathf.Min(520f, Screen.width * 0.78f);
            float h = 70f;
            float x = (Screen.width - w) * 0.5f;
            float y = Screen.height * 0.50f;
            // Match the visible GAMES button position on tall mobile screens.
            return new Rect(x, y - 20f, w, h);
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
                fontStyle = FontStyle.Bold,
                wordWrap = true
            };
            GUIStyle button = new GUIStyle(GUI.skin.button)
            {
                fontSize = Mathf.Clamp(Screen.width / 32, 20, 34),
                wordWrap = true
            };

            GUI.Label(new Rect(x, y - 145, w, 70), "KAISAR MMO", title);
            if (loading)
            {
                GUI.Label(GamesButtonRect(), "MEMUAT GAMES...", button);
                GUI.Label(new Rect(x, y + 55, w, 70), statusMessage, button);
            }
            else
            {
                if (GUI.Button(GamesButtonRect(), "GAMES", button)) StartGame();
                if (GUI.Button(new Rect(x, y + 65, w, h), "SETTINGS", button))
                    Debug.Log("Settings: existing game settings are unchanged.");
                if (GUI.Button(new Rect(x, y + 150, w, h), "EXIT", button)) Application.Quit();
            }

            if (!string.IsNullOrEmpty(statusMessage) && !loading)
            {
                GUI.color = Color.white;
                GUI.Label(new Rect(x, y + 235, w, 55), statusMessage, button);
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                GUI.color = new Color(1f, 0.35f, 0.35f, 1f);
                GUI.Label(new Rect(x, y + 235, w, 120), errorMessage, button);
                GUI.color = Color.white;
            }
        }

        private void StartGame()
        {
            if (loading) return;

            Debug.Log("=== GAMES BUTTON ACTIVATED ===");
            Debug.Log("Active scene: " + SceneManager.GetActiveScene().path);

            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GameScenePath);
            Debug.Log("KaisarWorld build index: " + buildIndex);

            if (buildIndex < 0)
            {
                loading = false;
                errorMessage = "SCENE KAISARWORLD TIDAK ADA DI BUILD APK. Periksa BuildScript dan path scene.";
                statusMessage = "Pemeriksaan scene gagal";
                Debug.LogError(errorMessage);
                return;
            }

            if (!Application.CanStreamedLevelBeLoaded(buildIndex))
            {
                loading = false;
                errorMessage = "APK tidak dapat memuat scene KaisarWorld pada Build Index " + buildIndex + ".";
                statusMessage = "Scene tidak dapat dimuat";
                Debug.LogError(errorMessage);
                return;
            }

            loading = true;
            errorMessage = "";
            statusMessage = "Membuka dunia kerajaan...";
            try
            {
                // A synchronous load avoids a UI stuck in an unfinished async-loading state.
                SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
            }
            catch (System.Exception ex)
            {
                loading = false;
                errorMessage = "Gagal membuka KaisarWorld: " + ex.Message;
                statusMessage = "Pemuatan gagal";
                Debug.LogException(ex);
            }
        }
    }
}
