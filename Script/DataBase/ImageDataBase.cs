using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDataBase 
{
    // ͼƬ���ݿ�
    public static Dictionary<string, Sprite> imageDataBase = new Dictionary<string, Sprite>();
    public static Dictionary<string, Sprite> aspectDataBase = new Dictionary<string, Sprite>();
    public static Dictionary<string, Sprite> verbDataBase = new Dictionary<string, Sprite>();
    public static Dictionary<string, Sprite> legacyDataBase = new Dictionary<string, Sprite>();
    public static Sprite TryGetImage(string key)
    {
        //Debug.Log("���Ի�ȡͼƬ" + key + "�Ƿ����" + imageDataBase.ContainsKey(key));
        if (!imageDataBase.ContainsKey(key))
            return imageDataBase["DefaultImage"];
        return imageDataBase[key];
    }
    public static Sprite TryGetVerbImage(string key)
    {
        //Debug.Log("��������" + key);
        if (!verbDataBase.ContainsKey(key))
            return verbDataBase["DefaultImage"];
       // Debug.Log("��������" + verbDataBase[key].name);
        return verbDataBase[key];
    }
    static ImageDataBase()
    {
        Sprite[] obs = Resources.LoadAll<Sprite>("Images/Element");
        //Debug.Log("������Դ"+ obs.Length);
        foreach (var v in obs)
        {
            //Debug.Log("��ǰ������Դ" + v.name);
            imageDataBase.Add(v.name, v);
        }
        obs = Resources.LoadAll<Sprite>("Images/Aspect");
        //Debug.Log("��������ͼƬ��Դ"+ obs.Length);
        foreach (var v in obs)
        {
            //Debug.Log("��ǰ����������Դ" + v.name);
            aspectDataBase.Add(v.name, v);
        }
        obs = Resources.LoadAll<Sprite>("Images/Verb");
        //Debug.Log("��������ͼƬ��Դ" + obs.Length);
        foreach (var v in obs)
        {
            //Debug.Log("��ǰ����������Դ" + v.name);
            verbDataBase.Add(v.name, v);
        }
        obs = Resources.LoadAll<Sprite>("Images/Legacy");
        //Debug.Log("��������ͼƬ��Դ"+ obs.Length);
        foreach (var v in obs)
        {
            //Debug.Log("��ǰ����������Դ" + v.name);
            legacyDataBase.Add(v.name, v);
        }
    }
}
