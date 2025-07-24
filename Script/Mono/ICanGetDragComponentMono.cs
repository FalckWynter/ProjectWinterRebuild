using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace PlentyFishFramework
{
    public interface ICanGetDragComponent : IController, IDropHandler
    {
        // 继承后可以接受物体拖拽
        public void RemoveDragListen(ICanDragComponentMono mono);
        public void AddDragListen(ICanDragComponentMono mono);

    }

    public class ICanGetDragComponentMono : MonoBehaviour, ICanGetDragComponent,IController
    {
        public GameModel model;
        public GameSystem system;
        public virtual void Start()
        {
            model = this.GetModel<GameModel>();
            system = this.GetSystem<GameSystem>();
        }

        public List<ICanDragComponentMono> containedObjects = new();


        // 这个组件会在鼠标抬起时尝试接收所有拖拽框中的物体
        public virtual IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
        public virtual void RemoveDragListen(ICanDragComponentMono mono)
        {
            // Debug.Log("移除订阅");
            mono.onStartDrag.RemoveListener(RemoveDragListen);
        }
        public virtual void AddDragListen(ICanDragComponentMono mono)
        {
            // Debug.Log("添加订阅");
            mono.onStartDrag.AddListener(RemoveDragListen);
        }
        public virtual void OnDrop(PointerEventData eventData)
        {
            // 重写此函数以添加接受到物体后的处理逻辑
            // 默认读取列表 这里应该改成，如果有承受父物体则改为由父物体接受装载事件
            // Debug.Log("触发父类放下事件");
            if (model == null || model.dragMonoList == null)
                return;


        }



    }
}