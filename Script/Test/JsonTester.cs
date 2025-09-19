using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonTester : MonoBehaviour
{
    void Start()
    {
        // 最外层对象
        JSONObject root = new JSONObject();

        // cultures 是数组
        JSONObject culturesArray = new JSONObject(JSONObject.Type.Array);

        // 单个 culture 对象
        JSONObject culture = new JSONObject();
        culture.AddField("id", "en");
        culture.AddField("endonym", "English");
        culture.AddField("exonym", "English");
        culture.AddField("fontscript", "latin");
        culture.AddField("boldallowed", true);
        culture.AddField("released", true);

        // 内嵌对象 uilabels
        JSONObject uiLabels = new JSONObject();
        uiLabels.AddField("UI_PAUSE", "Pause");
        culture.AddField("uilabels", uiLabels);

        // 把 culture 放到数组
        culturesArray.Add(culture);

        // 把数组放到 root
        root.AddField("cultures", culturesArray);

        // 转成字符串
        string jsonText = root.ToString(true); // true 表示漂亮打印

        // 保存到文件
        string path = Path.Combine(Application.dataPath, "StreamingAssets/Json/Localization/Language/English/en.json");
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, jsonText);

        Debug.Log("JSON saved to " + path);
    }
}
