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
        // 卡牌拖拽组件脚本
        public CardMono mono;
        // 光效渲染脚本
        public GraphicFader glowFader;
        // 拖拽音效脚本
        public ICanDragPlayAudioComponentMono dragAudioMono;
        public override void Start()
        {
            base.Start();
            // 获取和注册脚本们
            mono = GetComponent<CardMono>();
            dragAudioMono = GetComponent<ICanDragPlayAudioComponentMono>();
            onStartDrag.AddListener(dragAudioMono.PlayStartDragAudio);
            onEndDrag.AddListener(dragAudioMono.PlayEndDragAudio);
            // 订阅音效事件
            glowFader = transform.Find("Glow").GetComponent<GraphicFader>();

        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            // 分支判断 因为当卡牌数量大于1时会改为拖拽这张牌的副本
            // Debug.Log("开始拖拽" + textIndex);
            if (mono.stackCount == 1)
            {
                mono.RestoreOriginalRectTransform();
                // 重设父物体到公用拖拽父物体
                transform.SetParent(UtilSystem.dragParent.transform);
                transform.SetAsLastSibling(); // 把它放到同级 UI 中的最上层
                // 唤起开始拖拽事件以解除接受器对它发起的订阅
                onStartDrag.Invoke(this);
                // 触发按下事件以记录起点坐标
                OnPointerDown(eventData);
                // 如果有所属卡槽则解除属性订阅
                if (mono.BelongtoSlotMono != null)
                gameSystem.UnRegisterStackElementFromSlot(mono, mono.BelongtoSlotMono);
                // OnPointerDown(eventData);
            }
            else
            {
                // 创建一个副本给玩家拖动
                gameSystem.CreateCardCopyToBeDrag(mono, eventData);

            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // 设置颜色为亮白色，渐显
            glowFader.SetColor(UIStyle.hoverWhite);
            glowFader.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // 颜色先变为粉色
            glowFader.SetColor(UIStyle.brightPink);
            // 然后渐隐
            glowFader.Hide();
        }
    }
}