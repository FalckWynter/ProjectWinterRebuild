using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using QFramework;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
namespace PlentyFishFramework
{
    public class CardMono : MonoBehaviour, IController, ITableElement, IPointerClickHandler
    {
        // ���ƽű����

        // �����߼�����
        public AbstractCard card;
        // �������� belongtoSlot���ڿ�ʼ�϶�ʱ��ȡ������beforeSlot���϶�������ɼ���ʱ��ȡ��
        // 7.20 �������ڣ�belongtoSlot��¼���ǵ�ǰ�����Ŀ��ۣ���ʼ�϶�ʱ��ȡ��
        // BeforeSlot��¼��һ����Ч�����⿨�ۣ�������������
        // LastGridMono��¼��һ�����������Ա��ڻص�������
        public SlotMono BelongtoSlotMono { get { return belongtoSlotMono; } set => belongtoSlotMono = value; }
        public SlotMono BeforeSlotMono { get => beforeSlotMono; set => beforeSlotMono = value; }
        public SlotMono LastGridMono { get => lastGridMono; set { lastGridMono = value; /*Debug.Log(gameObject.name + "����Ϊ" + lastGridMono.gameObject.name); */} }
        private SlotMono belongtoSlotMono;
        private SlotMono beforeSlotMono;
        private SlotMono lastGridMono;

        // ����Ϸϵͳ�����ݵĶ���
        public GameModel model;
        public GameSystem gameSystem;
        //ʵ�忨���������
        public Image artwork;
        public TextMeshProUGUI label;
        public CardCounterMono cardCountMono;
        public GameObject decayViewObject;
        public TextMeshProUGUI decayCount;

        // ��Ϊ���ƶѵ��Ǻ��Լ���Mono��ʾ�й�ϵ�ģ����Խű�д������ ���ڿ��ܿ�������
        public int stackCount = 1;
        public int StackCount { get { return stackCount; } set { stackCount = value; cardCountMono.SetCount(stackCount); } }

        public CardICanDragComponentMono cardICanDragComponentMono;

        private void Start()
        {
            cardCountMono.SetCount(stackCount);
            gameSystem = this.GetSystem<GameSystem>();
            CacheOriginalRectTransform();
            cardICanDragComponentMono = GetComponent<CardICanDragComponentMono>();

        }

        public void LoadCardData(AbstractCard card)
        {
            artwork.sprite = card.icon;
            label.text = card.label + card.createIndex;
            this.card = card;
            UpdateCardDecayView();
        }

        //���ʵ���߼�
        // ����һ���ѵ�����
        public bool TrySubStack(ICanBeStack newStacker)
        {
            stackCount--;
            if (stackCount <= 0)
                Destroy(gameObject);
            return true;
        }
        // �Ƿ�����ĳ�ſ��Ľ��жѵ�
        public bool CanStackWith(ICanBeStack other)
        {
            //Debug.Log("�ܷ���жѵ�");
            // ��ֹ������ѵ�
            if (ReferenceEquals(other, this))
                return false;
            if (other is CardMono cardmono)
            {
                if (cardmono.card.IsEqualTo(this.card))
                    return true;
            }
            //Debug.Log("���ܽ��жѵ�");
            return false;
        }
        // ����һ���ѵ�����
        public bool TryAddStack(ICanBeStack other)
        {
            if (other is CardMono cardmono)
            {
                stackCount = stackCount + cardmono.stackCount;
                cardCountMono.SetCount(stackCount);
                return true;
            }
            return false;
        }
        // �ݻٿ���ʵ��
        public void DestroySelf()
        {
            // ����ק�б����Ƴ�
            gameSystem.RemoveDragListen(this.GetComponent<ICanDragComponentMono>());
            // ������۶���
            UnRegisterFromSlotMono();
            Destroy(gameObject);
        }
        // �ӿ������Ƴ�����
        public void UnRegisterFromSlotMono()
        {
            if (BelongtoSlotMono != null)
            {
                gameSystem.UnRegisterStackElementFromSlot(this, this.BelongtoSlotMono);
                //BelongtoSlotMono.slot.stackItemList.Remove(this);
            }
            if (BeforeSlotMono != null)
                BeforeSlotMono.slot.stackItemList.Remove(this);
        }

        private void OnDestroy()
        {
            //card.isDisposed = true;

            this.GetModel<GameModel>().RemoveCardMonoFromLevelList(this);
            this.GetModel<GameModel>().RemoveCardMonoFromTableList(this);
            if (BelongtoSlotMono != null)
            {
                // 3.2.1.2 ����ʱ�����ԭ�п��۵�ռ��
                gameSystem.UnRegisterStackElementFromSlot(this, BelongtoSlotMono);

            }
            //Debug.Log("��������");
        }

        public GameObject GetGameobject()
        {
            if(this.gameObject != null)
            return this.gameObject;
            return null;
        }

        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }

        // 2.3�ڣ��޸���gridlayoutgroup�г��ֵ���״ƫ������
        // ���濨��ԭʼ״̬
        public Vector2 originalSizeDelta;
        //public Vector2 originalAnchoredPosition;
        public Vector2 originalAnchorMin;
        public Vector2 originalAnchorMax;
        public Vector2 originalPivot;
        public Vector3 originalScale;

        public void CacheOriginalRectTransform()
        {
            RectTransform rect = GetComponent<RectTransform>();
            originalSizeDelta = rect.sizeDelta;
            //originalAnchoredPosition = rect.anchoredPosition;
            originalAnchorMin = rect.anchorMin;
            originalAnchorMax = rect.anchorMax;
            originalPivot = rect.pivot;
            originalScale = rect.localScale;
        }

        public void RestoreOriginalRectTransform()
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.sizeDelta = originalSizeDelta;
            //rect.anchoredPosition = originalAnchoredPosition;
            rect.anchorMin = originalAnchorMin;
            rect.anchorMax = originalAnchorMax;
            rect.pivot = originalPivot;
            rect.localScale = originalScale;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("���");
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                //UtilModel.tokenDetailWindow.ShowWindowForCard(card);
                UtilSystem.ShowCard(card);
            }
        }
        public void UpdateCardDecayView()
        {
            if (card == null || card.isCanDecayByTime == false)
            {
                //Debug.Log("û�п���" );

                HideCardDecayView();
            }
            else
            {
                //Debug.Log("����ʣ��ʱ��" + card.lifeTime);
                ShowCardDecayView();
            }

        }
        public void ShowCardDecayView()
        {
            decayViewObject.SetActive(true);
            decayCount.text = card.lifeTime.ToString("F1");
        }
        public void HideCardDecayView()
        {
            decayViewObject.SetActive(false);
        }
        public void Update()
        {
            UpdateCardDecayView();
            //if (mono.BelongtoSlotMono != null)
            //    inspectorOb = mono.BelongtoSlotMono.gameObject;
            //else
            //    inspectorOb = null;
          //  Debug.Log("BelongtoSlotMono: " + (mono.BelongtoSlotMono != null) +
          //", Slot: " + (mono.BelongtoSlotMono != null ? (mono.BelongtoSlotMono.slot != null).ToString() : "null") +
          //", isGreedy: " + (mono.BelongtoSlotMono != null && mono.BelongtoSlotMono.slot != null ? mono.BelongtoSlotMono.slot.isGreedy.ToString() : "null") +
          //", isCanBeDrag: " + cardICanDragComponentMono.isCanBeDrag);

            //if (mono.BelongtoSlotMono != null && mono.BelongtoSlotMono.slot != null && mono.BelongtoSlotMono.slot.isGreedy && cardICanDragComponentMono.isCanBeDrag)
            //{
            //    Debug.Log("�ر���ק");
            //    cardICanDragComponentMono.isCanBeDrag = false;
            //    return;
            //}
        }
    }
}