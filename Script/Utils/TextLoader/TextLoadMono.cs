using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace PlentyFishFramework
{
    public class TextLoadMono : MonoBehaviour
    {
        // ��������ű�
        public TextMeshProUGUI textUI;
        public string textContentKey = "";
        public bool isLabelUI = true, isPermitUpdateFont = true;
        public string defaultContent = "";
        public bool isPrintUpdateMention = false;
        // ����ű�������TextMeshPro�������ϣ����ڿ����滻������Դ������
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
                Debug.Log("ע���޸��¼�����");
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
            //Debug.Log("Ŀ������" + LanguageDataBase.currentFont.name);
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
            //    Debug.Log("��������Ϊ" + result);
            //}
            if (result == null) SetContent("û���ҵ�����");
            SetContent(result);

        }
        public void SetContent(string text)
        {
            //if (isPrintUpdateMention == true)
            //{
            //    Debug.Log("�����ı�Ϊ" + text);
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