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
        private int trackedFingerId = -1;
        private bool fingerStartedOnGames;

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

        private Rect GamesButtonRect()
        {
            float w = Mathf.Min(520f, Screen.width * 0.78f);
            float h = 70f;
            float x = (Screen.width - w) * 0.5f;
            float y = Screen.height * 0.50f;
            return new Rect(x, y - 45f, w, h);
        }

        // Explicit Android touch fallback. IMGUI buttons normally synthesize mouse
        // events for touch, but this ensures a tap still activates GAMES if they do not.
        private void Update()
        {
            if (loading || SceneManager.GetActiveScene().name != "MainMenu") return;
            if (Input.touchCount <= 0) return;

            Rect games = GamesButtonRect();
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 guiPoint = new Vector2(touch.position.x, Screen.height - touch.position.y);

                if (touch.phase == TouchPhase.Began)
                {
                    if (games.Contains(guiPoint))
                    {
                        trackedFingerId = touch.fingerId;
                        fingerStartedOnGames = true;
                    }
                }
                else if (touch.fingerId == trackedFingerId &&
                         (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                {
                    bool activate = fingerStartedOnGames && touch.phase == TouchPhase.Ended && games.Contains(guiPoint);
                    trackedFingerId = -1;
                    fingerStartedOnGames = false;
                    if (activate)
                    {
                        StartGame();
                        return;
                    }
                }
            }
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
                GUI.Label(new Rect(x, y + 30, w, 45), "Mohon tunggu", button);
                return;
            }

            if (GUI.Button(GamesButtonRect(), "GAMES", button))
                StartGame();

            if (GUI.Button(new Rect(x, y + 40, w, h), "SETTINGS", button))
                Debug.Log("Settings: existing game settings are unchanged.");

            if (GUI.Button(new Rect(x, y + 125, w, h), "EXIT", button))
                Application.Quit();

            if (!string.IsNullOrEmpty(statusMessage))
            {
                GUI.color = Color.white;
                GUI.Label(new Rect(x, y + 190, w, 36), statusMessage, button);
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                GUI.color = new Color(1f, 0.35f, 0.35f, 1f);
                GUI.Label(new Rect(x, y + 225, w, 110), errorMessage, button);
                GUI.color = Color.white;
            }
        }

        private void StartGame()
        {
            if (loading) return;
            loading = true;
            errorMessage = "";
            statusMessage = "Membuka KaisarWorld...";
            Debug.Log("=== GAMES BUTTON ACTIVATED ===");
            Debug.Log("Current scene: " + SceneManager.GetActiveScene().name);

            try
            {
                // Load by the scene's actual name rather than relying on a build index.
                SceneManager.LoadScene("KaisarWorld", LoadSceneMode.Single);
            }
            catch (System.Exception ex)
            {
                loading = false;
                statusMessage = "";
                errorMessage = "Gagal membuka KaisarWorld. Pastikan scene ikut APK.\n" + ex.Message;
                Debug.LogError(errorMessage);
                Debug.LogException(ex);
            }
        }
    }
}
