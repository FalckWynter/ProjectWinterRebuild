using QFramework;
using System.Collections.Generic;
using UnityEngine;

public class LogoManager : MonoBehaviour
{
    [System.Serializable]
    public class LogoEntry
    {
        public GameObject logoPrefab;      // Logo prefab，里面包含真实内容和一个 blockout 遮挡物
        public CanvasGroup blockout => logoPrefab.transform.Find("BlockOut").GetComponent<CanvasGroup>();        // 遮挡物体（全屏黑幕，带 CanvasGroup）
        public bool waitForInput = false;  // 是否等待玩家输入
        public float stayTime = 2f;        // 如果不等待输入，停留时间
        public string audioName;           // 渐入时播放的音效（可选）
    }

    public List<LogoEntry> logoSequence = new List<LogoEntry>();
    public float fadeDuration = 1f; // 渐入渐出时间（秒）

    public enum LogoState { Idle, FadeIn, Stay, WaitInput, FadeOut, Finished }

    public int currentIndex = -1;
    public LogoState logoState = LogoState.Idle;
    public float stateTimer = 0f;
    public LogoEntry currentEntry;

    private void Start()
    {
        NextLogo();
    }

    private void Update()
    {
        if (logoState == LogoState.Finished) return;
        if (currentEntry == null) return;

        stateTimer += Time.deltaTime;

        switch (logoState)
        {
            case LogoState.FadeIn:
                UpdateFade(1f, 0f);
                if (stateTimer >= fadeDuration)
                {
                    currentEntry.blockout.alpha = 0f;
                    if (currentEntry.waitForInput)
                        ChangeState(LogoState.WaitInput);
                    else
                        ChangeState(LogoState.Stay);
                }
                break;

            case LogoState.Stay:
                if (stateTimer >= currentEntry.stayTime)
                {
                    ChangeState(LogoState.FadeOut);
                }
                break;

            case LogoState.WaitInput:
                if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
                {
                    ChangeState(LogoState.FadeOut);
                }
                break;

            case LogoState.FadeOut:
                UpdateFade(0f, 1f);
                if (stateTimer >= fadeDuration)
                {
                    currentEntry.blockout.alpha = 1f;
                    currentEntry.logoPrefab.SetActive(false);
                    NextLogo();
                }
                break;
        }
    }

    private void NextLogo()
    {
        currentIndex++;
        if (currentIndex >= logoSequence.Count)
        {
            logoState = LogoState.Finished;
            Debug.Log("All logos finished.");
            // TODO: 在这里切换场景或进入主菜单
            return;
        }

        currentEntry = logoSequence[currentIndex];
        currentEntry.logoPrefab.SetActive(true);
        currentEntry.blockout.alpha = 1f;

        // 播放音效（渐入开始时）
        if (!string.IsNullOrEmpty(currentEntry.audioName))
        {
            AudioClip clip = AudioDataBase.TryGetAudio(currentEntry.audioName);
            if  ( clip != null)
            {
                AudioKit.PlaySound(clip);
            }
        }

        ChangeState(LogoState.FadeIn);
    }

    private void ChangeState(LogoState newState)
    {
        logoState = newState;
        stateTimer = 0f;
    }

    private void UpdateFade(float from, float to)
    {
        float t = Mathf.Clamp01(stateTimer / fadeDuration);
        currentEntry.blockout.alpha = Mathf.Lerp(from, to, t);
    }
}

//using QFramework;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LogoManager : MonoBehaviour
//{
//    [System.Serializable]
//    public class LogoEntry
//    {
//        public GameObject logoPrefab;      // Logo prefab，里面包含真实内容和一个 blockout 遮挡物
//        public CanvasGroup blockout => logoPrefab.transform.Find("BlockOut").GetComponent<CanvasGroup>();       // 遮挡物体（全屏黑幕，带 CanvasGroup）
//        public bool waitForInput = false;  // 是否等待玩家输入
//        public float stayTime = 2f;        // 如果不等待输入，停留时间
//        public string audioName;           // 渐入时播放的音效（可选）
//    }

//    public List<LogoEntry> logoSequence = new List<LogoEntry>();
//    public float fadeDuration = 1f; // 渐入渐出时间（秒）

//    private void Start()
//    {
//        StartCoroutine(PlayLogos());
//    }

//    private IEnumerator PlayLogos()
//    {
//        for (int i = 0; i < logoSequence.Count; i++)
//        {
//            var entry = logoSequence[i];
//            entry.logoPrefab.SetActive(true);

//            // 遮挡层初始全黑
//            entry.blockout.alpha = 1f;

//            // 播放音效（渐入开始时）
//            if (!string.IsNullOrEmpty(entry.audioName))
//            {
//                AudioClip clip = AudioDataBase.TryGetAudio(entry.audioName);
//                if ( clip != null)
//                {
//                    AudioKit.PlaySound(clip);
//                }
//            }

//            // 渐入：遮挡物从 1 -> 0
//            yield return StartCoroutine(Fade(entry.blockout, 1f, 0f, fadeDuration));

//            // 停留或等待输入
//            if (entry.waitForInput)
//            {
//                yield return StartCoroutine(WaitForInput());
//            }
//            else
//            {
//                yield return new WaitForSeconds(entry.stayTime);
//            }

//            // 渐出：遮挡物从 0 -> 1
//            yield return StartCoroutine(Fade(entry.blockout, 0f, 1f, fadeDuration));

//            // 完成后隐藏这个 prefab
//            entry.logoPrefab.SetActive(false);
//        }

//        Debug.Log("All logos finished.");
//        // TODO: 在这里切换场景或进入主菜单
//    }

//    private IEnumerator Fade(CanvasGroup cg, float from, float to, float duration)
//    {
//        float t = 0f;
//        cg.alpha = from;
//        while (t < duration)
//        {
//            t += Time.deltaTime;
//            cg.alpha = Mathf.Lerp(from, to, t / duration);
//            yield return null;
//        }
//        cg.alpha = to;
//    }

//    private IEnumerator WaitForInput()
//    {
//        yield return new WaitForSeconds(0.1f); // 避免误触
//        bool pressed = false;
//        while (!pressed)
//        {
//            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
//                pressed = true;
//            yield return null;
//        }
//    }
//}
