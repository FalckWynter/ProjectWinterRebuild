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
        public static string TimeUnitText = "��";
        public static UnityEvent OnLanguageChanged = new UnityEvent();

        public static Dictionary<string, AbstractLanguage> languageDataBase = new Dictionary<string, AbstractLanguage>();
        public static string currentLanguage = defaultLanguage;
        private static string defaultLanguage = "ChineseSimple";
        public static bool inited = false;
        static LanguageDataBase()
        {
            //chineseFont = Resources.Load<TMPro.TMP_FontAsset>("Font/Chinese7000");
            //fontDataBase[FontLanguage.Chinese] = chineseFont;
            //if (chineseFont == null) Debug.Log("Ŀ������Ϊ��");
            //else Debug.Log("��������" + chineseFont.name);
        }
        public static AbstractLanguage GetCurrentLangugage()
        {
            if (languageDataBase.ContainsKey(currentLanguage))
                return languageDataBase[currentLanguage];
            else
                return null;
        }
        // ��������ʱ �����������ֵ�����
        public static void SetLanguage(string language)
        {
            currentLanguage = language;
            //Debug.Log("��������Ϊ" + language);
            OnLanguageChanged.Invoke();
        }
        public static void AddNewLoadMono(TextLoadMono mono)
        {
            //textList.Add(mono);
            OnLanguageChanged.AddListener(mono.OnLanguageExchanged);
            //Debug.Log("�����Ŀ��" + mono.defaultContent);
            if (inited == true)
                mono.UpdateText();
        }
        public static void RemoveLoadMono(TextLoadMono mono)
        {
            OnLanguageChanged.RemoveListener(mono.OnLanguageExchanged);
        }
        public static string GetLanguageLabel(string key)
        {
            //Debug.Log("��ǰ����Ϊ" + currentLanguage);
            AbstractLanguage language = GetCurrentLangugage();
            string content = language.TryGetLanguageContent(key);
            content = ProcessContent(content);
            return content;

        }
        // ����+��Key ��ȡ������ֵ
        public static string GetSettingValue(string category, string key)
        {
            if (GameSettingDataBase.settingDataBase.TryGetValue(category, out var subDict))
            {
                if (subDict.TryGetValue(key, out var config))
                {
                    return config.GetValue().ToString();
                }
            }
            return $"[{category}:{key}]"; // ���û�ҵ���ԭ������ռλ��
        }
        // ������������ı�
        public static string ProcessContent(string content)
        {
            if (content == null) return null;
            if (!string.IsNullOrEmpty(content) && content.StartsWith("$"))
            {
                // ȥ����ǰ��� $
                content = content.Substring(1);
            }
            else
                return content;

            // ���� {SETTING:xxx} ��ʽ
            return Regex.Replace(content, @"\{\s*(\w+)\s*:\s*(\w+)\s*\}", match =>
            {
                string category = match.Groups[1].Value; // ���� SETTING
                string key = match.Groups[2].Value;      // ���� kbnormalspeed
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