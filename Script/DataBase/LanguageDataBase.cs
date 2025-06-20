using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageDataBase 
{
    private static TMP_FontAsset chineseFont;
    // 参与了订阅的TextMeshPro
    public static List<TextLoadMono> textList = new List<TextLoadMono>();
    // 当前的字体 根据当前语言取得
    public static TMP_FontAsset currentFont { get { return fontDataBase[currentLanguage]; } }
    public static FontLanguage currentLanguage = FontLanguage.Chinese;
    static LanguageDataBase()
    {
        chineseFont = Resources.Load<TMPro.TMP_FontAsset>("Font/Chinese7000");
        fontDataBase[FontLanguage.Chinese] = chineseFont;
        //if (chineseFont == null) Debug.Log("目标字体为空");
        //else Debug.Log("字体名称" + chineseFont.name);
    }
    // 设置语言时 更新所有文字的字体
    public static void SetLanguage(FontLanguage language)
    {
        currentLanguage = language;
        foreach (var item in textList)
        {
            if (item != null)
                item.UpdateFont();
        }
        for (int i = textList.Count - 1; i >= 0; i--)
        {
            if (textList[i] == null)
                textList.RemoveAt(i);
        }
    }
    public static void AddNewLoadMono(TextLoadMono mono)
    {
        textList.Add(mono);
    }
    public static void RemoveLoadMono(TextLoadMono mono)
    {
        textList.Remove(mono);
    }
    public static Dictionary<FontLanguage, TMP_FontAsset> fontDataBase = new Dictionary<FontLanguage, TMP_FontAsset>()
    {
        {FontLanguage.Chinese,chineseFont}
    };
    public enum FontLanguage
    {
        Chinese
    }
}
