using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using PlentyFishFramework;

public class GameCore : MonoBehaviour,IController
{
    UtilSystem utilSystem;
    public IArchitecture GetArchitecture()
    {
        return ProjectWinterArchitecture.Interface;
    }

    // Start is called before the first frame update
    void Start()
    {
        utilSystem = this.GetSystem<UtilSystem>();
        utilSystem.CreateCardGameObject(CardDataBase.TryGetCard("DefaultCard"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
