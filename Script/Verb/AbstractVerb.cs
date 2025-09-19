using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlentyFishFramework
{
    public class AbstractVerb :AbstractElement
    {
        // ����Ҫ��
        //public int index;
        //public string stringIndex, label, lore, comment;
        //public string iconName { set { iconname = value; } get { if (iconname == "") return stringIndex; return iconname; } }
        //private string iconname = "";
        //public Sprite icon { set { artwork = value; } get { if (artwork == null) artwork = ImageDataBase.TryGetVerbImage(iconName); return artwork; } }
        //private Sprite artwork;

        public string cardDropZoneID = "CardDropZone";

        // �ж���������ʵ��ű�
        public VerbMono verbMono;

        // 2.1������
        // �ж���ӵ�е�����ʵ�
        private Dictionary<string, int> verbAspectDictionary = new Dictionary<string, int>();
        //public List<string> verbRecipeGroup = new List<string>();
        //public Dictionary<string, int> cardAspectDictionary = new Dictionary<string, int>();
        // ����ʵ�ӿ� ���ڽ�����չ
        public Dictionary<string, int> aspectDictionary
        {
            get => verbAspectDictionary;
            set
            {
                verbAspectDictionary = value;
            }
        }
        
        // 2.2������
        // Ĭ���¼���������id����
        public string defaultSituationKey = "";
        public AbstractSituation situation;
        // �ж����Դ��Ŀ��ۡ����ƾ��еĿ��ۼ������б�
        public List<AbstractSlot> verbSlotList = new List<AbstractSlot>();
        public List<AbstractSlot> cardSlotList = new List<AbstractSlot>();
        public List<AbstractSlot> slotList => verbSlotList.Concat(cardSlotList).ToList();

        // 2.2�����ӣ�������verb����ʱˢ��mono����
        public UnityEvent<AbstractVerb, VerbExchangeReason> OnVerbDataChanged = new UnityEvent<AbstractVerb, VerbExchangeReason>();
        // ennum����
        public enum VerbExchangeReason { PossibleRecipeExchange,RecipeFinished,RecipeStarted,All,AddRecipeText }
        // 2.3����
        // �ж���ʼ�������еĿ���
        public List<AbstractCard> verbCardList = new List<AbstractCard>();

        // ��¼�ж�������ִ�е��¼��Ŀ��� *ֱ�ӽ����Ե�ǰ�¼�������
        public List<AbstractSlot> verbRecipeSlotList => situation.currentRecipe.recipeSlots;
        public override Sprite TryGetIcon()
        {
            return ImageDataBase.TryGetVerbImage(iconName);
        }
        public AbstractVerb GetNewCopy()
        {
            return GetNewCopy(this);
        }
        // ��ʵ��޸�Ҫ�����������Զ��Ƴ��޹�Ҫ��
        public void AddAspect(string key,int value)
        {
            //Debug.Log("�޸�" + key + "ֵ" + value);
            if (aspectDictionary.ContainsKey(key))
                aspectDictionary[key] += value;
            else
                aspectDictionary[key] = value;

            if (aspectDictionary[key] <= 0)
                aspectDictionary.Remove(key);
            if (verbMono != null)
            {
                //Debug.Log("���ýű�Ϊ��Ҫ����");
                verbMono.isNeedRefresh = true;
            }
        }
        public AbstractVerb GetNewCopy(AbstractVerb verb)
        {
            AbstractVerb retVerb = new AbstractVerb();
            retVerb.index = verb.index;
            retVerb.stringIndex = verb.stringIndex;
            retVerb.label = verb.label;
            retVerb.lore = verb.lore;
            retVerb.icon = verb.icon;/* ImageDataBase.imageDataBase[verb.stringIndex];*/
            retVerb.defaultSituationKey = verb.defaultSituationKey;
            //Debug.Log("�����µ�����" + verb.basicSituationKey);
            // ���в���
            retVerb.situation = SituationDataBase.TryGetSituation(verb.defaultSituationKey).GetNewCopy();
            foreach (AbstractSlot slot in verbSlotList)
            {
                AbstractSlot slotcopy = slot.GetNewCopy();
                slotcopy.isVerbSlot = true;
                slotcopy.isSlot = true;
                slotcopy.verb = retVerb;
                retVerb.verbSlotList.Add(slotcopy);
            }
            //retVerb.verbRecipeGroup = new List<string>(verb.verbRecipeGroup);
            return retVerb;
        }

        //public Dictionary<string, int> GetCombinedAspectDictionary()
        //{
        //    Dictionary<string, int> result = new Dictionary<string, int>();

        //    foreach (var pair in verbAspectDictionary)
        //    {
        //        result[pair.Key] = pair.Value;
        //    }

        //    foreach (var pair in cardAspectDictionary)
        //    {
        //        if (result.ContainsKey(pair.Key))
        //            result[pair.Key] += pair.Value;
        //        else
        //            result[pair.Key] = pair.Value;
        //    }

        //    return result;
        //}
    }
}
