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
                    // 标准的2卡牌事件
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
                    recipeDecks = new List<AbstractDeck>()
                    {
                        new AbstractDeck()
                        {
                            loadData = new Dictionary<string, int>()
                            {
                                {"DeckDrawTestCard",1 }
                            },
                            resetonexhaustion = true
                            
                        }
                    },
                    deckeffects = new Dictionary<string, int>()
                    {
                        {"TestDeck" ,1 }
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
                    },
                    ending = "DefaultEnding"
                }},
                {"TestRecipeLv3",new AbstractRecipe(){
                    // 2个默认卡牌结算时触发的结果
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
                    // 事件执行时卡槽放入卡牌出发的结果
                    index = 7,
                    stringIndex = "AddTestRecipe",
                    label = "添加替代测试事件" ,
                    description = "这重历史变得更强壮了?",
                    finishedLabel = "添加替代测试事件触发",
                    finishedDescription = "历史向上擢升",
                    isStartable = false,
                    requireElementDictionary = new Dictionary<string, int>()
                    {
                        { "TestCard" , 3 }
                    },
                    recipeLinker = new RecipeChainTrigger()
                    {
                        triggerNodes = new List<RecipeTriggerNode>()
                        {
                            new RecipeTriggerNode()
                            {
                                targetRecipeGroup = "DefaultGroup",
                                targetRecipe = "ConnectTestRecipe",
                                isAdditional = true,
                                requipeAspects = new Dictionary<string, int>()
                                {
                                    { "TestCard" , 2 }
                                }
                            }
                        }
                    },
                }},
                {"ConnectTestRecipe",new AbstractRecipe(){
                    // 在原有事件上触发的不结束连锁事件
                    index = 8,
                    stringIndex = "ConnectTestRecipe",
                    label = "连锁测试事件" ,
                    description = "这重历史正朝着更远的可能性延伸着?",
                    finishedDescription = "历史长亘不息。",
                    finishedLabel = "事件连锁触发",
                    isStartable = false,
                    isCreatable = false,
                    warpup = 15,
                    requireElementDictionary = new Dictionary<string, int>()
                    {
                        { "TestCard" , 2 }
                    },
                    
                }},
                {"DoubleDefaultRecipe",new AbstractRecipe(){
                    // 在原有事件上触发的不结束连锁事件
                    index = 9,
                    stringIndex = "DoubleDefaultRecipe",
                    label = "二连默认测试事件" ,
                    description = "这重历史会前往分裂的哪一支?",
                    finishedDescription = "历史分裂。",
                    finishedLabel = "又一重历史在此刻开始转动",
                    warpup = 10,
                    requireElementDictionary = new Dictionary<string, int>()
                    {
                        { "DefaultCard" , 2 }
                    },
                    recipeLinker = new RecipeChainTrigger()
                    {
                        triggerNodes = new List<RecipeTriggerNode>()
                        {
                            new RecipeTriggerNode()
                            {
                                targetRecipeGroup = "DefaultGroup",
                                targetRecipe = "AddDoubleDefaultRecipe",
                                isAdditional = true,
                                additionalVerb = "AddTestVerb",
                                requipeAspects = new Dictionary<string, int>()
                                {
                                    { "DefaultCard" , 2 }
                                }
                            }
                        }
                    },

                }},
                {"AddDoubleDefaultRecipe",new AbstractRecipe(){
                    // 在原有事件上触发的不结束连锁事件
                    index = 10,
                    stringIndex = "AddDoubleDefaultRecipe",
                    label = "新建二连默认测试事件" ,
                    description = "这重历史尚未成型...",
                    finishedDescription = "历史成型。",
                    finishedLabel = "这重历史回到了它应有的位置。",
                    maxWarpup = 30,
                    isCreatable = false,
                    requireElementDictionary = new Dictionary<string, int>()
                    {
                        { "DefaultCard" , 2 }
                    },

                }},
                 {"RecipeTriggerTestRecipe",new AbstractRecipe(){
                    // 在原有事件上触发的不结束连锁事件
                    index = 11,
                    stringIndex = "RecipeTriggerTestRecipe",
                    label = "事件XTrigger测试事件" ,
                    description = "这个事件用于测试在事件具有特定性相时产生连锁事件的功能。",
                    finishedDescription = "事件XTrigger测试成功执行完毕。",
                    finishedLabel = "事件XTrigger测试事件执行完毕。",
                    maxWarpup = 5,
                    isCreatable = true,
                    requireElementDictionary = new Dictionary<string, int>()
                    {
                        { "StoryCard" , 2 }
                    },

                }},
                {"RecipeTriggerTestLinkRecipe",new AbstractRecipe(){
                    // 在原有事件上触发的不结束连锁事件
                    index = 12,
                    stringIndex = "RecipeTriggerTestLinkRecipe",
                    label = "事件XTrigger测试事件-连锁效果" ,
                    description = "这个事件用于测试在事件具有特定性相时产生连锁事件后，所产生的连锁事件是否正确的功能。",
                    finishedDescription = "事件XTrigger的连锁事件测试成功执行完毕。",
                    finishedLabel = "事件XTrigger的连锁事件测试事件执行完毕。",
                    maxWarpup = 5,
                    isCreatable = false,
                    //requireElementDictionary = new Dictionary<string, int>()
                    //{
                    //    { "StoryCard" , 2 }
                    //},

                }},
            }
        },
        {"LifeCoreGroup",new Dictionary<string,AbstractRecipe>()
            {
                 {"LifeCoreBasicRecipe",new AbstractRecipe(){
                    index = 0,
                    stringIndex = "LifeCoreBasicRecipe",
                    label = "维生反应堆" ,
                    description = "制造电力，转化食物，人与非人的能源之所。",
                    isStartable = false,
                }},
            }
        },
        //{"LifeCoreGroup",new Dictionary<string,AbstractRecipe>()
        //    {
        //         {"LifeCoreBasicRecipe",new AbstractRecipe(){
        //            index = 0,
        //            stringIndex = "LifeCoreBasicRecipe",
        //            label = "维生反应堆" ,
        //            description = "制造电力，转化食物，人与非人的能源之所。",
        //            isStartable = false,
        //        }},
        //    }
        //},
        {"SpaceShipGroup",new Dictionary<string,AbstractRecipe>()
            {
                 {"SpaceShipBasicRecipe",new AbstractRecipe(){
                    index = 0,
                    stringIndex = "SpaceShipBasicRecipe",
                    label = "已损坏的\"L8级远行者飞船\"" ,
                    description = "一艘主要功能设施几乎损坏殆尽的飞船。如果我有足够的资源和对应的知识，也许我能修好它或者它的一部分。",
                    isStartable = false,
                }},
                 {"Falling",new AbstractRecipe(){
                    index = 1,
                    stringIndex = "Falling",
                    label = "坠毁!" ,
                    description = "风，光，火焰，太阳，声音。剧烈的失重感冲击着我的大脑，我正向着某处未知的坐标坠落。",
                    isStartable = false,
                    isCreatable = false,
                }},
                {"Falled",new AbstractRecipe(){
                    index = 2,
                    stringIndex = "Falled",
                    label = "紧急迫降!" ,
                    description = "幸好，飞船没有落入某片恶地或者海洋，我成功从坠落余波中活了下来。",
                    isStartable = false,
                    isCreatable = false,
                }},
                {"FirstCheck",new AbstractRecipe(){
                    index = 3,
                    stringIndex = "FirstCheck",
                    label = "勘察" ,
                    description = "在一切开始之前，我需要先搞清楚情况如何：我落在何处，飞船情况怎样，还有多少可用资源。",
                    isStartable = false,
                    isCreatable = false,
                }},
            }
        }
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