using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class SlotDataBase
    {
        // 卡槽数据库
        public static AbstractSlot TryGetSlot(string key)
        {
            if (slotDataBase.ContainsKey(key))
                return slotDataBase[key];
            return slotDataBase["ErrorSlot"];
        }
        // 这里产出的会是verb标记版本的
        public static AbstractSlot TryGetVerbSlot(string key)
        {
            AbstractSlot result;
            if (slotDataBase.ContainsKey(key))
                result = slotDataBase[key];
            else
                result = slotDataBase["ErrorSlot"];
            result.isSlot = true;
            return result;
        }
        // 这里产出的会是recipe标记版本的
        public static AbstractSlot TryGetRecipeSlot(string key)
        {
            AbstractSlot result;
            if (slotDataBase.ContainsKey(key))
                result = slotDataBase[key];
            else
                result = slotDataBase["ErrorSlot"];
            result.isRecipeSlot = true;
            return result;
        }

        public static Dictionary<string, AbstractSlot> slotDataBase = new Dictionary<string, AbstractSlot>()
        {
            {
                "DefaultSlot",new AbstractSlot()
                {
                    index = 0,
                    stringIndex = "DefaultSlot",
                    label = "默认卡槽" ,
                    lore = "默认卡槽的描述",
                    icon = ImageDataBase.TryGetVerbImage("dream"),

                }
            },
            {
                "TestSlot",new AbstractSlot()
                {
                    index = 1,
                    stringIndex = "TestSlot",
                    label = "测试卡槽" ,
                    lore = "这是一个测试卡槽",
                    icon = ImageDataBase.TryGetVerbImage("dream"),

                }
            },
            {
                "ErrorSlot",new AbstractSlot()
                {
                    index = 2,
                    stringIndex = "ErrorSlot",
                    label = "错误卡槽" ,
                    lore = "请检查是否输入了错误的卡槽代码",
                    icon = ImageDataBase.TryGetVerbImage("dream"),

                }
            },
            {
                "ShowTestSlot",new AbstractSlot()
                {
                    index = 3,
                    stringIndex = "ShowTestSlot",
                    label = "显示测试卡槽" ,
                    lore = "用于测试卡牌携带卡槽的显示功能正确性",
                    icon = ImageDataBase.TryGetVerbImage("dream"),
                    slotPossibleShowVerbList = new List<string>()
                    {
                        "All"
                    }

                }
            },
            {
                "HideTestSlot",new AbstractSlot()
                {
                    index = 4,
                    stringIndex = "HideTestSlot",
                    label = "隐藏测试卡槽" ,
                    lore = "用于测试卡牌携带卡槽的隐藏功能正确性",
                    icon = ImageDataBase.TryGetVerbImage("dream"),

                }
            },
            {
                "RecipeBringSlot",new AbstractSlot()
                {
                    index = 5,
                    stringIndex = "RecipeBringSlot",
                    label = "事件携带卡槽" ,
                    lore = "用于事件执行中显示携带卡槽的功能正确性",
                    icon = ImageDataBase.TryGetVerbImage("dream"),

                }
            },
        };
    }
}