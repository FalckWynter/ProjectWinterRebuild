using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class EndingDataBase
    {
        public static AbstractEnding TryGetEnding(string key)
        {
            if (endingDataBase.ContainsKey(key))
                return endingDataBase[key];
            return endingDataBase["DefaultEnding"];
        }
        public static Dictionary<string, AbstractEnding> endingDataBase = new Dictionary<string, AbstractEnding>()
        {
            {
                "DefaultEnding",new AbstractEnding()
                {
                    index = 0,
                    stringIndex = "DefaultEnding",
                    label = "DefaultEnding",
                    lore = "This Game Have Reached End",
                    endingType = AbstractEnding.EndingType.Normal,
                    animType = AbstractEnding.AnimType.LightNormal, 
                }
            },
            {
                "Return",new AbstractEnding()
                {
                    index = 1,
                    stringIndex = "Return",
                    label = "�麽",
                    lore = "�к���һ����޸�������ں�ˮ��Ѫ�����ػ�����\r\n" +
                    "�����������һ�̣��㼸���޷��ֱ���ߵ������ǻ�������������\r\n" +
                    "�ɴ�����������������������Ĵ�½����СΪһĨ�ҵ㡣\r\n" +
                    "ĸ�ǵĺ������ڵ�����Ļ�ϵ�����\r\n" +
                    "��֪������ι¶����ó̽������ǡ���\r\n" +
                    "������Ϊ����׹�䣬������Ϊ����Ż��ֹ�����",
                    endingType = AbstractEnding.EndingType.Normal,
                    animType = AbstractEnding.AnimType.LightNormal,
                }
            },
            {
                "BurnOut",new AbstractEnding()
                {
                    index = 2,
                    stringIndex = "BurnOut",
                    label = "����",
                    lore = "������ƴ��ȫ����ȴ�����޷��������İ��������\r\n " +
                    "���������䣬������ͻ���������˲������ջ����������־��\r\n " +
                    "ҹ������Ⱥ���貣�ȴû���κ�һ�������㡣\r\n " +
                    "�к����������ڷ�ɳ��ֲ��֮�£��·����δ���ڹ���\r\n " +
                    "ֻ��ɢ��Ĺ���������ļ�¼����֤������ͼ�ֿ����˵�������",
                    endingType = AbstractEnding.EndingType.Bad,
                    animType = AbstractEnding.AnimType.LightEvil,
                }
            }
        };
    }
}