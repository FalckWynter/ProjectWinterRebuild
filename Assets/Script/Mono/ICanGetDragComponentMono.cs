using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using QFramework;
using System.Linq;
namespace PlentyFishFramework
{
    public interface ICanGetDragComponent : IController, IDropHandler
    {
        public void AddObject(ICanDragComponentMono ob);
        public bool IsCanGetDragObject(ICanDragComponentMono mono);
        public void RemoveDragListen(ICanDragComponentMono mono);
        public void AddDragListen(ICanDragComponentMono mono);

    }

    public class ICanGetDragComponentMono : MonoBehaviour, ICanGetDragComponent
    {
        public GameModel model;
        public virtual void Start()
        {
            model = this.GetModel<GameModel>();
        }

        private List<ICanDragComponentMono> containedObjects = new();

        // 添加新物体到槽中
        public virtual void AddObject(ICanDragComponentMono obj)
        {
            // Debug.Log("触发父类放入事件");
            containedObjects.Add(obj);
            AddDragListen(obj);
        }

        // 这个组件会在鼠标抬起时尝试接收所有拖拽框中的物体
        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
        public virtual void RemoveDragListen(ICanDragComponentMono mono)
        {
            // Debug.Log("移除订阅");
            containedObjects.Remove(mono);
            mono.onStartDrag.RemoveListener(RemoveDragListen);
        }
        public virtual void AddDragListen(ICanDragComponentMono mono)
        {
            // Debug.Log("添加订阅");
            mono.onStartDrag.AddListener(RemoveDragListen);
            containedObjects.Add(mono);
        }
        public virtual void OnDrop(PointerEventData eventData)
        {
            // Debug.Log("触发父类放下事件");
            if (model == null || model.dragMonoList == null)
                return;

            foreach (ICanDragComponentMono item in model.dragMonoList)
            {
                if (item == null) continue;
                if (IsCanGetDragObject(item) == false)
                    continue;
                // Debug.Log("准备进行添加");
                AddObject(item);
            }


        }

        public virtual bool IsCanGetDragObject(ICanDragComponentMono mono)
        {
            return false;
        }
    }
}