using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class AspectDetailWindowMono : MonoBehaviour
    {
        public GameObject totalParent;
        public TextMeshProUGUI labelText, descriptionText;
        public Image artworkIcon;

        public CanvasGroup canvasGroup; // ���Ƶ��뵭��
        public float fadeDuration = 0.3f;

        private AbstractAspect currentAspect;
        private Coroutine transitionCoroutine;
        public LayoutElement layoutElement;


        private void Awake()
        {
            UtilModel.aspectDetailWindow = this;
            HideWindow();
            layoutElement = GetComponent<LayoutElement>();

        }
        private void Update()
        {
            // ����Ҽ����
            if (Input.GetMouseButtonDown(1)) // ����Ҽ�
            {
                Hide();
            }
            if (canvasGroup.alpha == 0)
                layoutElement.ignoreLayout = true;
            else
                layoutElement.ignoreLayout = false;
        }
        public void ShowWindow()
        {
            totalParent.gameObject.SetActive(true);
        }
        public void HideWindow()
        {
            canvasGroup.alpha = 0;
        }

        public void ShowAspect(AbstractAspect newAspect)
        {
            ShowWindow();
            // �����ǰ��ʾ����ͬһ�ſ�������
            if (currentAspect == newAspect) return;

            // ���������ڲ��Ŷ�����ֹͣ��
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);

            // �����嵱ǰ�Ǽ���״̬�����ȵ������ٸ������ݲ�����
            if (gameObject.activeSelf && canvasGroup.alpha > 0.01f)
            {
                transitionCoroutine = StartCoroutine(FadeOutThenShowNew(newAspect));
            }
            else
            {
                // ����ǹرյģ�ֱ����ʾ
                gameObject.SetActive(true);
                currentAspect = newAspect;
                UpdateContent(currentAspect);
                transitionCoroutine = StartCoroutine(FadeIn());
            }
        }

        private IEnumerator FadeOutThenShowNew(AbstractAspect newCard)
        {
            yield return FadeOut();
            currentAspect = newCard;
            UpdateContent(currentAspect);
            yield return FadeIn();
        }

        private IEnumerator FadeOut()
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = 0f;
        }

        private IEnumerator FadeIn()
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        private void UpdateContent(AbstractAspect card)
        {
            totalParent.gameObject.SetActive(true);
            // ������д���������ݸ����߼������ı���ͼ���ˢ��
            labelText.text = card.label;
            descriptionText.text = card.lore;
            artworkIcon.sprite = card.icon;
        }

        public void Hide()
        {
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);
            currentAspect = null;
            if(canvasGroup.alpha > 0)
            transitionCoroutine = StartCoroutine(FadeOutAndHide());
        }
        private IEnumerator FadeOutAndHide()
        {
            yield return FadeOut();
            //gameObject.SetActive(false);
            canvasGroup.alpha = 0f;
            currentAspect = null;
        }
    }
}