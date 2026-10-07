using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace KaisarMMO.UI
{
    public class MainMenuController : MonoBehaviour
    {
        Font font;

        void Awake()
        {
            // Keep the existing game/theme untouched. This only creates the startup UI.
            Application.targetFrameRate = 60;
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (font == null)
                font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        void Start()
        {
            EnsureEventSystem();
            BuildMenu();
        }

        void EnsureEventSystem()
        {
            if (EventSystem.current != null) return;
            var go = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            DontDestroyOnLoad(go);
        }

        void BuildMenu()
        {
            var canvasGO = new GameObject("MainMenuCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            var scaler = canvasGO.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0.5f;

            var bg = MakeImage(canvas.transform, "Background", new Color(0.025f, 0.035f, 0.06f, 1f));
            Stretch(bg.rectTransform);

            var title = MakeText(canvas.transform, "Title", "KAISAR MMO", 64, TextAnchor.MiddleCenter);
            SetRect(title.rectTransform, new Vector2(0.5f,0.5f), new Vector2(0.5f,0.5f), new Vector2(0,150), new Vector2(620,100));

            var subtitle = MakeText(canvas.transform, "Subtitle", "3D ONLINE ADVENTURE", 22, TextAnchor.MiddleCenter);
            SetRect(subtitle.rectTransform, new Vector2(0.5f,0.5f), new Vector2(0.5f,0.5f), new Vector2(0,88), new Vector2(620,55));

            MakeButton(canvas.transform, "GAMES", "GAMES", 0, StartGame);
            MakeButton(canvas.transform, "SETTINGS", "SETTINGS", 1, ShowSettings);
            MakeButton(canvas.transform, "EXIT", "EXIT", 2, QuitGame);

            var info = MakeText(canvas.transform, "Info", "TEGARIS82 • MOBILE MMO", 18, TextAnchor.MiddleCenter);
            SetRect(info.rectTransform, new Vector2(0.5f,0), new Vector2(0.5f,0), new Vector2(0,45), new Vector2(620,50));
        }

        Image MakeImage(Transform parent, string name, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            go.GetComponent<Image>().color = color;
            return go.GetComponent<Image>();
        }

        Text MakeText(Transform parent, string name, string value, int size, TextAnchor anchor)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var t = go.GetComponent<Text>();
            t.font = font;
            t.text = value;
            t.fontSize = size;
            t.alignment = anchor;
            t.color = Color.white;
            t.horizontalOverflow = HorizontalWrapMode.Overflow;
            t.verticalOverflow = VerticalWrapMode.Overflow;
            return t;
        }

        void MakeButton(Transform parent, string name, string label, int index, UnityEngine.Events.UnityAction action)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            SetRect(rt, new Vector2(0.5f,0.5f), new Vector2(0.5f,0.5f), new Vector2(0, 10-index*82), new Vector2(420,62));

            var image = go.GetComponent<Image>();
            image.color = new Color(0.10f,0.22f,0.38f,1f);
            var button = go.GetComponent<Button>();
            button.onClick.AddListener(action);

            var text = MakeText(go.transform, "Label", label, 25, TextAnchor.MiddleCenter);
            Stretch(text.rectTransform);
        }

        void StartGame()
        {
            SceneManager.LoadScene("KaisarWorld");
        }

        void ShowSettings()
        {
            Debug.Log("Settings menu coming soon.");
        }

        void QuitGame()
        {
            Application.Quit();
        }

        void Stretch(RectTransform r)
        {
            r.anchorMin = Vector2.zero;
            r.anchorMax = Vector2.one;
            r.offsetMin = Vector2.zero;
            r.offsetMax = Vector2.zero;
        }

        void SetRect(RectTransform r, Vector2 min, Vector2 max, Vector2 pos, Vector2 size)
        {
            r.anchorMin = min;
            r.anchorMax = max;
            r.anchoredPosition = pos;
            r.sizeDelta = size;
        }
    }
}
