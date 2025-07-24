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
        // 事件容器中当前的事件执行状态
        public SituationState situationState = SituationState.Prepare;
        public enum SituationState
        {
            Prepare, Excuting, WaitingForCollect, None
        }
        // 事件Log元素的结构
        public struct RecipeTextElement { public string recipeLabel, recipeDescription; public RecipeTextElement(string label, string description) { recipeLabel = label; recipeDescription = description; } }

        // 2.1基本事件所属事件组及事件
        public string basicRecipeGroupKey;
        public string basicRecipeKey;
        // 可能的事件组
        public List<string> possibleRecipeGroupKeyList = new List<string>();
        public AbstractRecipe basicRecipe = null;
        public AbstractRecipe currentRecipe = null;
        // 这是一个纯监视用变量 实用时需要创建复制
        public AbstractRecipe possibleRecipe = null;
        public List<RecipeTextElement> recipeTextList = new List<RecipeTextElement>();

        public AbstractSituation GetNewCopy(AbstractSituation element)
        {
            //Debug.Log("创建容器复制" + element.label);
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
            // 独有参数
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