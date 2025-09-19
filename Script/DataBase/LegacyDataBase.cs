using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class LegacyDataBase
    {
        public static AbstractLegacy TryGetLegacy(string key)
        {
            if (legacyDataBase.ContainsKey(key))
                return legacyDataBase[key];
            return legacyDataBase["DefaultLegacy"];
        }
        public static Dictionary<string, AbstractLegacy> legacyDataBase = new Dictionary<string, AbstractLegacy>()
        {
            {
                "DefaultLegacy", new AbstractLegacy()
                {
                    index = 0,
                    stringIndex = "DefaultLegacy",
                    description = "This is Default Legacy",
                    startDescription = "Started by default Legacy",
                    isPermitLoad = false
                }
            },
            {
                "Guider", new AbstractLegacy()
                {
                    index = 1,
                    stringIndex = "Guider",
                    label = "��",
                    description = "����������İ���������ϴ���̽�նӴ�Խ��ԭ�������뼫��֮�ء�  \r\n��񣬶����Ѳ������ڣ�Ψ�����Լ��ڷ������Ұ�����Ѱ�ҳ�·��",
                    legacyUnlockedDescription = "�����ǿ��Դ������ݿ�ʼ��Ϸ��",
                    startDescription = "����ϡ�����������Գ��ء�  \r\n����Ǳ�͵�ͼ�����𻵣�����Է����ֱ����Ȼ����  \r\n�������һ�ˣ���Ҳ֪��������ҵ�ˮԴ���ܿ�Σ�ա���������ʱ��ˡ�",
                    startingVerbsIDList = new List<string>(){"LandingShip"},
                    //effects = new List<string>(){"BurnedStoryCard"},
                    initLegacyRecipe = new List<AbstractLegacy.LegacyInitGroup>()
                    {
                        new AbstractLegacy.LegacyInitGroup()
                        {
                            InitWithRecipeGroup = "LandingShipGroup",
                            InitWithRecipeKey = "LandingShipInit",
                            startVerbID = "LandingShip"
                        }
                    }
                }
            },
            {
                "Engineer", new AbstractLegacy()
                {
                    index = 2,
                    stringIndex = "Engineer",
                    label = "����ʦ",
                    description = "�����ڷɴ��Ļ�����ȹ���������ҹ��  \r\n��Դѭ�����ƽ�ϵͳ������ά��װ�á���û�����ά����������һ�̶��޷�������ת��  \r\n��񣬴��ѻ�Ϊ�к�������뿿˫������ƴ�����Ŀ��ܡ�",
                    legacyUnlockedDescription = "�����ǿ��Դ������ݿ�ʼ��Ϸ��",
                    startDescription = "�к���ɢ����·���ںڰ�����˸��  \r\n����������ҳ������п����õĹ��ߡ�  \r\n����������κ�һ��Ͻ��������ܳ�Ϊ�����Ĳ�����  \r\n������һ��������ʼ������Χ����",
                    startingVerbsIDList = new List<string>(){"DefaultVerb"},
                    effects = new List<string>(){"BurnedStoryCard"},
                }
            },
            {
                "Scientist", new AbstractLegacy()
                {
                    index = 3,
                    stringIndex = "Scientist",
                    label = "��ѧ��",
                    description = "�������о�վ���޾�ʵ�����̽��δ֪�����������ݡ�  \r\n����Ϥ��������¼����֤�ķ�����Ҳ���׹������������ڻ���֮�С�  \r\n���ʵ�������ѻ�Ϊ�ҽ�������ֻ�ܰ��ǻ۴����Ұ��",
                    legacyUnlockedDescription = "�����ǿ��Դ������ݿ�ʼ��Ϸ��",
                    startDescription = "İ���Ŀ��������δ�����Ϣ��  \r\n��ʰ��һ�����Ƶ�ɨ��������Ļ����˸����ֵ�����������档  \r\n����������򲢷�ֻ��������Σ�ա���ֻҪ���ܽ���������ܡ�",
                    startingVerbsIDList = new List<string>(){"DefaultVerb"},
                    effects = new List<string>(){"BurnedStoryCard"},
                }
            }

        };
    }
}