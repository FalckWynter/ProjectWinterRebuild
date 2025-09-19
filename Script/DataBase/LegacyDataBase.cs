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
                    isPermitLoad = false
                }
            },
            {
                "Guider", new AbstractLegacy()
                {
                    index = 1,
                    stringIndex = "Guider",
                    label = "向导",
                    description = "你曾在无数陌生的星球上带领探险队穿越荒原、沼泽与极寒之地。  \r\n如今，队伍已不复存在，唯有你自己在废墟与荒野间继续寻找出路。",
                    legacyUnlockedDescription = "你总是可以从这个身份开始游戏。",
                    startDescription = "空气稀薄，重力略显沉重。  \r\n你的仪表和地图早已损坏，但你对方向的直觉依然敏锐。  \r\n即便孤身一人，你也知道该如何找到水源、避开危险……至少暂时如此。",
                    startingVerbsIDList = new List<string>(){"LandingShip"},
                    //effects = new List<string>(){"BurnedStoryCard"},
                    initLegacyRecipe = new List<AbstractLegacy.LegacyInitGroup>()
                    {
                        new AbstractLegacy.LegacyInitGroup()
                        {
                            InitWithRecipeGroup = "LandingShipGroup",
                            InitWithRecipeKey = "LandingShipInit",
                            startVerbID = "LandingShip"
                        }
                    }
                }
            },
            {
                "Engineer", new AbstractLegacy()
                {
                    index = 2,
                    stringIndex = "Engineer",
                    label = "工程师",
                    description = "你曾在飞船的机舱深处度过无数个昼夜。  \r\n能源循环、推进系统、生命维持装置……没有你的维护，它们连一刻都无法正常运转。  \r\n如今，船已化为残骸，你必须靠双手重新拼凑生的可能。",
                    legacyUnlockedDescription = "你总是可以从这个身份开始游戏。",
                    startDescription = "残骸四散，电路火花在黑暗中闪烁。  \r\n你蹲下身，翻找出几件尚可利用的工具。  \r\n在这个星球，任何一块废金属都可能成为救命的部件。  \r\n你深吸一口气，开始打量周围……",
                    startingVerbsIDList = new List<string>(){"DefaultVerb"},
                    effects = new List<string>(){"BurnedStoryCard"},
                }
            },
            {
                "Scientist", new AbstractLegacy()
                {
                    index = 3,
                    stringIndex = "Scientist",
                    label = "科学家",
                    description = "你曾在研究站的无菌实验室里，探究未知的物质与数据。  \r\n你熟悉分析、记录、验证的方法，也明白规律往往隐藏在混乱之中。  \r\n如今，实验室早已化为灰烬，而你只能把智慧带入荒野。",
                    legacyUnlockedDescription = "你总是可以从这个身份开始游戏。",
                    startDescription = "陌生的空气充斥着未解的气息。  \r\n你拾起一个残破的扫描器，屏幕上闪烁的数值令你心生好奇。  \r\n或许，这个星球并非只有死亡和危险——只要你能解读它的秘密。",
                    startingVerbsIDList = new List<string>(){"DefaultVerb"},
                    effects = new List<string>(){"BurnedStoryCard"},
                }
            }

        };
    }
}