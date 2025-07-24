using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using QFramework;
namespace PlentyFishFramework
{
    public class CardMono : MonoBehaviour, IController, ITableElement
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

        // ��Ϊ���ƶѵ��Ǻ��Լ���Mono��ʾ�й�ϵ�ģ����Խű�д������ ���ڿ��ܿ�������
        public int stackCount = 1;
        public int StackCount { get { return stackCount; } set { stackCount = value; cardCountMono.SetCount(stackCount); } }



        private void Start()
        {
            cardCountMono.SetCount(stackCount);
            gameSystem = this.GetSystem<GameSystem>();
            CacheOriginalRectTransform();
        }
        public void LoadCardData(AbstractCard card)
        {
            artwork.sprite = card.icon;
            label.text = card.label + card.createIndex;
            this.card = card;
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
                BelongtoSlotMono.slot.stackItemList.Remove(this);
            if (BeforeSlotMono != null)
                BeforeSlotMono.slot.stackItemList.Remove(this);
        }

        private void OnDestroy()
        {
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
    }
}