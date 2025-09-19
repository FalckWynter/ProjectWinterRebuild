using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class SituationDataBase
    {
        // 事件容器数据库
        public static AbstractSituation TryGetSituation(string key)
        {
            //Debug.Log("调用索引" + key + "存在情况" + situationDataBase.ContainsKey(key));
            if (situationDataBase.ContainsKey(key))
                return situationDataBase[key];
            return situationDataBase["DefaultSituation"];
        }
        public static Dictionary<string, AbstractSituation> situationDataBase = new Dictionary<string, AbstractSituation>()
    {
         {"DefaultSituation",new AbstractSituation(){
            index = 0,
            stringIndex = "DefaultSituation",
            label = "默认事件容器" ,
            lore = "默认事件容器",
            icon = ImageDataBase.TryGetImage("DefaultSituation"),
            basicRecipeGroupKey = "DefaultGroup",
            basicRecipeKey = "DefaultRecipe",
        }},
         {"TestSituation",new AbstractSituation(){
            index = 1,
            stringIndex = "TestSituation",
            label = "测试事件容器" ,
            lore = "测试事件容器",
            icon = ImageDataBase.TryGetImage("TestImage1"),
            basicRecipeGroupKey = "DefaultGroup",
            basicRecipeKey = "TestRecipeLv0",
            possibleRecipeGroupKeyList = new List<string>(){"DefaultGroup"}
        }},
         {"ErrorSituation",new AbstractSituation(){
            index = 2,
            stringIndex = "ErrorSituation",
            label = "错误的事件容器" ,
            lore = "输入了错误的或不存在的事件容器代码",
            icon = ImageDataBase.TryGetImage("DefaultSituation"),
            basicRecipeGroupKey = "DefaultGroup",
            basicRecipeKey = "ErrorSituationKey",
        }},
         {"LandingShipSituation",new AbstractSituation(){
            index = 3,
            stringIndex = "LandingShipSituation",
            label = "逃生舱事件容器" ,
            lore = "逃生舱行动框所具有的事件容器",
            //icon = ImageDataBase.TryGetImage("DefaultSituation"),
            iconName = "moon",
            basicRecipeGroupKey = "LandingShipGroup",
            basicRecipeKey = "LandingShipBasicRecipe",
        }},
         {"SpaceShipSituation",new AbstractSituation(){
            index = 4,
            stringIndex = "SpaceShipSituation",
            label = "飞船事件容器" ,
            lore = "飞船行动框所具有的事件容器",
            //icon = ImageDataBase.TryGetImage("DefaultSituation"),
            iconName = "work",
            basicRecipeGroupKey = "SpaceShipGroup",
            basicRecipeKey = "SpaceShipBasicRecipe",
            possibleRecipeGroupKeyList = new List<string>(){ "SpaceShipGroup" }

        }},
    };
    }
}