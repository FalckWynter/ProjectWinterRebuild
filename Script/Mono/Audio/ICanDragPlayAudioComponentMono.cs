using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace PlentyFishFramework
{
    public class ICanDragPlayAudioComponentMono : ICanPlayAudioComponentMono /*, IBeginDragHandler,IEndDragHandler*/
    {
        // ����ק�������ڲ�����Ч�Ľű�
        public string startDragAudioKey;
        public string endDragAudioKey;
        public string getStackAudioKey;
        public string getClickAudioKey;
        public string gameobjectDisableAudioKey;

        public void PlayStartDragAudio() => PlayAudio(startDragAudioKey);
        public void PlayEndDragAudio() => PlayAudio(endDragAudioKey);
        public void PlayGetStackAudio() => PlayAudio(getStackAudioKey);
        public void PlayGetClickAudio() => PlayAudio(getClickAudioKey);
        public void PlayGameobjectDisableAudio() => PlayAudio(gameobjectDisableAudioKey);



        // ���¼��õĶ���
        public void PlayStartDragAudio(ICanDragComponentMono mono) => PlayAudio(startDragAudioKey);
        public void PlayEndDragAudio(ICanDragComponentMono mono) => PlayAudio(endDragAudioKey);
        public void PlayGetStackAudio(ICanDragComponentMono mono) => PlayAudio(getStackAudioKey);
        public void PlayGetClickAudio(ICanDragComponentMono mono) => PlayAudio(getClickAudioKey);
        public void PlayGameobjectDisableAudio(ICanDragComponentMono mono) => PlayAudio(gameobjectDisableAudioKey);


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