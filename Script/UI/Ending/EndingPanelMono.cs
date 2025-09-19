using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class EndingPanelMono : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI labelText;
        public TextMeshProUGUI loreText;
        public Image iconImage;

        /// <summary>
        /// 将 AbstractEnding 数据应用到 UI
        /// </summary>
        public void SetEndingData(AbstractEnding endingData)
        {
            if (endingData == null)
            {
                Debug.LogWarning("EndingPanelMono: endingData is null");
                return;
            }

            if (labelText != null)
                labelText.text = endingData.label;

            if (loreText != null)
                loreText.text = endingData.lore;

            if (iconImage != null)
                iconImage.sprite = endingData.icon;
        }

    }
}