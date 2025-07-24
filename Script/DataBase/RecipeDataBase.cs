using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class RecipeDataBase
    {
        // 事件数据库
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
            //Debug.Log("尝试获取事件" + key + "的" + group);
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
                    label = "默认事件" ,
                    description = "一个默认事件",
                    isStartable = false,
                }},

                 {"ErrorGroupKey",new AbstractRecipe(){
                    index = 2,
                    stringIndex = "ErrorGroupKey",
                    label = "错误：不存在的事件组代码" ,
                    description = "输入了一个不存在的事件组代码，请检查输入?",
                    isStartable = false,

                }},
                 {"ErrorRecipeKey",new AbstractRecipe(){
                    index = 3,
                    stringIndex = "ErrorRecipeKey",
                    label = "错误：正确的事件组，不存在的事件代码" ,
                    description = "事件组正确，请检查是否输入了错误的事件索引?",
                    isStartable = false,

                }},
                 {"ErrorSituationKey",new AbstractRecipe(){
                    index = 4,
                    stringIndex = "ErrorSituationKey",
                    label = "错误：不正确的事件容器代码(这个事件为提示性默认事件)" ,
                    description = "请检查是否输入了不正确的事件容器代码?",
                    isStartable = false,

                }},
                 {"TestRecipeLv0",new AbstractRecipe(){
                    index = 6,
                    stringIndex = "TestRecipeLv0",
                    label = "未启用的测试事件" ,
                    description = "没有任何事件被选定时的模棱两可。",
                    isStartable = false,

                }},
                {"TestRecipe",new AbstractRecipe(){
                    index = 1,
                    stringIndex = "TestRecipe",
                    label = "测试事件" ,
                    description = "这重历史会落入哪一种可能性?",
                    isStartable = false,

                    requireElementDictionary = new Dictionary<string, int>()
                    {
                        { "TestCard" , 1 }
                    }
                }},
                {"TestRecipeLv2",new AbstractRecipe(){
                    index = 5,
                    stringIndex = "TestRecipeLv2",
                    label = "强化测试事件" ,
                    description = "这重历史将落入更确定的一种可能性中。",
                    isStartable = true,
                    excutingDescription = "历史正在前进...",
                    excutingLabel = "选择既定的历史",
                    finishedDescription = "历史已经修正。",
                    finishedLabel = "诸史已定",
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
                        // 操作 1：移除一张 TestCard
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

                        // 操作 2：为一张 TestCard 添加 1 点 TestAspect
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

                        // 操作 3：添加一张 DefaultCard
                        new CardEffect
                        {
                            filter = null, // 不需要筛选
                            maxTargets = 0, // 可省略，结构型操作不走筛选目标流程
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
                    label = "替代测试事件" ,
                    description = "这重历史也许会有另一枝可能性?",
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
                    label = "添加替代测试事件" ,
                    description = "这重历史变得更强壮了?",
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
        //            label = "默认事件" ,
        //            lore = "一个默认事件",
        //        }},
        //    }
        //}
    };
    }
}