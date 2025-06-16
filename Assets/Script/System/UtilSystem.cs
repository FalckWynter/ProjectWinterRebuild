using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
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
        cardPrefab.transform.parent = cardParent.transform;
        mono.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        return cardPrefab;

    }

}
