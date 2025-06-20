using QFramework;
using UnityEngine ;

namespace CubeMaster
{
   public class ExplosionFX : IUtility
   {
      private ParticleSystem cubeExplosionFX;

      ParticleSystem.MainModule cubeExplosionFXMainModule;

      public void PlayCubeExplosionFX(Vector3 position, Color color)
      {
         if (cubeExplosionFX == null)
         {
            cubeExplosionFX = Object.Instantiate(Resources.Load<ParticleSystem>("Prefabs/Explosion FX"));
            cubeExplosionFXMainModule = cubeExplosionFX.main;
         }
         cubeExplosionFXMainModule.startColor = new ParticleSystem.MinMaxGradient(color);
         cubeExplosionFX.transform.position = position;
         cubeExplosionFX.Play();
      }
   }
}