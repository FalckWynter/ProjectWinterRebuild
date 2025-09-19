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
                            label = "主音量",
                            value = 25,
                        }
                    },
                    {
                        "VoiceVolume" , new AbstractSettingConfig<int>(){
                            label = "人声音量",
                            value = 25,
                        }
                    },
                    {
                        "SoundVolume" , new AbstractSettingConfig<int>()
                        {
                            label = "音效音量",
                            value = 25,
                        }
                    },
                    {
                        "MusicVolume" , new AbstractSettingConfig<int>()
                        {
                            label = "音乐音量",
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
                            label = "一倍游戏速度",
                            value = KeyCode.N
                        }
                    },
                    {
                        "kbfastspeed" , new AbstractSettingConfig<KeyCode>()
                        {
                            label = "两倍游戏速度",
                            value = KeyCode.M
                        }
                    },
                    {
                        "kbabort" , new AbstractSettingConfig<KeyCode>()
                        {
                            label = "选项",
                            value = KeyCode.Escape
                        }
                    },
                    {
                        "kbpause" , new AbstractSettingConfig<KeyCode>()
                        {
                            label = "暂停",
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