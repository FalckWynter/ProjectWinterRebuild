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
    public string legacyID = "";
    public string endingID = "";

    public IArchitecture GetArchitecture()
    {
        return ProjectWinterArchitecture.Interface;
    }
    public void CreateCard()
    {
        GameObject ob = this.GetSystem<UtilSystem>().CreateCardGameObject(CardDataBase.TryGetCard(cardID));
        this.GetSystem<GameSystem>().MoveCardToClosestNullGrid(ob.GetComponent<ITableElement>(), null);
    }
    public void CreateVerb()
    {
        GameObject ob = this.GetSystem<UtilSystem>().CreateVerbGameObject(VerbDataBase.TryGetVerb(verbID));
        this.GetSystem<GameSystem>().MoveCardToClosestNullGrid(ob.GetComponent<ITableElement>(), null,2);

    }
    public void CreateLegacy()
    {
        
    }

    public void CreateEnding()
    {

    }
}
