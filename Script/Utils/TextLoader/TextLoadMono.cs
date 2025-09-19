using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace PlentyFishFramework
{
    public class TextLoadMono : MonoBehaviour
    {
        // 载入字体脚本
        public TextMeshProUGUI textUI;
        public string textContentKey = "";
        public bool isLabelUI = true, isPermitUpdateFont = true;
        public string defaultContent = "";
        public bool isPrintUpdateMention = false;
        // 这个脚本挂在有TextMeshPro的物体上，用于快速替换字体资源和语言
        void Start()
        {
            gameObject.name = textContentKey;
            textUI = GetComponent<TextMeshProUGUI>();
            if (textUI != null)
            {
                defaultContent = textUI.text;
            }
            if (isPrintUpdateMention == true)
            {
                Debug.Log("注册修改事件内容");
            }
            LanguageDataBase.AddNewLoadMono(this);

            //UpdateFont();
            //UpdateText();
        }
        private void OnDestroy()
        {
            LanguageDataBase.RemoveLoadMono(this);
        }
        public void OnLanguageExchanged()
        {
            //UpdateFont();
            UpdateText();
        }
        public void UpdateFont()
        {
            //Debug.Log("目标字体" + LanguageDataBase.currentFont.name);
            //if(text != null)
            //    text.font = LanguageDataBase.currentFont;
            //if (textUI != null)
            textUI.font = LanguageDataBase.GetCurrentLangugage().font;

        }
        public void UpdateText()
        {

            string result = LanguageDataBase.GetLanguageLabel(textContentKey);
            //if (isPrintUpdateMention == true)
            //{
            //    Debug.Log("更新内容为" + result);
            //}
            if (result == null) SetContent("没有找到文字");
            SetContent(result);

        }
        public void SetContent(string text)
        {
            //if (isPrintUpdateMention == true)
            //{
            //    Debug.Log("设置文本为" + text);
            //}
            if (textUI != null)
                textUI.text = text;
        }
        public void SetContentKey(string key)
        {
            textContentKey = key;
            UpdateText();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
    public enum TextLoadType
    {
        UI,Element,Label
    }
}