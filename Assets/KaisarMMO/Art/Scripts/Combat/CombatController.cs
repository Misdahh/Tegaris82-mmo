using UnityEngine;
using KaisarMMO.Networking;

namespace KaisarMMO.Combat
{
    public class CombatController:MonoBehaviour
    {
        public int baseDamage=25; public float attackCooldown=.55f,range=2.6f; float cd;
        void Update(){if(cd>0)cd-=Time.deltaTime;}
        public void RequestAttack(){if(cd>0)return;cd=attackCooldown;var cmd=new CombatEvent{attackerId="local",targetId="",damage=baseDamage,sequence=(uint)Time.frameCount};ClientConnection.Instance?.Send(JsonUtility.ToJson(cmd));}
    }
}
