using QFramework;
using SecretHistories.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlentyFishFramework
{
    public class CardICanDragComponentMono : ICanDragComponentMono, IPointerEnterHandler, IPointerExitHandler,IDragHandler
    {
        // ������ק����ű�
        public CardMono mono;
        // ��Ч��Ⱦ�ű�
        public GraphicFader glowFader;
        // ��ק��Ч�ű�
        public ICanDragPlayAudioComponentMono dragAudioMono; 
        public GameObject inspectorOb;
        public GameModel gameModel;
        public void Update()
        {
            //if (mono.BelongtoSlotMono != null)
            //    inspectorOb = mono.BelongtoSlotMono.gameObject;
            //else
            //    inspectorOb = null;
          //  Debug.Log("BelongtoSlotMono: " + (mono.BelongtoSlotMono != null) +
          //", Slot: " + (mono.BelongtoSlotMono != null ? (mono.BelongtoSlotMono.slot != null).ToString() : "null") +
          //", isGreedy: " + (mono.BelongtoSlotMono != null && mono.BelongtoSlotMono.slot != null ? mono.BelongtoSlotMono.slot.isGreedy.ToString() : "null") +
          //", isCanBeDrag: " + isCanBeDrag + "��������" + mono.belongtoSlotMono.slot.label);

            //if (mono.BelongtoSlotMono != null && mono.BelongtoSlotMono.slot != null && mono.BelongtoSlotMono.slot.isGreedy && isCanBeDrag)
            //{
            //    isCanBeDrag = false;
            //    return;
            //}
        }
        public override void Start()
        {
            base.Start();
            // ��ȡ��ע��ű���
            mono = GetComponent<CardMono>();
            dragAudioMono = GetComponent<ICanDragPlayAudioComponentMono>();
            onStartDrag.AddListener(dragAudioMono.PlayStartDragAudio);
            //onEndDrag.AddListener(dragAudioMono.PlayEndDragAudio);
            // ������Ч�¼�
            glowFader = transform.Find("Glow").GetComponent<GraphicFader>();
            gameModel = this.GetModel<GameModel>();

        }
        public override void OnBeginDrag(PointerEventData eventData)
        {

            if (isCanBeDrag == false) return;
            if (mono.BelongtoSlotMono != null && mono.BelongtoSlotMono.slot != null && mono.BelongtoSlotMono.slot.isGreedy)
            {
                //isCanBeDragInSlot = false;
                if (eventData.pointerDrag == gameObject)
                {
                    eventData.pointerDrag = null;
                }
                return;
            }

            // isCanBeDragInSlot = true;
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
        //public override void OnDrag(PointerEventData eventData)
        //{
        //    base.OnDrag(eventData);
        //    if (isCanBeDrag == false) return;
        //    if (isCanBeDragInSlot == false) return;
        //    if (isDragging)
        //    {
        //        gameModel.AddCardMonoToTableList(mono);
        //    }
        //}
        //public override void OnEndDrag(PointerEventData eventData)
        //{
        //    base.OnEndDrag(eventData);
        //    if (isCanBeDrag == false) return;
        //    if (isCanBeDragInSlot == false) return;

        //}
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (isCanBeDrag == false) return;
            base.OnPointerEnter(eventData);
            Debug.Log("����������ɫ");
            // ������ɫΪ����ɫ������
            glowFader.SetColor(UIStyle.hoverWhite);
            glowFader.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // if (isCanBeDrag == false) return;
            Debug.Log("���Թر���ɫ");
            // ��ɫ�ȱ�Ϊ��ɫ
            glowFader.SetColor(UIStyle.hoverWhite);
            // Ȼ����
            glowFader.Hide();
        }
    }
}