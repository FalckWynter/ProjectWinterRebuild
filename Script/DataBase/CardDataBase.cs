using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class CardDataBase
    {
        // �������ݿ�
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
            lore = "һ��Ĭ�Ͽ���",
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
            lore = "һ�Ų��Կ���",
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
            lore = "��ҹ�����ǡ�",
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
            lore = "���²�Ƭ",
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
            lore = "ȼ���ġ�ҹ�����ǡ�",
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
            label = "����Ŀ���ID" ,
            lore = "�㳢�Դ�һ������Ŀ���ID�л�ȡ���ƣ�",
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
            label = "������û�п����ˣ�" ,
            lore = "�㳢�Դ�һ��û�п��ÿ��ƵĲ��ɲ��俨���г�ȡ���ƣ����㲢û������Ĭ�Ͽ��Ƶ�ID��",
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
            label = "Recipe�Զ��忨����Կ���" ,
            lore = "���ڲ���Recipe���Լ�����Ŀ����г�ȡ���ƵĲ��Կ��ƣ�",
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