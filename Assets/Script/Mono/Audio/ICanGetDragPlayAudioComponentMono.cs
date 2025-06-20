using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlentyFishFramework
{
    public class ICanGetDragPlayAudioComponentMono : ICanPlayAudioComponentMono
    {
        public string getDragAudioKey;
        public void PlayGetDragAudio() => PlayAudio(getDragAudioKey);
        public void PlayGetDragAudio(ICanDragComponentMono mono) => PlayAudio(getDragAudioKey);
    }
}