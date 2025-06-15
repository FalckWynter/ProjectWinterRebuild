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

    // 获取该槽当前的所有堆叠组件
    public List<ICanBeStack> GetAllStackables()
    {
        return containedObjects
            .Select(obj => obj.GetComponent<ICanBeStack>())
            .Where(stack => stack != null)
            .ToList();
    }

    // 添加新物体到槽中
    public void AddObject(GameObject obj)
    {
        containedObjects.Add(obj);
        //obj.transform.SetParent(this.transform); // 视觉归属

        // 设置父物体为这个卡槽
        obj.transform.SetParent(transform);

        // 归位到卡槽中心（0,0）
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
    // 这个组件会在鼠标抬起时尝试接收所有拖拽框中的物体
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
        Debug.Log("触发放下事件");
        if (model == null || model.dragMonoList == null)
            return;

        foreach (ICanDragComponentMono item in model.dragMonoList)
        {
            if (item == null) continue;
            GameObject droppedObject = item.gameObject;
            var droppedStack = droppedObject.GetComponent<ICanBeStack>();
            if (droppedStack == null)
            {
                Debug.LogWarning("拖放对象不支持堆叠");
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
                Debug.Log("无法堆叠到该槽位中的所有对象");
            }

            //foreach (var dragMono in model.dragMonoList)
            //{
            //    if (dragMono == null) continue;
            //}
        }

        // 可选：清空拖拽列表（如果逻辑需要）
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
