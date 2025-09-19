using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class GameSceneGameCore : MonoBehaviour
    {
        public string backgroundMusicName = "TheRoadToBrancrug";
        public GameObject cardParent, dragParent, panelParent, cardSpawnPlace,asyncSceneLoader;
        private void Awake()
        {
            // ×¢²á»ù±¾ÄÚÈÝ
            UtilSystem.cardParent = cardParent;
            UtilSystem.dragParent = dragParent;
            UtilSystem.panelParent = panelParent;
            UtilSystem.cardSpawnPlace = cardSpawnPlace;
            UtilSystem.asyncSceneLoader = asyncSceneLoader;
        }
        public void SceneLoadFinished()
        {
            AudioManagerSystem.PlayMusic(backgroundMusicName);
        }
    }
}