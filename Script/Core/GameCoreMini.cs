using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class GameCoreMini : MonoBehaviour, IController
    {
        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }

        // Start is called before the first frame update
        void Start()
        {
            this.GetArchitecture();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}