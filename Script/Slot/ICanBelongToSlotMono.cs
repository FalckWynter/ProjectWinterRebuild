using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public interface ICanBelongToSlotMono
    {
        // �̳�����ӿڵĿ��Ա����ۼ�¼
        public SlotMono BelongtoSlotMono { get; set; }
        public SlotMono BeforeSlotMono { get; set; }
        public SlotMono LastGridMono { get; set; }
    }
}