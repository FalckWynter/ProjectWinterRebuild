using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonTester : MonoBehaviour
{
    void Start()
    {
        // ��������
        JSONObject root = new JSONObject();

        // cultures ������
        JSONObject culturesArray = new JSONObject(JSONObject.Type.Array);

        // ���� culture ����
        JSONObject culture = new JSONObject();
        culture.AddField("id", "en");
        culture.AddField("endonym", "English");
        culture.AddField("exonym", "English");
        culture.AddField("fontscript", "latin");
        culture.AddField("boldallowed", true);
        culture.AddField("released", true);

        // ��Ƕ���� uilabels
        JSONObject uiLabels = new JSONObject();
        uiLabels.AddField("UI_PAUSE", "Pause");
        culture.AddField("uilabels", uiLabels);

        // �� culture �ŵ�����
        culturesArray.Add(culture);

        // ������ŵ� root
        root.AddField("cultures", culturesArray);

        // ת���ַ���
        string jsonText = root.ToString(true); // true ��ʾƯ����ӡ

        // ���浽�ļ�
        string path = Path.Combine(Application.dataPath, "StreamingAssets/Json/Localization/Language/English/en.json");
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, jsonText);

        Debug.Log("JSON saved to " + path);
    }
}
