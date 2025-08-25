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
            }
        };
    }
}