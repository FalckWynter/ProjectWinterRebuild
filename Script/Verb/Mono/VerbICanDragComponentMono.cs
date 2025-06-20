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
            // ������Ч�¼�
            audiomono = GetComponent<ICanDragPlayAudioComponentMono>();
            onStartDrag.AddListener(audiomono.PlayStartDragAudio);
            onEndDrag.AddListener(audiomono.PlayEndDragAudio);
            glowFader = transform.Find("Glow").GetComponent<GraphicFader>();

        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            //// ��֧�ж� ��Ϊ��������������1ʱ���Ϊ��ק�����Ƶĸ���
            //// Debug.Log("��ʼ��ק" + textIndex);
            //if (mono.stackCount == 1)
            //{
            transform.SetAsLastSibling(); // �����ŵ�ͬ�� UI �е����ϲ�
            onStartDrag.Invoke(this);
            OnPointerDown(eventData);

            //}
            //else
            //{
            //    // ������Ϊ�µĿ��Ƹ������и�ֵ
            //    GameObject ob = this.GetSystem<UtilSystem>().CreateCardGameObject(mono.card);
            //    CardICanDragComponentMono newMono = ob.GetComponent<CardICanDragComponentMono>();
            //    newMono.transform.position = this.transform.position;
            //    newMono.Start();
            //    newMono.textIndex++;
            //    newMono.transform.SetAsLastSibling();

            //    // �����Լ�����������
            //    mono.StackCount--;
            //    //�޸��¼�����Ŀ��Ϊ�µĿ���
            //    StartCoroutine(TransferDragToCopyNextFrame(newMono, eventData));
            //    eventData.pointerDrag = null;
            //    //newMono.onEndDrag.AddListener(ResetDragState);
            //    //isCanBeDrag = false;

            //}
        }

        // ���µ��¼����ĵ��µĿ���
        IEnumerator TransferDragToCopyNextFrame(CardICanDragComponentMono copy, PointerEventData eventData)
        {
            yield return null; // �ȴ�һ֡��ȷ��EventSystem�ͷ�������Ȩ

            eventData.pointerDrag = copy.gameObject;
            // ǿ�ƽ�����ע��Ϊ��קĿ��
            EventSystem.current.SetSelectedGameObject(copy.gameObject);

            // �ֶ����ø�������ק��ʼ�߼�
            ExecuteEvents.Execute(copy.gameObject, eventData, ExecuteEvents.beginDragHandler);
            ExecuteEvents.Execute(copy.gameObject, eventData, ExecuteEvents.dragHandler);

        }

        // ������ק״̬ �����ڸ���������קʱ���ԭ�����ƵĲ����϶�״̬
        public void ResetDragState(ICanDragComponentMono mono)
        {
            // mono.onEndDrag.RemoveListener(ResetDragState);
            //isCanBeDrag = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
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
