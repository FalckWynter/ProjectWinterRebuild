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
        // 卡槽脚本
        public AbstractSlot slot { set { /*Debug.Log("产生修改" + value.label);*/ slt = value;  } get { return slt; ; } }
        private AbstractSlot slt;
        // 坐标
        public int x, y;
        // 背景图片 关闭以开启透明底
        public Image background;
        // 卡槽名称
        public TextMeshProUGUI slotLabel;

        // 最大可拥有卡槽物品数量
        public int maxSlotItemCount = 1;
        // 是否允许堆叠元素 已经堆叠的元素数量
        public bool isAllowStack = false;
        //public int stackCount = 0;

        // 设置卡槽数据
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
            // 根据group信息初始化外观，如锁定、类型图标等
        }
        // 更新堆叠数量
        private void Update()
        {
            //if(slot.isSlot)
            //Debug.Log("卡槽名称" + slot.label);
            //if (slot == null || slot.isSlot == false) return;
            //stackCount = slot.stackItemList.Count;
        }
    }
}