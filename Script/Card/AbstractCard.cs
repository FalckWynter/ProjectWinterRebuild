using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractCard : ICopyAble<AbstractCard>, ICanBeEqualCompare<AbstractCard>
    {
        // ����Ҫ��
        public static int sortIndex = 1;
        public int index, createIndex;
        public string stringIndex, label, lore, comment;
        public string iconName { set { iconname = value; } get { if (iconname == "") return stringIndex; return iconname; } }
        private string iconname = "";
        public Sprite icon { set { artwork = value; } get { if (artwork == null) artwork = ImageDataBase.TryGetImage(iconName); return artwork; } }
        private Sprite artwork;
        // ���ƶ�Ӧ�Ľű�
        public CardMono cardMono;
        // ����Я��������
        public Dictionary<string, int> aspectDictionary = new Dictionary<string, int>();
        // ���ƴ��еĿ���
        public List<AbstractSlot> cardSlotList = new List<AbstractSlot>();
        // 2.3�ڼ��� ��ע����Ϊ�Է����������Ƴ�����
        public bool isDisposed = false;

        public AbstractCard()
        {
            createIndex = sortIndex;
            sortIndex++;
        }
        public AbstractCard GetNewCopy()
        {
            return GetNewCopy(this);
        }
        // ������
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
            return retCard;
        }
        public static AbstractCard CreateNewCopy(AbstractCard card)
        {
            return card.GetNewCopy();
        }
        // �����
        public bool IsEqualTo(AbstractCard other)
        {
            bool isEqual = true;
            // Debug.Log("�ȽϽ��" + (index == other.index));
            if (index != other.index) return false;
            if (!UtilSystem.AreDictionariesEqual(aspectDictionary, other.aspectDictionary))
                return false;

            return isEqual;
        }
    }
}