using UnityEngine;

namespace KaisarMMO.Core
{
    [CreateAssetMenu(menuName="Kaisar MMO/Game Config")]
    public class GameConfig : ScriptableObject
    {
        public string serverHost = "127.0.0.1";
        public int serverPort = 28080;
        public float tickRate = 20f;
        public float snapshotRate = 10f;
        public float maxWalkSpeed = 5.5f;
        public float maxSprintSpeed = 8.5f;
        public float attackRange = 2.6f;
    }
}
