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
    public static Dictionary<string, Sprite> endingDataBase = new Dictionary<string, Sprite>();

    public static Sprite TryGetImage(string key)
    {
        //Debug.Log("尝试获取图片" + key + "是否存在" + imageDataBase.ContainsKey(key));
        if (!imageDataBase.ContainsKey(key))
            return imageDataBase["DefaultImage"];
        return imageDataBase[key];
    }
    public static Sprite TryGetAspectImage(string key)
    {
        //Debug.Log("尝试获取图片" + key + "是否存在" + imageDataBase.ContainsKey(key));
        if (!aspectDataBase.ContainsKey(key))
            return aspectDataBase["DefaultImage"];
        return aspectDataBase[key];
    }
    public static Sprite TryGetVerbImage(string key)
    {
        // Debug.Log("载入名称" + key);
        if (!verbDataBase.ContainsKey(key))
            return verbDataBase["DefaultImage"];
       // Debug.Log("返还名称" + verbDataBase[key].name);
        return verbDataBase[key];
    }
    public static Sprite TryGetLegacyImage(string key)
    {
        // Debug.Log("载入名称" + key);
        if (!legacyDataBase.ContainsKey(key))
            return legacyDataBase["DefaultImage"];
        // Debug.Log("返还名称" + legacyDataBase[key].name);
        return legacyDataBase[key];
    }
    public static Sprite TryGetEndingImage(string key)
    {
        // Debug.Log("载入名称" + key);
        if (!endingDataBase.ContainsKey(key))
            return endingDataBase["DefaultImage"];
        // Debug.Log("返还名称" + legacyDataBase[key].name);
        return endingDataBase[key];
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
        obs = Resources.LoadAll<Sprite>("Images/Ending");
        //Debug.Log("载入性相图片资源"+ obs.Length);
        foreach (var v in obs)
        {
            //Debug.Log("当前载入性相资源" + v.name);
            endingDataBase.Add(v.name, v);
        }
    }
}
