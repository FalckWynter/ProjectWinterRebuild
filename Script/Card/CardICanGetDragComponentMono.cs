using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlentyFishFramework
{
    public class CardICanGetDragComponentMono : ICanGetDragComponentMono
    {
        // 卡牌接受拖拽物品.
        // 订阅相关组件脚本
        public CardMono cardMono;
        public ICanDragComponentMono dragMono;
        public ICanDragPlayAudioComponentMono dragAudioMono;
        public override void Start()
        {
            base.Start();
            dragMono = GetComponent<ICanDragComponentMono>();
            cardMono = GetComponent<CardMono>();
            dragAudioMono = GetComponent<ICanDragPlayAudioComponentMono>();
        }

        public override void OnDrop(PointerEventData eventData)
        {
            // Debug.Log("触发父类放下事件");
            if (model == null || model.dragMonoList == null || model.dragMonoList.Count == 0)
                return;
            // 卡牌接收拖拽时会检查拖拽列表，对每一个卡牌都进行检查
            for (int i = model.dragMonoList.Count - 1;i >= 0;i --)
            {
                ICanDragComponentMono item = model.dragMonoList[i];
                if(cardMono.BelongtoSlotMono == null)
                {
                    this.GetSystem<GameSystem>().MoveCardToClosestNullGrid(item.GetComponent<ITableElement>(), item.GetComponent<ITableElement>().BelongtoSlotMono);
                }
                bool result = false;
                if (item.GetComponent<CardMono>() != null)
                    result = this.GetSystem<GameSystem>().MonoStackCardToSlot(item.GetComponent<CardMono>(), cardMono.BelongtoSlotMono,TableElementMonoType.Card);
                if (item.GetComponent<VerbMono>() != null)
                    result = this.GetSystem<GameSystem>().MonoStackCardToSlot(item.GetComponent<VerbMono>(), cardMono.BelongtoSlotMono, TableElementMonoType.Verb);
                // 如果是卡牌则尝试通过发送到卡槽进行一次堆叠来进行验证
                //if (item.GetComponent<CardMono>() != null)
                //    result =this.GetSystem<GameSystem>().StackCardToASlot(item.GetComponent<CardMono>(), cardMono.BelongtoSlotMono);
                // 并根据结果播放音效
                //if (result)
                //    dragAudioMono.PlayGetStackAudio();
                //else
                //    dragAudioMono.PlayEndDragAudio();

            }

        }
    }
}