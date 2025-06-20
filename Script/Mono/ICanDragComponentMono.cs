using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using QFramework;
using UnityEngine.Events;
namespace PlentyFishFramework
{
    public class ICanDragComponentMono : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IController, IBeginDragHandler
    {
        private GameSystem system;
        private bool isDragging = false;
        private Vector2 startPointerPos;
        public float dragThreshold = 0f; // 可调距离阈值
        CanvasGroup canvasGroup;
        public GameObject componentParent;
        public bool isCanBeDrag = true;
        public UnityEvent<ICanDragComponentMono> onStartDrag = new UnityEvent<ICanDragComponentMono>();
        public UnityEvent<ICanDragComponentMono> onEndDrag = new UnityEvent<ICanDragComponentMono>();
        public virtual void Start()
        {
            system = this.GetSystem<GameSystem>();
            canvasGroup = this.GetComponent<CanvasGroup>();
        }

        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            startPointerPos = eventData.position;
            isDragging = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isCanBeDrag == false) return;
            if (!isDragging)
            {
                float dist = Vector2.Distance(eventData.position, startPointerPos);
                if (dist >= dragThreshold)
                {
                    isDragging = true;
                    system.AddDragListen(this);
                    canvasGroup.blocksRaycasts = false;
                }
            }

            if (isDragging)
            {
                // 可以加入拖拽时的跟随逻辑，比如系统回调或 UI 移动
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    transform.parent as RectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out localPoint);
                (transform as RectTransform).anchoredPosition = localPoint;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isCanBeDrag == false) return;
            Debug.Log("结束拖拽");
            onEndDrag.Invoke(this);
            if (isDragging)
            {
                system.RemoveDragListen(this);
                isDragging = false;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (isCanBeDrag == false) return;
            Debug.Log("开始拖拽");
            onStartDrag.Invoke(this);
        }
    }
}
//public class ICanDragComponentMono : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IController
//{
//    GameSystem system;
//    private void Start()
//    {
//        system = this.GetSystem<GameSystem>();
//    }
//    public IArchitecture GetArchitecture()
//    {
//        return ProjectWinterArchitecture.Interface;
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        system.AddDragListen(this);
//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        system.RemoveDragListen(this);
//    }
//}
