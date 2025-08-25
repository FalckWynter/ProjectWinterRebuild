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
                }
            },
            {
                "Guider", new AbstractLegacy()
                {
                    index = 0,
                    stringIndex = "Guider",
                    description = "����������İ���������ϴ���̽�նӴ�Խ��ԭ�������뼫��֮�ء�  \r\n��񣬶����Ѳ������ڣ�Ψ�����Լ��ڷ������Ұ�����Ѱ�ҳ�·��",
                    startDescription = "����ϡ�����������Գ��ء�  \r\n����Ǳ�͵�ͼ�����𻵣�����Է����ֱ����Ȼ����  \r\n�������һ�ˣ���Ҳ֪��������ҵ�ˮԴ���ܿ�Σ�ա���������ʱ��ˡ�",
                    startingVerbsIDList = new List<string>(){"DefaultVerb"},
                    effects = new List<string>(){"BurnedStoryCard"},
                }
            },
        };
    }
}