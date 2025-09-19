using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class SliderSettingMono : AbstractSettingControlMono,IPointerDownHandler
    {
        public bool isInited = false;
        public TextMeshProUGUI sliderHint;
        public Slider slider;
        public TextMeshProUGUI sliderValueLabel;
        public  void SetInteractable(bool interactable)
        {
            if (interactable)
            {
                slider.interactable = true;
                sliderValueLabel.alpha = (1f);
            }
            else
            {
                slider.interactable = false;
                sliderValueLabel.alpha = (0.3f);
            }
        }
        public override void Activate()
        {
            base.Activate();
            RefreshValue();

        }
        public void Start()
        {
            Initialize(bindSettingType,bindSettingID);
        }
        public void Initialize(string type,string id)
        {
            Initialize(GameSettingDataBase.TryGetConfig(type,id));
        }
        public AbstractSettingConfigBase bindToConfig;

        public void RefreshValue()
        {
            var config = bindToConfig;
            gameObject.name = "SliderSetting_" + config.label;
            slider.value = config.GetValueAsFloat();
            sliderHint.text = config.label;
            sliderValueLabel.text = ((int)(config.GetValue())).ToString();
        }
        public void Initialize(AbstractSettingConfigBase config)
        {
            if (config == null) return;
            bindToConfig = config;
            // 设置内容
            RefreshValue();
            isInited = true;

        }

        public void OnValueChanged(float changingToValue)
        {
            //I added this guard clause because otherwise the OnValueChanged event can fire while the slider initial values are being set -
            //for example, if the minvalue is set to > the default control value of 0. This could be fixed by
            //adding the listener in code rather than the inspector, but I'm hewing away from that. It could also be 'fixed' by changing the
            //order of the initialisation steps, but that's half an hour of my time I don't want to lose again next time I fiddle :) - AK
            if (isInited)
            {
                Debug.Log("修改值到" + slider.value);
                sliderValueLabel.text = ((int)((slider.value))).ToString();
                bindToConfig.SetValue((int)slider.value);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isInited)
                AudioKit.PlaySound(AudioDataBase.TryGetAudio("UI_SliderMove"));

        }
    }
}