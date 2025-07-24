using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{ 
    public class RecipeThresholdsDominionMono : MonoBehaviour
    {
        // �¼�ִ������Ϣ����� ����ʣ��ʱ�����Ϣ
        public TextMeshProUGUI timeText;
        public Image timeCircleImage;
        public void SetTimer(AbstractRecipe recipe)
        {
            SetTimer(recipe.warpup, recipe.maxWarpup);
        }
        public void SetTimer(float warpup, float maxWarpup)
        {
            timeText.text = warpup.ToString("F1") + LanguageDataBase.TimeUnitText;
            timeCircleImage.fillAmount = 1 - (warpup / maxWarpup);
        }
    }
}