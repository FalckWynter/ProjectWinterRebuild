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

        // 3.1.2.2�� ��������ҳ�� ����
        // չʾ���ƺͿ��۵�����1
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
        //PointerEventData pointerData = new PointerEventData(eventSystem)
        //{
        //    position = Input.mousePosition
        //};

        //List<RaycastResult> raycastResults = new List<RaycastResult>();
        //uiRaycaster.Raycast(pointerData, raycastResults);

        //// ����������Ƿ��� detailPanel ����������
        //bool clickedOnPanel = raycastResults.Any(r => r.gameObject.transform.IsChildOf(gameObject.transform));
        //foreach(var item in raycastResults)
        //Debug.Log("�������" + item.gameObject.transform.name);
        //Debug.Log("���" + clickedOnPanel);

        //if (!clickedOnPanel)
        //{
        //    totalParent.SetActive(false); // ���ߵ������Լ��Ĺرպ���
        //}
        //HideWindow();
        void CloseDetailPanel()
        {
            if (totalParent != null)
            {
                totalParent.SetActive(false);
            }
        }

        // �ж�����Ƿ������κ�UIԪ����
        bool IsPointerOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //if (Input.GetMouseButtonDown(1)) // 1 ���Ҽ�
            //{
            //    //CloseDetailPanel();
            //    Hide();
            //}
        }
        // 3.1.2.3 �������뵭�� ����
        public CanvasGroup canvasGroup; // ���Ƶ��뵭��
        public float fadeDuration = 0.3f;

        private AbstractCard currentCard;
        private Coroutine transitionCoroutine;

        public void ShowCard(AbstractCard newCard)
        {
            ShowWindow();
            // �����ǰ��ʾ����ͬһ�ſ�������
            if (currentCard == newCard) return;

            // ���������ڲ��Ŷ�����ֹͣ��
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);

            // �����嵱ǰ�Ǽ���״̬�����ȵ������ٸ������ݲ�����
            if (gameObject.activeSelf && canvasGroup.alpha > 0.01f)
            {
                transitionCoroutine = StartCoroutine(FadeOutThenShowNew(newCard));
            }
            else
            {
                // ����ǹرյģ�ֱ����ʾ
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
            // �Զ�һ�����Ӷ�Ӧ��������ʾ
            if (card.isUnique && card.uniqueNessGroup != null)
                aspects.Add(card.uniqueNessGroup, 1);
            totalParent.gameObject.SetActive(true);
            simpleAspectParent.SetAspectDictionaryContent(aspects);
            simpleAspectParent.gameObject.SetActive(true);
            cardHintGameobject.SetActive(card.isUnique);
            // ������д���������ݸ����߼������ı���ͼ���ˢ��
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