using UnityEngine;
using UnityEngine.SceneManagement;

namespace KaisarMMO.UI
{
    // Built-in Unity GUI only: no UnityEngine.UI dependency.
    public class MainMenuController : MonoBehaviour
    {
        static MainMenuController instance;
        bool visible = true;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Create()
        {
            if (instance != null) return;
            var go = new GameObject("KaisarMMO_MainMenu");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<MainMenuController>();
        }

        void OnGUI()
        {
            if (!visible) return;

            // Keep the existing 3D scene, character, lighting and theme untouched.
            float w = Mathf.Min(520f, Screen.width * 0.78f);
            float h = 70f;
            float x = (Screen.width - w) * 0.5f;
            float y = Screen.height * 0.50f;

            GUIStyle title = new GUIStyle(GUI.skin.label) {
                fontSize = Mathf.Clamp(Screen.width / 18, 28, 56),
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            GUIStyle button = new GUIStyle(GUI.skin.button) {
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

        void StartGame()
        {
            visible = false;
            SceneManager.LoadScene("KaisarWorld");
        }
    }
}
