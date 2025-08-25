using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class LegacyDataBase
    {
        public static AbstractLegacy TryGetLegacy(string key)
        {
            if (legacyDataBase.ContainsKey(key))
                return legacyDataBase[key];
            return legacyDataBase["DefaultLegacy"];
        }
        public static Dictionary<string, AbstractLegacy> legacyDataBase = new Dictionary<string, AbstractLegacy>()
        {
            {
                "DefaultLegacy", new AbstractLegacy()
                {
                    index = 0,
                    stringIndex = "DefaultLegacy",
                    description = "This is Default Legacy",
                    startDescription = "Started by default Legacy",
                }
            },
            {
                "Guider", new AbstractLegacy()
                {
                    index = 0,
                    stringIndex = "Guider",
                    description = "你曾在无数陌生的星球上带领探险队穿越荒原、沼泽与极寒之地。  \r\n如今，队伍已不复存在，唯有你自己在废墟与荒野间继续寻找出路。",
                    startDescription = "空气稀薄，重力略显沉重。  \r\n你的仪表和地图早已损坏，但你对方向的直觉依然敏锐。  \r\n即便孤身一人，你也知道该如何找到水源、避开危险……至少暂时如此。",
                    startingVerbsIDList = new List<string>(){"DefaultVerb"},
                    effects = new List<string>(){"BurnedStoryCard"},
                }
            },
        };
    }
}