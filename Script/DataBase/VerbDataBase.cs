using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class VerbDataBase
    {
        public static AbstractVerb TryGetVerb(string key)
        {
            if (verbDataBase.ContainsKey(key))
                return verbDataBase[key].GetNewCopy();
            return verbDataBase["DefaultVerb"].GetNewCopy();
        }

        public static Dictionary<string, AbstractVerb> verbDataBase = new Dictionary<string, AbstractVerb>()
        {
            {
                "DefaultVerb",new AbstractVerb()
                {
                    index = 0,
                    stringIndex = "DefaultVerb",
                    label = "Ĭ���ж���" ,
                    lore = "һ�������ж���",
                    icon = ImageDataBase.TryGetVerbImage("dream")
                }
            }
        };
    }
}
