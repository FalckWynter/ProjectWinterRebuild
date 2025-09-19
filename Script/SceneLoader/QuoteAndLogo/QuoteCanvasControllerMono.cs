using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class QuoteCanvasControllerMono : MonoBehaviour
    {
        [Header("Logo �������� (�ⲿ��)")]
        public List<QuoteAutoPlayerMono> logoSequence = new List<QuoteAutoPlayerMono>();

        [Header("��������")]
        public CanvasGroup backgroundGroup;       // ʹ��CanvasGroup�����Ʊ���
        public bool fadeInBackground = true;        // �Ƿ��뵭������
        public bool fadeOutBackground = true;        // �Ƿ��뵭������
        public float backgroundFadeDuration = 1f; // �������뵭��ʱ��

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

            if (backgroundGroup != null)
            {
                backgroundGroup.alpha = 0f;
                backgroundGroup.blocksRaycasts = false;
            }
        }

        /// <summary>
        /// ��ͷ��ʼ���� Logo ����
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
                // ����������ֱ������
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