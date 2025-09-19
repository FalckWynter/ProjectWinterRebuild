using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class OptionTabButtonMono : MonoBehaviour, OptionElement
    {
        [SerializeField] private string elementID;
        public string ElementID
        {
            get { return elementID; }
            set { elementID = value; }
        }
        public OptionPanelManagerMono panelManager { get; set ; }
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => panelManager.ActivatePanel(ElementID));
        }

        public void Activate()
        {

        }
        public void DeActivate()
        {

        }

    }
}