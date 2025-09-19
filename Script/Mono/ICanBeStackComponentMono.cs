using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using QFramework;
using PlentyFishFramework;
//public class ICanBeStackComponentMono : MonoBehaviour
//{
//    public int StackCount { get; set; }

//    public int MaxStack { get; set; }

//    public bool IsCanBeStack(ICanBeStack newStacker)
//    {
//        return false;
//    }

//    public void TryAddStack()
//    {
        
//    }

//    public void TrySubStack()
//    {
        
//    }
//}
public interface ICanBeStack
{
    // 拥有这个接口的可以被其他要素堆叠
    public bool CanStackWith(ICanBeStack other);
    public bool TryAddStack(ICanBeStack other);
    public bool TrySubStack(ICanBeStack other);
    public void DestroySelf();

    public GameObject GetGameobject();

}
public interface ITableElement : ICanBeStack,ICanBelongToSlotMono
{
    // public TableElementMonoType tableElementMonoType { get; set; }
    // 桌面元素，即可以被卡槽记录，同时可以堆叠其他元素
}