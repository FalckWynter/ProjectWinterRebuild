using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class QuoteCanvasControllerMono : MonoBehaviour
    {
        [Header("Logo 播放序列 (外部绑定)")]
        public List<QuoteAutoPlayerMono> logoSequence = new List<QuoteAutoPlayerMono>();

        [Header("背景控制")]
        public CanvasGroup backgroundGroup;       // 使用CanvasGroup来控制背景
        public bool fadeInBackground = true;        // 是否淡入淡出背景
        public bool fadeOutBackground = true;        // 是否淡入淡出背景
        public float backgroundFadeDuration = 1f; // 背景淡入淡出时间

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

            if (backgroundGroup != null)
            {
                backgroundGroup.alpha = 0f;
                backgroundGroup.blocksRaycasts = false;
            }
        }

        /// <summary>
        /// 从头开始播放 Logo 序列
        /// </summary>
        public void PlaySequence()
        {
            if (logoSequence.Count == 0) return;
            currentIndex = 0;

            if (backgroundGroup != null)
            {
                backgroundGroup.gameObject.SetActive(true);
                if (fadeInBackground)
                    StartCoroutine(FadeCanvasGroup(backgroundGroup, 0f, 1f, backgroundFadeDuration, () =>
                    {
                        logoSequence[currentIndex].Play();
                    }));
                else
                {
                    SetCanvasGroupAlpha(backgroundGroup, 1f);
                    logoSequence[currentIndex].Play();
                }
            }
            else
            {
                logoSequence[currentIndex].Play();
            }
        }

        private void HandleLogoFinished(int index, int callbackValue)
        {
            onLogoFinished?.Invoke(index);

            currentIndex++;
            if (currentIndex < logoSequence.Count)
            {
                logoSequence[currentIndex].Play();
            }
            else
            {
                // 背景淡出或直接隐藏
                if (backgroundGroup != null)
                {
                    if (fadeOutBackground)
                        StartCoroutine(FadeCanvasGroup(backgroundGroup, 1f, 0f, backgroundFadeDuration, () =>
                        {
                            backgroundGroup.gameObject.SetActive(false);
                            onAllFinished?.Invoke();
                        }));
                    else
                    {
                        SetCanvasGroupAlpha(backgroundGroup, 0f);
                        backgroundGroup.gameObject.SetActive(false);
                        onAllFinished?.Invoke();
                    }
                }
                else
                {
                    onAllFinished?.Invoke();
                }
            }
        }

        private IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration, Action onComplete = null)
        {
            float time = 0f;
            group.alpha = from;
            group.blocksRaycasts = true;

            while (time < duration)
            {
                float t = time / duration;
                group.alpha = Mathf.Lerp(from, to, t);
                group.blocksRaycasts = group.alpha > 0.001f;
                time += Time.deltaTime;
                yield return null;
            }

            group.alpha = to;
            group.blocksRaycasts = group.alpha > 0.001f;
            onComplete?.Invoke();
        }

        private void SetCanvasGroupAlpha(CanvasGroup group, float alpha)
        {
            group.alpha = alpha;
            group.blocksRaycasts = alpha > 0.001f;
        }
    }
}