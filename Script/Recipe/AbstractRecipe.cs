using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractRecipe : ICopyAble<AbstractRecipe>, ICanBeEqualCompare<AbstractRecipe>
    {
        // ����Ҫ��
        public static int sortIndex = 1;
        public int index, createIndex;
        public static int recipeCreateIndex = 0;
        public string stringIndex, label, description, comment;
        public string iconName { set { iconname = value; } get { if (iconname == "") return stringIndex; return iconname; } }
        private string iconname = "";
        public Sprite icon { set { artwork = value; } get { if (artwork == null) artwork = ImageDataBase.TryGetImage(iconName); return artwork; } }
        private Sprite artwork;
        // 2.1�� ����ָ�����¼�
        public Dictionary<string, int> requireElementDictionary = new Dictionary<string, int>();

        // 2.2�� ִ���¼���Ҫ��
        //// ִ��ʱ��ʾ���������¼����ƣ����excutingLabelΪ������ԭ����label
        //public string excutingDescription = "";
        //private string excutingdescription = "";
        //public string excutingLabel { set { excutinglabel = value;} get { if (excutinglabel == null || excutinglabel == "") return label; else return excutinglabel; } }
        //private string excutinglabel = "";
        private string excutinglabel = "";
        public string excutingLabel { get => string.IsNullOrEmpty(excutinglabel) ? label : excutinglabel; set => excutinglabel = value; }

        private string excutingdescription = "";
        public string excutingDescription { get => string.IsNullOrEmpty(excutingdescription) ? description : excutingdescription; set => excutingdescription = value; }
        // ִ����Ϻ���ʾ���������¼�����
        private string finishedlabel = "";
        public string finishedLabel { get => string.IsNullOrEmpty(finishedlabel) ? label : finishedlabel; set => finishedlabel = value; }

        private string finisheddescription = "";
        public string finishedDescription { get => string.IsNullOrEmpty(finisheddescription) ? description : finisheddescription; set => finisheddescription = value; }


        // ״̬����
        // �Ƿ��ܱ�ִ��
        public bool isStartable = true;
        // �Ƿ��ܱ��ж����ҵ�������
        public bool isCreatable = true;
        // �Ƿ�����ʱ��ִ��
        public bool isInitExcuting = false;
        // ִ���¼�����ʱ��ʣ����ʱ
        public float warpup = 5, maxWarpup = 5;

        // �ж�����
        // �Ƿ�����ִ��
        public bool isExcuting { set { /*Debug.Log("�¼�" + createIndex + "����ִ���޸ĵ�" + (value).ToString());*/ isexcuting = value; } get { return isexcuting; } }
        public bool isexcuting = false;
        // �Ƿ��Ѿ�ִ�����
        public bool isFinished { set { /*Debug.Log( "�¼�" + createIndex + "��������޸ĵ�" + (value).ToString());*/  isfinished = value; } get { return isfinished; } }
        private bool isfinished = false;
        // �¼���ִ��״̬

        // 2.3�ڼ���
        // �¼�����ʱ�����Ŀ�������
        public List<CardEffect> effects = new List<CardEffect>();

        // 2.3�ڼ���
        // �¼���ʼִ��ʱЯ���Ŀ���
        public List<AbstractSlot> recipeSlots = new List<AbstractSlot>();

        // 2.3.2.1�� �¼�״̬����������
        // �¼��������������¼�����ʱ����Ҫ��������������
        public RecipeChainTrigger recipeLinker;
        // �¼����е�����
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
            // ������������ã���Ϊֻ��Ҫ��������Ҫ��ֵ
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