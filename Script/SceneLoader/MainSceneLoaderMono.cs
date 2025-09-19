using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class MainSceneLoaderMono : MonoBehaviour,IController
    {
        private static bool isExecutedSceneLoad = false;
        private int pendingCount = 0;
        public bool isPlayQuotePanel = false;
        [Header("����")]
        public QuoteCanvasControllerMono quoteCanvas; // ���� Logo ���ſ�����
        public MainSceneGameCore mainSceneGameCore;

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

            // ע��������Ҫ�ļ��ز���
            RegisterQuotePanel();

            // ���û���κμ��ز��裬ֱ�ӽ��볡�����
            if (pendingCount == 0)
            {
                OnSceneLoadFinished();
            }
        }

        private void RegisterQuotePanel()
        {
            if (quoteCanvas == null) return;
            if (isPlayQuotePanel == false) return;
            if (UtilModel.gameSceneStateManager.quoteScenePlayed == true) return;

            pendingCount++;
            quoteCanvas.onAllFinished += () =>
            {
                OnStepFinished();
            };
            quoteCanvas.PlaySequence();
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
            mainSceneGameCore.SceneLoadFinished();
        }
        //bool isExcutedSceneLoad = false;
        //public QuoteCanvasControllerMono quoteCanvas;
        //public MainSceneGameCore mainSceneGameCore;
        //public IArchitecture GetArchitecture()
        //{
        //    return ProjectWinterArchitecture.Interface;
        //}
        //// Start is called before the first frame update
        //void Start()
        //{
        //    DoSceneLoad();
        //}
        //public void DoSceneLoad()
        //{
        //    if (isExcutedSceneLoad) return;
        //    CheckQuotePanel();
        //}
        //public void CheckQuotePanel()
        //{
        //    if (UtilModel.gameSceneStateManager.quoteScenePlayed == true)
        //        return;
        //    quoteCanvas.onAllFinished += OnSceneLoadFinished;
        //    quoteCanvas.PlaySequence();
        //}
        //public void OnSceneLoadFinished()
        //{
        //    mainSceneGameCore.SceneLoadFinished();
        //}
        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}