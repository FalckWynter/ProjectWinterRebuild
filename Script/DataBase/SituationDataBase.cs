using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class SituationDataBase
    {
        // �¼��������ݿ�
        public static AbstractSituation TryGetSituation(string key)
        {
            //Debug.Log("��������" + key + "�������" + situationDataBase.ContainsKey(key));
            if (situationDataBase.ContainsKey(key))
                return situationDataBase[key];
            return situationDataBase["DefaultSituation"];
        }
        public static Dictionary<string, AbstractSituation> situationDataBase = new Dictionary<string, AbstractSituation>()
    {
         {"DefaultSituation",new AbstractSituation(){
            index = 0,
            stringIndex = "DefaultSituation",
            label = "Ĭ���¼�����" ,
            lore = "Ĭ���¼�����",
            icon = ImageDataBase.TryGetImage("DefaultSituation"),
            basicRecipeGroupKey = "DefaultGroup",
            basicRecipeKey = "DefaultRecipe",
        }},
         {"TestSituation",new AbstractSituation(){
            index = 1,
            stringIndex = "TestSituation",
            label = "�����¼�����" ,
            lore = "�����¼�����",
            icon = ImageDataBase.TryGetImage("TestImage1"),
            basicRecipeGroupKey = "DefaultGroup",
            basicRecipeKey = "TestRecipeLv0",
            possibleRecipeGroupKeyList = new List<string>(){"DefaultGroup"}
        }},
         {"ErrorSituation",new AbstractSituation(){
            index = 2,
            stringIndex = "ErrorSituation",
            label = "������¼�����" ,
            lore = "�����˴���Ļ򲻴��ڵ��¼���������",
            icon = ImageDataBase.TryGetImage("DefaultSituation"),
            basicRecipeGroupKey = "DefaultGroup",
            basicRecipeKey = "ErrorSituationKey",
        }},
         {"LandingShipSituation",new AbstractSituation(){
            index = 3,
            stringIndex = "LandingShipSituation",
            label = "�������¼�����" ,
            lore = "�������ж��������е��¼�����",
            //icon = ImageDataBase.TryGetImage("DefaultSituation"),
            iconName = "moon",
            basicRecipeGroupKey = "LandingShipGroup",
            basicRecipeKey = "LandingShipBasicRecipe",
        }},
         {"SpaceShipSituation",new AbstractSituation(){
            index = 4,
            stringIndex = "SpaceShipSituation",
            label = "�ɴ��¼�����" ,
            lore = "�ɴ��ж��������е��¼�����",
            //icon = ImageDataBase.TryGetImage("DefaultSituation"),
            iconName = "work",
            basicRecipeGroupKey = "SpaceShipGroup",
            basicRecipeKey = "SpaceShipBasicRecipe",
            possibleRecipeGroupKeyList = new List<string>(){ "SpaceShipGroup" }

        }},
    };
    }
}