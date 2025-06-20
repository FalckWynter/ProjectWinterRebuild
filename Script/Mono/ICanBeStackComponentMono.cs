using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using QFramework;
public class ICanBeStackComponentMono : MonoBehaviour
{
    public int StackCount { get; set; }

    public int MaxStack { get; set; }

    public bool IsCanBeStack(ICanBeStack newStacker)
    {
        return false;
    }

    public void TryAddStack()
    {
        
    }

    public void TrySubStack()
    {
        
    }
}
public interface ICanBeStack
{
    public bool CanStackWith(ICanBeStack other);
    public bool TryAddStack(ICanBeStack other);
    public bool TrySubStack(ICanBeStack other);
    public void DestroySelf();

}