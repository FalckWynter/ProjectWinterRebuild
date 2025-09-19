using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class UIManagerMono : MonoBehaviour, IController
    {
        public GameObject notifyHolder;
        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
        void Start()
        {
            UtilModel.notifyHolder = notifyHolder;
        }


    }
}