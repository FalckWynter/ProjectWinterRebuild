using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class QuoteAutoPlayerMono : MonoBehaviour
    {
        [Header("子物体引用")]
        public GameObject background;
        public GameObject content;
        public Image foregroundMask; // 遮挡前景，必须有Image组件

        [Header("参数")]
        public float fadeDuration = 1f;       // 淡入淡出时间
        public float minStayTime = 1f;        // 最小停留时间
        public float autoStayTime = 3f;       // 自动停留时间（仅在自动模式下生效）
        public bool waitForPlayerInput = true;// 是否需要玩家输入才能继续
        public int callbackValue = 0;         // 回调参数，可在Inspector设置

        [Header("回调")]
        public Action<int> onFadeOutComplete; // 外部可订阅

        private bool isPlaying = false;
        private bool canFadeOut = false;

        /// <summary>
        /// 播放整个流程：渐入 -> 停留 -> 渐出
        /// </summary>
        public void Play()
        {
            if (isPlaying) return;
            gameObject.SetActive(true);
            StartCoroutine(PlayRoutine());
        }

        private IEnumerator PlayRoutine()
        {
            isPlaying = true;
            gameObject.SetActive(true);

            // 1. 渐入
            yield return StartCoroutine(FadeForeground(1f, 0f, fadeDuration));

            // 2. 停留
            canFadeOut = false;
            yield return new WaitForSeconds(minStayTime);
            canFadeOut = true;

            if (waitForPlayerInput)
            {
                // 等待输入
                while (!Input.anyKeyDown)
                {
                    yield return null;
                }
            }
            else
            {
                // 自动倒计时
                yield return new WaitForSeconds(autoStayTime - minStayTime);
            }

            // 3. 渐出
            yield return StartCoroutine(FadeForeground(0f, 1f, fadeDuration));

            // 回调
            onFadeOutComplete?.Invoke(callbackValue);

            // 结束
            isPlaying = false;
            gameObject.SetActive(false);
        }

        private IEnumerator FadeForeground(float from, float to, float duration)
        {
            float time = 0f;
            Color c = foregroundMask.color;
            while (time < duration)
            {
                float t = time / duration;
                c.a = Mathf.Lerp(from, to, t);
                foregroundMask.color = c;
                time += Time.deltaTime;
                yield return null;
            }
            c.a = to;
            foregroundMask.color = c;
        }
    }
}