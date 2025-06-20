using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDataBase
{
    public static Dictionary<string, GameObject> prefabDataBase = new Dictionary<string, GameObject>();
    public static GameObject TryGetPrefab(string key)
    {
        if (!prefabDataBase.ContainsKey(key))
            return prefabDataBase["DefaultPrefab"];
        return prefabDataBase[key];
    }

    static PrefabDataBase()
    {
        GameObject[] obs = Resources.LoadAll<GameObject>("Prefabs/LoadPrefab");
        //Debug.Log("载入资源"+ obs.Length);
        foreach (var v in obs)
        {
            //Debug.Log("当前载入资源" + v.name);
            prefabDataBase.Add(v.name, v);
        }


    }
}
