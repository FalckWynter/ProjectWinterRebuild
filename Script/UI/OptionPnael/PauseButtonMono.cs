using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class PauseButtonMono : MonoBehaviour
    {
        [SerializeField] Image buttonImage;
        [SerializeField] TextMeshProUGUI buttonText;
        public TextLoadMono pauseTextLoadMono;
        // Start is called before the first frame update
        public void SetPausedText(bool isPaused)
        {
            if (isPaused)
            {
                //ButtonText.text = "Unpause <size=60%><alpha=#99>[SPACE]";
                //buttonText.text = "¼ÌÐøÓÎÏ·";
                pauseTextLoadMono.SetContentKey("UI_UNPAUSE");
            }
            else
            {
                //ButtonText.text = "Pause <size=60%><alpha=#99>[SPACE]";
                //buttonText.text = "ÔÝÍ£";
                pauseTextLoadMono.SetContentKey("UI_PAUSE");

            }
        }

        public void SetColor(Color color)
        {
            buttonImage.color = color;
        }
    }
}