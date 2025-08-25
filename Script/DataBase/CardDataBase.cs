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
            },
            cardSlotList = new List<AbstractSlot>()
            {
                SlotDataBase.TryGetSlot("StoryTestSlot"),
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
                SlotDataBase.TryGetSlot("HideTestSlot"),


            }
        }},
         {"StoryCard",new AbstractCard(){
            index = 2,
            stringIndex = "StoryCard",
            label = "StoryCard" ,
            lore = "《夜游漫记》",
            icon = ImageDataBase.TryGetImage("StoryCard"),
            decayToCardStringIndex = "CooledStoryClip",
            burnToCardStringIndex = "BurnedStoryCard",
            lifeTime = 15,
            isCanDecayByTime = true,
            aspectDictionary = new Dictionary<string, int>()
            {
                {"StoryAspect" , 3 }
            },
            cardSlotList = new List<AbstractSlot>()
            {


            }
        }},
        {"CooledStoryClip",new AbstractCard(){
            index = 3,
            stringIndex = "CooledStoryClip",
            label = "CooledStoryClip" ,
            lore = "故事残片",
            icon = ImageDataBase.TryGetImage("CooledStoryClip"),
            //isUnique = true,
            //uniqueNessGroup = "UsedStoryClip",
            aspectDictionary = new Dictionary<string, int>()
            {
                
            },
            cardSlotList = new List<AbstractSlot>()
            {


            }
        }},
        {"BurnedStoryCard",new AbstractCard(){
            index = 4,
            stringIndex = "BurnedStoryCard",
            label = "BurnedStoryCard" ,
            lore = "燃尽的《夜游漫记》",
            //isUnique = true,
            //uniqueNessGroup = "UsedStoryClip",
            icon = ImageDataBase.TryGetImage("BurnedStoryCard"),
            aspectDictionary = new Dictionary<string, int>()
            {
                {"Burned" , 1 }
            },
            cardSlotList = new List<AbstractSlot>()
            {


            },

            //cardXtriggersList = new List<RecipeChainTrigger>()
            //{
            //    new RecipeChainTrigger()
            //    {
            //        triggerNodes = new List<RecipeTriggerNode>()
            //        {
            //            new RecipeTriggerNode()
            //            {

            //            }
            //        }
            //    }
            //}
        }},
        {"ErrorDeckIDCard",new AbstractCard(){
            index = 5,
            stringIndex = "ErrorDeckIDCard",
            label = "错误的卡组ID" ,
            lore = "你尝试从一个错误的卡组ID中获取卡牌！",
            icon = ImageDataBase.TryGetImage("DefaultCard"),
            aspectDictionary = new Dictionary<string, int>()
            {
            },
            cardSlotList = new List<AbstractSlot>()
            {
            }
        }},
        {"DrawNullDeckCard",new AbstractCard(){
            index = 5,
            stringIndex = "DrawNullDeckCard",
            label = "卡组中没有卡牌了！" ,
            lore = "你尝试从一个没有可用卡牌的不可补充卡组中抽取卡牌，但你并没有设置默认卡牌的ID！",
            icon = ImageDataBase.TryGetImage("DefaultCard"),
            aspectDictionary = new Dictionary<string, int>()
            {

            },
            cardSlotList = new List<AbstractSlot>()
            {

            }
        }},
        {"DeckDrawTestCard",new AbstractCard(){
            index = 5,
            stringIndex = "DeckDrawTestCard",
            label = "Recipe自定义卡组测试卡牌" ,
            lore = "用于测试Recipe在自己定义的卡组中抽取卡牌的测试卡牌！",
            icon = ImageDataBase.TryGetImage("winter"),
            aspectDictionary = new Dictionary<string, int>()
            {

            },
            cardSlotList = new List<AbstractSlot>()
            {

            }
        }},
    };
    }
}