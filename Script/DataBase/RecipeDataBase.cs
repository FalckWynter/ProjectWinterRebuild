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
                    // ��׼��2�����¼�
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
                    },
                    ending = "DefaultEnding"
                }},
                {"TestRecipeLv3",new AbstractRecipe(){
                    // 2��Ĭ�Ͽ��ƽ���ʱ�����Ľ��
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
                    // �¼�ִ��ʱ���۷��뿨�Ƴ����Ľ��
                    index = 7,
                    stringIndex = "AddTestRecipe",
                    label = "�����������¼�" ,
                    description = "������ʷ��ø�ǿ׳��?",
                    finishedLabel = "�����������¼�����",
                    finishedDescription = "��ʷ����ߪ��",
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
                    // ��ԭ���¼��ϴ����Ĳ����������¼�
                    index = 8,
                    stringIndex = "ConnectTestRecipe",
                    label = "���������¼�" ,
                    description = "������ʷ�����Ÿ�Զ�Ŀ�����������?",
                    finishedDescription = "��ʷ��ب��Ϣ��",
                    finishedLabel = "�¼���������",
                    isStartable = false,
                    isCreatable = false,
                    warpup = 15,
                    requireElementDictionary = new Dictionary<string, int>()
                    {
                        { "TestCard" , 2 }
                    },
                    
                }},
                {"DoubleDefaultRecipe",new AbstractRecipe(){
                    // ��ԭ���¼��ϴ����Ĳ����������¼�
                    index = 9,
                    stringIndex = "DoubleDefaultRecipe",
                    label = "����Ĭ�ϲ����¼�" ,
                    description = "������ʷ��ǰ�����ѵ���һ֧?",
                    finishedDescription = "��ʷ���ѡ�",
                    finishedLabel = "��һ����ʷ�ڴ˿̿�ʼת��",
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
                    // ��ԭ���¼��ϴ����Ĳ����������¼�
                    index = 10,
                    stringIndex = "AddDoubleDefaultRecipe",
                    label = "�½�����Ĭ�ϲ����¼�" ,
                    description = "������ʷ��δ����...",
                    finishedDescription = "��ʷ���͡�",
                    finishedLabel = "������ʷ�ص�����Ӧ�е�λ�á�",
                    maxWarpup = 30,
                    isCreatable = false,
                    requireElementDictionary = new Dictionary<string, int>()
                    {
                        { "DefaultCard" , 2 }
                    },

                }},
                 {"RecipeTriggerTestRecipe",new AbstractRecipe(){
                    // ��ԭ���¼��ϴ����Ĳ����������¼�
                    index = 11,
                    stringIndex = "RecipeTriggerTestRecipe",
                    label = "�¼�XTrigger�����¼�" ,
                    description = "����¼����ڲ������¼������ض�����ʱ���������¼��Ĺ��ܡ�",
                    finishedDescription = "�¼�XTrigger���Գɹ�ִ����ϡ�",
                    finishedLabel = "�¼�XTrigger�����¼�ִ����ϡ�",
                    maxWarpup = 5,
                    isCreatable = true,
                    requireElementDictionary = new Dictionary<string, int>()
                    {
                        { "StoryCard" , 2 }
                    },

                }},
                {"RecipeTriggerTestLinkRecipe",new AbstractRecipe(){
                    // ��ԭ���¼��ϴ����Ĳ����������¼�
                    index = 12,
                    stringIndex = "RecipeTriggerTestLinkRecipe",
                    label = "�¼�XTrigger�����¼�-����Ч��" ,
                    description = "����¼����ڲ������¼������ض�����ʱ���������¼����������������¼��Ƿ���ȷ�Ĺ��ܡ�",
                    finishedDescription = "�¼�XTrigger�������¼����Գɹ�ִ����ϡ�",
                    finishedLabel = "�¼�XTrigger�������¼������¼�ִ����ϡ�",
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
                    label = "ά����Ӧ��" ,
                    description = "���������ת��ʳ�������˵���Դ֮����",
                    isStartable = false,
                }},
            }
        },
        //{"LifeCoreGroup",new Dictionary<string,AbstractRecipe>()
        //    {
        //         {"LifeCoreBasicRecipe",new AbstractRecipe(){
        //            index = 0,
        //            stringIndex = "LifeCoreBasicRecipe",
        //            label = "ά����Ӧ��" ,
        //            description = "���������ת��ʳ�������˵���Դ֮����",
        //            isStartable = false,
        //        }},
        //    }
        //},
        {"SpaceShipGroup",new Dictionary<string,AbstractRecipe>()
            {
                 {"SpaceShipBasicRecipe",new AbstractRecipe(){
                    index = 0,
                    stringIndex = "SpaceShipBasicRecipe",
                    label = "���𻵵�\"L8��Զ���߷ɴ�\"" ,
                    description = "һ����Ҫ������ʩ�����𻵴����ķɴ�����������㹻����Դ�Ͷ�Ӧ��֪ʶ��Ҳ�������޺�����������һ���֡�",
                    isStartable = false,
                }},
                 {"Falling",new AbstractRecipe(){
                    index = 1,
                    stringIndex = "Falling",
                    label = "׹��!" ,
                    description = "�磬�⣬���棬̫�������������ҵ�ʧ�ظг�����ҵĴ��ԣ���������ĳ��δ֪������׹�䡣",
                    isStartable = false,
                    isCreatable = false,
                }},
                {"Falled",new AbstractRecipe(){
                    index = 2,
                    stringIndex = "Falled",
                    label = "�����Ƚ�!" ,
                    description = "�Һã��ɴ�û������ĳƬ��ػ��ߺ����ҳɹ���׹���ನ�л���������",
                    isStartable = false,
                    isCreatable = false,
                }},
                {"FirstCheck",new AbstractRecipe(){
                    index = 3,
                    stringIndex = "FirstCheck",
                    label = "����" ,
                    description = "��һ�п�ʼ֮ǰ������Ҫ�ȸ���������Σ������ںδ����ɴ�������������ж��ٿ�����Դ��",
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
        //            label = "Ĭ���¼�" ,
        //            lore = "һ��Ĭ���¼�",
        //        }},
        //    }
        //}
    };
    }
}