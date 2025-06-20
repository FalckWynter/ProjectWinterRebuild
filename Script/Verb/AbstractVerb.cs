using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractVerb
    {
        public int index;
        public string stringIndex, label, lore, comment;
        public string iconName { set { iconname = value; } get { if (iconname == "") return stringIndex; return iconname; } }
        private string iconname = "";
        public Sprite icon { set { artwork = value; } get { if (artwork == null) artwork = ImageDataBase.TryGetVerbImage(iconName); return artwork; } }
        private Sprite artwork;

        public AbstractVerb GetNewCopy()
        {
            return GetNewCopy(this);
        }
        public AbstractVerb GetNewCopy(AbstractVerb verb)
        {
            AbstractVerb retVerb = new AbstractVerb();
            retVerb.index = verb.index;
            retVerb.stringIndex = verb.stringIndex;
            retVerb.label = verb.label;
            retVerb.lore = verb.lore;
            retVerb.icon = verb.icon;/* ImageDataBase.imageDataBase[verb.stringIndex];*/
            return retVerb;
        }
    }
}
