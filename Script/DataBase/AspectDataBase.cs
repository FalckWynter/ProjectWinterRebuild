using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AspectDataBase
    {
        // ����ʵ�
        public static AbstractAspect TryGetAspect(string key)
        {
            if (aspectDataBase.ContainsKey(key))
                return aspectDataBase[key];
            return null;
        }
        public static Dictionary<string, AbstractAspect> aspectDataBase = new Dictionary<string, AbstractAspect>()
    {
         {"DefaultAspect",new AbstractAspect(){
            index = 0,
            stringIndex = "DefaultAspect",
            label = "Ĭ������" ,
            lore = "\"����δ����ģ�����ɡ�\"",
            icon = ImageDataBase.TryGetImage("DefaultAspect")
        }},
         {"TestAspect",new AbstractAspect(){
            index = 1,
            stringIndex = "TestAspect",
            label = "��������" ,
            lore = "������֮����δ����ʷ�еĹ�ȥ��",
            icon = ImageDataBase.TryGetImage("TestImage1")
        }},
    };
    }
}