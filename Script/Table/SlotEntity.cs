using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class SlotEntity 
    {
        // ���۵�����ʵ�壬���������꼰���� �Ƿ�����
        public int rowIndex;
        public int colIndex;
        public AbstractSlot slot;
        public SlotMono slotMono;
        // ����չ�ֶΣ������λ״̬�����ơ�������͵ȣ�
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