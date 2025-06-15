using UnityEngine;

namespace QFramework
{
    public class UnRegisterWhenDisabledExample : MonoBehaviour
    {
        void Start()
        {
            var receivedGameObj = new GameObject();
            var eventA = new EasyEvent();

            eventA.Register(() =>
            {
                Debug.Log("Received");
            }).UnRegisterWhenDisabled(receivedGameObj);

            eventA.Trigger(); // Received
            eventA.Trigger(); // Received
            eventA.Trigger(); // Received

            receivedGameObj.SetActive(false);

            eventA.Trigger(); // Noting
            eventA.Trigger(); // Noting
            eventA.Trigger(); // Noting
        }
    }
}