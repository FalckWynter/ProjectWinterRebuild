using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    // 性相元素脚本
    public class ElementFrameMono : MonoBehaviour
    {
        // 对应的组件
        public Image artwork;
        public TextMeshProUGUI countText;
        public RectTransform rect;
        public Button button;
        public float withNumberAddWidth = 22.5f, noNumberWidth = 40;
        public AbstractAspect currentAspect;
        public void SetDetail(AbstractAspect aspect,int value)
        {
            currentAspect = aspect;
            SetDetail(aspect.icon, value);
        }
        // 设置性相图片和数量
        private void SetDetail(string key, int value)
        {
            Sprite icon = ImageDataBase.TryGetVerbImage(key);
            if (icon != null)
            SetDetail(icon, value);
        }
        private void SetDetail(Sprite icon, int count)
        {
            artwork.sprite = icon;
            countText.text = (count).ToString();
            if(count <= 1)
            {
                countText.gameObject.SetActive(false);
                rect.sizeDelta = new Vector2(noNumberWidth, rect.sizeDelta.y);
            }
            else if(count <= 9)
            {
                countText.gameObject.SetActive(true);
                rect.sizeDelta = new Vector2(noNumberWidth + withNumberAddWidth, rect.sizeDelta.y);

            }
            else
            {
                countText.gameObject.SetActive(true);
                rect.sizeDelta = new Vector2(noNumberWidth + 2 * withNumberAddWidth, rect.sizeDelta.y);
            }

        }
        // Start is called before the first frame update
        void Start()
        {
            rect = GetComponent<RectTransform>();
            button = GetComponent<Button>();
            button.onClick.AddListener(OpenShowAspectPanel);
        }
        public void OpenShowAspectPanel()
        {
            UtilSystem.ShowAspect(currentAspect);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}