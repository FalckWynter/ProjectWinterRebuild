using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class ICanPlayAudioComponentMono : MonoBehaviour, ICanPlayAudio
{
    // ֻ��һ�����ܺ����Ľű������ڲ���ָ�����Ƶ���Ч
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