using PlentyFishFramework;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PlentyFishFramework
{
    public class LanguageDataBase : AbstractModel
    {
        public static string TimeUnitText = "秒";
        public static UnityEvent OnLanguageChanged = new UnityEvent();

        public static Dictionary<string, AbstractLanguage> languageDataBase = new Dictionary<string, AbstractLanguage>();
        public static string currentLanguage = defaultLanguage;
        private static string defaultLanguage = "ChineseSimple";
        public static bool inited = false;
        static LanguageDataBase()
        {
            //chineseFont = Resources.Load<TMPro.TMP_FontAsset>("Font/Chinese7000");
            //fontDataBase[FontLanguage.Chinese] = chineseFont;
            //if (chineseFont == null) Debug.Log("目标字体为空");
            //else Debug.Log("字体名称" + chineseFont.name);
        }
        public static AbstractLanguage GetCurrentLangugage()
        {
            if (languageDataBase.ContainsKey(currentLanguage))
                return languageDataBase[currentLanguage];
            else
                return null;
        }
        // 设置语言时 更新所有文字的字体
        public static void SetLanguage(string language)
        {
            currentLanguage = language;
            //Debug.Log("设置语言为" + language);
            OnLanguageChanged.Invoke();
        }
        public static void AddNewLoadMono(TextLoadMono mono)
        {
            //textList.Add(mono);
            OnLanguageChanged.AddListener(mono.OnLanguageExchanged);
            //Debug.Log("添加了目标" + mono.defaultContent);
            if (inited == true)
                mono.UpdateText();
        }
        public static void RemoveLoadMono(TextLoadMono mono)
        {
            OnLanguageChanged.RemoveListener(mono.OnLanguageExchanged);
        }
        public static string GetLanguageLabel(string key)
        {
            //Debug.Log("当前语言为" + currentLanguage);
            AbstractLanguage language = GetCurrentLangugage();
            string content = language.TryGetLanguageContent(key);
            content = ProcessContent(content);
            return content;

        }
        // 大类+子Key 获取变量的值
        public static string GetSettingValue(string category, string key)
        {
            if (GameSettingDataBase.settingDataBase.TryGetValue(category, out var subDict))
            {
                if (subDict.TryGetValue(key, out var config))
                {
                    return config.GetValue().ToString();
                }
            }
            return $"[{category}:{key}]"; // 如果没找到，原样返回占位符
        }
        // 处理带变量的文本
        public static string ProcessContent(string content)
        {
            if (content == null) return null;
            if (!string.IsNullOrEmpty(content) && content.StartsWith("$"))
            {
                // 去掉最前面的 $
                content = content.Substring(1);
            }
            else
                return content;

            // 查找 {SETTING:xxx} 形式
            return Regex.Replace(content, @"\{\s*(\w+)\s*:\s*(\w+)\s*\}", match =>
            {
                string category = match.Groups[1].Value; // 比如 SETTING
                string key = match.Groups[2].Value;      // 比如 kbnormalspeed
                return GetSettingValue(category, key);
            });
        }
        protected override void OnInit()
        {
        }
        //public static Dictionary<FontLanguage, TMP_FontAsset> fontDataBase = new Dictionary<FontLanguage, TMP_FontAsset>()
        //{
        //    {FontLanguage.Chinese,chineseFont}
        //};
        //public enum FontLanguage
        //{
        //    Chinese
        //}
    }
}