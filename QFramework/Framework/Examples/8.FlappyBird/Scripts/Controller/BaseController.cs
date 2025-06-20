using QFramework;
using UnityEngine;

namespace FlappyBird
{
    public class BaseController : MonoBehaviour , IController
    {
        public IArchitecture GetArchitecture()
        {
            return FlappyBirdContext.Interface;
        }
    }
}