using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class JsonLoaderTest : MonoBehaviour
{
    public RootData loadedData;

    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Json/Localization/Language/English/culture.json");
        string jsonText = File.ReadAllText(path);
        Debug.Log("ƴ��·��" + path);
        if(jsonText == null)
        {
            Debug.Log("�ı�������");
            return;
        }
        Debug.Log("�ı�" + jsonText);

        // �� JSONObject ����
        JSONObject root = new JSONObject(jsonText);

        loadedData = new RootData();

        JSONObject culturesArray = root["cultures"];
        foreach (JSONObject cultureObj in culturesArray.List)
        {
            CultureData c = new CultureData();
            c.id = cultureObj["id"].StringValue;
            c.endonym = cultureObj["endonym"].StringValue;
            c.exonym = cultureObj["exonym"].StringValue;
            c.fontscript = cultureObj["fontscript"].StringValue;
            c.boldallowed = cultureObj["boldallowed"].BoolValue;
            c.released = cultureObj["released"].BoolValue;

            // uilabels -> Dictionary
            JSONObject labelsObj = cultureObj["uilabels"];
            for (int i = 0; i < labelsObj.List.Count; i++)
            {
                string key = labelsObj.Keys[i];
                string value = labelsObj.List[i].StringValue;
                c.uilabels[key] = value;
            }

            loadedData.cultures.Add(c);
        }

        Debug.Log("Loaded cultures count: " + loadedData.cultures.Count);
    }
}


[System.Serializable]
public class CultureData
{
    public string id;
    public string endonym;
    public string exonym;
    public string fontscript;
    public bool boldallowed;
    public bool released;

    // Unity �������л� Dictionary������ĳ� SerializableDictionary
    // ����򵥵İ취���Լ���װһ��
    public Dictionary<string, string> uilabels = new Dictionary<string, string>();
}

[System.Serializable]
public class RootData
{
    public List<CultureData> cultures = new List<CultureData>();
}
