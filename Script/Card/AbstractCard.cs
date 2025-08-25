using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractCard : ICopyAble<AbstractCard>, ICanBeEqualCompare<AbstractCard>
    {
        // 基本要素
        public static int sortIndex = 1;
        public int index, createIndex;
        public string stringIndex, label, lore, comment;
        public string iconName { set { iconname = value; } get { if (iconname == "") return stringIndex; return iconname; } }
        private string iconname = "";
        public Sprite icon { set { artwork = value; } get { if (artwork == null) artwork = ImageDataBase.TryGetImage(iconName); return artwork; } }
        private Sprite artwork;
        // 卡牌对应的脚本
        public CardMono cardMono;
        // 卡牌携带的性相
        public Dictionary<string, int> aspectDictionary = new Dictionary<string, int>();
        // 卡牌带有的卡槽
        public List<AbstractSlot> cardSlotList = new List<AbstractSlot>();
        // 2.3节加入 标注卡牌为以废弃，便于移除物体，并且不会被加入事项检查
        public bool isDisposed = false;
        // 3.2.1.3节加入 决定卡牌是否会消逝
        public float lifeTime = 0;
        // 是否会随时间计算消逝计数器
        public bool isCanDecayByTime = false;
        // 消逝后会变成的卡牌
        public string decayToCardStringIndex;
        public string burnToCardStringIndex;
        // 消逝过程中是否会改变颜色 暂时不起效
        public bool isDecayColorWhenDecay = false;
        // 是否从燃烧卡槽进入行动框，为true时会被销毁(你不需要为这个变量赋值)
        public bool isInVerbsByConsumeSlot = false;

        // 3.2.1.4加入 独一组和独一ID
        // 是否为唯一卡牌
        public bool isUnique = false;
        public string uniqueNessGroup;

        // 3.2.2.3加入 卡牌的xtrigger触发器
        public List<CardXTrigger> cardXtriggersList = new List<CardXTrigger>();

        public AbstractCard()
        {
            createIndex = sortIndex;
            sortIndex++;
        }
        public AbstractCard GetNewCopy()
        {
            return GetNewCopy(this);
        }
        // 复制器
        public AbstractCard GetNewCopy(AbstractCard card)
        {
            AbstractCard retCard = new AbstractCard();
            retCard.index = card.index;
            retCard.stringIndex = card.stringIndex;
            retCard.label = card.label;
            retCard.lore = card.lore;
            if (card.icon == null)
                retCard.icon = ImageDataBase.TryGetImage(card.iconName);/* ImageDataBase.imageDataBase[card.stringIndex];*/
            else
                retCard.icon = card.icon;
            retCard.aspectDictionary = new Dictionary<string, int>(card.aspectDictionary);
            //retCard.cardSlotList = new List<AbstractSlot>(card.cardSlotList);
            foreach (AbstractSlot slot in card.cardSlotList)
                retCard.cardSlotList.Add(slot.GetNewCopy());
            retCard.isDisposed = card.isDisposed;
            retCard.lifeTime = card.lifeTime;
            retCard.isCanDecayByTime = card.isCanDecayByTime;
            retCard.decayToCardStringIndex = card.decayToCardStringIndex;
            retCard.burnToCardStringIndex = card.burnToCardStringIndex;
            retCard.isDecayColorWhenDecay = card.isDecayColorWhenDecay;
            retCard.isUnique = card.isUnique;
            retCard.uniqueNessGroup = card.uniqueNessGroup;
            retCard.cardXtriggersList = card.cardXtriggersList;
            return retCard;
        }
        public static AbstractCard CreateNewCopy(AbstractCard card)
        {
            return card.GetNewCopy();
        }
        // 相等器
        public bool IsEqualTo(AbstractCard other)
        {
            bool isEqual = true;
            // Debug.Log("比较结果" + (index == other.index));
            if (index != other.index) return false;
            if (!UtilSystem.AreDictionariesEqual(aspectDictionary, other.aspectDictionary))
                return false;

            return isEqual;
        }
    }
}