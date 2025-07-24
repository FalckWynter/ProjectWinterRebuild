using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractSituation : ICopyAble<AbstractSituation>
    {
        public static int sortIndex = 1;
        public int index, createIndex;
        public static int situationCreateIndex = 0;
        public string stringIndex, label, lore, comment;
        public string iconName { set { iconname = value; } get { if (iconname == "") return stringIndex; return iconname; } }
        private string iconname = "";
        public Sprite icon { set { artwork = value; } get { if (artwork == null) artwork = ImageDataBase.TryGetImage(iconName); return artwork; } }
        private Sprite artwork;
        // �¼������е�ǰ���¼�ִ��״̬
        public SituationState situationState = SituationState.Prepare;
        public enum SituationState
        {
            Prepare, Excuting, WaitingForCollect, None
        }
        // �¼�LogԪ�صĽṹ
        public struct RecipeTextElement { public string recipeLabel, recipeDescription; public RecipeTextElement(string label, string description) { recipeLabel = label; recipeDescription = description; } }

        // 2.1�����¼������¼��鼰�¼�
        public string basicRecipeGroupKey;
        public string basicRecipeKey;
        // ���ܵ��¼���
        public List<string> possibleRecipeGroupKeyList = new List<string>();
        public AbstractRecipe basicRecipe = null;
        public AbstractRecipe currentRecipe = null;
        // ����һ���������ñ��� ʵ��ʱ��Ҫ��������
        public AbstractRecipe possibleRecipe = null;
        public List<RecipeTextElement> recipeTextList = new List<RecipeTextElement>();

        public AbstractSituation GetNewCopy(AbstractSituation element)
        {
            //Debug.Log("������������" + element.label);
            AbstractSituation retSituation = new AbstractSituation();
            retSituation.createIndex = situationCreateIndex++;
            retSituation.index = element.index;
            retSituation.stringIndex = element.stringIndex;
            retSituation.label = element.label;
            retSituation.lore = element.lore;
            if (element.icon == null)
                retSituation.icon = ImageDataBase.TryGetImage(element.iconName);/* ImageDataBase.imageDataBase[card.stringIndex];*/
            else
                retSituation.icon = element.icon;
            retSituation.basicRecipeKey = element.basicRecipeKey;
            retSituation.basicRecipeGroupKey = element.basicRecipeGroupKey;
            // ���в���
            retSituation.possibleRecipeGroupKeyList = new List<string>();
            foreach (string item in possibleRecipeGroupKeyList)
                retSituation.possibleRecipeGroupKeyList.Add(item);
            retSituation.recipeTextList = new List<RecipeTextElement>(element.recipeTextList);
            retSituation.basicRecipe = RecipeDataBase.TryGetRecipe(retSituation.basicRecipeKey, retSituation.basicRecipeGroupKey).GetNewCopy();
            retSituation.possibleRecipe = retSituation.basicRecipe;
            retSituation.currentRecipe = retSituation.basicRecipe;

            return retSituation;
        }

        public AbstractSituation GetNewCopy()
        {
            return GetNewCopy(this);
        }


    }
}