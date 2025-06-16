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
        Debug.Log("������������¼�");

        base.AddObject(mono);

        //���Է��� ����ɹ���ֱ�ӽ�������
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
                Debug.Log("�����");
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
        Debug.Log("�Ƴ�����");
       
    }
    public void PutItemIntoSlot(ICanDragComponentMono mono)
    {
        // ���ø�����Ϊ�������
        mono.transform.SetParent(transform);

        // ��λ���������ģ�0,0��
        RectTransform rect = mono.transform as RectTransform;
        if (rect != null)
            rect.anchoredPosition = Vector2.zero;
    }
    public override bool IsCanGetDragObject(ICanDragComponentMono mono)
    {
        Debug.Log("�ж��ܷ�ע��");
        ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
        if(stackItemList.Count < maxSlotItemCount)
        {
            if((stackmono is AbstractCard card))
            {
                return true;
            }
        }
        Debug.Log("���뿨�����ݽ׶�");
        foreach(ICanBeStack item in stackItemList)
        {
            if (item.CanStackWith(stackmono) == false)
                return false;
        }
        Debug.Log("���Է���");
        return true;
    }
    public override void OnDrop(PointerEventData eventData)
    {
        Debug.Log("������������¼�");
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
