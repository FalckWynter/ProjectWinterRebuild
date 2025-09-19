using PlentyFishFramework;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    //public class DropZoneMono : MonoBehaviour, IController
    //{

    //    public RectTransform rectTransform;
    //    public Canvas canvas;
    //    public CanvasGroup canvasGroup;

    //    private bool isDragging = false;    
    //    // 网格大小
    //    public float gridWidth = 75f;
    //    public float gridHeight = 115f;
    //    public int xCount = 25, yCount = 9;
    //    public float firstGridOffsetX = 60, firstGridOffsetY = 80;
    //    public float lastGridOffsetX = 10, lastGridOffsetY = 0;
    //    void Awake()
    //    {
    //        rectTransform = GetComponent<RectTransform>();
    //        canvas = GetComponentInParent<Canvas>();
    //        canvasGroup = GetComponent<CanvasGroup>();

    //        // 初始状态不阻挡射线
    //        canvasGroup.blocksRaycasts = false;
    //    }

    //    public Vector2 dragOffset;

    //    void Update()
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            canvasGroup.blocksRaycasts = true;

    //            if (IsTopMostUnderPointer(Input.mousePosition))
    //            {
    //                isDragging = true;
    //                canvasGroup.blocksRaycasts = true;

    //                // 记录鼠标与物体中心的偏移
    //                RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //                    canvas.transform as RectTransform,
    //                    Input.mousePosition,
    //                    canvas.worldCamera,
    //                    out var localPos);

    //                dragOffset = rectTransform.anchoredPosition - localPos;
    //            }
    //            else
    //                canvasGroup.blocksRaycasts = false;


    //        }

    //        if (Input.GetMouseButton(0) && isDragging)
    //        {
    //            RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //                canvas.transform as RectTransform,
    //                Input.mousePosition,
    //                canvas.worldCamera,
    //                out var localPos);

    //            rectTransform.anchoredPosition = localPos + dragOffset;
    //        }

    //        if (Input.GetMouseButtonUp(0) && isDragging)
    //        {
    //            isDragging = false;
    //            canvasGroup.blocksRaycasts = false;

    //            // 当前坐标
    //            Vector2 pos = rectTransform.anchoredPosition;

    //            // 修正偏移（X 需要 -75）
    //            float correctedX = pos.x ;
    //            float correctedY = pos.y;

    //            // 计算对齐的格子坐标（四舍五入到最近格子）
    //            float snappedX = Mathf.Round((correctedX - firstGridOffsetX) / gridWidth) * gridWidth + firstGridOffsetX;
    //            float snappedY = Mathf.Round((correctedY - firstGridOffsetY) / gridHeight) * gridHeight + firstGridOffsetY;

    //            // 限制边界范围
    //            float minX = firstGridOffsetX;
    //            float maxX = firstGridOffsetX + (xCount - 1) * gridWidth;
    //            float minY = firstGridOffsetY;
    //            float maxY = firstGridOffsetY + (yCount - 1) * gridHeight;

    //            snappedX = Mathf.Clamp(snappedX, minX, maxX) + lastGridOffsetX;
    //            snappedY = Mathf.Clamp(snappedY, minY, maxY) + lastGridOffsetY;

    //            // 赋值回 rectTransform
    //            rectTransform.anchoredPosition = new Vector2(snappedX, snappedY);

    //            MoveToTopMostDropZone(this.gameObject);
    //        }
    //    }


    //    // 检查鼠标下方的最上层物体是不是自己
    //    private bool IsTopMostUnderPointer(Vector2 screenPos)
    //    {
    //        PointerEventData eventData = new PointerEventData(EventSystem.current);
    //        eventData.position = screenPos;
    //        List<RaycastResult> results = new List<RaycastResult>();
    //        EventSystem.current.RaycastAll(eventData, results);
    //        Debug.Log("");
    //        foreach (var item in results)
    //            Debug.Log("堆栈元素" + item.gameObject.name);
    //        Debug.Log("堆栈顶" + results[0].gameObject.name + "目标" + gameObject.name);


    //        return results.Count > 0 && results[0].gameObject == gameObject;
    //    }

    //    IArchitecture IBelongToArchitecture.GetArchitecture()
    //    {
    //        return ProjectWinterArchitecture.Interface;
    //    }
    //    /// <summary>
    //    /// 将 target 移动到它父物体下 Tag 为 "DropZone" 的最高层级子物体中
    //    /// </summary>
    //    /// <summary>
    //    /// 将 target 移动到它父物体下，Tag 为 DropZone 的子物体中层级最高的位置
    //    /// </summary>
    //    public static void MoveToTopMostDropZone( GameObject target)
    //    {
    //        if (target == null || target.transform.parent == null)
    //        {
    //            Debug.LogWarning("目标物体或父物体为空！");
    //            return;
    //        }

    //        Transform parent = target.transform.parent;
    //        int maxIndex = -1;

    //        // 找到父物体下所有 DropZone 子物体中最高的索引
    //        foreach (Transform child in parent)
    //        {
    //            if (child.CompareTag("DropZone"))
    //            {
    //                if (child.GetSiblingIndex() > maxIndex)
    //                {
    //                    maxIndex = child.GetSiblingIndex();
    //                }
    //            }
    //        }

    //        if (maxIndex >= 0)
    //        {
    //            // 设置 target 的 SiblingIndex 排在 DropZone 最高层级后面
    //            target.transform.SetSiblingIndex(maxIndex + 1);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("父物体下没有 DropZone 子物体，target 保持原顺序。");
    //        }
    //    }
    //}
    public class DropZoneDragHelper : MonoBehaviour, IController
    {
        public Transform dropzoneParent;  // 所有 DropZone 的父物体
        public Canvas canvas;
        // 这个放置区的ID，用于检索
        public string dropZoneStringID = "Default";

        private bool isDragging = false;
        public RectTransform rectTransform;
        private Vector2 dragOffset;
        // 网格大小
        public float gridWidth = 75f;
        public float gridHeight = 115f;
        public int xCount = 25, yCount = 9;
        public float firstGridOffsetX = 60, firstGridOffsetY = 80;
        public float lastGridOffsetX = 10, lastGridOffsetY = 0;
        public int slotIndexX, slotIndexY;
        public SlotMono targetSlot;
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            dropzoneParent = transform.parent;
            canvas = GetComponentInParent<Canvas>();
            transform.localScale = Vector3.one;

        }
        //public void CheckToCloestGrid()
        //{
        //    if (dropzoneParent == null)
        //        dropzoneParent = transform.parent;
        //    // 可以在这里加 Snap 网格逻辑
        //    // 当前坐标
        //    Vector2 pos = rectTransform.anchoredPosition;

        //    // 修正偏移（X 需要 -75）
        //    float correctedX = pos.x;
        //    float correctedY = pos.y;

        //    // 计算对齐的格子坐标（四舍五入到最近格子）
        //    float snappedX = Mathf.Round((correctedX - firstGridOffsetX) / gridWidth) * gridWidth + firstGridOffsetX;
        //    float snappedY = Mathf.Round((correctedY - firstGridOffsetY) / gridHeight) * gridHeight + firstGridOffsetY;
        //    slotIndexX = snappedX;
        //    slotIndexY = snappedY;
        //    // 限制边界范围
        //    float minX = firstGridOffsetX;
        //    float maxX = firstGridOffsetX + (xCount - 1) * gridWidth;
        //    float minY = firstGridOffsetY;
        //    float maxY = firstGridOffsetY + (yCount - 1) * gridHeight;

        //    snappedX = Mathf.Clamp(snappedX, minX, maxX) + lastGridOffsetX;
        //    snappedY = Mathf.Clamp(snappedY, minY, maxY) + lastGridOffsetY;

        //    // 赋值回 rectTransform
        //    rectTransform.anchoredPosition = new Vector2(snappedX, snappedY);
        //}
        public void CheckToCloestGrid()
        {
            if (dropzoneParent == null)
                dropzoneParent = transform.parent;

            // 当前坐标
            Vector2 pos = rectTransform.anchoredPosition;

            // 修正偏移（例如你提到的 X 偏移 -75，可以直接在这里处理）
            float correctedX = pos.x -75;
            float correctedY = pos.y;

            // 计算格子索引（行列坐标）
            int gridX = Mathf.RoundToInt((correctedX - firstGridOffsetX) / gridWidth);
            int gridY = Mathf.RoundToInt((correctedY - firstGridOffsetY) / gridHeight);

            // 限制边界范围
            gridX = Mathf.Clamp(gridX, 0, xCount - 1);
            gridY = Mathf.Clamp(gridY, 0, yCount - 1);

            // 保存格子索引
            slotIndexX = gridX;
            slotIndexY = gridY;

            targetSlot = this.GetModel<GameModel>().table.slotGroups[slotIndexX, slotIndexY].slotMono;

            // 根据格子索引反推实际坐标
            float snappedX = firstGridOffsetX + gridX * gridWidth + lastGridOffsetX;
            float snappedY = firstGridOffsetY + gridY * gridHeight + lastGridOffsetY;

            // 应用回 UI
            rectTransform.anchoredPosition = new Vector2(snappedX + 75, snappedY);

            Debug.Log($"物体吸附到格子: ({gridX}, {gridY}) -> 坐标({snappedX}, {snappedY})");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(dropzoneParent == null)
                    dropzoneParent = transform.parent;

                GameObject topDropZone = GetTopMostDropZoneUnderPointer(Input.mousePosition);

                // 找到鼠标下的最高层可交互物体（包括卡牌、行动框、面板等）
                GameObject topUI = GetTopMostUIUnderPointer(Input.mousePosition);

                //// 如果鼠标下最高层物体不是 DropZone（说明是卡牌/行动框/面板），就不允许拖拽 DropZone
                if (topUI != null && topUI != topDropZone)
                {
                    return; // 有其他更高优先级的物体，不拖拽 DropZone
                }
                Debug.Log("通过父物体检测");
                if (topDropZone == gameObject)
                {
                    // 开始拖拽
                    isDragging = true;

                    // 记录偏移
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        canvas.transform as RectTransform,
                        Input.mousePosition,
                        canvas.worldCamera,
                        out Vector2 localPos);

                    dragOffset = rectTransform.anchoredPosition - localPos;
                }
            }

            if (Input.GetMouseButton(0) && isDragging)
            {
                // 跟随鼠标
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    Input.mousePosition,
                    canvas.worldCamera,
                    out Vector2 localPos);

                rectTransform.anchoredPosition = localPos + dragOffset;
            }

            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                isDragging = false;
                transform.SetAsLastSibling();

                CheckToCloestGrid();
            }
        }

        //private GameObject GetTopMostDropZoneUnderPointer(Vector2 screenPos)
        //{
        //    PointerEventData eventData = new PointerEventData(EventSystem.current);
        //    eventData.position = screenPos;

        //    List<RaycastResult> results = new List<RaycastResult>();
        //    EventSystem.current.RaycastAll(eventData, results);

        //    GameObject topDropZone = null;
        //    int topOrder = int.MinValue;

        //    foreach (var r in results)
        //    {
        //        if (r.gameObject.transform.IsChildOf(dropzoneParent) && r.gameObject.CompareTag("DropZone"))
        //        {
        //            int order = 0;
        //            Canvas c = r.gameObject.GetComponentInParent<Canvas>();
        //            if (c != null) order += c.sortingOrder * 1000;

        //            order += r.gameObject.transform.GetSiblingIndex();

        //            if (order > topOrder)
        //            {
        //                topOrder = order;
        //                topDropZone = r.gameObject;
        //            }
        //        }
        //    }

        //    return topDropZone;
        //}
        private GameObject GetTopMostDropZoneUnderPointer(Vector2 screenPos)
        {
            GameObject topDropZone = null;
            int topOrder = int.MinValue;

            foreach (Transform child in dropzoneParent)
            {
                if (child.CompareTag("DropZone"))
                {
                    RectTransform rt = child as RectTransform;
                    if (rt == null) continue;
                    //Debug.Log("进入范围检测");
                    // 判断鼠标是否在这个DropZone范围内

                    bool isOver = RectTransformUtility.RectangleContainsScreenPoint(rt, screenPos, canvas.worldCamera);

                    //if (isOver)
                    //{
                    //    Debug.Log("鼠标在 DropZone 范围内");
                    //}


                    if (isOver)
                    {
                        int order = 0;
                        Canvas c = rt.GetComponentInParent<Canvas>();
                        if (c != null) order += c.sortingOrder * 1000;

                        order += rt.GetSiblingIndex();

                        if (order > topOrder)
                        {
                            topOrder = order;
                            topDropZone = child.gameObject;
                        }
                    }
                }
            }

            return topDropZone;
        }

        private GameObject GetTopMostUIUnderPointer(Vector2 screenPos)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = screenPos
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            // Debug.Log("堆栈数量" + results.Count);

            if (results.Count > 2)
            {
                // 返回堆栈顶的 UI
                return results[0].gameObject;
            }
            return null;
        }

        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
    }
}