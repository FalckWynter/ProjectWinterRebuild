using PlentyFishFramework;
using QFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LogoManager : MonoBehaviour
{
    [Header("Logo �������� (�ⲿ��)")]
    public List<QuoteAutoPlayerMono> logoSequence = new List<QuoteAutoPlayerMono>();

    [Header("�ص�")]
    public Action<int> onLogoFinished; // ����Logo��ɣ���������
    public Action onAllFinished;       // ����Logo�������

    private int currentIndex = -1;

    private void Start()
    {
        // ��ÿ�� logo �󶨻ص�
        for (int i = 0; i < logoSequence.Count; i++)
        {
            int index = i; // ����հ�����
            logoSequence[i].onFadeOutComplete += (value) =>
            {
                HandleLogoFinished(index, value);
            };
        }
    }

    /// <summary>
    /// ��ͷ��ʼ���� Logo ����
    /// </summary>
    public void PlaySequence()
    {
        if (logoSequence.Count == 0) return;
        currentIndex = 0;
        logoSequence[currentIndex].Play();
    }

    private void HandleLogoFinished(int index, int callbackValue)
    {
        // �������� Logo ������ɵĻص�
        onLogoFinished?.Invoke(index);

        // ������һ��
        currentIndex++;
        if (currentIndex < logoSequence.Count)
        {
            logoSequence[currentIndex].Play();
        }
        else
        {
            // ���в������
            onAllFinished?.Invoke();
        }
    }
}
//public class LogoManager : MonoBehaviour
//{
//    [System.Serializable]
//    public class LogoEntry
//    {
//        public GameObject logoPrefab;      // Logo prefab�����������ʵ���ݺ�һ�� blockout �ڵ���
//        public CanvasGroup blockout => logoPrefab.transform.Find("BlockOut").GetComponent<CanvasGroup>();        // �ڵ����壨ȫ����Ļ���� CanvasGroup��
//        public bool waitForInput = false;  // �Ƿ�ȴ��������
//        public float stayTime = 2f;        // ������ȴ����룬ͣ��ʱ��
//        public string audioName;           // ����ʱ���ŵ���Ч����ѡ��
//    }

//    public List<LogoEntry> logoSequence = new List<LogoEntry>();
//    public float fadeDuration = 1f; // ���뽥��ʱ�䣨�룩

//    public enum LogoState { Idle, FadeIn, Stay, WaitInput, FadeOut, Finished }

//    public int currentIndex = -1;
//    public LogoState logoState = LogoState.Idle;
//    public float stateTimer = 0f;
//    public LogoEntry currentEntry;

//    private void Start()
//    {
//        NextLogo();
//    }

//    private void Update()
//    {
//        if (logoState == LogoState.Finished) return;
//        if (currentEntry == null) return;

//        stateTimer += Time.deltaTime;

//        switch (logoState)
//        {
//            case LogoState.FadeIn:
//                UpdateFade(1f, 0f);
//                if (stateTimer >= fadeDuration)
//                {
//                    currentEntry.blockout.alpha = 0f;
//                    if (currentEntry.waitForInput)
//                        ChangeState(LogoState.WaitInput);
//                    else
//                        ChangeState(LogoState.Stay);
//                }
//                break;

//            case LogoState.Stay:
//                if (stateTimer >= currentEntry.stayTime)
//                {
//                    ChangeState(LogoState.FadeOut);
//                }
//                break;

//            case LogoState.WaitInput:
//                if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
//                {
//                    ChangeState(LogoState.FadeOut);
//                }
//                break;

//            case LogoState.FadeOut:
//                UpdateFade(0f, 1f);
//                if (stateTimer >= fadeDuration)
//                {
//                    currentEntry.blockout.alpha = 1f;
//                    currentEntry.logoPrefab.SetActive(false);
//                    NextLogo();
//                }
//                break;
//        }
//    }

//    private void NextLogo()
//    {
//        currentIndex++;
//        if (currentIndex >= logoSequence.Count)
//        {
//            logoState = LogoState.Finished;
//            Debug.Log("All logos finished.");
//            // TODO: �������л�������������˵�
//            return;
//        }

//        currentEntry = logoSequence[currentIndex];
//        currentEntry.logoPrefab.SetActive(true);
//        currentEntry.blockout.alpha = 1f;

//        // ������Ч�����뿪ʼʱ��
//        if (!string.IsNullOrEmpty(currentEntry.audioName))
//        {
//            AudioClip clip = AudioDataBase.TryGetAudio(currentEntry.audioName);
//            if  ( clip != null)
//            {
//                AudioKit.PlaySound(clip);
//            }
//        }

//        ChangeState(LogoState.FadeIn);
//    }

//    private void ChangeState(LogoState newState)
//    {
//        logoState = newState;
//        stateTimer = 0f;
//    }

//    private void UpdateFade(float from, float to)
//    {
//        float t = Mathf.Clamp01(stateTimer / fadeDuration);
//        currentEntry.blockout.alpha = Mathf.Lerp(from, to, t);
//    }
//}

