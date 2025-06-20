using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace PlentyFishFramework
{
    public class VerbICanGetDragComponentMono : ICanGetDragComponentMono,IDropHandler
    {
        public VerbMono verbmono;
        public ICanGetDragPlayAudioComponentMono getDragAudioMono;
        public override void Start()
        {
            base.Start();
            verbmono = GetComponent<VerbMono>();
        }
        public override void AddObject(ICanDragComponentMono mono)
        {
            // �Ժ��������Ե������Ѿ���OnDrop�д������
            // ��������û�п����ص��¼�������ֱ�ӵ��õ��ź���
            // verbmono.TryAddStack(mono.GetComponent<ICanBeStack>());
            CardMono stackmono = mono.GetComponent<CardMono>();
            if(stackmono != null)
            {
                getDragAudioMono.PlayGetDragAudio();
                stackmono.DestroySelf();
            }
        }
        public override bool IsCanGetDragObject(ICanDragComponentMono mono)
        {
            // �жϽ���Ķ����Ƿ���Է���
            // ��ͼ�����ֲ�λ��Ҫ��
            ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
            Debug.Log("���뿨�����ݽ׶�");
            if (stackmono is CardMono cardmono)
                return true;
            Debug.Log("���Է���");
            return false;
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