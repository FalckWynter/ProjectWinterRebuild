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
                        SlotDataBase.TryGetSlot("TestSlot")

                    }
                }
            }
        };
    }
}
