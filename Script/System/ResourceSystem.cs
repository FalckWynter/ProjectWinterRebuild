using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace PlentyFishFramework
{
    public class ResourceSystem : AbstractSystem
    {
        ResourceModel resourceModel;
        protected override void OnInit()
        {
            resourceModel = this.GetModel<ResourceModel>();
            LoadUserConfig();
            LoadPathConfig();
            LoadLanguageDataBase();
        }
        public void LoadUserConfig()
        {
            // 1. ��ȡ�û�����
            string userConfigPath = Path.Combine(Application.dataPath, "StreamingAssets/Json/Config/UserConfig.json");
            string jsonText = File.ReadAllText(userConfigPath);
            // �� JSONObject ����
            JSONObject root = new JSONObject(jsonText);

            UserConfig userConfig = new UserConfig();

            // ��ֵ������
            userConfig.language = root["language"].StringValue;

            // ���ำֵ��ģ����
            resourceModel.userConfig = userConfig;

        }
        public void LoadPathConfig()
        {
            // 1. ��ȡ�û�����
            string filePathConfigPath = Path.Combine(Application.dataPath, "StreamingAssets/Json/Config/FilePaths.json");
            string jsonText = File.ReadAllText(filePathConfigPath);

            // �� JSONObject ����
            JSONObject root = new JSONObject(jsonText);

            FilePathConfig filePathConfig = new FilePathConfig();

            // ��ֵ������
            filePathConfig.localizationRoot = root["LocalizationRoot"].StringValue;
            filePathConfig.cultureFileName = root["CultureFileName"].StringValue;


            // ���ำֵ��ģ����
            resourceModel.filePathConfig = filePathConfig;
        }
        public void LoadLanguageDataBase()
        {
            Debug.Log("���Կ��������");
            // 3. ƴ�������ļ�·��
            //string culturePath = Path.Combine(Application.dataPath, $"{resourceModel.filePathConfig.localizationRoot}/{resourceModel.userConfig.language}/culture.json");
            // 3. ��ȡ��·��
            string localizationRootPath = Path.Combine(Application.dataPath, resourceModel.filePathConfig.localizationRoot);

            // 4. ������������Ŀ¼
            string[] languageDirs = Directory.GetDirectories(localizationRootPath);
            // ����ԭ�е����Դʵ�
            LanguageDataBase.languageDataBase.Clear();
            LanguageDataBase.currentLanguage = resourceModel.userConfig.language;

            foreach (string langDir in languageDirs)
            {
                string langName = Path.GetFileName(langDir); // �������ƣ����� ChineseSimple / English
                string culturePath = Path.Combine(langDir, "culture.json");

                if (!File.Exists(culturePath))
                {
                    Debug.LogWarning($"���� {langName} ȱ�� culture.json �ļ���������");
                    continue;
                }

                // 4. ��ȡ���� JSON
                string cultureJson = File.ReadAllText(culturePath);
                JSONObject cultureObj = new JSONObject(cultureJson);
                cultureObj = cultureObj["cultures"][0];
                JSONObject labelsObj = cultureObj["uilabels"];
                //Debug.Log("��������" + resourceModel.userConfig.language);
                //Debug.Log("��������" + langName);
                AbstractLanguage language = new AbstractLanguage();
                language.languageContentDictionary.Clear();
                language.label = cultureObj["endonym"].StringValue;
                //foreach (var key in labelsObj.Keys)
                //{
                //    string value = labelsObj[key].StringValue;
                //    language.languageContentDictionary[key] = value;
                //}

                for (int i = 0; i < labelsObj.List.Count; i++)
                {
                    string key = labelsObj.Keys[i];
                    string value = labelsObj.List[i].StringValue;
                    language.languageContentDictionary[key] = value;

                }

                LanguageDataBase.languageDataBase.Add(langName, language);
            }
            LanguageDataBase.inited = true;
            LanguageDataBase.SetLanguage(LanguageDataBase.currentLanguage);
            LanguageDataBase.OnLanguageChanged.Invoke();

        }
        public void LoadJson()
        {

        }
        public static JSONObject ReadJsonByPath(string path)
        {
            string jsonText = File.ReadAllText(path);
            if (jsonText == null)
            {
                Debug.LogWarning(path + "û�ж�ȡ����Ӧ��Json�ļ�");
                return null;
            }
            // �� JSONObject ����
            JSONObject root = new JSONObject(jsonText);
            return root;
        }
    }
}