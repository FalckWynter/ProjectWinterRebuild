using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class DeckDataBase 
    {
        static DeckDataBase()
        {
            foreach(var item in preloadDeckDataBase)
            {
                // 创建一个全新的副本用于调用
                deckDataBase.Add(item.Key,item.Value.GetNewCopy());
            }
        }
        public static AbstractDeck TryGetDeck(string key)
        {
            if (deckDataBase.ContainsKey(key))
                return deckDataBase[key];
            return deckDataBase["ErrorDeckID"];
        }

        // 在这里写卡组，因为这里的局内数据会产生修改，所以需要新建一组变量
        public static Dictionary<string, AbstractDeck> preloadDeckDataBase = new Dictionary<string, AbstractDeck>()
        {
            {"ErrorDeckID" , new AbstractDeck()
            {
                loadData = new Dictionary<string, int>()
                {
                    {"ErrorDeckIDCard",1 }
                },
                resetonexhaustion = true,
            }
            },
            {"TestDeck" , new AbstractDeck()
            {
                loadData = new Dictionary<string, int>()
                {
                    {"CooledStoryClip",1 }
                },
                defaultCardStringID = "StoryCard",
                resetonexhaustion = false,
            }
            },
        };


        // 这里作为运行时变量
        public static Dictionary<string, AbstractDeck> deckDataBase = new Dictionary<string, AbstractDeck>()
        {

        };

    }
}