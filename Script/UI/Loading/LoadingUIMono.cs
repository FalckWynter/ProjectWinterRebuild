using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class LoadingUIMono : MonoBehaviour
    {
        [Header("UI ���")]
        public CanvasGroup canvasGroup;
        public Slider progressBar;
        public TextMeshProUGUI progressText;
        public TextMeshProUGUI pressAnyKeyText;

        private void Awake()
        {
            if (pressAnyKeyText != null)
                pressAnyKeyText.gameObject.SetActive(false);
        }

        /// <summary>
        /// ���½��������ı�
        /// </summary>
        public void UpdateProgress(float progress)
        {
            if (progressBar != null)
                progressBar.value = progress;

            if (progressText != null)
                progressText.text = string.Format("{0:P0}", progress);
        }

        /// <summary>
        /// ��ʾ��Ұ����������
        /// </summary>
        public void ShowPressAnyKeyHint()
        {
            if (pressAnyKeyText != null)
                pressAnyKeyText.gameObject.SetActive(true);
            if (progressText != null)
                progressText.gameObject.SetActive(false);
        }

        /// <summary>
        /// ���� UI
        /// </summary>
        public IEnumerator FadeIn(float duration)
        {
            gameObject.SetActive(true);
            if (progressText != null)
                progressText.gameObject.SetActive(true);
            if (pressAnyKeyText != null)
                pressAnyKeyText.gameObject.SetActive(false);

            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 0;

            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(timer / duration);
                yield return null;
            }
            canvasGroup.alpha = 1;
        }

        /// <summary>
        /// ���� UI
        /// </summary>
        public IEnumerator FadeOut(float duration)
        {
            canvasGroup.blocksRaycasts = false;

            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = 1 - Mathf.Clamp01(timer / duration);
                yield return null;
            }
            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// ��������͸����
        /// </summary>
        public void SetAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;
            canvasGroup.blocksRaycasts = alpha > 0;
            gameObject.SetActive(alpha > 0);
        }

        public void StartExitAndSelfDestroy(float fadeOutDuration, bool fadeOut,float delay)
        {
            StartCoroutine(ExitAndDestroy(fadeOutDuration, fadeOut,delay));
        }

        private IEnumerator ExitAndDestroy(float fadeOutDuration, bool fadeOut, float delay)
        {
            yield return new WaitForSeconds(delay);



            if (fadeOut)
                yield return FadeOut(fadeOutDuration);
            else
                SetAlpha(0);

            Destroy(this.transform.root.gameObject); // ��������Canvas
        }

    }
}