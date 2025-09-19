using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class VerbDataBase
    {
        // �ж������ݿ�
        public static AbstractVerb TryGetVerb(string key)
        {
            if (verbDataBase.ContainsKey(key))
                return verbDataBase[key];
            return verbDataBase["DefaultVerb"];
        }

        public static Dictionary<string, AbstractVerb> verbDataBase = new Dictionary<string, AbstractVerb>()
        {
            {
                "DefaultVerb",new AbstractVerb()
                {
                    index = 0,
                    stringIndex = "DefaultVerb",
                    label = "Ĭ���ж���" ,
                    lore = "һ�������ж���",
                    icon = ImageDataBase.TryGetVerbImage("dream"),

                    defaultSituationKey = "TestSituation",
                    verbSlotList = new List<AbstractSlot>()
                    {
                        SlotDataBase.TryGetSlot("DefaultSlot"),
                        SlotDataBase.TryGetSlot("TestSlot"),
                        //SlotDataBase.TryGetSlot("GreedyTestSlot"),
                        SlotDataBase.TryGetSlot("ConsumeTestSlot"),

                    }
                }
            },
            {
                "AddTestVerb",new AbstractVerb()
                {
                    index = 1,
                    stringIndex = "AddTestVerb",
                    label = "��Ӳ����ж���" ,
                    lore = "һ�����������¼����ж���",
                    icon = ImageDataBase.TryGetVerbImage("addTestVerb"),

                    defaultSituationKey = "TestSituation",
                    verbSlotList = new List<AbstractSlot>()
                    {
                        SlotDataBase.TryGetSlot("DefaultSlot"),
                        SlotDataBase.TryGetSlot("TestSlot"),



                    }
                }
            },
            {
                "LifeSupportCore",new AbstractVerb()
                {
                    index = 2,
                    stringIndex = "LifeSupportCore",
                    label = "ά����Ӧ��" ,
                    lore = "����ά�����ڴ˴����л��������Ҫ��ʩ��",
                    //icon = ImageDataBase.TryGetVerbImage("addTestVerb"),
                    iconName = "orangetime",
                    defaultSituationKey = "LifeCoreSituation",
                    verbSlotList = new List<AbstractSlot>()
                    {




                    }
                }
            },
            {
                "SpaceShip",new AbstractVerb()
                {
                    index = 3,
                    stringIndex = "SpaceShip",
                    label = "�ɴ�" ,
                    lore = "һ�����Ǻ����������ܱ����ȶ��Ŀռ䡣",
                    //icon = ImageDataBase.TryGetVerbImage("addTestVerb"),
                    iconName = "work",
                    defaultSituationKey = "SpaceShipSituation",
                    verbSlotList = new List<AbstractSlot>()
                    {




                    }
                }
            },
            {
                "LandingShip",new AbstractVerb()
                {
                    index = 4,
                    stringIndex = "LandingShip",
                    label = "������" ,
                    lore = "��������ģ�顣���������ʮ��Сʱ���ҵĹ�ȥ�������뽫����",
                    //icon = ImageDataBase.TryGetVerbImage("addTestVerb"),
                    iconName = "moon",
                    defaultSituationKey = "LandingShipSituation",
                    verbSlotList = new List<AbstractSlot>()
                    {
                        SlotDataBase.TryGetSlot("LandingShipSlotUser"),
                        SlotDataBase.TryGetSlot("LandingShipSlotTool"),



                    }
                }
            },
        };
    }
}
