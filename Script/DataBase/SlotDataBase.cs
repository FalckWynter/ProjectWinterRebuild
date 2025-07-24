using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class SlotDataBase
    {
        // �������ݿ�
        public static AbstractSlot TryGetSlot(string key)
        {
            if (slotDataBase.ContainsKey(key))
                return slotDataBase[key];
            return slotDataBase["ErrorSlot"];
        }
        // ��������Ļ���verb��ǰ汾��
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
        // ��������Ļ���recipe��ǰ汾��
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
                    label = "Ĭ�Ͽ���" ,
                    lore = "Ĭ�Ͽ��۵�����",
                    icon = ImageDataBase.TryGetVerbImage("dream"),

                }
            },
            {
                "TestSlot",new AbstractSlot()
                {
                    index = 1,
                    stringIndex = "TestSlot",
                    label = "���Կ���" ,
                    lore = "����һ�����Կ���",
                    icon = ImageDataBase.TryGetVerbImage("dream"),

                }
            },
            {
                "ErrorSlot",new AbstractSlot()
                {
                    index = 2,
                    stringIndex = "ErrorSlot",
                    label = "���󿨲�" ,
                    lore = "�����Ƿ������˴���Ŀ��۴���",
                    icon = ImageDataBase.TryGetVerbImage("dream"),

                }
            },
            {
                "ShowTestSlot",new AbstractSlot()
                {
                    index = 3,
                    stringIndex = "ShowTestSlot",
                    label = "��ʾ���Կ���" ,
                    lore = "���ڲ��Կ���Я�����۵���ʾ������ȷ��",
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
                    label = "���ز��Կ���" ,
                    lore = "���ڲ��Կ���Я�����۵����ع�����ȷ��",
                    icon = ImageDataBase.TryGetVerbImage("dream"),

                }
            },
            {
                "RecipeBringSlot",new AbstractSlot()
                {
                    index = 5,
                    stringIndex = "RecipeBringSlot",
                    label = "�¼�Я������" ,
                    lore = "�����¼�ִ������ʾЯ�����۵Ĺ�����ȷ��",
                    icon = ImageDataBase.TryGetVerbImage("dream"),

                }
            },
        };
    }
}