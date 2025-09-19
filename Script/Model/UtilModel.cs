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
        public static float GameLogicSpeed => (float)gameSpeedManager.CurrentSpeed;
        //private static float gameLogicSpeed = 1f;
        // ��Ϸ�߼�ʱ�侭�����㱶�����ź��deltaTime
        public static float GameLogicDeltaTime => GameLogicSpeed * Time.deltaTime;

        public static TokenDetailsWindowMono tokenDetailWindow;
        public static AspectDetailWindowMono aspectDetailWindow;
        public static SlotDetailsWindowMono slotDetailWindow;
        public static GameObject notifyHolder;

        public static GameSpeedManager gameSpeedManager = new GameSpeedManager();
        public static GameSceneManager gameSceneStateManager = new GameSceneManager();
        public static GameSceneDataManager gameSceneDataManager = new GameSceneDataManager();
        protected override void OnInit()
        {

        }

    
    }

}