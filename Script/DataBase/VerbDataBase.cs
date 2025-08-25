using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class VerbDataBase
    {
        // 行动框数据库
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
                    label = "默认行动框" ,
                    lore = "一个测试行动框",
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
                    label = "添加测试行动框" ,
                    lore = "一个测试新增事件的行动框",
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
                    label = "维生反应堆" ,
                    lore = "用于维持我在此处进行基本活动的重要设施。",
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
                    label = "飞船" ,
                    lore = "一处在星海缠流中仍能保持稳定的空间。",
                    //icon = ImageDataBase.TryGetVerbImage("addTestVerb"),
                    iconName = "work",
                    defaultSituationKey = "SpaceShipSituation",
                    verbSlotList = new List<AbstractSlot>()
                    {




                    }
                }
            }
        };
    }
}
