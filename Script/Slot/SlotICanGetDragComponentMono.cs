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
        // 卡槽接收拖拽内容的脚本
        public SlotMono slotMono;
        public ICanDragPlayAudioComponentMono dragAudioMono;
        // 这里修改为发起者 主要变量都交给slotmono去存储
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
            // Debug.Log("物体名称" + eventData.pointerDrag.gameObject.name);
            bool result = false;
            // 对卡牌和行动框根据自身性质进行分装
            if(eventData.pointerDrag.GetComponent<CardMono>() != null)
                result = this.GetSystem<GameSystem>().MonoStackCardToSlot(eventData.pointerDrag.GetComponent<CardMono>(), slotMono,TableElementMonoType.Slot);
            if (eventData.pointerDrag.GetComponent<VerbMono>() != null)
                result = this.GetSystem<GameSystem>().MonoStackCardToSlot(eventData.pointerDrag.GetComponent<VerbMono>(), slotMono, TableElementMonoType.Slot);

            //// 跳过判定 考虑把判定移动到函数里
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