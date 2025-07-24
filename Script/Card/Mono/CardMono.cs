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
        // 卡牌脚本组件

        // 抽象逻辑数据
        public AbstractCard card;
        // 区别在于 belongtoSlot会在开始拖动时被取消，而beforeSlot在拖动结束完成计算时才取消
        // 7.20 区别在于，belongtoSlot记录的是当前所属的卡槽，开始拖动时就取消
        // BeforeSlot记录上一个有效的任意卡槽，包括桌面网格
        // LastGridMono记录上一个桌面网格，以便于回到桌面上
        public SlotMono BelongtoSlotMono { get { return belongtoSlotMono; } set => belongtoSlotMono = value; }
        public SlotMono BeforeSlotMono { get => beforeSlotMono; set => beforeSlotMono = value; }
        public SlotMono LastGridMono { get => lastGridMono; set { lastGridMono = value; /*Debug.Log(gameObject.name + "设置为" + lastGridMono.gameObject.name); */} }
        private SlotMono belongtoSlotMono;
        private SlotMono beforeSlotMono;
        private SlotMono lastGridMono;

        // 对游戏系统和数据的订阅
        public GameModel model;
        public GameSystem gameSystem;
        //实体卡牌组件订阅
        public Image artwork;
        public TextMeshProUGUI label;
        public CardCounterMono cardCountMono;

        // 因为卡牌堆叠是和自己的Mono显示有关系的，所以脚本写在这里 后期可能考虑移走
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

        //借口实现逻辑
        // 减少一个堆叠计数
        public bool TrySubStack(ICanBeStack newStacker)
        {
            stackCount--;
            if (stackCount <= 0)
                Destroy(gameObject);
            return true;
        }
        // 是否能与某张卡拍进行堆叠
        public bool CanStackWith(ICanBeStack other)
        {
            //Debug.Log("能否进行堆叠");
            // 防止与自身堆叠
            if (ReferenceEquals(other, this))
                return false;
            if (other is CardMono cardmono)
            {
                if (cardmono.card.IsEqualTo(this.card))
                    return true;
            }
            //Debug.Log("不能进行堆叠");
            return false;
        }
        // 增加一个堆叠计数
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
        // 摧毁卡牌实体
        public void DestroySelf()
        {
            // 从拖拽列表中移除
            gameSystem.RemoveDragListen(this.GetComponent<ICanDragComponentMono>());
            // 解除卡槽订阅
            UnRegisterFromSlotMono();
            Destroy(gameObject);
        }
        // 从卡槽中移除订阅
        public void UnRegisterFromSlotMono()
        {
            if (BelongtoSlotMono != null)
                BelongtoSlotMono.slot.stackItemList.Remove(this);
            if (BeforeSlotMono != null)
                BeforeSlotMono.slot.stackItemList.Remove(this);
        }

        private void OnDestroy()
        {
            //Debug.Log("发起销毁");
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

        // 2.3节，修复从gridlayoutgroup中出现的形状偏移问题
        // 缓存卡牌原始状态
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