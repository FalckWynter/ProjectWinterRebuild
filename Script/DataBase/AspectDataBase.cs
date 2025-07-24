using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AspectDataBase
    {
        // 性相词典
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
            label = "默认性相" ,
            lore = "\"悬而未定的模棱两可。\"",
            icon = ImageDataBase.TryGetImage("DefaultAspect")
        }},
         {"TestAspect",new AbstractAspect(){
            index = 1,
            stringIndex = "TestAspect",
            label = "测试性相" ,
            lore = "被记叙之物在未刻历史中的过去。",
            icon = ImageDataBase.TryGetImage("TestImage1")
        }},
    };
    }
}