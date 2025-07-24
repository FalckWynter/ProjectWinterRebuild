using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDataBase 
{
    // 图片数据库
    public static Dictionary<string, Sprite> imageDataBase = new Dictionary<string, Sprite>();
    public static Dictionary<string, Sprite> aspectDataBase = new Dictionary<string, Sprite>();
    public static Dictionary<string, Sprite> verbDataBase = new Dictionary<string, Sprite>();
    public static Dictionary<string, Sprite> legacyDataBase = new Dictionary<string, Sprite>();
    public static Sprite TryGetImage(string key)
    {
        //Debug.Log("尝试获取图片" + key + "是否存在" + imageDataBase.ContainsKey(key));
        if (!imageDataBase.ContainsKey(key))
            return imageDataBase["DefaultImage"];
        return imageDataBase[key];
    }
    public static Sprite TryGetVerbImage(string key)
    {
        //Debug.Log("载入名称" + key);
        if (!verbDataBase.ContainsKey(key))
            return verbDataBase["DefaultImage"];
       // Debug.Log("返还名称" + verbDataBase[key].name);
        return verbDataBase[key];
    }
    static ImageDataBase()
    {
        Sprite[] obs = Resources.LoadAll<Sprite>("Images/Element");
        //Debug.Log("载入资源"+ obs.Length);
        foreach (var v in obs)
        {
            //Debug.Log("当前载入资源" + v.name);
            imageDataBase.Add(v.name, v);
        }
        obs = Resources.LoadAll<Sprite>("Images/Aspect");
        //Debug.Log("载入性相图片资源"+ obs.Length);
        foreach (var v in obs)
        {
            //Debug.Log("当前载入性相资源" + v.name);
            aspectDataBase.Add(v.name, v);
        }
        obs = Resources.LoadAll<Sprite>("Images/Verb");
        //Debug.Log("载入性相图片资源" + obs.Length);
        foreach (var v in obs)
        {
            //Debug.Log("当前载入性相资源" + v.name);
            verbDataBase.Add(v.name, v);
        }
        obs = Resources.LoadAll<Sprite>("Images/Legacy");
        //Debug.Log("载入性相图片资源"+ obs.Length);
        foreach (var v in obs)
        {
            //Debug.Log("当前载入性相资源" + v.name);
            legacyDataBase.Add(v.name, v);
        }
    }
}
