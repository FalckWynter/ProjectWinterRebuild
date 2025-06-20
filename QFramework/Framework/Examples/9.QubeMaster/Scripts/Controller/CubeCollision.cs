using QFramework;
using UnityEngine ;

namespace CubeMaster
{
   public class CubeCollision : MonoController
   {
      private Cube cube;
      private ExplosionFX explosionFX;
      private CubeSpawner cubeSpawner;
      private Collider[] colliders = new Collider[10];
      const float explosionForce = 400f;
      const float explosionRadius = 1.5f;
      const float pushForce = 2.5f;

      private void Start()
      {
         explosionFX = this.GetUtility<ExplosionFX>();
         cubeSpawner = this.GetSystem<CubeSpawner>();
         cube = GetComponent<Cube>();
      }

      private void OnCollisionEnter(Collision collision)
      {
         Cube otherCube = collision.gameObject.GetComponent<Cube>();

         // 判断是否碰到其他cube，且根据cubeid 只判断一次
         if (otherCube == null || cube.CubeID <= otherCube.CubeID) return;
         // 判断值是否相等
         if (cube.CubeNumber != otherCube.CubeNumber) return;
         
         Vector3 contactPoint = collision.contacts[0].point;

         // 判断cube的数是不是到了最大
         if (otherCube.CubeNumber < cubeSpawner.MaxCubeNumber)
         {
            Cube newCube = cubeSpawner.Spawn(cube.CubeNumber * 2, contactPoint + Vector3.up * 1.6f);
                  
            newCube.CubeRigidbody.AddForce(new Vector3(0, .3f, 1f) * pushForce, ForceMode.Impulse);
            
            this.SendCommand<AddScoreCommand>();
            
            // add some torque
            float randomValue = Random.Range(-20f, 20f);
            Vector3 randomDirection = Vector3.one * randomValue;
            newCube.CubeRigidbody.AddTorque(randomDirection);
         }

         // 爆炸周围的cube
         var size = Physics.OverlapSphereNonAlloc(contactPoint, 2f, colliders);
               
         for (int i = 0; i < size; i++)
         {
            var coll = colliders[i];
            if (coll.attachedRigidbody != null)
               coll.attachedRigidbody.AddExplosionForce(explosionForce, contactPoint, explosionRadius);
         }
               
         explosionFX.PlayCubeExplosionFX(contactPoint, cube.CubeColor);

         cubeSpawner.DestroyCube(cube);
         cubeSpawner.DestroyCube(otherCube);
      }
   }
}