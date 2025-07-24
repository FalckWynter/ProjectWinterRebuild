using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public interface ICanBelongToSlotMono
    {
        // 继承这个接口的可以被卡槽记录
        public SlotMono BelongtoSlotMono { get; set; }
        public SlotMono BeforeSlotMono { get; set; }
        public SlotMono LastGridMono { get; set; }
    }
}