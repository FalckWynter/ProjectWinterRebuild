using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class VerbMono : MonoBehaviour, ICanBeInSlot
    {
        AbstractVerb verb;
        public Image artwork;
        public void LoadVerbData(AbstractVerb verb)
        {
            this.verb = verb;
            artwork.sprite = verb.icon;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}