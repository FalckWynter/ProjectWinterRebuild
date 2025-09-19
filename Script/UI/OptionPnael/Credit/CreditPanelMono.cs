using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace PlentyFishFramework
{
    public class CreditPanelMono : MonoBehaviour
    {
        public GameObject creditElementParent;
        public TextMeshProUGUI title, contentText;
        public string path = "/Json/Credits/Credits.json";
        public string jsonkey = "developers";
        public void InitCredits()
        {
            string realPath = Application.streamingAssetsPath + path;
            Debug.Log("读取资源路径" + realPath);
            var root= ResourceSystem.ReadJsonByPath(realPath);
            JSONObject developers = root[jsonkey];
            List<CreditElement> list = new List<CreditElement>();
            if (developers == null)
            {
                Debug.LogWarning("developers 字段不存在或不是数组");
                return;
            }

            // 遍历数组
            foreach (JSONObject dev in developers.List)
            {
                CreditElement element = new CreditElement
                {
                    label = dev["label"].StringValue,
                    description = dev["description"].StringValue,
                    iconName = dev["iconName"].StringValue
                };
                list.Add(element);
            }
            ClearCreditElements();

            GameObject prefab = PrefabDataBase.TryGetPrefab("CreditCardPrefab");
            foreach(var item in list)
            {
                GameObject creditObject = GameObject.Instantiate(prefab, creditElementParent.transform);
                CreditCardMono creditMono = creditObject.GetComponent<CreditCardMono>();
                creditMono.SetCard(item.label, item.iconName);
                creditMono.button.onClick.AddListener(() => SetCreditDetail(item));
            }

        }
        public void SetCreditDetail(CreditElement element)
        {
            title.text = element.label;
            contentText.text = element.description;
        }
        public void ClearCreditElements()
        {
            for (int i = creditElementParent.transform.childCount - 1; i >= 0; i--)
            {
                GameObject child = creditElementParent.transform.GetChild(i).gameObject;
                GameObject.Destroy(child);
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
        public class CreditElement
        {
            public string label, description, iconName;
        }
         
    }
}
