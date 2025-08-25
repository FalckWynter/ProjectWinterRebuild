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
            // ������Ч�¼�
            glowFader = transform.Find("Glow").GetComponent<GraphicFader>();

        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(UtilSystem.dragParent.transform);

            transform.SetAsLastSibling(); // �����ŵ�ͬ�� UI �е����ϲ�
            onStartDrag.Invoke(this);
            OnPointerDown(eventData);
            if (mono.BelongtoSlotMono != null)
                gameSystem.UnRegisterStackElementFromSlot(mono, mono.BelongtoSlotMono);
        }


        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            // ������ɫΪ����ɫ������
            glowFader.SetColor(UIStyle.hoverWhite);
            glowFader.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // ��ɫ�ȱ�Ϊ��ɫ
            glowFader.SetColor(UIStyle.brightPink);
            // Ȼ����
            glowFader.Hide();
        }
    }
}
