using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class LanguagePanelMono : IShowablePanelMono
    {
        public GameObject choiceParent;
        public CanvasGroup group;
        bool inited = false;
        private void Start()
        {
            if (inited == false)
                InitLanguagePanel();
            Debug.Log("初始化语言面板");
        }
        public override void Show()
        {
            group.alpha = 1;
            group.blocksRaycasts = true;
        }
        public override void Hide()
        {
            group.alpha = 0;
            group.blocksRaycasts = false;
        }
        public void SetLanguage(string language)
        {
            LanguageDataBase.SetLanguage(language);
        }
        public void InitLanguagePanel()
        {
            inited = true;
            var languageDictionary = LanguageDataBase.languageDataBase;
            GameObject prefab = PrefabDataBase.TryGetPrefab("LanguageChoice");
            foreach(var pair in languageDictionary)
            {
                Debug.Log("当前语言" + pair.Key);
                GameObject choiceObject = GameObject.Instantiate(prefab,choiceParent.transform);
                Button choiceButton = choiceObject.GetComponent<Button>();
                choiceObject.GetComponent<ButtonComponentRegister>().label.text = pair.Value.label;
                choiceObject.GetComponent<ButtonComponentRegister>().button.onClick.AddListener(() => SetLanguage(pair.Key));
            }
        }
    }
}