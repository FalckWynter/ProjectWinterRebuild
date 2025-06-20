using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextLoadMono : MonoBehaviour
{
    public TextMeshPro text;
    public TextMeshProUGUI textUI;
    // 这个脚本挂在有TextMeshPro的物体上，用于快速替换字体资源和语言
    void Start()
    {
        LanguageDataBase.AddNewLoadMono(this);
        text = GetComponent<TextMeshPro>();
        textUI = GetComponent<TextMeshProUGUI>();
        UpdateFont();
    }
    private void OnDestroy()
    {
        LanguageDataBase.RemoveLoadMono(this);
    }

    public void UpdateFont()
    {
        //Debug.Log("目标字体" + LanguageDataBase.currentFont.name);
        if(text != null)
            text.font = LanguageDataBase.currentFont;
        if (textUI != null)
            textUI.font = LanguageDataBase.currentFont;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
