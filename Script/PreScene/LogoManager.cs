using PlentyFishFramework;
using QFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LogoManager : MonoBehaviour
{
    [Header("Logo 播放序列 (外部绑定)")]
    public List<QuoteAutoPlayerMono> logoSequence = new List<QuoteAutoPlayerMono>();

    [Header("回调")]
    public Action<int> onLogoFinished; // 单个Logo完成，传入索引
    public Action onAllFinished;       // 所有Logo播放完毕

    private int currentIndex = -1;

    private void Start()
    {
        // 给每个 logo 绑定回调
        for (int i = 0; i < logoSequence.Count; i++)
        {
            int index = i; // 避免闭包问题
            logoSequence[i].onFadeOutComplete += (value) =>
            {
                HandleLogoFinished(index, value);
            };
        }
    }

    /// <summary>
    /// 从头开始播放 Logo 序列
    /// </summary>
    public void PlaySequence()
    {
        if (logoSequence.Count == 0) return;
        currentIndex = 0;
        logoSequence[currentIndex].Play();
    }

    private void HandleLogoFinished(int index, int callbackValue)
    {
        // 触发单个 Logo 播放完成的回调
        onLogoFinished?.Invoke(index);

        // 播放下一个
        currentIndex++;
        if (currentIndex < logoSequence.Count)
        {
            logoSequence[currentIndex].Play();
        }
        else
        {
            // 所有播放完成
            onAllFinished?.Invoke();
        }
    }
}
//public class LogoManager : MonoBehaviour
//{
//    [System.Serializable]
//    public class LogoEntry
//    {
//        public GameObject logoPrefab;      // Logo prefab，里面包含真实内容和一个 blockout 遮挡物
//        public CanvasGroup blockout => logoPrefab.transform.Find("BlockOut").GetComponent<CanvasGroup>();        // 遮挡物体（全屏黑幕，带 CanvasGroup）
//        public bool waitForInput = false;  // 是否等待玩家输入
//        public float stayTime = 2f;        // 如果不等待输入，停留时间
//        public string audioName;           // 渐入时播放的音效（可选）
//    }

//    public List<LogoEntry> logoSequence = new List<LogoEntry>();
//    public float fadeDuration = 1f; // 渐入渐出时间（秒）

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
//            // TODO: 在这里切换场景或进入主菜单
//            return;
//        }

//        currentEntry = logoSequence[currentIndex];
//        currentEntry.logoPrefab.SetActive(true);
//        currentEntry.blockout.alpha = 1f;

//        // 播放音效（渐入开始时）
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

