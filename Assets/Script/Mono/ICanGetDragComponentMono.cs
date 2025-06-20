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

        // ��������嵽����
        public virtual void AddObject(ICanDragComponentMono obj)
        {
            // Debug.Log("������������¼�");
            containedObjects.Add(obj);
            AddDragListen(obj);
        }

        // �������������̧��ʱ���Խ���������ק���е�����
        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
        public virtual void RemoveDragListen(ICanDragComponentMono mono)
        {
            // Debug.Log("�Ƴ�����");
            containedObjects.Remove(mono);
            mono.onStartDrag.RemoveListener(RemoveDragListen);
        }
        public virtual void AddDragListen(ICanDragComponentMono mono)
        {
            // Debug.Log("��Ӷ���");
            mono.onStartDrag.AddListener(RemoveDragListen);
            containedObjects.Add(mono);
        }
        public virtual void OnDrop(PointerEventData eventData)
        {
            // Debug.Log("������������¼�");
            if (model == null || model.dragMonoList == null)
                return;

            foreach (ICanDragComponentMono item in model.dragMonoList)
            {
                if (item == null) continue;
                if (IsCanGetDragObject(item) == false)
                    continue;
                // Debug.Log("׼���������");
                AddObject(item);
            }


        }

        public virtual bool IsCanGetDragObject(ICanDragComponentMono mono)
        {
            return false;
        }
    }
}