using UnityEditor;
using UnityEngine;
using System;

public class ProjectTimeTracker : EditorWindow
{
    private const string TotalTimeKey = "MyProject_TotalTimeInSeconds";
    private static DateTime sessionStartTime;
    private static bool initialized = false;

    private string addHourInput = "1";
    private string subtractHourInput = "0.5";
    private Vector2 scroll;

    [MenuItem("Tools/Usage Stats/Project Time Tracker")]
    public static void ShowWindow()
    {
        GetWindow<ProjectTimeTracker>("Project Time Tracker");
    }

    private void OnEnable()
    {
        if (!initialized)
        {
            sessionStartTime = DateTime.Now;
            initialized = true;

            EditorApplication.quitting += OnEditorQuitting;
        }
    }

    private void OnGUI()
    {
        if (!initialized)
            OnEnable();

        scroll = EditorGUILayout.BeginScrollView(scroll);

        EditorGUILayout.LabelField(" 项目使用时间统计", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        TimeSpan currentSession = DateTime.Now - sessionStartTime;
        int storedSeconds = EditorPrefs.GetInt(TotalTimeKey, 0);
        int totalSeconds = storedSeconds + (int)currentSession.TotalSeconds;

        TimeSpan totalTime = TimeSpan.FromSeconds(totalSeconds);

        EditorGUILayout.LabelField("当前会话:", FormatTime(currentSession));
        EditorGUILayout.LabelField("累计总时间:", FormatTime(totalTime));

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(" 手动增加时间", EditorStyles.boldLabel);
        addHourInput = EditorGUILayout.TextField("增加小时数", addHourInput);

        if (GUILayout.Button("添加时间"))
        {
            if (TryParseHour(addHourInput, out int deltaSeconds))
            {
                AddTime(deltaSeconds);
            }
            else
            {
                EditorUtility.DisplayDialog("输入无效", "请输入一个正数小时数（支持小数）", "确定");
            }
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(" 手动减少时间", EditorStyles.boldLabel);
        subtractHourInput = EditorGUILayout.TextField("减少小时数", subtractHourInput);

        if (GUILayout.Button("减少时间"))
        {
            if (TryParseHour(subtractHourInput, out int deltaSeconds))
            {
                RemoveTime(deltaSeconds);
            }
            else
            {
                EditorUtility.DisplayDialog("输入无效", "请输入一个正数小时数（支持小数）", "确定");
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void OnEditorQuitting()
    {
        TimeSpan sessionDuration = DateTime.Now - sessionStartTime;
        int sessionSeconds = (int)sessionDuration.TotalSeconds;
        int previousSeconds = EditorPrefs.GetInt(TotalTimeKey, 0);
        EditorPrefs.SetInt(TotalTimeKey, previousSeconds + sessionSeconds);
    }

    private string FormatTime(TimeSpan time)
    {
        return $"{(int)time.TotalHours}h {time.Minutes}m {time.Seconds}s";
    }

    private bool TryParseHour(string input, out int seconds)
    {
        if (float.TryParse(input, out float hours) && hours > 0f)
        {
            seconds = Mathf.RoundToInt(hours * 3600f);
            return true;
        }

        seconds = 0;
        return false;
    }

    private void AddTime(int seconds)
    {
        int prev = EditorPrefs.GetInt(TotalTimeKey, 0);
        EditorPrefs.SetInt(TotalTimeKey, prev + seconds);
        Debug.Log($"[ProjectTimeTracker] 添加了 {(seconds / 3600f):F2} 小时");
    }

    private void RemoveTime(int seconds)
    {
        int prev = EditorPrefs.GetInt(TotalTimeKey, 0);
        int newValue = Mathf.Max(prev - seconds, 0);
        EditorPrefs.SetInt(TotalTimeKey, newValue);
        Debug.Log($"[ProjectTimeTracker] 减少了 {(seconds / 3600f):F2} 小时");
    }
}
