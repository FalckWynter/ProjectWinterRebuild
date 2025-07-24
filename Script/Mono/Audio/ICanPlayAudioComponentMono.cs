using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class ICanPlayAudioComponentMono : MonoBehaviour, ICanPlayAudio
{
    // 只有一个功能函数的脚本，用于播放指定名称的音效
    public void PlayAudio(string key)
    {
        if (key == null || key == "") return;
        AudioKit.PlaySound(AudioDataBase.TryGetAudio(key));
    }
}
public interface ICanPlayAudio
{
    public void PlayAudio(string key);
}