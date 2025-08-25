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
        // 2.3�ڼ��� ��ע����Ϊ�Է����������Ƴ����壬���Ҳ��ᱻ����������
        public bool isDisposed = false;
        // 3.2.1.3�ڼ��� ���������Ƿ������
        public float lifeTime = 0;
        // �Ƿ����ʱ��������ż�����
        public bool isCanDecayByTime = false;
        // ���ź���ɵĿ���
        public string decayToCardStringIndex;
        public string burnToCardStringIndex;
        // ���Ź������Ƿ��ı���ɫ ��ʱ����Ч
        public bool isDecayColorWhenDecay = false;
        // �Ƿ��ȼ�տ��۽����ж���Ϊtrueʱ�ᱻ����(�㲻��ҪΪ���������ֵ)
        public bool isInVerbsByConsumeSlot = false;

        // 3.2.1.4���� ��һ��Ͷ�һID
        // �Ƿ�ΪΨһ����
        public bool isUnique = false;
        public string uniqueNessGroup;

        // 3.2.2.3���� ���Ƶ�xtrigger������
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