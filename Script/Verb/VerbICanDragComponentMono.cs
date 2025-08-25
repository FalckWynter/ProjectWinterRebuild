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
        public GraphicFader glowFader;
        public override void Start()
        {
            base.Start();
            mono = GetComponent<VerbMono>();
            // 订阅音效事件
            glowFader = transform.Find("Glow").GetComponent<GraphicFader>();

        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(UtilSystem.dragParent.transform);

            transform.SetAsLastSibling(); // 把它放到同级 UI 中的最上层
            onStartDrag.Invoke(this);
            OnPointerDown(eventData);
            if (mono.BelongtoSlotMono != null)
                gameSystem.UnRegisterStackElementFromSlot(mono, mono.BelongtoSlotMono);
        }


        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
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
