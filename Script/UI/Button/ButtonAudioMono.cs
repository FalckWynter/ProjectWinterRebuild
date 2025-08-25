using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class ButtonAudioMono : ICanPlayAudioComponentMono, IPointerEnterHandler
    {
        public Button button;
        public string clickAudio = "UI_ButtonClick", HoverAudio = "token_hover_OLD";
        public string closeAudioName = "UI_ButtonClose";
        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(PlayClickAudio);
        }
        public void PlayClickAudio()
        {
            PlayAudio(clickAudio);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!button.interactable) return;
            PlayAudio(HoverAudio);
        }

    }
}