using UnityEngine;
using UnityEditor;
//using UnityEditor.SceneManagement;
using TMPro;
using System.IO;

public class TMPFontReplacer /*: EditorWindow*/
{
    //public TMP_FontAsset oldFont;  // 旧字体
    //public TMP_FontAsset newFont;  // 新字体
    //public string targetFolder = "Assets/MyTargetFolder"; // 目标目录

    //[MenuItem("Tools/TMP Font Replacer")]
    //public static void ShowWindow()
    //{
    //    GetWindow<TMPFontReplacer>("TMP Font Replacer");
    //}

    //void OnGUI()
    //{
    //    GUILayout.Label("批量替换 TMP 字体", EditorStyles.boldLabel);
    //    oldFont = (TMP_FontAsset)EditorGUILayout.ObjectField("旧字体 (要替换的)", oldFont, typeof(TMP_FontAsset), false);
    //    newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("新字体 (替换为)", newFont, typeof(TMP_FontAsset), false);
    //    targetFolder = EditorGUILayout.TextField("目标目录 (Assets/...)", targetFolder);

    //    if (GUILayout.Button("替换目标目录中的字体"))
    //    {
    //        ReplaceTMPFontsInTargetFolder();
    //    }
    //}

    //void ReplaceTMPFontsInTargetFolder()
    //{
    //    if (oldFont == null || newFont == null)
    //    {
    //        Debug.LogError("请先指定旧字体和新字体！");
    //        return;
    //    }

    //    if (!AssetDatabase.IsValidFolder(targetFolder))
    //    {
    //        Debug.LogError($"目标目录无效: {targetFolder}");
    //        return;
    //    }

    //    int modifiedCount = 0;

    //    // 1️⃣ 替换目标目录中的场景文件（.unity）
    //    string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { targetFolder });
    //    foreach (string guid in sceneGuids)
    //    {
    //        string scenePath = AssetDatabase.GUIDToAssetPath(guid);
    //        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
    //        bool sceneModified = false;

    //        foreach (var text in FindObjectsOfType<TextMeshPro>())
    //        {
    //            if (text.font == oldFont)
    //            {
    //                Undo.RecordObject(text, "Replace TMP Font");
    //                text.font = newFont;
    //                sceneModified = true;
    //                modifiedCount++;
    //            }
    //        }

    //        foreach (var textUI in FindObjectsOfType<TextMeshProUGUI>())
    //        {
    //            if (textUI.font == oldFont)
    //            {
    //                Undo.RecordObject(textUI, "Replace TMP Font");
    //                textUI.font = newFont;
    //                sceneModified = true;
    //                modifiedCount++;
    //            }
    //        }

    //        if (sceneModified)
    //        {
    //            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    //            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    //        }
    //    }

    //    // 2️⃣ 替换目标目录中的 Prefab（.prefab）
    //    string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { targetFolder });
    //    foreach (string guid in prefabGuids)
    //    {
    //        string path = AssetDatabase.GUIDToAssetPath(guid);
    //        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

    //        if (prefab != null)
    //        {
    //            bool changed = false;
    //            var texts = prefab.GetComponentsInChildren<TextMeshPro>(true);
    //            var textsUI = prefab.GetComponentsInChildren<TextMeshProUGUI>(true);

    //            foreach (var text in texts)
    //            {
    //                if (text.font == oldFont)
    //                {
    //                    Undo.RecordObject(text, "Replace TMP Font");
    //                    text.font = newFont;
    //                    changed = true;
    //                }
    //            }

    //            foreach (var textUI in textsUI)
    //            {
    //                if (textUI.font == oldFont)
    //                {
    //                    Undo.RecordObject(textUI, "Replace TMP Font");
    //                    textUI.font = newFont;
    //                    changed = true;
    //                }
    //            }

    //            if (changed)
    //            {
    //                PrefabUtility.SavePrefabAsset(prefab);
    //                modifiedCount++;
    //            }
    //        }
    //    }

    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //    Debug.Log($"✅ 替换完成，共修改 {modifiedCount} 处 TextMeshPro 组件的字体 (目录: {targetFolder})。");
    //}
}
