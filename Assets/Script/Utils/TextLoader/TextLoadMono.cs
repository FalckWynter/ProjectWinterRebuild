using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextLoadMono : MonoBehaviour
{
    public TextMeshPro text;
    public TextMeshProUGUI textUI;
    // ����ű�������TextMeshPro�������ϣ����ڿ����滻������Դ������
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
        //Debug.Log("Ŀ������" + LanguageDataBase.currentFont.name);
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
