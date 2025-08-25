using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlentyFishFramework
{
    public class CardICanGetDragComponentMono : ICanGetDragComponentMono
    {
        // ���ƽ�����ק��Ʒ.
        // �����������ű�
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
            // Debug.Log("������������¼�");
            if (model == null || model.dragMonoList == null || model.dragMonoList.Count == 0)
                return;
            // ���ƽ�����קʱ������ק�б���ÿһ�����ƶ����м��
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
                // ����ǿ�������ͨ�����͵����۽���һ�ζѵ���������֤
                //if (item.GetComponent<CardMono>() != null)
                //    result =this.GetSystem<GameSystem>().StackCardToASlot(item.GetComponent<CardMono>(), cardMono.BelongtoSlotMono);
                // �����ݽ��������Ч
                //if (result)
                //    dragAudioMono.PlayGetStackAudio();
                //else
                //    dragAudioMono.PlayEndDragAudio();

            }

        }
    }
}