using QFramework;
using UnityEngine;

namespace CubeMaster
{
    public class MonoController : MonoBehaviour , IController
    {
        public IArchitecture GetArchitecture()
        {
            return CubeMaster.Interface;
        }
    }
}