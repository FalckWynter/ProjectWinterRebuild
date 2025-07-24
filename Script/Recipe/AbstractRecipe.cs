using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractRecipe : ICopyAble<AbstractRecipe>, ICanBeEqualCompare<AbstractRecipe>
    {
        // 基本要素
        public static int sortIndex = 1;
        public int index, createIndex;
        public static int recipeCreateIndex = 0;
        public string stringIndex, label, description, comment;
        public string iconName { set { iconname = value; } get { if (iconname == "") return stringIndex; return iconname; } }
        private string iconname = "";
        public Sprite icon { set { artwork = value; } get { if (artwork == null) artwork = ImageDataBase.TryGetImage(iconName); return artwork; } }
        private Sprite artwork;
        // 2.1节 查找指定的事件
        public Dictionary<string, int> requireElementDictionary = new Dictionary<string, int>();

        // 2.2节 执行事件的要素
        //// 执行时显示的描述和事件名称，如果excutingLabel为空则保留原来的label
        //public string excutingDescription = "";
        //private string excutingdescription = "";
        //public string excutingLabel { set { excutinglabel = value;} get { if (excutinglabel == null || excutinglabel == "") return label; else return excutinglabel; } }
        //private string excutinglabel = "";
        private string excutinglabel = "";
        public string excutingLabel { get => string.IsNullOrEmpty(excutinglabel) ? label : excutinglabel; set => excutinglabel = value; }

        private string excutingdescription = "";
        public string excutingDescription { get => string.IsNullOrEmpty(excutingdescription) ? description : excutingdescription; set => excutingdescription = value; }
        // 执行完毕后显示的描述和事件名称
        private string finishedlabel = "";
        public string finishedLabel { get => string.IsNullOrEmpty(finishedlabel) ? label : finishedlabel; set => finishedlabel = value; }

        private string finisheddescription = "";
        public string finishedDescription { get => string.IsNullOrEmpty(finisheddescription) ? description : finisheddescription; set => finisheddescription = value; }


        // 状态变量
        // 是否能被执行
        public bool isStartable = true;
        // 是否能被行动框找到并创建
        public bool isCreatable = true;
        // 是否载入时就执行
        public bool isInitExcuting = false;
        // 执行事件的用时和剩余用时
        public float warpup = 5, maxWarpup = 5;

        // 行动变量
        // 是否正在执行
        public bool isExcuting { set { /*Debug.Log("事件" + createIndex + "正在执行修改到" + (value).ToString());*/ isexcuting = value; } get { return isexcuting; } }
        public bool isexcuting = false;
        // 是否已经执行完毕
        public bool isFinished { set { /*Debug.Log( "事件" + createIndex + "结算完成修改到" + (value).ToString());*/  isfinished = value; } get { return isfinished; } }
        private bool isfinished = false;
        // 事件的执行状态

        // 2.3节加入
        // 事件结算时产生的卡牌修饰
        public List<CardEffect> effects = new List<CardEffect>();

        // 2.3节加入
        // 事件开始执行时携带的卡槽
        public List<AbstractSlot> recipeSlots = new List<AbstractSlot>();

        // 2.3.2.1节 事件状态与连锁加入
        // 事件连锁器，用于事件结算时计算要连锁发生的事情
        public RecipeChainTrigger recipeLinker;
        // 事件带有的性相
        public Dictionary<string, int> recipeAspectDictionary = new Dictionary<string, int>();
        public RecipeExcutingState recipeExcutingState
        {
            get
            {
                if (isFinished) return RecipeExcutingState.Finished;
                if (isExcuting) return RecipeExcutingState.Excuting;
                return RecipeExcutingState.Prepare;
            }
        }
        public enum RecipeExcutingState { Prepare, Excuting, Finished, None }


        public AbstractRecipe GetNewCopy(AbstractRecipe recipe)
        {
            AbstractRecipe retRecipe = new AbstractRecipe();
            retRecipe.createIndex = recipeCreateIndex++;
            retRecipe.index = recipe.index;
            retRecipe.stringIndex = recipe.stringIndex;
            retRecipe.label = recipe.label;
            retRecipe.description = recipe.description;
            if (recipe.icon == null)
                retRecipe.icon = ImageDataBase.TryGetImage(recipe.iconName);/* ImageDataBase.imageDataBase[card.stringIndex];*/
            else
                retRecipe.icon = recipe.icon;
            retRecipe.requireElementDictionary = recipe.requireElementDictionary;
            retRecipe.isStartable = recipe.isStartable;
            retRecipe.isCreatable = recipe.isCreatable;
            retRecipe.isInitExcuting = recipe.isInitExcuting;
            retRecipe.warpup = recipe.warpup;
            retRecipe.maxWarpup = recipe.maxWarpup;
            retRecipe.isExcuting = recipe.isExcuting;
            retRecipe.isFinished = recipe.isFinished;
            retRecipe.excutingDescription = recipe.excutingDescription;
            retRecipe.excutingLabel = recipe.excutingLabel;
            retRecipe.recipeSlots = new List<AbstractSlot>();
            foreach (AbstractSlot slot in recipe.recipeSlots)
            {
                AbstractSlot item = slot.GetNewCopy();
                item.isRecipeSlot = true;
                retRecipe.recipeSlots.Add(item);
            }
            // 这个变量传引用，因为只需要方法不需要改值
            retRecipe.recipeLinker = recipe.recipeLinker;
            retRecipe.recipeAspectDictionary = new Dictionary<string, int>(recipe.recipeAspectDictionary);
            return retRecipe;
        }

        public AbstractRecipe GetNewCopy()
        {
            return GetNewCopy(this);
        }
        public bool IsEqualTo(AbstractRecipe other)
        {
            if (other.stringIndex == this.stringIndex)
                return true;
            else
                return false;
        }
    }
}