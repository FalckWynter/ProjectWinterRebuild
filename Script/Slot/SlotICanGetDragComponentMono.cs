using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
namespace PlentyFishFramework
{
    public class SlotICanGetDragComponentMono : ICanGetDragComponentMono
    {
        // ���۽�����ק���ݵĽű�
        public SlotMono slotMono;
        public ICanDragPlayAudioComponentMono dragAudioMono;
        // �����޸�Ϊ������ ��Ҫ����������slotmonoȥ�洢
        public override void Start()
        {
            base.Start();
            slotMono = this.GetComponent<SlotMono>();
            dragAudioMono = GetComponent<ICanDragPlayAudioComponentMono>();
        }

        public override void OnDrop(PointerEventData eventData)
        {
            if (model == null || model.dragMonoList == null)
                return;
            // Debug.Log("��������" + eventData.pointerDrag.gameObject.name);
            bool result = false;
            // �Կ��ƺ��ж�������������ʽ��з�װ
            if(eventData.pointerDrag.GetComponent<CardMono>() != null)
                result = this.GetSystem<GameSystem>().MonoStackCardToSlot(eventData.pointerDrag.GetComponent<CardMono>(), slotMono,TableElementMonoType.Slot);
            if (eventData.pointerDrag.GetComponent<VerbMono>() != null)
                result = this.GetSystem<GameSystem>().MonoStackCardToSlot(eventData.pointerDrag.GetComponent<VerbMono>(), slotMono, TableElementMonoType.Slot);

            //// �����ж� ���ǰ��ж��ƶ���������
            //if (slotMono.slot.isSlot)
            //        result = this.GetSystem<GameSystem>().MonoStackCardToSlot(eventData.pointerDrag.GetComponent<VerbMono>(), eventData.pointerDrag.GetComponent<VerbMono>().LastGridMono, TableElementMonoType.Slot);
            //    else
            //        result = this.GetSystem<GameSystem>().MonoStackCardToSlot(eventData.pointerDrag.GetComponent<VerbMono>(), slotMono,TableElementMonoType.Slot);

            //if(slotMono.slot.isSlot)
            //    result = this.GetSystem<GameSystem>().StackElementWithSlot(eventData.pointerDrag.GetComponent<CardMono>(),slotMono);
            //else
            //    result = this.GetSystem<GameSystem>().StackElementWithGrid(eventData.pointerDrag.GetComponent<CardMono>(), slotMono);
            //if (eventData.pointerDrag.GetComponent<VerbMono>() != null)
            //    if (slotMono.slot.isSlot)
            //        result = false;
            //    else
            //        result = this.GetSystem<GameSystem>().StackElementWithGrid(eventData.pointerDrag.GetComponent<VerbMono>(), slotMono);
            //if (result)
            //    dragAudioMono.PlayGetStackAudio();

        }

    }
}