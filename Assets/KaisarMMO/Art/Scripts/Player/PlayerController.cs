using UnityEngine;
using KaisarMMO.Networking;
using KaisarMMO.Combat;

namespace KaisarMMO.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public string PlayerId="local"; public float walkSpeed=5.5f,sprintSpeed=8.5f,gravity=-20f; public Transform visualRoot; public CombatController combat;
        CharacterController cc; Vector3 velocity; uint tick;
        void Awake(){cc=GetComponent<CharacterController>();}
        void Update(){if(!enabled)return; Vector2 input=new(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")); Move(input,Input.GetKey(KeyCode.LeftShift)); if(Input.GetKeyDown(KeyCode.Space))combat?.RequestAttack();}
        public void Move(Vector2 input,bool sprint){Vector3 dir=new Vector3(input.x,0,input.y);if(dir.sqrMagnitude>1)dir.Normalize();if(dir.sqrMagnitude>.001f){float speed=sprint?sprintSpeed:walkSpeed;transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(dir),Time.deltaTime*12);cc.Move(dir*speed*Time.deltaTime);}if(cc.isGrounded&&velocity.y<0)velocity.y=-2;velocity.y+=gravity*Time.deltaTime;cc.Move(velocity*Time.deltaTime);}
        public InputCommand ToInput(){return new InputCommand{tick=++tick,x=Input.GetAxisRaw("Horizontal"),z=Input.GetAxisRaw("Vertical"),sprint=Input.GetKey(KeyCode.LeftShift),attack=false,yaw=transform.eulerAngles.y};}
        public void ApplyAuthoritative(PlayerSnapshot s){transform.position=Vector3.Lerp(transform.position,new Vector3(s.x,s.y,s.z),.5f);transform.rotation=Quaternion.Euler(0,s.yaw,0);}
    }
}
