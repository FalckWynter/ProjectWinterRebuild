using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDataBase 
{
    public static Dictionary<string, AudioClip> soundDataBase = new Dictionary<string, AudioClip>();

    public static AudioClip TryGetAudio(string key)
    {
        if (!soundDataBase.ContainsKey(key))
            return null;
        return soundDataBase[key];
    }

    static AudioDataBase()
    {
        AudioClip[] obs = Resources.LoadAll<AudioClip>("Audios/Sounds");
        //Debug.Log("载入资源"+ obs.Length);
        foreach (var v in obs)
        {
            //Debug.Log("当前载入资源" + v.name);
            soundDataBase.Add(v.name, v);
        }

    }
}
