using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class OptionPanelManagerMono : IShowablePanelMono
    {
        public List<OptionPanelMono> optionPanelList = new List<OptionPanelMono>();
        public List<OptionTabButtonMono> optionButtonList = new List<OptionTabButtonMono>();

        public GameObject optionTabParentGameObject;
        public GameObject optionPanelParentGameObject;
        public CanvasGroup canvasGroup;
        public CanvasGroupFader optionPanelFader, backgroundFader;
        public void ShowWithDefaultOption()
        {
            if (canvasGroup.alpha == 0)
            {
                if(optionPanelFader != null)
                {
                    optionPanelFader.Show();
                }
                else
                {
                    Show();
                }
                if(backgroundFader != null)
                {
                    backgroundFader.Show();

                }
                else
                {
                }
                if (optionButtonList.Count > 0)
                    ActivatePanel(optionButtonList[0].ElementID);
            }
            else
            {
                if (optionPanelFader != null)
                {
                    optionPanelFader.Show();
                }
                else
                {
                    Show();
                }
                if (backgroundFader != null)
                {
                    backgroundFader.Show();

                }
                else
                {
                }
            }
        }
        public override void Show()
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;

        }
        public void DefaultHide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
        public override void Hide()
        {

            if (optionPanelFader != null)
            {
                optionPanelFader.Hide();
            }
            else
            {
                DefaultHide();
            }
            if (backgroundFader != null)
            {
                backgroundFader.Hide();

            }
            else
            {
            }


        }
        public void ActivatePanel(string id)
        {
            foreach(OptionPanelMono item in optionPanelList)
            {
                if(item.ElementID == id)
                {
                    item.Show();
                }
                else
                {
                    item.Hide();
                }
            }
        }
        public void RegisterOptionElement(OptionElement element)
        {
            element.panelManager = this;
            if (element is OptionPanelMono mono)
                optionPanelList.Add(mono);
            else if (element is OptionTabButtonMono buttonMono)
                optionButtonList.Add(buttonMono);

        }
        public void AddParentElement()
        {
            optionPanelList.Clear();
            optionButtonList.Clear();

            // 检查 panel parent 的一级子物体
            if (optionPanelParentGameObject != null)
            {
                foreach (Transform child in optionPanelParentGameObject.transform)
                {
                    OptionPanelMono panel = child.GetComponent<OptionPanelMono>();
                    if (panel != null)
                    {
                        //optionPanelList.Add(panel);
                        RegisterOptionElement(panel);
                    }
                }
            }

            // 检查 tab parent 的一级子物体
            if (optionTabParentGameObject != null)
            {
                foreach (Transform child in optionTabParentGameObject.transform)
                {
                    OptionTabButtonMono button = child.GetComponent<OptionTabButtonMono>();
                    if (button != null)
                    {
                        //optionButtonList.Add(button);
                        RegisterOptionElement(button);
                    }
                }
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            AddParentElement();
            canvasGroup = GetComponent<CanvasGroup>();

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
    public interface OptionElement
    {
        public string ElementID { set; get; }
        public OptionPanelManagerMono panelManager { set; get; }
    }
}