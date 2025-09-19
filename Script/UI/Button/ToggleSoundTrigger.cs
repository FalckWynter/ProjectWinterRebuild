using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleSoundTrigger : MonoBehaviour, IPointerEnterHandler {

    [SerializeField]
	string soundFXNameOn = "UI_ButtonClick";
	[SerializeField]
	string soundFXNameOff = "";
    [SerializeField]
    string hoverFXName = "token_hover_OLD";

	Toggle button;

    void Start () {
		button = GetComponent<Toggle>();

        if (button != null)
			button.onValueChanged.AddListener( DoClickSound );
	}
	
	void DoClickSound(bool isOn) {
		if (isOn)
			AudioKit.PlaySound(AudioDataBase.TryGetAudio(soundFXNameOn));
		else
		{
			var clip = AudioDataBase.TryGetAudio(soundFXNameOff);
			if(clip != null)
            AudioKit.PlaySound(clip);
		}

	}

    public void OnPointerEnter(PointerEventData eventData) {
        if (!button.interactable)
            return;

        AudioKit.PlaySound(AudioDataBase.TryGetAudio(hoverFXName));
    }
}
