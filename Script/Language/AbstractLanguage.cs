using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractLanguage : AbstractElement
    {
        // ������Զ�Ӧ��������������
        public string fontStringIndex;
        // ������Զ�Ӧ��������Դ
        public TMP_FontAsset font;
        // ������Զ�Ӧ����������
        public Dictionary<string, string> languageContentDictionary = new Dictionary<string, string>();


        public string TryGetLanguageContent(string key)
        {
            if (languageContentDictionary.ContainsKey(key))
                return languageContentDictionary[key];
            return null;
        }
    }
}