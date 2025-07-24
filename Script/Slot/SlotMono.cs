using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class SlotMono : MonoBehaviour
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

        // ���ÿ�������
        public void LoadSlotData(AbstractSlot slot)
        {
            this.slot = slot;
            slotLabel.text = slot.label;
            slotLabel.text = slot.createIndex.ToString();
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
            //if(slot.isSlot)
            //Debug.Log("��������" + slot.label);
            //if (slot == null || slot.isSlot == false) return;
            //stackCount = slot.stackItemList.Count;
        }
    }
}