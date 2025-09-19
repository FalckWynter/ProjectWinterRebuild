using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractElement
    {
        public int index;
        public string stringIndex, label, lore, comment;
        public string iconName { set { iconname = value; } get { if (iconname == "") return stringIndex; return iconname; } }
        private string iconname = "";
        public Sprite icon { set { artwork = value; } get { if (artwork == null) artwork = TryGetIcon(); return artwork; } }
        private Sprite artwork;

        public virtual Sprite TryGetIcon()
        {
            return ImageDataBase.TryGetImage(iconName);
        }
    }
}