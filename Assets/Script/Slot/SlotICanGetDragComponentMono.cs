using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace PlentyFishFramework
{
    public class SlotICanGetDragComponentMono : ICanGetDragComponentMono
    {
        public int maxSlotItemCount = 1;
        public List<ICanBeStack> stackItemList = new List<ICanBeStack>();
        public ICanGetDragPlayAudioComponentMono getDragAudioMono;
        public override void Start()
        {
            base.Start();
            getDragAudioMono = GetComponent<ICanGetDragPlayAudioComponentMono>();
        }
        public override void AddObject(ICanDragComponentMono mono)
        {
            // Debug.Log("触发子类放入事件");


            //尝试放入 如果成功则直接结束放入
            ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
            if (stackItemList.Count < maxSlotItemCount)
            {
                base.AddObject(mono);
                PutItemIntoSlot(mono);
            }
        }
        public override void AddDragListen(ICanDragComponentMono mono)
        {
            base.AddDragListen(mono);
            ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
            stackItemList.Add(stackmono);
        }
        public override void RemoveDragListen(ICanDragComponentMono mono)
        {
            base.RemoveDragListen(mono);
            ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
            stackItemList.Remove(stackmono);
            // Debug.Log("移除订阅");

        }
        public void PutItemIntoSlot(ICanDragComponentMono mono)
        {
            getDragAudioMono.PlayGetDragAudio();
            //// 设置父物体为这个卡槽
            //mono.transform.SetParent(transform);

            //// 归位到卡槽中心（0,0）
            //RectTransform rect = mono.transform as RectTransform;
            //if (rect != null)
            //    rect.anchoredPosition = Vector2.zero;

            // mono 的父物体是 parent
            Transform parent = mono.transform.parent;

            // 将 slot 的坐标从世界坐标转换为 mono 所在空间的局部坐标
            Vector3 localPos = parent.InverseTransformPoint(transform.position);

            // 赋值给 mono 的 RectTransform
            RectTransform rect = mono.transform as RectTransform;
            if (rect != null)
                rect.anchoredPosition = localPos;

            // mono.transform.SetParent(transform);

            // 归位到卡槽中心（0,0）
            //RectTransform rect = mono.transform as RectTransform;
            //RectTransform parentRect = transform as RectTransform;
            //if (rect != null)
            //    rect.anchoredPosition = parentRect.anchoredPosition;
        }
        public override bool IsCanGetDragObject(ICanDragComponentMono mono)
        {
            // Debug.Log("判断能否注入");
            // 意图是区分槽位内要素
            ICanBeInSlot newmono = mono.GetComponent<ICanBeInSlot>();
            //ICanBeStack stackmono = mono.GetComponent<ICanBeStack>();
            if (stackItemList.Count < maxSlotItemCount)
            {
                    return true;
            }
            //VerbMono verbmono = mono.GetComponent<VerbMono>();
            //if (verbmono != null)
            //    return true;
            // 这一节目前没有影响，因为最大数量为1 所以总是在为空时对卡牌有效
            // Debug.Log("进入卡牌内容阶段");
            //foreach(ICanBeStack item in stackItemList)
            //{
            //    if (item.CanStackWith(stackmono) == false)
            //        return false;
            //}
            // Debug.Log("可以放入");
            return false;
        }
        //public override void OnDrop(PointerEventData eventData)
        //{
        //    // Debug.Log("触发子类放下事件");
        //    if (model == null || model.dragMonoList == null)
        //        return;

        //    foreach (ICanDragComponentMono item in model.dragMonoList)
        //    {
        //        if (item == null) continue;
        //        if (IsCanGetDragObject(item) == false)
        //            continue;
        //        AddObject(item);
        //    }
        //}
        public int viewer = 0;
        private void Update()
        {
            viewer = stackItemList.Count;
        }
    }
}