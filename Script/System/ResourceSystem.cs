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
            // 1. 读取用户配置
            string userConfigPath = Path.Combine(Application.dataPath, "StreamingAssets/Json/Config/UserConfig.json");
            string jsonText = File.ReadAllText(userConfigPath);
            // 用 JSONObject 解析
            JSONObject root = new JSONObject(jsonText);

            UserConfig userConfig = new UserConfig();

            // 赋值到类中
            userConfig.language = root["language"].StringValue;

            // 将类赋值到模型中
            resourceModel.userConfig = userConfig;

        }
        public void LoadPathConfig()
        {
            // 1. 读取用户配置
            string filePathConfigPath = Path.Combine(Application.dataPath, "StreamingAssets/Json/Config/FilePaths.json");
            string jsonText = File.ReadAllText(filePathConfigPath);

            // 用 JSONObject 解析
            JSONObject root = new JSONObject(jsonText);

            FilePathConfig filePathConfig = new FilePathConfig();

            // 赋值到类中
            filePathConfig.localizationRoot = root["LocalizationRoot"].StringValue;
            filePathConfig.cultureFileName = root["CultureFileName"].StringValue;


            // 将类赋值到模型中
            resourceModel.filePathConfig = filePathConfig;
        }
        public void LoadLanguageDataBase()
        {
            Debug.Log("语言库重载完毕");
            // 3. 拼接语言文件路径
            //string culturePath = Path.Combine(Application.dataPath, $"{resourceModel.filePathConfig.localizationRoot}/{resourceModel.userConfig.language}/culture.json");
            // 3. 获取父路径
            string localizationRootPath = Path.Combine(Application.dataPath, resourceModel.filePathConfig.localizationRoot);

            // 4. 遍历所有语言目录
            string[] languageDirs = Directory.GetDirectories(localizationRootPath);
            // 清理原有的语言词典
            LanguageDataBase.languageDataBase.Clear();
            LanguageDataBase.currentLanguage = resourceModel.userConfig.language;

            foreach (string langDir in languageDirs)
            {
                string langName = Path.GetFileName(langDir); // 语言名称，例如 ChineseSimple / English
                string culturePath = Path.Combine(langDir, "culture.json");

                if (!File.Exists(culturePath))
                {
                    Debug.LogWarning($"语言 {langName} 缺少 culture.json 文件，跳过。");
                    continue;
                }

                // 4. 读取语言 JSON
                string cultureJson = File.ReadAllText(culturePath);
                JSONObject cultureObj = new JSONObject(cultureJson);
                cultureObj = cultureObj["cultures"][0];
                JSONObject labelsObj = cultureObj["uilabels"];
                //Debug.Log("载入语言" + resourceModel.userConfig.language);
                //Debug.Log("载入语言" + langName);
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
                Debug.LogWarning(path + "没有读取到对应的Json文件");
                return null;
            }
            // 用 JSONObject 解析
            JSONObject root = new JSONObject(jsonText);
            return root;
        }
    }
}