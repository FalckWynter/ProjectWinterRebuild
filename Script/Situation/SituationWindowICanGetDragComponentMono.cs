using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlentyFishFramework
{
    public class SituationWindowICanGetDragComponentMono : ICanGetDragComponentMono,IDropHandler
    {
        // �ж��������յ����Ƶ���Ӧ�ű� ���ڵ��ؽ��յ��Ŀ���
        public override void OnDrop(PointerEventData eventData)
        {
            // Ĭ�϶�ȡ�б� ����Ӧ�øĳɣ�����г��ܸ��������Ϊ�ɸ��������װ���¼�
            // Debug.Log("������������¼�");
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
