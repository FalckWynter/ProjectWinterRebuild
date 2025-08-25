using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class UtilModel : AbstractModel
    {
        // ͨ�����ݿ� �洢һЩ��Ϸ�ײ��߼�����ֵ
        // ��Ϸ�¼��߼��ļ��㱶��
        public static float GameLogicSpeed => gameLogicSpeed;
        private static float gameLogicSpeed = 1f;
        // ��Ϸ�߼�ʱ�侭�����㱶�����ź��deltaTime
        public static float GameLogicDeltaTime => gameLogicSpeed * Time.deltaTime;

        public static TokenDetailsWindowMono tokenDetailWindow;
        public static AspectDetailWindowMono aspectDetailWindow;
        public static SlotDetailsWindowMono slotDetailWindow;
        
        protected override void OnInit()
        {

        }

    
    }
}