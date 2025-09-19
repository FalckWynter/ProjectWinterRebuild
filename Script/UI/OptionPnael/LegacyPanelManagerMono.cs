using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

namespace PlentyFishFramework
{
    public class LegacyPanelManagerMono : MonoBehaviour
    {
        public CanvasGroup legacyPanel, background;
        public List<AbstractLegacy> legacyList = new List<AbstractLegacy>();
        public int selectLegacyPlace = -1;
        public List<Image> legacyIcon;
        public List<TextMeshProUGUI> legacyLabel;
        public List<Toggle> toggleList = new List<Toggle>();
        public TextMeshProUGUI legacyDescription,legacyMainLabel,legacyMentionText;
        public CanvasGroupFader legacyFader, backgroundFader;
        public void SelectLegacy(int legacy)
        {
            if (legacy < 0)
                return;

            if (selectLegacyPlace == legacy)
                return;

            if (toggleList[legacy].isOn == false)
                return;


            if (legacyPanel.alpha == 0)
                return;
            selectLegacyPlace = legacy;
            UpdateLegacyDescription();
        }
        public void UpdateLegacyDescription()
        {
            if (selectLegacyPlace >= legacyList.Count)
                return;
            legacyDescription.text = legacyList[selectLegacyPlace].description;
            legacyMainLabel.text = legacyList[selectLegacyPlace].label;
            legacyMentionText.text = legacyList[selectLegacyPlace].legacyUnlockedDescription;
        }

        public void Initialize()
        {

            // 从 legacyDataBase 中获取符合条件的前 3 个
            legacyList = LegacyDataBase.legacyDataBase.Values
                .Where(x => x.isPermitLoad == true)   // 筛选条件
                .OrderBy(x => x.index)                 // 按 index 升序
                .Take(3)                               // 取前 3 个
                .ToList();
            SetLeagcySelectPanel();
            //selectLegacyPlace = 0;
            toggleList[0].isOn = true;
            UpdateLegacyDescription();

        }
        public void SetLeagcySelectPanel()
        {
            for(int i = 0; i < legacyList.Count;i++)
            {
                legacyIcon[i].sprite = legacyList[i].icon;
                legacyLabel[i].text = legacyList[i].label;
            }
        }
        public void ShowPanel()
        {
            legacyFader.Show();
            backgroundFader.Show();
            //legacyPanel.alpha = 1;
            //legacyPanel.blocksRaycasts = true;
            //background.gameObject.SetActive(true);
        }
        public void HidePanel()
        {
            legacyFader.Hide();
            backgroundFader.Hide();
            //legacyPanel.alpha = 0;
            //legacyPanel.blocksRaycasts = false;
            //background.gameObject.SetActive(false);
        }
        private void Start()
        {
            Initialize();
        }
    }
}