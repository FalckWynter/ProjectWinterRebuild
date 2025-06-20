using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase
{
    public static AbstractCard TryGetCard(string key)
    {
        if (cardDataBase.ContainsKey(key))
            return cardDataBase[key].GetNewCopy();
        return cardDataBase["DefaultCard"].GetNewCopy();
    }
    public static Dictionary<string, AbstractCard> cardDataBase = new Dictionary<string, AbstractCard>()
    {
         {"DefaultCard",new AbstractCard(){
            index = 0,
            stringIndex = "DefaultCard",
            label = "ƒ¨»œø®≈∆" ,
            lore = "“ª’≈≤‚ ‘ø®≈∆",
            icon = ImageDataBase.TryGetImage("DefaultCard")
        }},
    };
}
