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
    //    // �����С
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

    //        // ��ʼ״̬���赲����
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

    //                // ��¼������������ĵ�ƫ��
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

    //            // ��ǰ����
    //            Vector2 pos = rectTransform.anchoredPosition;

    //            // ����ƫ�ƣ�X ��Ҫ -75��
    //            float correctedX = pos.x ;
    //            float correctedY = pos.y;

    //            // �������ĸ������꣨�������뵽������ӣ�
    //            float snappedX = Mathf.Round((correctedX - firstGridOffsetX) / gridWidth) * gridWidth + firstGridOffsetX;
    //            float snappedY = Mathf.Round((correctedY - firstGridOffsetY) / gridHeight) * gridHeight + firstGridOffsetY;

    //            // ���Ʊ߽緶Χ
    //            float minX = firstGridOffsetX;
    //            float maxX = firstGridOffsetX + (xCount - 1) * gridWidth;
    //            float minY = firstGridOffsetY;
    //            float maxY = firstGridOffsetY + (yCount - 1) * gridHeight;

    //            snappedX = Mathf.Clamp(snappedX, minX, maxX) + lastGridOffsetX;
    //            snappedY = Mathf.Clamp(snappedY, minY, maxY) + lastGridOffsetY;

    //            // ��ֵ�� rectTransform
    //            rectTransform.anchoredPosition = new Vector2(snappedX, snappedY);

    //            MoveToTopMostDropZone(this.gameObject);
    //        }
    //    }


    //    // �������·������ϲ������ǲ����Լ�
    //    private bool IsTopMostUnderPointer(Vector2 screenPos)
    //    {
    //        PointerEventData eventData = new PointerEventData(EventSystem.current);
    //        eventData.position = screenPos;
    //        List<RaycastResult> results = new List<RaycastResult>();
    //        EventSystem.current.RaycastAll(eventData, results);
    //        Debug.Log("");
    //        foreach (var item in results)
    //            Debug.Log("��ջԪ��" + item.gameObject.name);
    //        Debug.Log("��ջ��" + results[0].gameObject.name + "Ŀ��" + gameObject.name);


    //        return results.Count > 0 && results[0].gameObject == gameObject;
    //    }

    //    IArchitecture IBelongToArchitecture.GetArchitecture()
    //    {
    //        return ProjectWinterArchitecture.Interface;
    //    }
    //    /// <summary>
    //    /// �� target �ƶ������������� Tag Ϊ "DropZone" ����߲㼶��������
    //    /// </summary>
    //    /// <summary>
    //    /// �� target �ƶ������������£�Tag Ϊ DropZone ���������в㼶��ߵ�λ��
    //    /// </summary>
    //    public static void MoveToTopMostDropZone( GameObject target)
    //    {
    //        if (target == null || target.transform.parent == null)
    //        {
    //            Debug.LogWarning("Ŀ�����������Ϊ�գ�");
    //            return;
    //        }

    //        Transform parent = target.transform.parent;
    //        int maxIndex = -1;

    //        // �ҵ������������� DropZone ����������ߵ�����
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
    //            // ���� target �� SiblingIndex ���� DropZone ��߲㼶����
    //            target.transform.SetSiblingIndex(maxIndex + 1);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("��������û�� DropZone �����壬target ����ԭ˳��");
    //        }
    //    }
    //}
    public class DropZoneDragHelper : MonoBehaviour, IController
    {
        public Transform dropzoneParent;  // ���� DropZone �ĸ�����
        public Canvas canvas;
        // �����������ID�����ڼ���
        public string dropZoneStringID = "Default";

        private bool isDragging = false;
        public RectTransform rectTransform;
        private Vector2 dragOffset;
        // �����С
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
        //    // ����������� Snap �����߼�
        //    // ��ǰ����
        //    Vector2 pos = rectTransform.anchoredPosition;

        //    // ����ƫ�ƣ�X ��Ҫ -75��
        //    float correctedX = pos.x;
        //    float correctedY = pos.y;

        //    // �������ĸ������꣨�������뵽������ӣ�
        //    float snappedX = Mathf.Round((correctedX - firstGridOffsetX) / gridWidth) * gridWidth + firstGridOffsetX;
        //    float snappedY = Mathf.Round((correctedY - firstGridOffsetY) / gridHeight) * gridHeight + firstGridOffsetY;
        //    slotIndexX = snappedX;
        //    slotIndexY = snappedY;
        //    // ���Ʊ߽緶Χ
        //    float minX = firstGridOffsetX;
        //    float maxX = firstGridOffsetX + (xCount - 1) * gridWidth;
        //    float minY = firstGridOffsetY;
        //    float maxY = firstGridOffsetY + (yCount - 1) * gridHeight;

        //    snappedX = Mathf.Clamp(snappedX, minX, maxX) + lastGridOffsetX;
        //    snappedY = Mathf.Clamp(snappedY, minY, maxY) + lastGridOffsetY;

        //    // ��ֵ�� rectTransform
        //    rectTransform.anchoredPosition = new Vector2(snappedX, snappedY);
        //}
        public void CheckToCloestGrid()
        {
            if (dropzoneParent == null)
                dropzoneParent = transform.parent;

            // ��ǰ����
            Vector2 pos = rectTransform.anchoredPosition;

            // ����ƫ�ƣ��������ᵽ�� X ƫ�� -75������ֱ�������ﴦ��
            float correctedX = pos.x -75;
            float correctedY = pos.y;

            // ��������������������꣩
            int gridX = Mathf.RoundToInt((correctedX - firstGridOffsetX) / gridWidth);
            int gridY = Mathf.RoundToInt((correctedY - firstGridOffsetY) / gridHeight);

            // ���Ʊ߽緶Χ
            gridX = Mathf.Clamp(gridX, 0, xCount - 1);
            gridY = Mathf.Clamp(gridY, 0, yCount - 1);

            // �����������
            slotIndexX = gridX;
            slotIndexY = gridY;

            targetSlot = this.GetModel<GameModel>().table.slotGroups[slotIndexX, slotIndexY].slotMono;

            // ���ݸ�����������ʵ������
            float snappedX = firstGridOffsetX + gridX * gridWidth + lastGridOffsetX;
            float snappedY = firstGridOffsetY + gridY * gridHeight + lastGridOffsetY;

            // Ӧ�û� UI
            rectTransform.anchoredPosition = new Vector2(snappedX + 75, snappedY);

            Debug.Log($"��������������: ({gridX}, {gridY}) -> ����({snappedX}, {snappedY})");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(dropzoneParent == null)
                    dropzoneParent = transform.parent;

                GameObject topDropZone = GetTopMostDropZoneUnderPointer(Input.mousePosition);

                // �ҵ�����µ���߲�ɽ������壨�������ơ��ж������ȣ�
                GameObject topUI = GetTopMostUIUnderPointer(Input.mousePosition);

                //// ����������߲����岻�� DropZone��˵���ǿ���/�ж���/��壩���Ͳ�������ק DropZone
                if (topUI != null && topUI != topDropZone)
                {
                    return; // �������������ȼ������壬����ק DropZone
                }
                Debug.Log("ͨ����������");
                if (topDropZone == gameObject)
                {
                    // ��ʼ��ק
                    isDragging = true;

                    // ��¼ƫ��
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
                // �������
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
                    //Debug.Log("���뷶Χ���");
                    // �ж�����Ƿ������DropZone��Χ��

                    bool isOver = RectTransformUtility.RectangleContainsScreenPoint(rt, screenPos, canvas.worldCamera);

                    //if (isOver)
                    //{
                    //    Debug.Log("����� DropZone ��Χ��");
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
            // Debug.Log("��ջ����" + results.Count);

            if (results.Count > 2)
            {
                // ���ض�ջ���� UI
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