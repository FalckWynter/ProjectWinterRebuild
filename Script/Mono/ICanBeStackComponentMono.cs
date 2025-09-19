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
    // ӵ������ӿڵĿ��Ա�����Ҫ�ضѵ�
    public bool CanStackWith(ICanBeStack other);
    public bool TryAddStack(ICanBeStack other);
    public bool TrySubStack(ICanBeStack other);
    public void DestroySelf();

    public GameObject GetGameobject();

}
public interface ITableElement : ICanBeStack,ICanBelongToSlotMono
{
    // public TableElementMonoType tableElementMonoType { get; set; }
    // ����Ԫ�أ������Ա����ۼ�¼��ͬʱ���Զѵ�����Ԫ��
}