using QFramework;
using SecretHistories.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlentyFishFramework
{
    public class CardICanDragComponentMono : ICanDragComponentMono, IPointerEnterHandler, IPointerExitHandler
    {
        // ������ק����ű�
        public CardMono mono;
        // ��Ч��Ⱦ�ű�
        public GraphicFader glowFader;
        // ��ק��Ч�ű�
        public ICanDragPlayAudioComponentMono dragAudioMono;
        public override void Start()
        {
            base.Start();
            // ��ȡ��ע��ű���
            mono = GetComponent<CardMono>();
            dragAudioMono = GetComponent<ICanDragPlayAudioComponentMono>();
            onStartDrag.AddListener(dragAudioMono.PlayStartDragAudio);
            onEndDrag.AddListener(dragAudioMono.PlayEndDragAudio);
            // ������Ч�¼�
            glowFader = transform.Find("Glow").GetComponent<GraphicFader>();

        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            // ��֧�ж� ��Ϊ��������������1ʱ���Ϊ��ק�����Ƶĸ���
            // Debug.Log("��ʼ��ק" + textIndex);
            if (mono.stackCount == 1)
            {
                mono.RestoreOriginalRectTransform();
                // ���踸���嵽������ק������
                transform.SetParent(UtilSystem.dragParent.transform);
                transform.SetAsLastSibling(); // �����ŵ�ͬ�� UI �е����ϲ�
                // ����ʼ��ק�¼��Խ����������������Ķ���
                onStartDrag.Invoke(this);
                // ���������¼��Լ�¼�������
                OnPointerDown(eventData);
                // ��������������������Զ���
                if (mono.BelongtoSlotMono != null)
                gameSystem.UnRegisterStackElementFromSlot(mono, mono.BelongtoSlotMono);
                // OnPointerDown(eventData);
            }
            else
            {
                // ����һ������������϶�
                gameSystem.CreateCardCopyToBeDrag(mono, eventData);

            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // ������ɫΪ����ɫ������
            glowFader.SetColor(UIStyle.hoverWhite);
            glowFader.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // ��ɫ�ȱ�Ϊ��ɫ
            glowFader.SetColor(UIStyle.brightPink);
            // Ȼ����
            glowFader.Hide();
        }
    }
}