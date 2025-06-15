using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardMono : MonoBehaviour, ICanBeStack
{
    public Image artwork;
    public TextMeshProUGUI lore;
    public AbstractCard card;
    public CardCounterMono cardCountMono;
    private int stackCount = 1;
    public int StackCount { set; get; }

    private int maxStack = -1;
    public int MaxStack { set; get; }
    private void Start()
    {
        cardCountMono.SetCount(stackCount);
    }
    public bool IsCanBeStack<T>(T newStacker) where T:ICanBeStack
    {
        if (newStacker is not AbstractCard newCard)
        {
            return false;
        }
        return true;
    }

    public void LoadCardData(AbstractCard card)
    {
        artwork.sprite = card.icon;
        lore.text = card.lore;
        this.card = card;
    }

    public void TryAddStack(ICanBeStack newStacker,GameObject ob)
    {
        stackCount = stackCount + 1;
        cardCountMono.SetCount(stackCount);
        GameObject.Destroy(ob);

    }

    public void TrySubStack(ICanBeStack newStacker,GameObject ob = null)
    {
        
    }
}
