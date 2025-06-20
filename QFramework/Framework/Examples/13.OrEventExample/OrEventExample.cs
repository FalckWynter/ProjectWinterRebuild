using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Example
{
    public class OrEventExample : MonoBehaviour
    {
        private BindableProperty<int> mPropertyA = new BindableProperty<int>(10);
        private BindableProperty<int> mPropertyB = new BindableProperty<int>(5);

        private EasyEvent EventA = new EasyEvent();
        
        // Start is called before the first frame update
        void Start()
        {
            mPropertyA
                .Or(EventA)
                .Or(mPropertyB)
                .Register(() =>
                {
                    Debug.Log("Event Received");
                }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mPropertyA.Value++;
            }

            if (Input.GetMouseButtonDown(1))
            {
                mPropertyB.Value++;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                EventA.Trigger();
            }
        }
    }
}