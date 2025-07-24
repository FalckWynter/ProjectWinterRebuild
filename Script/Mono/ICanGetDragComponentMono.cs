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
        // �̳к���Խ���������ק
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


        // �������������̧��ʱ���Խ���������ק���е�����
        public virtual IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
        public virtual void RemoveDragListen(ICanDragComponentMono mono)
        {
            // Debug.Log("�Ƴ�����");
            mono.onStartDrag.RemoveListener(RemoveDragListen);
        }
        public virtual void AddDragListen(ICanDragComponentMono mono)
        {
            // Debug.Log("��Ӷ���");
            mono.onStartDrag.AddListener(RemoveDragListen);
        }
        public virtual void OnDrop(PointerEventData eventData)
        {
            // ��д�˺�������ӽ��ܵ������Ĵ����߼�
            // Ĭ�϶�ȡ�б� ����Ӧ�øĳɣ�����г��ܸ��������Ϊ�ɸ��������װ���¼�
            // Debug.Log("������������¼�");
            if (model == null || model.dragMonoList == null)
                return;


        }



    }
}