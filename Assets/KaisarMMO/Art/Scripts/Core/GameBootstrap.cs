using UnityEngine;using KaisarMMO.Player;using KaisarMMO.Combat;using KaisarMMO.Networking;
namespace KaisarMMO.Core
{
 public class GameBootstrap:MonoBehaviour
 {
  public GameConfig config;
  void Start(){Application.targetFrameRate=60;var net=new GameObject("Network");net.AddComponent<ClientConnection>();var player=new GameObject("Player");player.AddComponent<CharacterController>();var pc=player.AddComponent<PlayerController>();var combat=player.AddComponent<CombatController>();pc.combat=combat;var world=gameObject.AddComponent<WorldClient>();world.localPlayer=pc;world.config=config;CreateCamera(player.transform);}
  void CreateCamera(Transform target){var c=new GameObject("ThirdPersonCamera").AddComponent<Camera>();c.tag="MainCamera";c.transform.position=target.position+new Vector3(0,4,-7);c.transform.LookAt(target.position+Vector3.up*1.5f);c.gameObject.AddComponent<ThirdPersonFollow>().target=target;}
 }
 public class ThirdPersonFollow:MonoBehaviour{public Transform target;public Vector3 offset=new(0,4,-7);void LateUpdate(){if(!target)return;transform.position=Vector3.Lerp(transform.position,target.position+offset,Time.deltaTime*8);transform.LookAt(target.position+Vector3.up*1.5f);}}
}
