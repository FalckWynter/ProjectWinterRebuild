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

        EditorGUILayout.LabelField(" ��Ŀʹ��ʱ��ͳ��", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        TimeSpan currentSession = DateTime.Now - sessionStartTime;
        int storedSeconds = EditorPrefs.GetInt(TotalTimeKey, 0);
        int totalSeconds = storedSeconds + (int)currentSession.TotalSeconds;

        TimeSpan totalTime = TimeSpan.FromSeconds(totalSeconds);

        EditorGUILayout.LabelField("��ǰ�Ự:", FormatTime(currentSession));
        EditorGUILayout.LabelField("�ۼ���ʱ��:", FormatTime(totalTime));

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(" �ֶ�����ʱ��", EditorStyles.boldLabel);
        addHourInput = EditorGUILayout.TextField("����Сʱ��", addHourInput);

        if (GUILayout.Button("���ʱ��"))
        {
            if (TryParseHour(addHourInput, out int deltaSeconds))
            {
                AddTime(deltaSeconds);
            }
            else
            {
                EditorUtility.DisplayDialog("������Ч", "������һ������Сʱ����֧��С����", "ȷ��");
            }
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(" �ֶ�����ʱ��", EditorStyles.boldLabel);
        subtractHourInput = EditorGUILayout.TextField("����Сʱ��", subtractHourInput);

        if (GUILayout.Button("����ʱ��"))
        {
            if (TryParseHour(subtractHourInput, out int deltaSeconds))
            {
                RemoveTime(deltaSeconds);
            }
            else
            {
                EditorUtility.DisplayDialog("������Ч", "������һ������Сʱ����֧��С����", "ȷ��");
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
        Debug.Log($"[ProjectTimeTracker] ����� {(seconds / 3600f):F2} Сʱ");
    }

    private void RemoveTime(int seconds)
    {
        int prev = EditorPrefs.GetInt(TotalTimeKey, 0);
        int newValue = Mathf.Max(prev - seconds, 0);
        EditorPrefs.SetInt(TotalTimeKey, newValue);
        Debug.Log($"[ProjectTimeTracker] ������ {(seconds / 3600f):F2} Сʱ");
    }
}
