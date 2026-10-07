using System.Collections.Generic;
using UnityEngine;
using KaisarMMO.Player;
using KaisarMMO.Core;

namespace KaisarMMO.Networking
{
    public class WorldClient : MonoBehaviour
    {
        public GameConfig config; public PlayerController localPlayer;
        readonly Dictionary<string,RemotePlayer> remotes=new();
        float sendTimer;
        void Start(){ if(config==null) config=Resources.Load<GameConfig>("GameConfig"); }
        void Update(){ if(localPlayer==null||ClientConnection.Instance==null)return; sendTimer-=Time.deltaTime;if(sendTimer<=0){sendTimer=1f/20f; var p=localPlayer.ToInput(); ClientConnection.Instance.Send(JsonUtility.ToJson(p));} }
        public void ApplySnapshot(PlayerSnapshot s){ if(localPlayer!=null && s.playerId==localPlayer.PlayerId){localPlayer.ApplyAuthoritative(s);return;} if(!remotes.TryGetValue(s.playerId,out var r)){var go=GameObject.CreatePrimitive(PrimitiveType.Capsule);go.name="Remote_"+s.playerId;r=go.AddComponent<RemotePlayer>();r.Id=s.playerId;remotes[s.playerId]=r;}r.SetTarget(new Vector3(s.x,s.y,s.z),s.yaw); }
        class RemotePlayer:MonoBehaviour{public string Id;Vector3 target;float yaw;public void SetTarget(Vector3 p,float y){target=p;yaw=y;}void Update(){transform.position=Vector3.Lerp(transform.position,target,Mathf.Clamp01(Time.deltaTime*12));transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.Euler(0,yaw,0),Mathf.Clamp01(Time.deltaTime*12));}}
    }
}
