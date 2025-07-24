using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlentyFishFramework
{
    public class SituationWindowICanGetDragComponentMono : ICanGetDragComponentMono,IDropHandler
    {
        // 行动框面板接收到卡牌的响应脚本 用于弹回接收到的卡牌
        public override void OnDrop(PointerEventData eventData)
        {
            // 默认读取列表 这里应该改成，如果有承受父物体则改为由父物体接受装载事件
            // Debug.Log("触发父类放下事件");
            if (model == null || model.dragMonoList == null)
                return;
            for (int i = model.dragMonoList.Count - 1;i >=0;i--  )
            {
                ICanDragComponentMono item = model.dragMonoList[i];
                if (item == null) continue;
                if(item.GetComponent<ITableElement>() != null)
                {
                    ITableElement element = item.GetComponent<ITableElement>();
                    system.MonoStackCardToSlot(element,element.LastGridMono,TableElementMonoType.SituationWindows);
                    //bool result = system.StackCardToASlot(element, element.LastGridMono);
                    //if (result)
                    //    GetComponent<ICanDragPlayAudioComponentMono>().PlayGetStackAudio();
                }
  
            }
        }
    }
}
