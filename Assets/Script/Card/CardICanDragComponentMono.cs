using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardICanDragComponentMono : ICanDragComponentMono
{
    public CardMono mono;
    public int textIndex = 1;
    public override void Start()
    {
        base.Start();
        mono = GetComponent<CardMono>();
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {

        Debug.Log("开始拖拽" + textIndex);
        if (mono.stackCount == 1)
        {
            Debug.Log("执行初始化函数" + textIndex);
            onStartDrag.Invoke(this);
            OnPointerDown(eventData);

        }
        else
        {

            GameObject ob = this.GetSystem<UtilSystem>().CreateCardGameObject(mono.card);
            CardICanDragComponentMono newMono = ob.GetComponent<CardICanDragComponentMono>();
            newMono.transform.position = this.transform.position;
            newMono.Start();
            newMono.textIndex++;
            newMono.transform.SetAsLastSibling();
            StartCoroutine(TransferDragToCopyNextFrame(newMono, eventData));

            mono.StackCount--;
            eventData.pointerDrag = null;
            //newMono.onEndDrag.AddListener(ResetDragState);
            //isCanBeDrag = false;

        }
    }


    IEnumerator TransferDragToCopyNextFrame(CardICanDragComponentMono copy, PointerEventData eventData)
    {
        yield return null; // 等待一帧，确保EventSystem释放鼠标控制权

        eventData.pointerDrag = copy.gameObject;
        // 强制将副本注册为拖拽目标
        EventSystem.current.SetSelectedGameObject(copy.gameObject);

        // 手动调用副本的拖拽开始逻辑
        ExecuteEvents.Execute(copy.gameObject, eventData, ExecuteEvents.beginDragHandler);
        ExecuteEvents.Execute(copy.gameObject, eventData, ExecuteEvents.dragHandler);

    }
    public void ResetDragState(ICanDragComponentMono mono)
    {
        mono.onEndDrag.RemoveListener(ResetDragState);
        isCanBeDrag = true;
    }
}
