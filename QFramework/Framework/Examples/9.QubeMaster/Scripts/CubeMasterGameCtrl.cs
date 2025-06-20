using QFramework;
using UnityEngine;

namespace CubeMaster
{
    public class CubeMaster : Architecture<CubeMaster>
    {
        protected override void Init()
        {
            RegisterSystem(new CubeSpawner());
            RegisterUtility(new ExplosionFX());
            RegisterModel(new RuntimeData());
        }
    }
}