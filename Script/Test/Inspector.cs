using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class Inspector : MonoBehaviour, IController
    {
        public List<ICanDragComponentMono> dragmonoList;
        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }

        // Start is called before the first frame update
        void Start()
        {
            dragmonoList = this.GetModel<GameModel>().dragMonoList;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}