using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class RecipeDataBase
    {
        // �¼����ݿ�
        public static string defaultGroupKey = "DefaultGroup";
        public static string errorGroupKey = "ErrorGroupKey";
        public static string defaultRecipeKey = "DefaultGroup";
        public static string errorRecipeKey = "ErrorRecipeKey";

        public static Dictionary<string, AbstractRecipe> TryGetRecipeGroup(string group)
        {
            if (recipeDataBase.ContainsKey(group))
            {
                return recipeDataBase[group];
            }
            else
            {
                return null;
            }
        }
        public static AbstractRecipe TryGetRecipe(string key, string group)
        {
            //Debug.Log("���Ի�ȡ�¼�" + key + "��" + group);
            if (recipeDataBase.ContainsKey(group))
            {
                if (recipeDataBase[group].ContainsKey(key))
                {
                    return recipeDataBase[group][key];
                }
                else
                {
                    return recipeDataBase[defaultGroupKey][errorGroupKey];

                }
            }
            else
            {
                return recipeDataBase[defaultGroupKey][errorRecipeKey];
            }
            //return recipeDataBase["DefaultGroup"]["DefaultCard"].GetNewCopy();
        }
        public static Dictionary<string, Dictionary<string, AbstractRecipe>> recipeDataBase = new Dictionary<string, Dictionary<string, AbstractRecipe>>()
    {
        {"DefaultGroup",new Dictionary<string,AbstractRecipe>()
            {
                 {"DefaultRecipe",new AbstractRecipe(){
                    index = 0,
                    stringIndex = "DefaultRecipe",
                    label = "Ĭ���¼�" ,
                    description = "һ��Ĭ���¼�",
                    isStartable = false,
                }},

                 {"ErrorGroupKey",new AbstractRecipe(){
                    index = 2,
                    stringIndex = "ErrorGroupKey",
                    label = "���󣺲����ڵ��¼������" ,
                    description = "������һ�������ڵ��¼�����룬��������?",
                    isStartable = false,

                }},
                 {"ErrorRecipeKey",new AbstractRecipe(){
                    index = 3,
                    stringIndex = "ErrorRecipeKey",
                    label = "������ȷ���¼��飬�����ڵ��¼�����" ,
                    description = "�¼�����ȷ�������Ƿ������˴�����¼�����?",
                    isStartable = false,

                }},
                 {"ErrorSituationKey",new AbstractRecipe(){
                    index = 4,
                    stringIndex = "ErrorSituationKey",
                    label = "���󣺲���ȷ���¼���������(����¼�Ϊ��ʾ��Ĭ���¼�)" ,
                    description = "�����Ƿ������˲���ȷ���¼���������?",
                    isStartable = false,

                }},
                 {"TestRecipeLv0",new AbstractRecipe(){
                    index = 6,
                    stringIndex = "TestRecipeLv0",
                    label = "δ���õĲ����¼�" ,
                    description = "û���κ��¼���ѡ��ʱ��ģ�����ɡ�",
                    isStartable = false,

                }},
                {"TestRecipe",new AbstractRecipe(){
                    index = 1,
                    stringIndex = "TestRecipe",
                    label = "�����¼�" ,
                    description = "������ʷ��������һ�ֿ�����?",
                    isStartable = false,

                    requireElementDictionary = new Dictionary<string, int>()
                    {
                        { "TestCard" , 1 }
                    }
                }},
                {"TestRecipeLv2",new AbstractRecipe(){
                    index = 5,
                    stringIndex = "TestRecipeLv2",
                    label = "ǿ�������¼�" ,
                    description = "������ʷ�������ȷ����һ�ֿ������С�",
                    isStartable = true,
                    excutingDescription = "��ʷ����ǰ��...",
                    excutingLabel = "ѡ��ȶ�����ʷ",
                    finishedDescription = "��ʷ�Ѿ�������",
                    finishedLabel = "��ʷ�Ѷ�",
                    requireElementDictionary = new Dictionary<string, int>()
                    {
                        { "TestCard" , 2 }
                    },
                    recipeSlots = new List<AbstractSlot>()
                    {
                        SlotDataBase.TryGetRecipeSlot("RecipeBringSlot")
                    },
                    recipeAspectDictionary = new Dictionary<string, int>()
                    {
                        {"DefaultAspect" , 1 }
                    },
                    recipeLinker = new RecipeChainTrigger()
                    {
                        triggerNodes = new List<RecipeTriggerNode>()
                        {
                            new RecipeTriggerNode()
                            {
                                targetRecipeGroup = "DefaultGroup",
                                targetRecipe = "TestRecipeLv3",
                                requipeAspects = new Dictionary<string, int>()
                                {
                                    { "DefaultCard" , 2 } 
                                }
                            }
                        }
                    },
                    effects = new List<CardEffect>()
                    {
                        // ���� 1���Ƴ�һ�� TestCard
                        new CardEffect
                        {
                            filter = new CardFilter
                            {
                                rules = new List<CardFilterRule>
                                {
                                    new CardFilterRule
                                    {
                                        type = "StringIndexEquals",
                                        key = "TestCard"
                                    }
                                }
                            },
                            maxTargets = 1,
                            actions = new List<CardEffectAction>
                            {
                                new CardEffectAction
                                {
                                    type = "RemoveCard"
                                }
                            }
                        },

                        // ���� 2��Ϊһ�� TestCard ��� 1 �� TestAspect
                        new CardEffect
                        {
                            filter = new CardFilter
                            {
                                rules = new List<CardFilterRule>
                                {
                                    new CardFilterRule
                                    {
                                        type = "StringIndexEquals",
                                        key = "TestCard"
                                    }
                                }
                            },
                            maxTargets = 1,
                            actions = new List<CardEffectAction>
                            {
                                new CardEffectAction
                                {
                                    type = "AddAspect",
                                    key = "TestAspect",
                                    value = 1
                                }
                            }
                        },

                        // ���� 3�����һ�� DefaultCard
                        new CardEffect
                        {
                            filter = null, // ����Ҫɸѡ
                            maxTargets = 0, // ��ʡ�ԣ��ṹ�Ͳ�������ɸѡĿ������
                            actions = new List<CardEffectAction>
                            {
                                new CardEffectAction
                                {
                                    type = "AddCard",
                                    key = "DefaultCard",
                                    value = 1
                                }
                            }
                        }
                    }
                }},
                {"TestRecipeLv3",new AbstractRecipe(){
                    index = 6,
                    stringIndex = "TestRecipeLv3",
                    label = "��������¼�" ,
                    description = "������ʷҲ�������һ֦������?",
                    isStartable = false,
                    isCreatable = false,
                    requireElementDictionary = new Dictionary<string, int>()
                    {
                        { "TestCard" , 1 }
                    },
                    
                    
                }},
                {"AddTestRecipeLv4",new AbstractRecipe(){
                    index = 7,
                    stringIndex = "AddTestRecipe",
                    label = "�����������¼�" ,
                    description = "������ʷ��ø�ǿ׳��?",
                    isStartable = false,
                    requireElementDictionary = new Dictionary<string, int>()
                    {
                        { "TestCard" , 3 }
                    },


                }},
            }
        },
        //{"TestGroup",new Dictionary<string,AbstractRecipe>()
        //    {
        //         {"TestRecipe",new AbstractRecipe(){
        //            index = 0,
        //            stringIndex = "DefaultRecipe",
        //            label = "Ĭ���¼�" ,
        //            lore = "һ��Ĭ���¼�",
        //        }},
        //    }
        //}
    };
    }
}