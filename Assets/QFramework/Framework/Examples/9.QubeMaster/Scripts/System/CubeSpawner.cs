using UnityEngine ;
using System.Collections.Generic ;
using QFramework;

namespace CubeMaster
{
   public class CubeSpawner : AbstractSystem
   {

      Queue<Cube> cubesQueue = new Queue<Cube>();
      private int cubesQueueCapacity = 20;
      private bool autoQueueGrow = true;

      private GameObject cubePrefab;
      private Color[] cubeColors;

      public int MaxCubeNumber { get; private set; }
      // 最大4096 (2^12)

      private readonly int maxPower = 12;
      
      private Transform poolTrans;

      protected override void OnInit()
      {
         cubePrefab = Resources.Load<GameObject>("Prefabs/Cube");
         poolTrans = new GameObject("Cubes").transform;
         MaxCubeNumber = (int)Mathf.Pow(2, maxPower);
         cubeColors = new Color[maxPower];
         Random.InitState(1);
         for (int i = 0; i < maxPower; i++)
         {
            cubeColors[i] = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));
         }
         InitializeCubesQueue();
      }


      private void InitializeCubesQueue()
      {
         for (int i = 0; i < cubesQueueCapacity; i++)
            AddCubeToQueue();
      }

      private void AddCubeToQueue()
      {
         Cube cube = Object.Instantiate(cubePrefab, poolTrans)
            .GetComponent<Cube>();

         cube.gameObject.SetActive(false);
         cube.IsMainCube = false;
         cubesQueue.Enqueue(cube);
      }

      public Cube Spawn(int number, Vector3 position)
      {
         if (cubesQueue.Count == 0)
         {
            if (autoQueueGrow)
            {
               cubesQueueCapacity++;
               AddCubeToQueue();
            }
            else
            {
               Debug.LogError("no more cubes in the pool");
               return null;
            }
         }

         Cube cube = cubesQueue.Dequeue();
         cube.transform.position = position;
         cube.SetNumber(number);
         cube.SetColor(GetColor(number));
         cube.gameObject.SetActive(true);

         return cube;
      }

      public void DestroyCube(Cube cube)
      {
         cube.CubeRigidbody.velocity = Vector3.zero;
         cube.CubeRigidbody.angularVelocity = Vector3.zero;
         cube.transform.rotation = Quaternion.identity;
         cube.IsMainCube = false;
         cube.gameObject.SetActive(false);
         cubesQueue.Enqueue(cube);
      }

      private Color GetColor(int number)
      {
         return cubeColors[(int)(Mathf.Log(number) / Mathf.Log(2)) - 1];
      }
   }
}