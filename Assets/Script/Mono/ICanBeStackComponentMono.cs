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
    public int StackCount { set; get; }
    public int MaxStack { set; get; }
    public bool IsCanBeStack<T>(T newStacker) where T:ICanBeStack;
    public void TryAddStack(ICanBeStack newStacker,GameObject ob = null);
    public void TrySubStack(ICanBeStack newStacker,GameObject ob = null);
}