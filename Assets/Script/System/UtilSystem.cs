using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using PlentyFishFramework;
namespace PlentyFishFramework
{
    public class UtilSystem : AbstractSystem
    {
        public GameObject cardParent;
        protected override void OnInit()
        {
            cardParent = GameObject.Find("MainCanvas/CardParent");
        }

        public GameObject CreateCardGameObject(AbstractCard card)
        {
            AbstractCard newCard = card.GetNewCopy();
            GameObject cardPrefab = PrefabDataBase.TryGetPrefab("CardPrefab");
            cardPrefab = GameObject.Instantiate(cardPrefab);
            CardMono mono = cardPrefab.GetComponent<CardMono>();
            mono.LoadCardData(newCard);
            //cardPrefab.transform.parent = cardParent.transform;
            cardPrefab.transform.SetParent(cardParent.transform, false);
            mono.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            return cardPrefab;

        }
        public GameObject CreateVerbGameObject(AbstractVerb element)
        {
            AbstractVerb newElement = element.GetNewCopy();
            GameObject elementPrefab = PrefabDataBase.TryGetPrefab("VerbPrefab");
            elementPrefab = GameObject.Instantiate(elementPrefab);
            VerbMono mono = elementPrefab.GetComponent<VerbMono>();
            mono.LoadVerbData(newElement);
            //cardPrefab.transform.parent = cardParent.transform;
            elementPrefab.transform.SetParent(cardParent.transform, false);
            mono.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            return elementPrefab;

        }

    }
}