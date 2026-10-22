using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

        private void Awake()
        {
            // MainMenu.unity already contains this component. Do not create a second
            // IMGUI controller at runtime; duplicate OnGUI calls can leave menu text
            // visually stacked over the loading state.
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            EnsureMenuCamera();
        }

        // The original MainMenu scene did not contain a Camera. On Android this can
        // leave the app on a black screen after the Unity splash. Create a minimal
        // background camera at runtime when the scene has none; IMGUI draws the menu.
        private static void EnsureMenuCamera()
        {
            Camera existing = Camera.main;
            if (existing != null) return;

            GameObject cameraObject = new GameObject("Main Camera (Runtime Fallback)");
            cameraObject.tag = "MainCamera";
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.025f, 0.04f, 0.08f, 1f);
            camera.cullingMask = 0;
            camera.orthographic = true;
            camera.orthographicSize = 5f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 100f;
            camera.transform.position = new Vector3(0f, 0f, -10f);
            camera.transform.rotation = Quaternion.identity;
            camera.targetDisplay = 0;
        }

        private void OnDestroy()
        {
            if (instance == this) instance = null;
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
            loading = true;
            errorMessage = "";
            statusMessage = "Menyiapkan dunia kerajaan...";
            StartCoroutine(LoadGameRoutine());
        }

        private IEnumerator LoadGameRoutine()
        {
            Debug.Log("=== GAMES BUTTON ACTIVATED ===");
            Debug.Log("Active scene: " + SceneManager.GetActiveScene().path);

            int buildIndex = SceneUtility.GetBuildIndexByScenePath(GameScenePath);
            Debug.Log("KaisarWorld build index: " + buildIndex);

            if (buildIndex < 0 || !Application.CanStreamedLevelBeLoaded(buildIndex))
            {
                loading = false;
                errorMessage = "SCENE KAISARWORLD TIDAK DAPAT DIMUAT. Pastikan scene ada di Build Settings dan build APK terbaru dari project ini.";
                statusMessage = "Pemeriksaan scene gagal";
                Debug.LogError(errorMessage);
                yield break;
            }

            statusMessage = "Memuat dunia kerajaan...";
            AsyncOperation operation = null;
            try
            {
                operation = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
            }
            catch (System.Exception ex)
            {
                loading = false;
                errorMessage = "Gagal memulai pemuatan: " + ex.Message;
                statusMessage = "Pemuatan gagal";
                Debug.LogException(ex);
                yield break;
            }

            if (operation == null)
            {
                loading = false;
                errorMessage = "Unity tidak berhasil memulai pemuatan scene KaisarWorld.";
                statusMessage = "Pemuatan gagal";
                Debug.LogError(errorMessage);
                yield break;
            }

            while (!operation.isDone)
            {
                statusMessage = "Memuat dunia kerajaan... " + Mathf.RoundToInt(operation.progress * 100f) + "%";
                yield return null;
            }

            Debug.Log("KaisarWorld scene load completed.");
        }

    }
}
