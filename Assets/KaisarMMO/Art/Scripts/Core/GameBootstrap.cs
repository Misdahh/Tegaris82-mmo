using UnityEngine;
using UnityEngine.SceneManagement;
using KaisarMMO.Player;
using KaisarMMO.Combat;
using KaisarMMO.Networking;

namespace KaisarMMO.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        public GameConfig config;

        void Start()
        {
            Application.targetFrameRate = 60;
            CreateWorldVisuals();

            if (FindObjectOfType<ClientConnection>() == null)
            {
                var net = new GameObject("Network");
                net.AddComponent<ClientConnection>();
            }

            var player = new GameObject("Player");
            player.transform.position = new Vector3(0, 1, 0);

            var cc = player.AddComponent<CharacterController>();
            cc.height = 2f;
            cc.radius = .45f;
            cc.center = new Vector3(0, 1, 0);

            var visual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            visual.name = "PlayerVisual";
            visual.transform.SetParent(player.transform, false);
            visual.transform.localPosition = new Vector3(0, 1, 0);
            visual.transform.localScale = new Vector3(.8f, 1f, .8f);
            Destroy(visual.GetComponent<Collider>());

            var pc = player.AddComponent<PlayerController>();
            var combat = player.AddComponent<CombatController>();
            pc.combat = combat;

            var world = gameObject.GetComponent<WorldClient>();
            if (world == null) world = gameObject.AddComponent<WorldClient>();
            world.localPlayer = pc;
            world.config = config;

            CreateCamera(player.transform);
            if (GetComponent<WorldRuntimeUI>() == null) gameObject.AddComponent<WorldRuntimeUI>();
        }

        void CreateWorldVisuals()
        {
            if (GameObject.Find("WorldGround") != null) return;

            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "WorldGround";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(20, 1, 20);

            for (int i = 0; i < 12; i++)
            {
                float a = i * Mathf.PI * 2f / 12f;
                var tree = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                tree.name = "WorldTree_" + i;
                tree.transform.position = new Vector3(Mathf.Cos(a) * 10f, 1f, Mathf.Sin(a) * 10f);
                tree.transform.localScale = new Vector3(.7f, 1.5f, .7f);
            }
        }

        void CreateCamera(Transform target)
        {
            var old = Camera.main;
            if (old != null) Destroy(old.gameObject);

            var c = new GameObject("ThirdPersonCamera").AddComponent<Camera>();
            c.tag = "MainCamera";
            c.transform.position = target.position + new Vector3(0, 4, -7);
            c.transform.LookAt(target.position + Vector3.up * 1.5f);
            var follow = c.gameObject.AddComponent<ThirdPersonFollow>();
            follow.target = target;
        }
    }

    public class ThirdPersonFollow : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0, 4, -7);
        void LateUpdate()
        {
            if (!target) return;
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * 8);
            transform.LookAt(target.position + Vector3.up * 1.5f);
        }
    }

    public class WorldRuntimeUI : MonoBehaviour
    {
        GUIStyle title, info, button;
        void Start()
        {
            title = new GUIStyle(GUI.skin.label) { fontSize = 34, alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
            info = new GUIStyle(GUI.skin.label) { fontSize = 20, alignment = TextAnchor.MiddleCenter };
            button = new GUIStyle(GUI.skin.button) { fontSize = 22 };
        }
        void OnGUI()
        {
            float w = Mathf.Min(520f, Screen.width * .8f);
            float x = (Screen.width - w) * .5f;
            GUI.Label(new Rect(x, 20, w, 60), "KAISAR WORLD", title);
            GUI.Label(new Rect(x, 80, w, 45), "GAME BERHASIL DIMUAT", info);
            if (GUI.Button(new Rect(20, Screen.height - 80, 220, 55), "KEMBALI", button))
                SceneManager.LoadScene("MainMenu");
        }
    }
}
