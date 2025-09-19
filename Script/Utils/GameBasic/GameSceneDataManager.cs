using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class GameSceneDataManager 
    {
        public GameLoadLegacyDataArgs waitingLoadLegacyData;
        public GameEningDataArgs willEndingData;
    }
    public class GameLoadLegacyDataArgs
    {
        public bool isLoadSaveFile = false;
        public int saveFilePlace = -1;
        public string legacyKey = "DefaultLegacy";

    }
    public class GameEningDataArgs
    {
        public string endingKey;
    }
}