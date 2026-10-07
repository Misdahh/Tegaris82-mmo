using System;using System.Collections.Generic;using UnityEngine;
namespace KaisarMMO.Quests
{
 [Serializable] public class QuestObjective{public string id;public string description;public int required;public int progress;}
 [Serializable] public class Quest{public string id;public string title;public List<QuestObjective> objectives=new();public bool Complete=>objectives.TrueForAll(o=>o.progress>=o.required);}
 public class QuestSystem:MonoBehaviour{public List<Quest> active=new();public void AddProgress(string objective,int amount){foreach(var q in active)foreach(var o in q.objectives)if(o.id==objective)o.progress=Mathf.Min(o.required,o.progress+amount);}}
}
