using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class EndingSceneLoaderMono : MonoBehaviour,IController
    {
        private bool isExecutedSceneLoad = false;
        private int pendingCount = 0;
        public EndingSceneGameCoreMono endingSceneGameCoreMono;
        public EndingPanelMono endingPanelMono;
        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }


        void Start()
        {
            DoSceneLoad();
        }

        public void DoSceneLoad()
        {
            if (isExecutedSceneLoad) return;
            isExecutedSceneLoad = true;

            LoadEndingPanelData();
            // 如果没有任何加载步骤，直接进入场景完成
            if (pendingCount == 0)
            {
                OnSceneLoadFinished();
            }
        }
        public void LoadEndingPanelData()
        {
            var data = UtilModel.gameSceneDataManager.willEndingData;
            if (data == null) return;
            var ending = EndingDataBase.TryGetEnding(data.endingKey);
            if (ending == null) return;
            endingPanelMono.SetEndingData(ending);

        }
        private void OnStepFinished()
        {
            pendingCount--;
            if (pendingCount <= 0)
            {
                OnSceneLoadFinished();
            }
        }

        private void OnSceneLoadFinished()
        {
            endingSceneGameCoreMono.SceneLoadFinished();
        }

    }
}