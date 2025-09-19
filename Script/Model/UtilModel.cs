using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class UtilModel : AbstractModel
    {
        // 通用数据库 存储一些游戏底层逻辑的数值
        // 游戏事件逻辑的计算倍率
        public static float GameLogicSpeed => (float)gameSpeedManager.CurrentSpeed;
        //private static float gameLogicSpeed = 1f;
        // 游戏逻辑时间经过计算倍率缩放后的deltaTime
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