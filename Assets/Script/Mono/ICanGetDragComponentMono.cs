using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using QFramework;
using System.Linq;

public class ICanGetDragComponentMono : MonoBehaviour,IController, IDropHandler
{
    GameModel model;
    private readonly List<GameObject> containedObjects = new();

    // ��ȡ�ò۵�ǰ�����жѵ����
    public List<ICanBeStack> GetAllStackables()
    {
        return containedObjects
            .Select(obj => obj.GetComponent<ICanBeStack>())
            .Where(stack => stack != null)
            .ToList();
    }

    // ��������嵽����
    public void AddObject(GameObject obj)
    {
        containedObjects.Add(obj);
        //obj.transform.SetParent(this.transform); // �Ӿ�����

        // ���ø�����Ϊ�������
        obj.transform.SetParent(transform);

        // ��λ���������ģ�0,0��
        RectTransform rect = obj.transform as RectTransform;
        if (rect != null)
        {
            rect.anchoredPosition = Vector2.zero;
        }
        else
        {
            obj.transform.localPosition = Vector3.zero;
        }
    }

    private void Start()
    {
        model = this.GetModel<GameModel>();
    }
    // �������������̧��ʱ���Խ���������ק���е�����
    public IArchitecture GetArchitecture()
    {
        return ProjectWinterArchitecture.Interface;
    }
    public void RemoveDragListen(ICanDragComponentMono mono)
    {
        mono.onStartDrag.RemoveListener(RemoveDragListen);
        containedObjects.Remove(mono.gameObject);
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("���������¼�");
        if (model == null || model.dragMonoList == null)
            return;

        foreach (ICanDragComponentMono item in model.dragMonoList)
        {
            if (item == null) continue;
            GameObject droppedObject = item.gameObject;
            var droppedStack = droppedObject.GetComponent<ICanBeStack>();
            if (droppedStack == null)
            {
                Debug.LogWarning("�ϷŶ���֧�ֶѵ�");
                return;
            }

            var existingStacks = GetAllStackables();

            if (existingStacks.Count == 0)
            {
                item.onStartDrag.AddListener(RemoveDragListen);
                AddObject(droppedObject);
                return;
            }

            bool allAccepted = true;

            foreach (var existing in existingStacks)
            {
                if (!existing.IsCanBeStack(droppedStack))
                {
                    allAccepted = true;
                    break;
                }
            }

            if (allAccepted)
            {
                foreach (var existing in existingStacks)
                {
                    existing.TryAddStack(droppedStack,droppedObject);
                }
            }
            else
            {
                Debug.Log("�޷��ѵ����ò�λ�е����ж���");
            }

            //foreach (var dragMono in model.dragMonoList)
            //{
            //    if (dragMono == null) continue;
            //}
        }

        // ��ѡ�������ק�б�����߼���Ҫ��
        // model.dragMonoList.Clear();
    }

    //public virtual void OnPointerUp(PointerEventData eventData)
    //{
    //    var parts = model.dragMonoList;
    //    foreach(var item in parts)
    //    {
    //        item.transform.parent = this.transform;
    //        item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

    //    }
    //}


}
