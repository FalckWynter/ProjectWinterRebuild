using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class MainTopicOptionPanelManagerMono : MonoBehaviour
    {
        public CanvasGroupFader versionPanel, optionPanel,dlcAndModPanel, languagePanel, currentPanel,creditPanel;
        public CreditPanelMono creditPanelMono,versionPanelMono;

        public CanvasGroupFader background;
        private void Start()
        {
            creditPanelMono.InitCredits();
            versionPanelMono.InitCredits();
        }
        public void ShowVersionPanel()
        {
            ShowPanel(versionPanel);
        }
        public void ShowOptionPanel()
        {
            ShowPanel(optionPanel);
        }
        public void ShowLanguagePanel()
        {
            ShowPanel(languagePanel);
        }
        public void ShowDLCAndModPanel()
        {
            return;
            ShowPanel(dlcAndModPanel);
        }
        public void ShowCreditPanel()
        {
            ShowPanel(creditPanel);
        }
        public void HideCurrentPanel()
        {
            HidePanel(currentPanel);
        }
        public void ShowPanel(CanvasGroupFader group)
        {
            if (currentPanel != null) return;
            currentPanel = group;
            group.Show();
            //group.alpha = 1;
            //group.blocksRaycasts = true;
            background.gameObject.SetActive(true);
        }
        public void HidePanel(CanvasGroupFader group)
        {
            group.Hide();
            //group.alpha = 0;
            //group.blocksRaycasts = false;
            currentPanel = null;
            background.gameObject.SetActive(false);

        }
    }
}