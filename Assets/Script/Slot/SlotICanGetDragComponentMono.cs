using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace PlentyFishFramework
{
    public class SlotICanGetDragComponentMono : ICanGetDragComponentMono
    {
        public int maxSlotItemCount = 1;
        public List<ICanBeStack> stackItemList = new List<ICanBeStack>();
        public ICanGetDragPlayAudioComponentMono getDragAudioMono;
        public override void Start()
        {
            base.Start();
            getDragAudioMono = GetComponent<ICanGetDragPlayAudioComponentMono>();
        }
        public override void AddObject(ICanDragComponentMono mono)
        {
            // Debug.Log("������������¼�");


            //���Է��� ����ɹ���ֱ�ӽ�������
            ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
            if (stackItemList.Count < maxSlotItemCount)
            {
                base.AddObject(mono);
                PutItemIntoSlot(mono);
            }
        }
        public override void AddDragListen(ICanDragComponentMono mono)
        {
            base.AddDragListen(mono);
            ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
            stackItemList.Add(stackmono);
        }
        public override void RemoveDragListen(ICanDragComponentMono mono)
        {
            base.RemoveDragListen(mono);
            ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
            stackItemList.Remove(stackmono);
            // Debug.Log("�Ƴ�����");

        }
        public void PutItemIntoSlot(ICanDragComponentMono mono)
        {
            getDragAudioMono.PlayGetDragAudio();
            //// ���ø�����Ϊ�������
            //mono.transform.SetParent(transform);

            //// ��λ���������ģ�0,0��
            //RectTransform rect = mono.transform as RectTransform;
            //if (rect != null)
            //    rect.anchoredPosition = Vector2.zero;

            // mono �ĸ������� parent
            Transform parent = mono.transform.parent;

            // �� slot ���������������ת��Ϊ mono ���ڿռ�ľֲ�����
            Vector3 localPos = parent.InverseTransformPoint(transform.position);

            // ��ֵ�� mono �� RectTransform
            RectTransform rect = mono.transform as RectTransform;
            if (rect != null)
                rect.anchoredPosition = localPos;

            // mono.transform.SetParent(transform);

            // ��λ���������ģ�0,0��
            //RectTransform rect = mono.transform as RectTransform;
            //RectTransform parentRect = transform as RectTransform;
            //if (rect != null)
            //    rect.anchoredPosition = parentRect.anchoredPosition;
        }
        public override bool IsCanGetDragObject(ICanDragComponentMono mono)
        {
            // Debug.Log("�ж��ܷ�ע��");
            // ��ͼ�����ֲ�λ��Ҫ��
            ICanBeInSlot newmono = mono.GetComponent<ICanBeInSlot>();
            //ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
            if (stackItemList.Count < maxSlotItemCount)
            {
                    return true;
            }
            //VerbMono verbmono = mono.GetComponent<VerbMono>();
            //if (verbmono != null)
            //    return true;
            // ��һ��Ŀǰû��Ӱ�죬��Ϊ�������Ϊ1 ����������Ϊ��ʱ�Կ�����Ч
            // Debug.Log("���뿨�����ݽ׶�");
            //foreach(ICanBeStack item in stackItemList)
            //{
            //    if (item.CanStackWith(stackmono) == false)
            //        return false;
            //}
            // Debug.Log("���Է���");
            return false;
        }
        //public override void OnDrop(PointerEventData eventData)
        //{
        //    // Debug.Log("������������¼�");
        //    if (model == null || model.dragMonoList == null)
        //        return;

        //    foreach (ICanDragComponentMono item in model.dragMonoList)
        //    {
        //        if (item == null) continue;
        //        if (IsCanGetDragObject(item) == false)
        //            continue;
        //        AddObject(item);
        //    }
        //}
        public int viewer = 0;
        private void Update()
        {
            viewer = stackItemList.Count;
        }
    }
}