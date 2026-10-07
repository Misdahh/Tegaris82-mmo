using System.Collections.Generic;
using UnityEngine;
namespace KaisarMMO.Inventory
{
    public enum EquipmentSlot{Weapon,Helmet,Chest,Gloves,Legs,Boots,Accessory}
    public class EquipmentSystem:MonoBehaviour
    { readonly Dictionary<EquipmentSlot,string> equipped=new(); public void Equip(EquipmentSlot slot,string itemId){equipped[slot]=itemId;} public string Get(EquipmentSlot slot)=>equipped.TryGetValue(slot,out var v)?v:null; }
}
