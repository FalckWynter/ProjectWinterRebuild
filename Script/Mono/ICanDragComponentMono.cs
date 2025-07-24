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
        // �϶���� ��������Ա��϶�
        public GameSystem gameSystem;
        // �Ƿ����ڱ���ק
        public bool isDragging = false;
        // ��ʼ�϶���λ��
        public Vector2 startPointerPos;
        public bool isKeepOffset = false; // �������Ƿ񱣳ֳ�ʼƫ��
        private Vector2 dragOffset;      // ���������ڼ�¼��ʼƫ��
        public float dragThreshold = 0f; // �ɵ�������ֵ
        // �����Ƿ�ɻ����Ϳ���ק
        CanvasGroup canvasGroup;
        public bool isCanBeDrag = true;
        // ��ק��ض�������
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
        // ������ʱ��¼λ�ò�������ק״̬
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
                // ���û������ק�У��Ҿ���С����ק��ֵ����ʱ����
                float dist = Vector2.Distance(eventData.position, startPointerPos);
                if (dist >= dragThreshold)
                {
                    isDragging = true;
                    gameSystem.AddDragListen(this);
                    canvasGroup.blocksRaycasts = false;
                    // ��������¼��ʼƫ�ƣ��������ñ���ƫ��ģʽ�£�
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
                //// ���Լ�����קʱ�ĸ����߼�������ϵͳ�ص��� UI �ƶ�
                //Vector2 localPoint;
                //RectTransformUtility.ScreenPointToLocalPointInRectangle(
                //    transform.parent as RectTransform,
                //    eventData.position,
                //    eventData.pressEventCamera,
                //    out localPoint);
                //(transform as RectTransform).anchoredPosition = localPoint;
                // ���������ק �����������λ��
                Vector2 localMousePos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    transform.parent as RectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out localMousePos);

                // �޸ģ����� isKeepOffset �����Ƿ���ϳ�ʼƫ��
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
            // ������קʱ������ק���Ĳ��������������赲
            if (isCanBeDrag == false) return;
            //Debug.Log("������ק");
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
            // ��ʼ��קʱ�ƶ�����ʱ��ק�������� ������ʼ��ק�¼�
            if (isCanBeDrag == false) return;
            transform.SetParent(UtilSystem.dragParent.transform);
            transform.SetAsLastSibling(); // �����ŵ�ͬ�� UI �е����ϲ�
            //Debug.Log("��ʼ��ק" + UtilSystem.dragParent.transform.name + "��ǰ������" + transform.parent.name );
            onStartDrag.Invoke(this);
        }


        public void OnDestroy()
        {
            // ���ݻ�ʱ���ôݻ��¼�
            onDestroy.Invoke(this);
        }
    }
}
