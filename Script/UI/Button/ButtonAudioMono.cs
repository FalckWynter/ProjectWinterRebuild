using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class ButtonAudioMono : ICanPlayAudioComponentMono
    {
        public Button button;
        public string clickAudio, HoverAudio;
        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(PlayClickAudio);
        }
        public void PlayClickAudio()
        {
            PlayAudio(clickAudio);
        }
    }
}