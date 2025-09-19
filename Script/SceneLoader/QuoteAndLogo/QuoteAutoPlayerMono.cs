using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class QuoteAutoPlayerMono : MonoBehaviour
    {
        [Header("����������")]
        public GameObject background;
        public GameObject content;
        public Image foregroundMask; // �ڵ�ǰ����������Image���

        [Header("����")]
        public float fadeDuration = 1f;       // ���뵭��ʱ��
        public float minStayTime = 1f;        // ��Сͣ��ʱ��
        public float autoStayTime = 3f;       // �Զ�ͣ��ʱ�䣨�����Զ�ģʽ����Ч��
        public bool waitForPlayerInput = true;// �Ƿ���Ҫ���������ܼ���
        public int callbackValue = 0;         // �ص�����������Inspector����

        [Header("�ص�")]
        public Action<int> onFadeOutComplete; // �ⲿ�ɶ���

        private bool isPlaying = false;
        private bool canFadeOut = false;

        /// <summary>
        /// �����������̣����� -> ͣ�� -> ����
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

            // 1. ����
            yield return StartCoroutine(FadeForeground(1f, 0f, fadeDuration));

            // 2. ͣ��
            canFadeOut = false;
            yield return new WaitForSeconds(minStayTime);
            canFadeOut = true;

            if (waitForPlayerInput)
            {
                // �ȴ�����
                while (!Input.anyKeyDown)
                {
                    yield return null;
                }
            }
            else
            {
                // �Զ�����ʱ
                yield return new WaitForSeconds(autoStayTime - minStayTime);
            }

            // 3. ����
            yield return StartCoroutine(FadeForeground(0f, 1f, fadeDuration));

            // �ص�
            onFadeOutComplete?.Invoke(callbackValue);

            // ����
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