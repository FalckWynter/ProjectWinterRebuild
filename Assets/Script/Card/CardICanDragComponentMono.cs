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

        Debug.Log("��ʼ��ק" + textIndex);
        if (mono.stackCount == 1)
        {
            Debug.Log("ִ�г�ʼ������" + textIndex);
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
        yield return null; // �ȴ�һ֡��ȷ��EventSystem�ͷ�������Ȩ

        eventData.pointerDrag = copy.gameObject;
        // ǿ�ƽ�����ע��Ϊ��קĿ��
        EventSystem.current.SetSelectedGameObject(copy.gameObject);

        // �ֶ����ø�������ק��ʼ�߼�
        ExecuteEvents.Execute(copy.gameObject, eventData, ExecuteEvents.beginDragHandler);
        ExecuteEvents.Execute(copy.gameObject, eventData, ExecuteEvents.dragHandler);

    }
    public void ResetDragState(ICanDragComponentMono mono)
    {
        mono.onEndDrag.RemoveListener(ResetDragState);
        isCanBeDrag = true;
    }
}
