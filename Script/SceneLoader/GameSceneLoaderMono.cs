using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class GameSceneLoaderMono : MonoBehaviour,IController
    {
        private bool isExecutedSceneLoad = false;
        private int pendingCount = 0;
        public GameSceneGameCore gameSceneGameCore;
        UtilSystem utilSystem;
        public Transform dropZoneParent;

        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }

        void Start()
        {
            utilSystem = this.GetSystem<UtilSystem>();
            DoSceneLoad();
        }

        public void DoSceneLoad()
        {
            if (isExecutedSceneLoad) return;
            isExecutedSceneLoad = true;

            this.GetSystem<GameSystem>().InitTable();
            CreateDropZone();
            LoadLegacyData();
            // ���û���κμ��ز��裬ֱ�ӽ��볡�����
            if (pendingCount == 0)
            {
                OnSceneLoadFinished();
            }
        }
        public void CreateDropZone()
        {
            GameModel.dropZoneParent = dropZoneParent;
            UtilSystem.CreateCardDropZone("CardDefault",new Vector2(960,540));
            UtilSystem.CreateVerbDropZone("VerbDefault",new Vector2(1027.5f,770));


        }
        public void LoadLegacyData()
        {
            var data = UtilModel.gameSceneDataManager.waitingLoadLegacyData;
            if (data == null)
            {
                Debug.Log("û����������");
                return;
            }

            Debug.Log("����Ŀ��" + data.legacyKey);

            if (data.isLoadSaveFile)
            {

            }
            else
            {
                utilSystem.LoadLegacy(data.legacyKey);
            }

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
            gameSceneGameCore.SceneLoadFinished();
        }
    }
}