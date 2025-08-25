using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractAspect : AbstractElement, ICopyAble<AbstractAspect>
    {
        public bool isVisible = true;
        public List<CardXTrigger> cardXtriggersList = new List<CardXTrigger>();

        public AbstractAspect GetNewCopy(AbstractAspect element)
        {
            //性相不存在复制的概念，只作为数据库存在
            return element;
        }

        public AbstractAspect GetNewCopy()
        {
            return GetNewCopy(this);
        }
    }
}
