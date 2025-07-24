using PlentyFishFramework;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCreateEntityMono : MonoBehaviour, IController
{
    // 创建实体脚本 用于向指定位置创建实体
    public string cardID = "";
    public string verbID = "";
    public IArchitecture GetArchitecture()
    {
        return ProjectWinterArchitecture.Interface;
    }
    public void CreateCard()
    {
        this.GetSystem<UtilSystem>().CreateCardGameObject(CardDataBase.TryGetCard(cardID));
    }
    public void CreateVerb()
    {
        this.GetSystem<UtilSystem>().CreateVerbGameObject(VerbDataBase.TryGetVerb(verbID));

    }
}
