using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlentyFishFramework;
public interface ICopyAble<T>
{
    abstract T GetNewCopy();
}
public interface ICanBeEqualCompare<T>
{
    abstract bool IsEqualTo(T other);
}
public interface ICanBeInSlot
{
    public SlotICanGetDragComponentMono slotParent { set; get; }
    abstract bool IsInSlot();

}