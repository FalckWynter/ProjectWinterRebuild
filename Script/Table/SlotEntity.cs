using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class SlotEntity 
    {
        // 卡槽的数据实体，包含其坐标及内容 是否锁定
        public int rowIndex;
        public int colIndex;
        public AbstractSlot slot;
        public SlotMono slotMono;
        // 可扩展字段（比如槽位状态、限制、标记类型等）
        public string slotType; // "default" / "action" / "resource"
        public bool isLocked;

        public SlotEntity(int row, int col)
        {
            rowIndex = row;
            colIndex = col;
            slotType = "default";
            isLocked = false;
        }
    }
}