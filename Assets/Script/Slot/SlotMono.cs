using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotMono : ICanGetDragComponentMono
{
    public int maxSlotItemCount = 1;
    public List<ICanBeStack> stackItemList = new List<ICanBeStack>();
    public override void AddObject(ICanDragComponentMono mono)
    {
        Debug.Log("触发子类放入事件");

        base.AddObject(mono);

        //尝试放入 如果成功则直接结束放入
        ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
        if (stackItemList.Count < maxSlotItemCount)
        {
            stackItemList.Add(stackmono);
            PutItemIntoSlot(mono);
        }
        else
        {
            foreach (ICanBeStack item in stackItemList)
            {
                Debug.Log("检测结果");
                if (item.TryAddStack(stackmono))
                {
                    stackmono.DestroySelf();
                    break;
                }
            }
        }

    }
    public override void RemoveDragListen(ICanDragComponentMono mono)
    {
        base.RemoveDragListen(mono);
        ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
        stackItemList.Remove(stackmono);
        Debug.Log("移除订阅");
       
    }
    public void PutItemIntoSlot(ICanDragComponentMono mono)
    {
        // 设置父物体为这个卡槽
        mono.transform.SetParent(transform);

        // 归位到卡槽中心（0,0）
        RectTransform rect = mono.transform as RectTransform;
        if (rect != null)
            rect.anchoredPosition = Vector2.zero;
    }
    public override bool IsCanGetDragObject(ICanDragComponentMono mono)
    {
        Debug.Log("判断能否注入");
        ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
        if(stackItemList.Count < maxSlotItemCount)
        {
            if((stackmono is AbstractCard card))
            {
                return true;
            }
        }
        Debug.Log("进入卡牌内容阶段");
        foreach(ICanBeStack item in stackItemList)
        {
            if (item.CanStackWith(stackmono) == false)
                return false;
        }
        Debug.Log("可以放入");
        return true;
    }
    public override void OnDrop(PointerEventData eventData)
    {
        Debug.Log("触发子类放下事件");
        if (model == null || model.dragMonoList == null)
            return;

        foreach (ICanDragComponentMono item in model.dragMonoList)
        {
            if (item == null) continue;
            if (IsCanGetDragObject(item) == false)
                continue;
            AddObject(item);
        }
    }
    public int viewer = 0;
    private void Update()
    {
        viewer = stackItemList.Count;
    }
}
