using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class CardDataBase
    {
        // 卡牌数据库
        public static AbstractCard TryGetCard(string key)
        {
            if (cardDataBase.ContainsKey(key))
                return cardDataBase[key].GetNewCopy();
            return cardDataBase["DefaultCard"].GetNewCopy();
        }
        public static Dictionary<string, AbstractCard> cardDataBase = new Dictionary<string, AbstractCard>()
    {
         {"DefaultCard",new AbstractCard(){
            index = 0,
            stringIndex = "DefaultCard",
            label = "Default" ,
            lore = "一张默认卡牌",
            icon = ImageDataBase.TryGetImage("DefaultCard"),
            aspectDictionary = new Dictionary<string, int>()
            {
                {"DefaultAspect" , 1 }
            }
        }},
         {"TestCard",new AbstractCard(){
            index = 1,
            stringIndex = "TestCard",
            label = "Test" ,
            lore = "一张测试卡牌",
            icon = ImageDataBase.TryGetImage("TestImage1"),
            aspectDictionary = new Dictionary<string, int>()
            {
                {"TestAspect" , 2 }
            },
            cardSlotList = new List<AbstractSlot>()
            {
                SlotDataBase.TryGetSlot("ShowTestSlot"),
                SlotDataBase.TryGetSlot("HideTestSlot")

            }
        }},
    };
    }
}