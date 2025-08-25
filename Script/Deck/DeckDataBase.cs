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
                // ����һ��ȫ�µĸ������ڵ���
                deckDataBase.Add(item.Key,item.Value.GetNewCopy());
            }
        }
        public static AbstractDeck TryGetDeck(string key)
        {
            if (deckDataBase.ContainsKey(key))
                return deckDataBase[key];
            return deckDataBase["ErrorDeckID"];
        }

        // ������д���飬��Ϊ����ľ������ݻ�����޸ģ�������Ҫ�½�һ�����
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


        // ������Ϊ����ʱ����
        public static Dictionary<string, AbstractDeck> deckDataBase = new Dictionary<string, AbstractDeck>()
        {

        };

    }
}