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
        // 基础要素
        //public int index;
        //public string stringIndex, label, lore, comment;
        //public string iconName { set { iconname = value; } get { if (iconname == "") return stringIndex; return iconname; } }
        //private string iconname = "";
        //public Sprite icon { set { artwork = value; } get { if (artwork == null) artwork = ImageDataBase.TryGetVerbImage(iconName); return artwork; } }
        //private Sprite artwork;

        public string cardDropZoneID = "CardDropZone";

        // 行动框所属的实体脚本
        public VerbMono verbMono;

        // 2.1节增加
        // 行动框拥有的性相词典
        private Dictionary<string, int> verbAspectDictionary = new Dictionary<string, int>();
        //public List<string> verbRecipeGroup = new List<string>();
        //public Dictionary<string, int> cardAspectDictionary = new Dictionary<string, int>();
        // 性相词典接口 用于进行拓展
        public Dictionary<string, int> aspectDictionary
        {
            get => verbAspectDictionary;
            set
            {
                verbAspectDictionary = value;
            }
        }
        
        // 2.2节增加
        // 默认事件容器及其id索引
        public string defaultSituationKey = "";
        public AbstractSituation situation;
        // 行动框自带的卡槽、卡牌具有的卡槽及其总列表
        public List<AbstractSlot> verbSlotList = new List<AbstractSlot>();
        public List<AbstractSlot> cardSlotList = new List<AbstractSlot>();
        public List<AbstractSlot> slotList => verbSlotList.Concat(cardSlotList).ToList();

        // 2.2节增加，用于在verb更新时刷新mono订阅
        public UnityEvent<AbstractVerb, VerbExchangeReason> OnVerbDataChanged = new UnityEvent<AbstractVerb, VerbExchangeReason>();
        // ennum类型
        public enum VerbExchangeReason { PossibleRecipeExchange,RecipeFinished,RecipeStarted,All,AddRecipeText }
        // 2.3节增
        // 行动框开始后管理具有的卡牌
        public List<AbstractCard> verbCardList = new List<AbstractCard>();

        // 记录行动框正在执行的事件的卡槽 *直接建立对当前事件的引用
        public List<AbstractSlot> verbRecipeSlotList => situation.currentRecipe.recipeSlots;
        public override Sprite TryGetIcon()
        {
            return ImageDataBase.TryGetVerbImage(iconName);
        }
        public AbstractVerb GetNewCopy()
        {
            return GetNewCopy(this);
        }
        // 向词典修改要素数量，并自动移除无关要素
        public void AddAspect(string key,int value)
        {
            //Debug.Log("修改" + key + "值" + value);
            if (aspectDictionary.ContainsKey(key))
                aspectDictionary[key] += value;
            else
                aspectDictionary[key] = value;

            if (aspectDictionary[key] <= 0)
                aspectDictionary.Remove(key);
            if (verbMono != null)
            {
                //Debug.Log("设置脚本为需要更新");
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
            //Debug.Log("创建新的样本" + verb.basicSituationKey);
            // 独有参数
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
