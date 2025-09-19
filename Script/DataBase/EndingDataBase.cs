using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class EndingDataBase
    {
        public static AbstractEnding TryGetEnding(string key)
        {
            if (endingDataBase.ContainsKey(key))
                return endingDataBase[key];
            return endingDataBase["DefaultEnding"];
        }
        public static Dictionary<string, AbstractEnding> endingDataBase = new Dictionary<string, AbstractEnding>()
        {
            {
                "DefaultEnding",new AbstractEnding()
                {
                    index = 0,
                    stringIndex = "DefaultEnding",
                    label = "DefaultEnding",
                    lore = "This Game Have Reached End",
                    endingType = AbstractEnding.EndingType.Normal,
                    animType = AbstractEnding.AnimType.LightNormal, 
                }
            },
            {
                "Return",new AbstractEnding()
                {
                    index = 1,
                    stringIndex = "Return",
                    label = "归航",
                    lore = "残骸被一块块修复，零件在汗水与血迹中重获秩序。\r\n" +
                    "引擎轰鸣的那一刻，你几乎无法分辨耳边的声音是机器还是心跳。\r\n" +
                    "飞船挣脱了星球的引力，荒凉的大陆逐渐缩小为一抹灰点。\r\n" +
                    "母星的航道已在导航屏幕上点亮。\r\n" +
                    "你知道，这段孤独的旅程将被铭记——\r\n" +
                    "不是因为你曾坠落，而是因为你带着火种归来。",
                    endingType = AbstractEnding.EndingType.Normal,
                    animType = AbstractEnding.AnimType.LightNormal,
                }
            },
            {
                "BurnOut",new AbstractEnding()
                {
                    index = 2,
                    stringIndex = "BurnOut",
                    label = "湮灭",
                    lore = "你曾经拼尽全力，却依旧无法逃离这颗陌生的星球。\r\n " +
                    "饥饿、寒冷，或者是突如其来的伤病，最终击垮了你的意志。\r\n " +
                    "夜空依旧群星璀璨，却没有任何一颗属于你。\r\n " +
                    "残骸静静掩埋在风沙与植被之下，仿佛你从未存在过。\r\n " +
                    "只有散落的工具与破损的记录，见证你曾试图抵抗命运的挣扎。",
                    endingType = AbstractEnding.EndingType.Bad,
                    animType = AbstractEnding.AnimType.LightEvil,
                }
            }
        };
    }
}