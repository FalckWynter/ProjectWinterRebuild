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
            // 对函数合理性的验算已经在OnDrop中处理过了
            // 由于现在没有卡牌重叠事件，所以直接调用叠放函数
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
            // 判断介入的东西是否可以放入
            // 意图是区分槽位内要素
            ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
            Debug.Log("进入卡牌内容阶段");
            if (stackmono is CardMono cardmono)
                return true;
            Debug.Log("可以放入");
            return false;
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