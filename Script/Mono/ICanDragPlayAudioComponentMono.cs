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

        // ���¼��õĶ���
        public void PlayStartDragAudio(ICanDragComponentMono mono) => PlayAudio(startDragAudioKey);
        public void PlayEndDragAudio(ICanDragComponentMono mono) => PlayAudio(endDragAudioKey);
        // public void PlayGetStackAudio(ICanDragComponentMono mono) => PlayAudio(getStackAudioKey);
        // ����������
        //public void OnBeginDrag(PointerEventData eventData)
        //{
        //    Debug.Log("���ſ�ʼ��Ч");
        //    PlayStartDragAudio();
        //}

        //public void OnEndDrag(PointerEventData eventData)
        //{
        //    Debug.Log("���Ž�����Ч");
        //    PlayEndDragAudio();
        //}
    }
}