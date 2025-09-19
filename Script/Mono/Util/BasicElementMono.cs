using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class BasicElementMono : MonoBehaviour
    {
        public AbstractElement manifest;
        // 区别在于 belongtoSlot会在开始拖动时被取消，而beforeSlot在拖动结束完成计算时才取消
        // 7.20 区别在于，belongtoSlot记录的是当前所属的卡槽，开始拖动时就取消
        // BeforeSlot记录上一个有效的任意卡槽，包括桌面网格
        // LastGridMono记录上一个桌面网格，以便于回到桌面上
        public SlotMono BelongtoSlotMono { get { return belongtoSlotMono; } set => belongtoSlotMono = value; }
        public SlotMono BeforeSlotMono { get => beforeSlotMono; set => beforeSlotMono = value; }
        public SlotMono LastGridMono { get => lastGridMono; set { lastGridMono = value; /*Debug.Log(gameObject.name + "设置为" + lastGridMono.gameObject.name); */} }
        private SlotMono belongtoSlotMono;
        private SlotMono beforeSlotMono;
        private SlotMono lastGridMono;
    }
}