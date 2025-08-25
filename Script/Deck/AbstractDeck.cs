using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
namespace PlentyFishFramework
{
    public class AbstractDeck : AbstractElement
    {
        public Dictionary<string, int> deck = new Dictionary<string, int>();
        public Dictionary<string, int> loadData = new Dictionary<string, int>();
        public string defaultCardStringID = "DrawNullDeckCard";
        public bool resetonexhaustion = false;
        public int cardDraws = 1;

        private static System.Random rng = new System.Random(); // �滻Ϊ��ʵ��ʹ�õ������ʵ��
        public AbstractDeck GetNewCopy()
        {
            AbstractDeck deck = new AbstractDeck();
            deck.index = this.index;
            deck.stringIndex = this.stringIndex;
            deck.label = this.label;
            deck.lore = this.lore;
            deck.comment = this.comment;
            deck.loadData = new Dictionary<string, int>(this.loadData);
            deck.deck = new Dictionary<string, int>(deck.loadData);
            deck.defaultCardStringID = this.defaultCardStringID;
            deck.resetonexhaustion = this.resetonexhaustion;
            deck.cardDraws = this.cardDraws;
            return deck;
        }
        // ��ȡһ�ſ���
        public string DrawCard()
        {
            EnsureDeckIsReady();

            if (deck.Count == 0)
                return defaultCardStringID;

            int total = deck.Values.Sum();
            int roll = rng.Next(total);
            int cumulative = 0;

            foreach (var kvp in deck)
            {
                cumulative += kvp.Value;
                if (roll < cumulative)
                {
                    string card = kvp.Key;

                    if (deck[card] == 1)
                        deck.Remove(card);
                    else
                        deck[card]--;

                    return card;
                }
            }

            return defaultCardStringID; // ���۲���ִ�е���
        }

        // ��ȡָ�������Ŀ���
        public List<string> DrawLotsCard(int count)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < count; i++)
                result.Add(DrawCard());
            return result;
        }

        // ��ȡĬ�������Ŀ���
        public List<string> DrawAutoCountCard()
        {
            return DrawLotsCard(cardDraws);
        }

        // ˽�з���������������ؿ���
        private void EnsureDeckIsReady()
        {
            if (deck.Count == 0)
            {
                if (resetonexhaustion && loadData.Count > 0)
                {
                    foreach (var kvp in loadData)
                        deck[kvp.Key] = kvp.Value;
                }
            }
        }
    }
}