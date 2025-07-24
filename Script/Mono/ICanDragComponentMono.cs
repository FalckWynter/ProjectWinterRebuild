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
        // 拖动组件 该物体可以被拖动
        public GameSystem gameSystem;
        // 是否正在被拖拽
        public bool isDragging = false;
        // 开始拖动的位置
        public Vector2 startPointerPos;
        public bool isKeepOffset = false; // 新增：是否保持初始偏移
        private Vector2 dragOffset;      // 新增：用于记录初始偏移
        public float dragThreshold = 0f; // 可调距离阈值
        // 物体是否可互动和可拖拽
        CanvasGroup canvasGroup;
        public bool isCanBeDrag = true;
        // 拖拽相关动作订阅
        public UnityEvent<ICanDragComponentMono> onStartDrag = new UnityEvent<ICanDragComponentMono>();
        public UnityEvent<ICanDragComponentMono> onEndDrag = new UnityEvent<ICanDragComponentMono>();
        public UnityEvent<ICanDragComponentMono> onDragFailed = new UnityEvent<ICanDragComponentMono>();
        public UnityEvent<ICanDragComponentMono> onDestroy = new UnityEvent<ICanDragComponentMono>();
        public int textIndex = 1;

        public virtual void Start()
        {
            gameSystem = this.GetSystem<GameSystem>();
            canvasGroup = this.GetComponent<CanvasGroup>();
        }

        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
        // 鼠标点下时记录位置并重置拖拽状态
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
                // 如果没有在拖拽中，且距离小于拖拽阈值则暂时不动
                float dist = Vector2.Distance(eventData.position, startPointerPos);
                if (dist >= dragThreshold)
                {
                    isDragging = true;
                    gameSystem.AddDragListen(this);
                    canvasGroup.blocksRaycasts = false;
                    // 新增：记录初始偏移（仅在启用保持偏移模式下）
                    if (isKeepOffset)
                    {
                        Vector2 localMousePos;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                            transform.parent as RectTransform,
                            eventData.position,
                            eventData.pressEventCamera,
                            out localMousePos);

                        dragOffset = ((RectTransform)transform).anchoredPosition - localMousePos;
                    }
                }
            }

            if (isDragging)
            {
                //// 可以加入拖拽时的跟随逻辑，比如系统回调或 UI 移动
                //Vector2 localPoint;
                //RectTransformUtility.ScreenPointToLocalPointInRectangle(
                //    transform.parent as RectTransform,
                //    eventData.position,
                //    eventData.pressEventCamera,
                //    out localPoint);
                //(transform as RectTransform).anchoredPosition = localPoint;
                // 如果正在拖拽 则跟随鼠标进行位移
                Vector2 localMousePos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    transform.parent as RectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out localMousePos);

                // 修改：根据 isKeepOffset 决定是否加上初始偏移
                if (isKeepOffset)
                {
                    ((RectTransform)transform).anchoredPosition = localMousePos + dragOffset;
                }
                else
                {
                    ((RectTransform)transform).anchoredPosition = localMousePos;
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // 结束拖拽时结束拖拽订阅并重新启用射线阻挡
            if (isCanBeDrag == false) return;
            //Debug.Log("结束拖拽");
            onEndDrag.Invoke(this);
            if (isDragging)
            {
                gameSystem.RemoveDragListen(this);
                isDragging = false;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            // 开始拖拽时移动到临时拖拽父物体中 并唤起开始拖拽事件
            if (isCanBeDrag == false) return;
            transform.SetParent(UtilSystem.dragParent.transform);
            transform.SetAsLastSibling(); // 把它放到同级 UI 中的最上层
            //Debug.Log("开始拖拽" + UtilSystem.dragParent.transform.name + "当前父物体" + transform.parent.name );
            onStartDrag.Invoke(this);
        }


        public void OnDestroy()
        {
            // 被摧毁时调用摧毁事件
            onDestroy.Invoke(this);
        }
    }
}
