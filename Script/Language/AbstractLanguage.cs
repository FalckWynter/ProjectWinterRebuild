using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractLanguage : AbstractElement
    {
        // 这个语言对应的文字字体名称
        public string fontStringIndex;
        // 这个语言对应的字体资源
        public TMP_FontAsset font;
        // 这个语言对应的文字内容
        public Dictionary<string, string> languageContentDictionary = new Dictionary<string, string>();


        public string TryGetLanguageContent(string key)
        {
            if (languageContentDictionary.ContainsKey(key))
                return languageContentDictionary[key];
            return null;
        }
    }
}