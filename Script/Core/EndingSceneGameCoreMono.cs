using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class EndingSceneGameCoreMono : MonoBehaviour
    {
        public string backgroundMusicName = "";

        public AsyncSceneLoader sceneLoader;
        public void SceneLoadFinished()
        {
            AudioManagerSystem.PlayMusic(backgroundMusicName);
        }

    }
}