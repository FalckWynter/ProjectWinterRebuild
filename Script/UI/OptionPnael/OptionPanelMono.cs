using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class OptionPanelMono : MonoBehaviour, OptionElement
    {
        [SerializeField] private string elementID;
        public string ElementID
        {
            get { return elementID; }
            set { elementID = value; }
        }
        public OptionPanelManagerMono panelManager { get ; set; }
        public List<AbstractSettingControlMono> controlList = new List<AbstractSettingControlMono>();
        public void Show()
        {
            gameObject.SetActive(true);
            foreach(var control in controlList)
            {
                control.Activate();
            }
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void GetControlList()
        {
            controlList.Clear();
            // 获取 A 自身以及所有子物体上的 AbstractSettingControlMono 组件
            var allControls = GetComponentsInChildren<AbstractSettingControlMono>(includeInactive: true);

            foreach (var control in allControls)
            {
                controlList.Add(control);
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}