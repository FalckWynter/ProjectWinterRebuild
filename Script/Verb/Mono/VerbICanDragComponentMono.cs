using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using QFramework;
namespace PlentyFishFramework { 
public class VerbICanDragComponentMono : ICanDragComponentMono, IPointerEnterHandler, IPointerExitHandler
    {
        public VerbMono mono;
        public ICanDragPlayAudioComponentMono audiomono;
        public GraphicFader glowFader;
        public int textIndex = 1;
        public override void Start()
        {
            base.Start();
            mono = GetComponent<VerbMono>();
            // 订阅音效事件
            audiomono = GetComponent<ICanDragPlayAudioComponentMono>();
            onStartDrag.AddListener(audiomono.PlayStartDragAudio);
            onEndDrag.AddListener(audiomono.PlayEndDragAudio);
            glowFader = transform.Find("Glow").GetComponent<GraphicFader>();

        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            //// 分支判断 因为当卡牌数量大于1时会改为拖拽这张牌的副本
            //// Debug.Log("开始拖拽" + textIndex);
            //if (mono.stackCount == 1)
            //{
            transform.SetAsLastSibling(); // 把它放到同级 UI 中的最上层
            onStartDrag.Invoke(this);
            OnPointerDown(eventData);

            //}
            //else
            //{
            //    // 创建并为新的卡牌副本进行赋值
            //    GameObject ob = this.GetSystem<UtilSystem>().CreateCardGameObject(mono.card);
            //    CardICanDragComponentMono newMono = ob.GetComponent<CardICanDragComponentMono>();
            //    newMono.transform.position = this.transform.position;
            //    newMono.Start();
            //    newMono.textIndex++;
            //    newMono.transform.SetAsLastSibling();

            //    // 减少自己的样本数量
            //    mono.StackCount--;
            //    //修改事件订阅目标为新的卡牌
            //    StartCoroutine(TransferDragToCopyNextFrame(newMono, eventData));
            //    eventData.pointerDrag = null;
            //    //newMono.onEndDrag.AddListener(ResetDragState);
            //    //isCanBeDrag = false;

            //}
        }

        // 将新的事件订阅到新的卡牌
        IEnumerator TransferDragToCopyNextFrame(CardICanDragComponentMono copy, PointerEventData eventData)
        {
            yield return null; // 等待一帧，确保EventSystem释放鼠标控制权

            eventData.pointerDrag = copy.gameObject;
            // 强制将副本注册为拖拽目标
            EventSystem.current.SetSelectedGameObject(copy.gameObject);

            // 手动调用副本的拖拽开始逻辑
            ExecuteEvents.Execute(copy.gameObject, eventData, ExecuteEvents.beginDragHandler);
            ExecuteEvents.Execute(copy.gameObject, eventData, ExecuteEvents.dragHandler);

        }

        // 重置拖拽状态 用于在副本结束拖拽时解除原来卡牌的不可拖动状态
        public void ResetDragState(ICanDragComponentMono mono)
        {
            // mono.onEndDrag.RemoveListener(ResetDragState);
            //isCanBeDrag = true;
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
