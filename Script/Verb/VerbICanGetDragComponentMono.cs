using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using QFramework;

namespace PlentyFishFramework
{
    public class VerbICanGetDragComponentMono : ICanGetDragComponentMono,IDropHandler
    {
        // 行动框接受拖拽物体组件脚本
        public VerbMono verbMono;
        public ICanDragPlayAudioComponentMono dragAudioMono;

        public override void Start()
        {
            base.Start();
            verbMono = GetComponent<VerbMono>();
            dragAudioMono = GetComponent<ICanDragPlayAudioComponentMono>();

        }


        public override void OnDrop(PointerEventData eventData)
        {
            // Debug.Log("触发父类放下事件");
            if (model == null || model.dragMonoList == null)
                return;

            for (int i = model.dragMonoList.Count - 1; i >= 0; i--)
            {
                ICanDragComponentMono item = model.dragMonoList[i];
                bool result = false;
                if (item.GetComponent<CardMono>() != null)
                    result = this.GetSystem<GameSystem>().MonoStackCardToSlot(item.GetComponent<CardMono>(), verbMono.BelongtoSlotMono,TableElementMonoType.Verb);
                if (item.GetComponent<VerbMono>() != null)
                    result = this.GetSystem<GameSystem>().MonoStackCardToSlot(item.GetComponent<VerbMono>(), verbMono.BelongtoSlotMono, TableElementMonoType.Verb);

                // 将卡牌分发到这个卡槽 实际上会产生尝试叠加的效果
                //if (item.GetComponent<CardMono>() != null)
                //    result = this.GetSystem<GameSystem>().StackCardToASlot(item.GetComponent<CardMono>(), verbMono.BelongtoSlotMono);
                //if (result)
                //    dragAudioMono.PlayGetStackAudio();
            }
        }
    }
}