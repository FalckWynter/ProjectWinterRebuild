using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class BasicElementMono : MonoBehaviour
    {
        public AbstractElement manifest;
        // �������� belongtoSlot���ڿ�ʼ�϶�ʱ��ȡ������beforeSlot���϶�������ɼ���ʱ��ȡ��
        // 7.20 �������ڣ�belongtoSlot��¼���ǵ�ǰ�����Ŀ��ۣ���ʼ�϶�ʱ��ȡ��
        // BeforeSlot��¼��һ����Ч�����⿨�ۣ�������������
        // LastGridMono��¼��һ�����������Ա��ڻص�������
        public SlotMono BelongtoSlotMono { get { return belongtoSlotMono; } set => belongtoSlotMono = value; }
        public SlotMono BeforeSlotMono { get => beforeSlotMono; set => beforeSlotMono = value; }
        public SlotMono LastGridMono { get => lastGridMono; set { lastGridMono = value; /*Debug.Log(gameObject.name + "����Ϊ" + lastGridMono.gameObject.name); */} }
        private SlotMono belongtoSlotMono;
        private SlotMono beforeSlotMono;
        private SlotMono lastGridMono;
    }
}