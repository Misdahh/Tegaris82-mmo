using UnityEngine;
using UnityEngine.SceneManagement;

namespace KaisarMMO.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private static MainMenuController instance;
        private bool loading;
        private string errorMessage = "";

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

            if (GUI.Button(new Rect(x, y - 45, w, h), "GAMES", button))
                StartGame();

            if (GUI.Button(new Rect(x, y + 40, w, h), "SETTINGS", button))
                Debug.Log("Settings: existing game settings are unchanged.");

            if (GUI.Button(new Rect(x, y + 125, w, h), "EXIT", button))
                Application.Quit();

            if (!string.IsNullOrEmpty(errorMessage))
            {
                GUI.color = Color.red;
                GUI.Label(new Rect(x, y + 205, w, 80), errorMessage);
                GUI.color = Color.white;
            }
        }

        private void StartGame()
        {
            if (loading) return;
            loading = true;
            errorMessage = "";
            Debug.Log("=== GAMES BUTTON PRESSED ===");

            // This project has a tiny runtime-generated world. A synchronous scene
            // switch is intentionally used here: it removes the Android async
            // loading state that previously left the menu stuck on "MEMUAT GAMES...".
            try
            {
                int index = SceneUtility.GetBuildIndexByScenePath(
                    "Assets/KaisarMMO/Art/Scenes/KaisarWorld.unity");

                Debug.Log("KaisarWorld build index = " + index);
                if (index < 0)
                    throw new System.Exception("KaisarWorld belum masuk Build Settings.");

                SceneManager.LoadScene(index, LoadSceneMode.Single);
            }
            catch (System.Exception ex)
            {
                loading = false;
                errorMessage = "Gagal membuka Games:\n" + ex.Message;
                Debug.LogException(ex);
            }
        }
    }
}
