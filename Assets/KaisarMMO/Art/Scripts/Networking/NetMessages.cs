using System;
using UnityEngine;

namespace KaisarMMO.Networking
{
    [Serializable] public struct LoginRequest { public string account; public string token; }
    [Serializable] public struct LoginResponse { public bool ok; public string playerId; public string message; }
    [Serializable] public struct InputCommand { public uint tick; public float x; public float z; public bool sprint; public bool attack; public float yaw; }
    [Serializable] public struct PlayerSnapshot { public string playerId; public float x; public float y; public float z; public float yaw; public int hp; public int level; }
    [Serializable] public struct CombatEvent { public string attackerId; public string targetId; public int damage; public uint sequence; }
}
