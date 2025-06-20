using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace PlentyFishFramework
{
    public class ICanDragPlayAudioComponentMono : ICanPlayAudioComponentMono /*, IBeginDragHandler,IEndDragHandler*/
    {
        public string startDragAudioKey;
        public string endDragAudioKey;
        // public string getStackAudioKey;

        public void PlayStartDragAudio() => PlayAudio(startDragAudioKey);
        public void PlayEndDragAudio() => PlayAudio(endDragAudioKey);
        // public void PlayGetStackAudio() => PlayAudio(getStackAudioKey);

        // 给事件用的订阅
        public void PlayStartDragAudio(ICanDragComponentMono mono) => PlayAudio(startDragAudioKey);
        public void PlayEndDragAudio(ICanDragComponentMono mono) => PlayAudio(endDragAudioKey);
        // public void PlayGetStackAudio(ICanDragComponentMono mono) => PlayAudio(getStackAudioKey);
        // 触发器下移
        //public void OnBeginDrag(PointerEventData eventData)
        //{
        //    Debug.Log("播放开始音效");
        //    PlayStartDragAudio();
        //}

        //public void OnEndDrag(PointerEventData eventData)
        //{
        //    Debug.Log("播放结束音效");
        //    PlayEndDragAudio();
        //}
    }
}