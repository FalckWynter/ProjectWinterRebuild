using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class SlotMono : MonoBehaviour,IPointerClickHandler
    {
        // ���۽ű�
        public AbstractSlot slot { set { /*Debug.Log("�����޸�" + value.label);*/ slt = value;  } get { return slt; ; } }
        private AbstractSlot slt;
        // ����
        public int x, y;
        // ����ͼƬ �ر��Կ���͸����
        public Image background;
        // ��������
        public TextMeshProUGUI slotLabel;

        // ����ӵ�п�����Ʒ����
        public int maxSlotItemCount = 1;
        // �Ƿ�����ѵ�Ԫ�� �Ѿ��ѵ���Ԫ������
        public bool isAllowStack = false;
        //public int stackCount = 0;
        // 3.2.1.1 ������ľ����ۼ���
        public GameObject slotHintGameobject, slotGreedyHintGameobject, slotConsumeHintGameobject;
        // ���ӿ������Ԫ������
        public int stackItemCountInspector;

        // ���ÿ�������
        public void LoadSlotData(AbstractSlot slot)
        {
            this.slot = slot;
            slotLabel.text = slot.label;
            slotLabel.text = slot.label + slot.createIndex.ToString();
            gameObject.name = slot.stringIndex;
            if(!slot.isSlot)
            {
                slotHintGameobject.gameObject.SetActive(false);
            }
            else
            {
                slotHintGameobject.gameObject.SetActive(true);
                slotGreedyHintGameobject.gameObject.SetActive(slot.isGreedy);
                slotConsumeHintGameobject.gameObject.SetActive(slot.isConsumes);

            }
            //GetComponent<SizeTestInspectorMono>().ForceUpdate();
        }
        public void Highlight(bool on)
        {
            background.color = on ? Color.yellow : Color.white;
        }

        public void SetupUI(SlotEntity group)
        {
            // ����group��Ϣ��ʼ����ۣ�������������ͼ���
        }
        // ���¶ѵ�����
        private void Update()
        {
            //Debug.Log("��������" + slot.label);
            //if (slot == null || slot.isSlot == false) return;
            //stackCount = slot.stackItemList.Count;
            if (slot != null)
                stackItemCountInspector = slot.stackItemList.Count;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (slot.isSlot == true && eventData.button == PointerEventData.InputButton.Left)
            {
                //UtilModel.tokenDetailWindow.ShowWindowForCard(card);
                UtilSystem.ShowSlot(slot);
            }
        }
    }
}