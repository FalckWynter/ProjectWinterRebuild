using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class TokenDetailsWindowMono : MonoBehaviour, IPointerClickHandler
    {

        // 3.1.2.2节 卡槽描述页面 加入
        // 展示卡牌和卡槽的性相1
        public GameObject totalParent;
        public AspectLineParentMono requireAspectParent, forbiddenAspectParent, essentialAspectParent, simpleAspectParent;
        public TextMeshProUGUI labelText, descriptionText;
        public Image artworkIcon;
        public LayoutElement layoutElement;
        public GameObject cardHintGameobject;

        public void ShowWindow()
        {
            totalParent.gameObject.SetActive(true);
        }
        public void HideWindow()
        {
            canvasGroup.alpha = 0;
        }
        public void ShowWindowForSlot(AbstractSlot slot)
        {

        }
        public void ShowWindowForCard(AbstractCard card)
        {


            ShowCard(card);

        }
        public void SetAspectParentContent(Dictionary<string,int> dictionary)
        {

           
        }
        private void Awake()
        {
            UtilModel.tokenDetailWindow = this;
            HideWindow();
            layoutElement = GetComponent<LayoutElement>();

        }
        // Start is called before the first frame update

        private GraphicRaycaster uiRaycaster;
        private EventSystem eventSystem;
        // Update is called once per frame
        void Start()
        {
            uiRaycaster = FindObjectOfType<GraphicRaycaster>();
            eventSystem = EventSystem.current;
        }

        void Update()
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
        //PointerEventData pointerData = new PointerEventData(eventSystem)
        //{
        //    position = Input.mousePosition
        //};

        //List<RaycastResult> raycastResults = new List<RaycastResult>();
        //uiRaycaster.Raycast(pointerData, raycastResults);

        //// 检查点击对象是否是 detailPanel 或其子物体
        //bool clickedOnPanel = raycastResults.Any(r => r.gameObject.transform.IsChildOf(gameObject.transform));
        //foreach(var item in raycastResults)
        //Debug.Log("点击物体" + item.gameObject.transform.name);
        //Debug.Log("结果" + clickedOnPanel);

        //if (!clickedOnPanel)
        //{
        //    totalParent.SetActive(false); // 或者调用你自己的关闭函数
        //}
        //HideWindow();
        void CloseDetailPanel()
        {
            if (totalParent != null)
            {
                totalParent.SetActive(false);
            }
        }

        // 判断鼠标是否点击在任何UI元素上
        bool IsPointerOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //if (Input.GetMouseButtonDown(1)) // 1 是右键
            //{
            //    //CloseDetailPanel();
            //    Hide();
            //}
        }
        // 3.1.2.3 描述淡入淡出 加入
        public CanvasGroup canvasGroup; // 控制淡入淡出
        public float fadeDuration = 0.3f;

        private AbstractCard currentCard;
        private Coroutine transitionCoroutine;

        public void ShowCard(AbstractCard newCard)
        {
            ShowWindow();
            // 如果当前显示的是同一张卡，忽略
            if (currentCard == newCard) return;

            // 如果面板正在播放动画，停止它
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);

            // 如果面板当前是激活状态，则先淡出，再更新内容并淡入
            if (gameObject.activeSelf && canvasGroup.alpha > 0.01f)
            {
                transitionCoroutine = StartCoroutine(FadeOutThenShowNew(newCard));
            }
            else
            {
                // 面板是关闭的，直接显示
                gameObject.SetActive(true);
                currentCard = newCard;
                UpdateContent(currentCard);
                transitionCoroutine = StartCoroutine(FadeIn());
            }
        }

        private IEnumerator FadeOutThenShowNew(AbstractCard newCard)
        {
            yield return FadeOut();
            currentCard = newCard;
            UpdateContent(currentCard);
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

        private void UpdateContent(AbstractCard card)
        {
            int childCount = simpleAspectParent.transform.childCount;
            Dictionary<string, int> aspects = new Dictionary<string, int>(card.aspectDictionary);
            // 对独一组增加对应的性相显示
            if (card.isUnique && card.uniqueNessGroup != null)
                aspects.Add(card.uniqueNessGroup, 1);
            totalParent.gameObject.SetActive(true);
            simpleAspectParent.SetAspectDictionaryContent(aspects);
            simpleAspectParent.gameObject.SetActive(true);
            cardHintGameobject.SetActive(card.isUnique);
            // 这里填写你的面板内容更新逻辑，如文本、图标等刷新
            labelText.text = card.label;
            descriptionText.text = card.lore;
            artworkIcon.sprite = card.icon;
        }

        public void Hide()
        {
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);
            currentCard = null;
            if (canvasGroup.alpha > 0)

                transitionCoroutine = StartCoroutine(FadeOutAndHide());
        }
        private IEnumerator FadeOutAndHide()
        {
            yield return FadeOut();
            //gameObject.SetActive(false);
            canvasGroup.alpha = 0f;
            currentCard = null;
        }


    }
}