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
    public class SlotDetailsWindowMono : MonoBehaviour, IPointerClickHandler
    {

        // 3.1.2.2节 卡槽描述页面 加入
        // 展示卡牌和卡槽的性相1
        public GameObject totalParent;
        public AspectLineParentMono requireAspectParent, forbiddenAspectParent, essentialAspectParent, simpleAspectParent;
        public TextMeshProUGUI labelText, descriptionText;
        public Image artworkIcon;
        public LayoutElement layoutElement;
        public GameObject slotHintGameobject,slotGreedyHintGameobject,slotConsumeHintGameobject;

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
        public void ShowWindowForCard(AbstractSlot slot)
        {


            ShowSlot(slot);

        }
        public void SetAspectParentContent(Dictionary<string,int> dictionary)
        {

           
        }
        private void Awake()
        {
            UtilModel.slotDetailWindow = this;
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

        private AbstractSlot currentSlot;
        private Coroutine transitionCoroutine;
        public void ShowSlot(AbstractSlot newSlot)
        {
            ShowWindow();
            // 如果当前显示的是同一张卡，忽略
            if (currentSlot == newSlot) return;

            // 如果面板正在播放动画，停止它
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);

            // 如果面板当前是激活状态，则先淡出，再更新内容并淡入
            if (gameObject.activeSelf && canvasGroup.alpha > 0.01f)
            {
                transitionCoroutine = StartCoroutine(FadeOutThenShowNew(newSlot));
            }
            else
            {
                // 面板是关闭的，直接显示
                gameObject.SetActive(true);
                currentSlot = newSlot;
                UpdateContent(currentSlot);
                transitionCoroutine = StartCoroutine(FadeIn());
            }
            slotHintGameobject.SetActive(false);
            slotGreedyHintGameobject.SetActive(false);
            slotConsumeHintGameobject.SetActive(false);

            if (newSlot.isGreedy)
                slotGreedyHintGameobject.SetActive(true);
            if (newSlot.isConsumes)
                slotConsumeHintGameobject.SetActive(true);
            if(newSlot.isConsumes || newSlot.isGreedy)
            {
                slotHintGameobject.SetActive(true);
            }
        }

        private IEnumerator FadeOutThenShowNew(AbstractSlot newSlot)
        {
            yield return FadeOut();
            currentSlot = newSlot;
            UpdateContent(currentSlot);
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

        private void UpdateContent(AbstractSlot slot)
        {
            //int childCount = simpleAspectParent.transform.childCount;
            //if (slot.requipredAspectsDictionary.Count > 0)
                requireAspectParent.SetAspectDictionaryContent(slot.requipredAspectsDictionary);
            //if (slot.forbiddenAspectsDictionary.Count > 0)
                forbiddenAspectParent.SetAspectDictionaryContent(slot.forbiddenAspectsDictionary);
            //if (slot.essentialAspectsDictionary.Count > 0)
                essentialAspectParent.SetAspectDictionaryContent(slot.essentialAspectsDictionary);
            //Dictionary<string, int> aspects = slot.aspectDictionary;
            //totalParent.gameObject.SetActive(true);
            //simpleAspectParent.SetAspectDictionaryContent(aspects);
            //simpleAspectParent.gameObject.SetActive(true);
            // 这里填写你的面板内容更新逻辑，如文本、图标等刷新
            labelText.text = slot.label;
            descriptionText.text = slot.lore;
            artworkIcon.sprite = slot.icon;
        }

        public void Hide()
        {
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);
            currentSlot = null;
            if (canvasGroup.alpha > 0)

                transitionCoroutine = StartCoroutine(FadeOutAndHide());
        }
        private IEnumerator FadeOutAndHide()
        {
            yield return FadeOut();
            //gameObject.SetActive(false);
            canvasGroup.alpha = 0f;
            currentSlot = null;
        }

    }
}