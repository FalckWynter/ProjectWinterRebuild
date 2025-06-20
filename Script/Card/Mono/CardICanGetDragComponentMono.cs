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
            // 对函数合理性的验算已经在OnDrop中处理过了
            // 由于现在没有卡牌重叠事件，所以直接调用叠放函数
            cardmono.TryAddStack(mono.GetComponent<ICanBeStack>());
        }
        public override bool IsCanGetDragObject(ICanDragComponentMono mono)
        {
            // 判断介入的东西是否可以放入
            // 意图是区分槽位内要素
            ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
            Debug.Log("进入卡牌内容阶段");
            if (cardmono.CanStackWith(stackmono) == false)
                return false;
            Debug.Log("可以放入");
            return true;
        }
        public override void OnDrop(PointerEventData eventData)
        {
            Debug.Log("触发子类放下事件");
            if (model == null || model.dragMonoList == null)
            {
                return;
            }
            Debug.Log("越过模型检测");

            foreach (ICanDragComponentMono item in model.dragMonoList)
            {
                Debug.Log("检测物体");
                if (item == null) continue;
                if (IsCanGetDragObject(item) == false)
                    continue;
                AddObject(item);
            }
        }
    }
}