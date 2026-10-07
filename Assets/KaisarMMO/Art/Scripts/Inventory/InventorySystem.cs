using System;
using System.Collections.Generic;
using UnityEngine;
namespace KaisarMMO.Inventory
{
    [Serializable] public class ItemStack{public string itemId;public int amount;}
    public class InventorySystem:MonoBehaviour
    { public int capacity=60; public List<ItemStack> items=new(); public bool Add(string id,int amount){var s=items.Find(x=>x.itemId==id);if(s!=null){s.amount+=amount;return true;}if(items.Count>=capacity)return false;items.Add(new ItemStack{itemId=id,amount=amount});return true;} public bool Remove(string id,int amount){var s=items.Find(x=>x.itemId==id);if(s==null||s.amount<amount)return false;s.amount-=amount;if(s.amount==0)items.Remove(s);return true;} }
}
