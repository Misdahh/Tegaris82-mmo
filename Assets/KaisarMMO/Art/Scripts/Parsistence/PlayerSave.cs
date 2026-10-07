using System;using UnityEngine;
namespace KaisarMMO.Persistence
{
 [Serializable] public class PlayerSave{public string playerId;public int level=1;public int hp=100;public float x,y,z;public string[] inventory;public string[] equipment;}
 public interface IPlayerRepository{bool Load(string id,out PlayerSave save);bool Save(PlayerSave save);}
 public class LocalJsonRepository:IPlayerRepository{readonly string root;public LocalJsonRepository(){root=Application.persistentDataPath;}public bool Load(string id,out PlayerSave save){save=null;string p=System.IO.Path.Combine(root,id+".json");if(!System.IO.File.Exists(p))return false;save=JsonUtility.FromJson<PlayerSave>(System.IO.File.ReadAllText(p));return true;}public bool Save(PlayerSave save){System.IO.File.WriteAllText(System.IO.Path.Combine(root,save.playerId+".json"),JsonUtility.ToJson(save,true));return true;}}
}
