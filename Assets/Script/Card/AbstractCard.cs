using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractCard : ICopyAble<AbstractCard>
{
    public int index;
    public string stringIndex, label, lore,comment;
    public string iconName { set { iconname = value; } get { if (iconname == "") return stringIndex; return iconname; } }
    private string iconname = "";
    public Sprite icon
    {
        set { cardImage = value; }
        get
        {
            if (cardImage == null)
            {
                cardImage = ImageDataBase.TryGetImage(iconName);
            }
            return cardImage;
        }
    }
    private Sprite cardImage;

    public AbstractCard GetNewCopy()
    {
        return GetNewCopy(this);
    }
    public AbstractCard GetNewCopy(AbstractCard card)
    {
        AbstractCard retCard = new AbstractCard();
        retCard.index = card.index;
        retCard.stringIndex = card.stringIndex;
        retCard.label = card.label;
        retCard.lore = card.lore;
        retCard.icon = ImageDataBase.TryGetImage(card.iconName);/* ImageDataBase.imageDataBase[card.stringIndex];*/
        return retCard;
    }
    public static AbstractCard CreateNewCopy(AbstractCard card)
    {
        AbstractCard retCard = new AbstractCard();
        retCard.index = card.index;
        retCard.stringIndex = card.stringIndex;
        retCard.label = card.label;
        retCard.lore = card.lore;
        retCard.icon = ImageDataBase.TryGetImage(card.iconName);
        return retCard;
    }
}
