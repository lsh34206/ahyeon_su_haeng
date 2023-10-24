using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class GameManager
{
   private GameObject _player;
   private HashSet<GameObject> _monsters = new HashSet<GameObject>();

   public GameObject Spwan(Define.WorldObject type, string path, Transform parent = null)
   {
      GameObject go = Managers.Resource.Instantiate(path, parent);
      switch (type)
      {
         case Define.WorldObject.Monster:
            _monsters.Add(go);
            break;
         case Define.WorldObject.Player:
            _player = go;
            break;
      }

      return go;
   }

   public Define.WorldObject GetWorldObjectType(GameObject go)
   {
      BaseController bc = go.GetComponent<BaseController>();
      if (bc == null)
         return Define.WorldObject.Unknown;
      return bc.WorldObjectType;
   }

   public void Despwan(GameObject go)
   {
      Define.WorldObject type = GetWorldObjectType(go);
      switch (type)
      {
         case Define.WorldObject.Monster:
         {
            if (_monsters.Contains(go))
               _monsters.Remove(go);
         }
            break;
         case Define.WorldObject.Player:
            if (_player == go)
               _player = null;
            break;
      }
     Managers.Resource.Destroy(go);
   }
}