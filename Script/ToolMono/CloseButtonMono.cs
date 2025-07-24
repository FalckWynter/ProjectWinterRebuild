using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    // X��ť�ű�
    public class CloseButtonMono : MonoBehaviour, IPointerClickHandler
    {
        public ICanDragPlayAudioComponentMono dragPlayAudioMono;
       // public GameObject ob;
        // Start is called before the first frame update
        void Start()
        {
            dragPlayAudioMono = GetComponent<ICanDragPlayAudioComponentMono>();
           // GetComponent<Button>().onClick.AddListener(CloseTargetObject);
        }


        public void CloseTargetObject()
        {
            //ob.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            dragPlayAudioMono.PlayGetClickAudio();
        }
    }
}
