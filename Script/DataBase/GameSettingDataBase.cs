using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlentyFishFramework
{
    public static class GameSettingDataBase
    {
        public static AbstractSettingConfigBase TryGetConfig(string type,string key)
        {
            if (settingDataBase.ContainsKey(type) && settingDataBase[type].ContainsKey(key))
                return settingDataBase[type][key];
            return null;
        }
        //public static AudioSetting audioSetting = new AudioSetting();
        public static Dictionary<string,Dictionary<string, AbstractSettingConfigBase>> settingDataBase = new Dictionary<string, Dictionary<string, AbstractSettingConfigBase>>()
        {
            { 
                "AudioSetting" , new Dictionary<string, AbstractSettingConfigBase>()
                {
                    { 
                     "MainAudioVolume" , new AbstractSettingConfig<int>()
                        {
                            label = "������",
                            value = 25,
                        }
                    },
                    {
                        "VoiceVolume" , new AbstractSettingConfig<int>(){
                            label = "��������",
                            value = 25,
                        }
                    },
                    {
                        "SoundVolume" , new AbstractSettingConfig<int>()
                        {
                            label = "��Ч����",
                            value = 25,
                        }
                    },
                    {
                        "MusicVolume" , new AbstractSettingConfig<int>()
                        {
                            label = "��������",
                            value = 15,
                        }
                    }
                }
            },
            {
                "SETTING" , new Dictionary<string, AbstractSettingConfigBase>()
                {
                    {
                        "kbnormalspeed" , new AbstractSettingConfig<KeyCode>()
                        {
                            label = "һ����Ϸ�ٶ�",
                            value = KeyCode.N
                        }
                    },
                    {
                        "kbfastspeed" , new AbstractSettingConfig<KeyCode>()
                        {
                            label = "������Ϸ�ٶ�",
                            value = KeyCode.M
                        }
                    },
                    {
                        "kbabort" , new AbstractSettingConfig<KeyCode>()
                        {
                            label = "ѡ��",
                            value = KeyCode.Escape
                        }
                    },
                    {
                        "kbpause" , new AbstractSettingConfig<KeyCode>()
                        {
                            label = "��ͣ",
                            value = KeyCode.Space
                        }
                    }
                }
            }
        };
        static GameSettingDataBase()
        {
            OnInit();
        }
        public static void OnInit()
        {
            SetDefaultValue();
            LoadFileValue();
        }
        public static void SetDefaultValue()
        {

        }
        public static void LoadFileValue()
        {

        }
    }

}