using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace PlentyFishFramework
{
    public class AudioManagerSystem : AbstractSystem
    {
        protected override void OnInit()
        {
            CreateExchangeListen();
        }
        public void CreateExchangeListen()
        {
            var mainVolumeConfig = GameSettingDataBase.TryGetConfig("AudioSetting","MainAudioVolume");
            var soundVolumeConfig = GameSettingDataBase.TryGetConfig("AudioSetting","SoundVolume");
            var voiceVolumeConfig = GameSettingDataBase.TryGetConfig("AudioSetting","VoiceVolume");
            var musicVolumeConfig = GameSettingDataBase.TryGetConfig("AudioSetting","MusicVolume");
            //Debug.Log("创建修改订阅");

            mainVolumeConfig.OnValueChanged.AddListener(
               () => {
                   AudioKit.Settings.SoundVolume.Value = mainVolumeConfig.GetValueAsFloat() * 0.01f * soundVolumeConfig.GetValueAsFloat() * 0.01f;
                   AudioKit.Settings.VoiceVolume.Value = mainVolumeConfig.GetValueAsFloat() * 0.01f * voiceVolumeConfig.GetValueAsFloat() * 0.01f;
                   AudioKit.Settings.MusicVolume.Value = mainVolumeConfig.GetValueAsFloat() * 0.01f * musicVolumeConfig.GetValueAsFloat() * 0.01f;
                   //Debug.Log("设置为" + AudioKit.Settings.SoundVolume.Value);

               }
                );
            soundVolumeConfig.OnValueChanged.AddListener(
                () =>
                {
                    AudioKit.Settings.SoundVolume.Value = mainVolumeConfig.GetValueAsFloat() * 0.01f * soundVolumeConfig.GetValueAsFloat() * 0.01f;
                    // Debug.Log("设置为" + AudioKit.Settings.SoundVolume.Value);
                }
                );
                voiceVolumeConfig.OnValueChanged.AddListener(
                () =>
                {
                    AudioKit.Settings.VoiceVolume.Value = mainVolumeConfig.GetValueAsFloat() * 0.01f * voiceVolumeConfig.GetValueAsFloat() * 0.01f;
                }
                );
                musicVolumeConfig.OnValueChanged.AddListener(
                () =>
                {
                    AudioKit.Settings.MusicVolume.Value = mainVolumeConfig.GetValueAsFloat() * 0.01f * musicVolumeConfig.GetValueAsFloat() * 0.01f;
                }
                );
        }
        public static void PlaySound(string key)
        {
            AudioClip clip = AudioDataBase.TryGetAudio(key);
            if (clip == null) return;
            AudioKit.PlaySound(clip);
        }
        public static void PlayMusic(string key)
        {
            AudioClip clip = AudioDataBase.TryGetAudio(key);
            // Debug.Log("尝试播放音乐" + key + "是否有音乐" + clip == null);
            if (clip == null) return;
            AudioKit.PlayMusic(clip);
        }
        public static void StopCurrentMusic()
        {
            AudioKit.StopMusic();
        }
    }
}