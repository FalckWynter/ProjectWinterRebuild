using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlentyFishFramework
{
    public class CardICanGetDragComponentMono : ICanGetDragComponentMono
    {
        public CardMono cardmono;
        public override void Start()
        {
            base.Start();
            cardmono = GetComponent<CardMono>();
        }
        public override void AddObject(ICanDragComponentMono mono)
        {
            // �Ժ��������Ե������Ѿ���OnDrop�д������
            // ��������û�п����ص��¼�������ֱ�ӵ��õ��ź���
            cardmono.TryAddStack(mono.GetComponent<ICanBeStack>());
        }
        public override bool IsCanGetDragObject(ICanDragComponentMono mono)
        {
            // �жϽ���Ķ����Ƿ���Է���
            // ��ͼ�����ֲ�λ��Ҫ��
            ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
            Debug.Log("���뿨�����ݽ׶�");
            if (cardmono.CanStackWith(stackmono) == false)
                return false;
            Debug.Log("���Է���");
            return true;
        }
        public override void OnDrop(PointerEventData eventData)
        {
            Debug.Log("������������¼�");
            if (model == null || model.dragMonoList == null)
            {
                return;
            }
            Debug.Log("Խ��ģ�ͼ��");

            foreach (ICanDragComponentMono item in model.dragMonoList)
            {
                Debug.Log("�������");
                if (item == null) continue;
                if (IsCanGetDragObject(item) == false)
                    continue;
                AddObject(item);
            }
        }
    }
}