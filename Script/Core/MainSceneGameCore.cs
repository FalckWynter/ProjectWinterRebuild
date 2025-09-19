using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlentyFishFramework
{
    public class MainSceneGameCore : MonoBehaviour,IController
    {
        public string backgroundMusicName = "";
        public AsyncSceneLoader sceneLoader;
        public LegacyPanelManagerMono legacyPanelManagerMono;
        private void Awake()
        {

        }
        public void SceneLoadFinished()
        {
            AudioManagerSystem.PlayMusic(backgroundMusicName);
        }
        public void StartGame()
        {
            UtilModel.gameSceneDataManager.waitingLoadLegacyData = new GameLoadLegacyDataArgs() {
                isLoadSaveFile = false,
                legacyKey = legacyPanelManagerMono.legacyList[legacyPanelManagerMono.selectLegacyPlace].stringIndex
            };
            sceneLoader.LoadGameScene();
        }
        public void ExitGame()
        {
            Application.Quit();
        }
        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
    }
}