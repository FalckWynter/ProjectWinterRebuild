using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QFramework.Example
{
    public class UnRegisterWhenCurrentSceneUnloadedExample : MonoBehaviour
    {
        private static bool mRegisterd = false;
        private static EasyEvent mExampleEvent = new EasyEvent();
        async void Start()
        {
            if (!mRegisterd)
            {
                mRegisterd = true;
                
                mExampleEvent.Register(() =>
                {
                    Debug.Log("Received When Scene Not Changed");
                }).UnRegisterWhenCurrentSceneUnloaded();

                var gameObj = new GameObject("gameObj");
                DontDestroyOnLoad(gameObj);
                mExampleEvent.Register(() =>
                {
                    Debug.Log("Received When GameObj Not Destroyed");
                }).UnRegisterWhenGameObjectDestroyed(gameObj);

                mExampleEvent.Register(() =>
                {
                    Debug.Log("Received Forever");
                });
                Debug.Log("@@@@ In Current Scene @@@@");
                mExampleEvent.Trigger();

                Debug.Log("@@@@ After GameObject Destroyed @@@@");
                Destroy(gameObj);
                await Task.Delay(TimeSpan.FromSeconds(0.1f));   
                mExampleEvent.Trigger();
                
                Debug.Log("@@@@ After Scene Unloaded/Changed @@@@");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                await Task.Delay(TimeSpan.FromSeconds(0.1f));
                mExampleEvent.Trigger();
            }
        }
    }
}
// @@@@ In Current Scene @@@@
// Received When Scene Not Changed
// Received When GameObj Not Destroyed
// Received Forever
// @@@@ After GameObject Destroyed @@@@
// Received When Scene Not Changed
// Received Forever
// @@@@ After Scene Unloaded/Changed @@@@
// Received Forever