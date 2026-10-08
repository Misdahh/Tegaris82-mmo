using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KaisarMMO.Player;

namespace KaisarMMO.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        private PlayerActor player;
        private readonly List<EnemyActor> enemies = new List<EnemyActor>();
        private int gold;
        private int defeated;

        private void Start()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            SetupLighting();
            BuildWorld();
            CreatePlayer();
            CreateEnemies();
            CreateUI();
        }

        private void SetupLighting()
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.32f, 0.38f, 0.45f);
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.006f;
            RenderSettings.ambientLight = new Color(0.45f, 0.48f, 0.55f);

            var sun = GameObject.Find("KaisarSun");
            if (sun == null)
            {
                sun = new GameObject("KaisarSun");
                var light = sun.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 1.15f;
                light.color = new Color(1f, 0.9f, 0.75f);
                sun.transform.rotation = Quaternion.Euler(38f, -32f, 0f);
            }
        }

        private Material Mat(Color c, float metallic = 0f, float smooth = 0.25f)
        {
            var m = new Material(Shader.Find("Standard"));
            m.color = c;
            m.SetFloat("_Metallic", metallic);
            m.SetFloat("_Glossiness", smooth);
            return m;
        }

        private GameObject Cube(string name, Vector3 pos, Vector3 scale, Material mat)
        {
            var g = GameObject.CreatePrimitive(PrimitiveType.Cube);
            g.name = name;
            g.transform.position = pos;
            g.transform.localScale = scale;
            g.GetComponent<Renderer>().material = mat;
            return g;
        }

        private GameObject Cylinder(string name, Vector3 pos, Vector3 scale, Material mat)
        {
            var g = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            g.name = name;
            g.transform.position = pos;
            g.transform.localScale = scale;
            g.GetComponent<Renderer>().material = mat;
            return g;
        }

        private void BuildWorld()
        {
            if (GameObject.Find("KaisarWorldGenerated") != null) return;
            var root = new GameObject("KaisarWorldGenerated");

            Material grass = Mat(new Color(0.20f, 0.32f, 0.16f));
            Material road = Mat(new Color(0.35f, 0.30f, 0.24f));
            Material stone = Mat(new Color(0.35f, 0.36f, 0.39f));
            Material wood = Mat(new Color(0.30f, 0.12f, 0.055f));
            Material red = Mat(new Color(0.55f, 0.035f, 0.025f));
            Material goldM = Mat(new Color(0.82f, 0.58f, 0.08f), 0.55f, 0.65f);
            Material water = Mat(new Color(0.06f, 0.28f, 0.42f), 0.15f, 0.75f);
            Material tree = Mat(new Color(0.08f, 0.25f, 0.08f));
            Material trunk = Mat(new Color(0.24f, 0.12f, 0.05f));

            var ground = Cube("WorldGround", new Vector3(0,-0.3f,0), new Vector3(120,0.6f,120), grass);
            ground.transform.SetParent(root.transform);

            var mainRoad = Cube("ImperialRoad", new Vector3(0,0.02f,0), new Vector3(10,0.08f,110), road);
            mainRoad.transform.SetParent(root.transform);
            var crossRoad = Cube("ImperialRoadCross", new Vector3(0,0.025f,0), new Vector3(110,0.09f,10), road);
            crossRoad.transform.SetParent(root.transform);

            // Palace complex
            Cube("PalaceBase", new Vector3(0,1.0f,25), new Vector3(26,2,18), stone).transform.SetParent(root.transform);
            Cube("PalaceHall", new Vector3(0,6,25), new Vector3(18,9,10), wood).transform.SetParent(root.transform);
            Cube("PalaceRoof", new Vector3(0,11.2f,25), new Vector3(21,1.2f,13), red).transform.SetParent(root.transform);
            for (int i=-2;i<=2;i++)
            {
                var col=Cylinder("PalaceColumn",new Vector3(i*3.8f,4.3f,19.6f),new Vector3(.55f,4.3f,.55f),goldM);
                col.transform.SetParent(root.transform);
            }
            Cube("PalaceDoor", new Vector3(0,3.2f,19.85f), new Vector3(4,6,0.5f), red).transform.SetParent(root.transform);

            // Bridge and water
            var pond = Cube("RoyalLake", new Vector3(38,-0.05f,22), new Vector3(28,0.1f,24), water);
            pond.transform.SetParent(root.transform);

            // Trees around the kingdom
            for (int i=0;i<42;i++)
            {
                float a=i*2.399f;
                float r=34f+(i%5)*6f;
                Vector3 p=new Vector3(Mathf.Cos(a)*r,0,Mathf.Sin(a)*r);
                var t=Cylinder("TreeTrunk",p+Vector3.up*2f,new Vector3(.65f,2f,.65f),trunk);
                var crown=GameObject.CreatePrimitive(PrimitiveType.Sphere);
                crown.name="TreeCrown";
                crown.transform.position=p+Vector3.up*5.1f;
                crown.transform.localScale=Vector3.one*3.6f;
                crown.GetComponent<Renderer>().material=tree;
                t.transform.SetParent(root.transform);
                crown.transform.SetParent(root.transform);
            }

            // Market stalls / houses
            for (int i=-2;i<=2;i++)
            {
                float z=10+i*8;
                Cube("House",new Vector3(18,2.2f,z),new Vector3(7,4.4f,5),wood).transform.SetParent(root.transform);
                Cube("HouseRoof",new Vector3(18,5.0f,z),new Vector3(8,1.0f,6),red).transform.SetParent(root.transform);
                Cube("House",new Vector3(-18,2.2f,z),new Vector3(7,4.4f,5),wood).transform.SetParent(root.transform);
                Cube("HouseRoof",new Vector3(-18,5.0f,z),new Vector3(8,1.0f,6),red).transform.SetParent(root.transform);
            }
        }

        private void CreatePlayer()
        {
            var go = new GameObject("Player");
            go.transform.position = new Vector3(0,0.1f,-25f);
            var controller=go.AddComponent<CharacterController>();
            controller.height=2f; controller.radius=.42f; controller.center=Vector3.up;

            var body=GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name="ImperialHero";
            body.transform.SetParent(go.transform,false);
            body.transform.localPosition=Vector3.up;
            body.transform.localScale=new Vector3(.75f,1f,.75f);
            body.GetComponent<Renderer>().material=Mat(new Color(0.12f,0.16f,0.24f));
            Destroy(body.GetComponent<Collider>());

            var head=GameObject.CreatePrimitive(PrimitiveType.Sphere);
            head.transform.SetParent(go.transform,false);
            head.transform.localPosition=new Vector3(0,2.15f,0);
            head.transform.localScale=Vector3.one*.52f;
            head.GetComponent<Renderer>().material=Mat(new Color(0.72f,0.50f,0.35f));
            Destroy(head.GetComponent<Collider>());

            var weapon=GameObject.CreatePrimitive(PrimitiveType.Cube);
            weapon.name="ImperialSword";
            weapon.transform.SetParent(go.transform,false);
            weapon.transform.localPosition=new Vector3(.62f,1.0f,.45f);
            weapon.transform.localRotation=Quaternion.Euler(25,0,25);
            weapon.transform.localScale=new Vector3(.12f,1.15f,.08f);
            weapon.GetComponent<Renderer>().material=Mat(new Color(.75f,.78f,.82f),.8f,.8f);
            Destroy(weapon.GetComponent<Collider>());

            player=go.AddComponent<PlayerActor>();
            player.speed=5.2f;
            player.sprintSpeed=8.0f;
            player.onAttack=Attack;
            CreateCamera(go.transform);
        }

        private void CreateCamera(Transform target)
        {
            var old=Camera.main;
            if(old!=null) Destroy(old.gameObject);
            var cam=new GameObject("MainCamera").AddComponent<Camera>();
            cam.tag="MainCamera";
            cam.fieldOfView=62;
            cam.transform.position=target.position+new Vector3(0,5.2f,-8.5f);
            cam.transform.LookAt(target.position+Vector3.up*1.2f);
            var follow=cam.gameObject.AddComponent<FollowCamera>();
            follow.target=target;
        }

        private void CreateEnemies()
        {
            Material armor=Mat(new Color(.32f,.08f,.07f));
            for(int i=0;i<7;i++)
            {
                float a=i*Mathf.PI*2f/7f;
                Vector3 p=new Vector3(Mathf.Cos(a)*14f,0.1f,Mathf.Sin(a)*14f+8f);
                var e=new GameObject("Bandit_"+i);
                e.transform.position=p;
                var cc=e.AddComponent<CharacterController>();
                cc.height=2f; cc.radius=.4f; cc.center=Vector3.up;
                var body=GameObject.CreatePrimitive(PrimitiveType.Capsule);
                body.transform.SetParent(e.transform,false);
                body.transform.localPosition=Vector3.up;
                body.GetComponent<Renderer>().material=armor;
                Destroy(body.GetComponent<Collider>());
                var ai=e.AddComponent<EnemyActor>();
                ai.target=player;
                ai.hp=60;
                ai.onDeath=EnemyDied;
                enemies.Add(ai);
            }
        }

        private void CreateUI()
        {
            var ui=new GameObject("KaisarGameUI");
            var hud=ui.AddComponent<KaisarGameUI>();
            hud.player=player;
            hud.onAttack=Attack;
            hud.onBack=BackToMenu;
            hud.GetStats=()=> "Level 1   HP "+Mathf.RoundToInt(player.hp)+"/100   Musuh "+defeated+"/7   Emas "+gold;
        }

        private void Attack()
        {
            if(player==null || !player.CanAttack) return;
            player.TriggerAttack();
            for(int i=enemies.Count-1;i>=0;i--)
            {
                if(enemies[i]==null) { enemies.RemoveAt(i); continue; }
                if(Vector3.Distance(player.transform.position,enemies[i].transform.position)<=3.2f)
                    enemies[i].TakeDamage(25);
            }
        }

        private void EnemyDied(EnemyActor e)
        {
            defeated++;
            gold+=10;
            enemies.Remove(e);
            if(e!=null) Destroy(e.gameObject);
        }

        private void BackToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public class PlayerActor : MonoBehaviour
    {
        public float speed=5.2f,sprintSpeed=8f,hp=100f;
        public System.Action onAttack;
        private CharacterController cc;
        private Vector3 velocity;
        private float attackCd;
        private Vector2 touchMove;
        public bool CanAttack => attackCd<=0;

        void Awake(){cc=GetComponent<CharacterController>();}
        void Update()
        {
            attackCd-=Time.deltaTime;
            Vector2 input=new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
            if(KaisarGameInput.TouchMove.sqrMagnitude>.01f) input=KaisarGameInput.TouchMove;
            bool sprint=Input.GetKey(KeyCode.LeftShift)||KaisarGameInput.Sprint;
            Move(input,sprint);
            if(Input.GetKeyDown(KeyCode.Space)) TriggerAttack();
        }
        public void Move(Vector2 input,bool sprint)
        {
            input=Vector2.ClampMagnitude(input,1);
            Vector3 dir=new Vector3(input.x,0,input.y);
            if(dir.sqrMagnitude>.001f)
            {
                float s=sprint?sprintSpeed:speed;
                cc.Move(dir*s*Time.deltaTime);
                transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(dir),Time.deltaTime*12f);
            }
            if(cc.isGrounded && velocity.y<0) velocity.y=-2f;
            velocity.y+=-22f*Time.deltaTime;
            cc.Move(velocity*Time.deltaTime);
        }
        public void TriggerAttack(){if(!CanAttack)return;attackCd=.55f;onAttack?.Invoke();}
        public void Damage(float d){hp=Mathf.Max(0,hp-d);}
    }

    public class EnemyActor : MonoBehaviour
    {
        public PlayerActor target;
        public int hp=60;
        public System.Action<EnemyActor> onDeath;
        float hitCd;
        void Update()
        {
            if(target==null)return;
            hitCd-=Time.deltaTime;
            float d=Vector3.Distance(transform.position,target.transform.position);
            if(d<9f && d>2.4f)
            {
                Vector3 dir=(target.transform.position-transform.position); dir.y=0;
                transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(dir),Time.deltaTime*5f);
                var cc=GetComponent<CharacterController>();
                cc.Move(transform.forward*2.2f*Time.deltaTime);
            }
            if(d<=2.4f && hitCd<=0){hitCd=1.2f;target.Damage(8);}
        }
        public void TakeDamage(int d){hp-=d;if(hp<=0)onDeath?.Invoke(this);}
    }

    public class FollowCamera : MonoBehaviour
    {
        public Transform target;
        Vector3 offset=new Vector3(0,5.2f,-8.5f);
        void LateUpdate()
        {
            if(!target)return;
            transform.position=Vector3.Lerp(transform.position,target.position+offset,Time.deltaTime*7f);
            transform.LookAt(target.position+Vector3.up*1.15f);
        }
    }

    public static class KaisarGameInput
    {
        public static Vector2 TouchMove;
        public static bool Sprint;
    }

    public class KaisarGameUI : MonoBehaviour
    {
        public PlayerActor player;
        public System.Action onAttack,onBack;
        public System.Func<string> GetStats;
        GUIStyle title,stats,button;
        bool dragging;
        void Start()
        {
            title=new GUIStyle(GUI.skin.label){fontSize=Mathf.Clamp(Screen.width/22,24,38),alignment=TextAnchor.UpperLeft,fontStyle=FontStyle.Bold};
            stats=new GUIStyle(GUI.skin.label){fontSize=Mathf.Clamp(Screen.width/38,16,25),alignment=TextAnchor.UpperLeft};
            button=new GUIStyle(GUI.skin.button){fontSize=Mathf.Clamp(Screen.width/32,18,28)};
        }
        void Update()
        {
            Vector2 mv=Vector2.zero;
            for(int i=0;i<Input.touchCount;i++)
            {
                Touch t=Input.GetTouch(i);
                if(t.position.x<Screen.width*.48f && t.position.y<Screen.height*.55f)
                {
                    Vector2 center=new Vector2(Screen.width*.18f,Screen.height*.22f);
                    mv=Vector2.ClampMagnitude((t.position-center)/(Screen.height*.14f),1);
                    dragging=true;
                }
            }
            if(Input.touchCount==0)dragging=false;
            KaisarGameInput.TouchMove=mv;
        }
        void OnGUI()
        {
            if(player==null)return;
            GUI.Label(new Rect(22,18,Screen.width*.9f,48),"KAISAR WORLD",title);
            GUI.Label(new Rect(22,66,Screen.width*.9f,40),GetStats(),stats);

            // Joystick
            float r=Screen.height*.075f;
            Vector2 c=new Vector2(Screen.width*.18f,Screen.height*.22f);
            GUI.color=new Color(1,1,1,.18f);
            GUI.DrawTexture(new Rect(c.x-r,c.y-r,r*2,r*2),Texture2D.whiteTexture);
            GUI.color=new Color(1,1,1,.38f);
            Vector2 knob=c+KaisarGameInput.TouchMove*r*.65f;
            GUI.DrawTexture(new Rect(knob.x-r*.38f,knob.y-r*.38f,r*.76f,r*.76f),Texture2D.whiteTexture);
            GUI.color=Color.white;

            float bw=Screen.width*.22f;
            if(GUI.Button(new Rect(Screen.width-bw-24,Screen.height-155,bw,72),"SERANG",button))onAttack?.Invoke();
            if(GUI.Button(new Rect(Screen.width-bw-24,Screen.height-75,bw,58),"KELUAR",button))onBack?.Invoke();

            GUIStyle hint=new GUIStyle(GUI.skin.label){fontSize=16,alignment=TextAnchor.MiddleCenter};
            GUI.Label(new Rect(20,Screen.height-52,Screen.width*.48f,35),"Joystick kiri • SERANG untuk bertarung",hint);
        }
    }
}
