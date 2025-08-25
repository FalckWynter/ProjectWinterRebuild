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
            //���಻���ڸ��Ƶĸ��ֻ��Ϊ���ݿ����
            return element;
        }

        public AbstractAspect GetNewCopy()
        {
            return GetNewCopy(this);
        }
    }
}
