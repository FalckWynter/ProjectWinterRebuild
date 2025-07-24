using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace PlentyFishFramework
{
    public class CardCounterMono : MonoBehaviour
    {
        // 卡牌计数器脚本 根据传入的值决定是否显示计数小图标
        public GameObject parent;
        public TextMeshProUGUI countText;
        private void Start()
        {
            parent = gameObject;
            countText = transform.Find("StackCount").GetComponent<TextMeshProUGUI>();
        }
        public void SetCount(int value)
        {
            countText.text = value.ToString();
            if (parent == null) return;
            if (value > 1)
            {
                parent.SetActive(true);
            }
            else
            {
                parent.SetActive(false);
            }
        }
    }
}