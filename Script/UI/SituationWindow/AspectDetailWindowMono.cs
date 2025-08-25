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

        public CanvasGroup canvasGroup; // 控制淡入淡出
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
            // 检查右键点击
            if (Input.GetMouseButtonDown(1)) // 鼠标右键
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
            // 如果当前显示的是同一张卡，忽略
            if (currentAspect == newAspect) return;

            // 如果面板正在播放动画，停止它
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);

            // 如果面板当前是激活状态，则先淡出，再更新内容并淡入
            if (gameObject.activeSelf && canvasGroup.alpha > 0.01f)
            {
                transitionCoroutine = StartCoroutine(FadeOutThenShowNew(newAspect));
            }
            else
            {
                // 面板是关闭的，直接显示
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
            // 这里填写你的面板内容更新逻辑，如文本、图标等刷新
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