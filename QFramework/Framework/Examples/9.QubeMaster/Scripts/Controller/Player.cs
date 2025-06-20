using System.Collections;
using QFramework;
using UnityEngine ;

namespace CubeMaster
{
   public class Player : MonoController
   {
      [SerializeField] private float moveSpeed;
      [SerializeField] private float pushForce;
      [SerializeField] private float cubeMaxPosX;
      [Space] [SerializeField] private TouchSlider touchSlider;

      private Cube mainCube;

      private bool isPointerDown;
      private bool canMove;
      private Vector3 cubePos;

      private CubeSpawner cubeSpawner;

      private void Start()
      {
         cubeSpawner = this.GetSystem<CubeSpawner>();
         SpawnCube();
         canMove = true;

         touchSlider.OnPointerDownEvent += OnPointerDown;
         touchSlider.OnPointerDragEvent += OnPointerDrag;
         touchSlider.OnPointerUpEvent += OnPointerUp;
      }

      private void Update()
      {
         if (isPointerDown)
            mainCube.transform.position = Vector3.Lerp(
               mainCube.transform.position,
               cubePos,
               moveSpeed * Time.deltaTime
            );
      }

      private void OnPointerDown()
      {
         isPointerDown = true;
      }

      private void OnPointerDrag(float xMovement)
      {
         if (isPointerDown)
         {
            cubePos = mainCube.transform.position;
            cubePos.x = xMovement * cubeMaxPosX;
         }
      }

      private void OnPointerUp()
      {
         if (isPointerDown && canMove)
         {
            isPointerDown = false;
            canMove = false;

            // Push the cube:
            mainCube.CubeRigidbody.AddForce(Vector3.forward * pushForce, ForceMode.Impulse);

            StartCoroutine(SpawnNewCube());
         }
      }

      private WaitForSeconds waitForSeconds = new WaitForSeconds(0.3f);
      IEnumerator SpawnNewCube()
      {
         yield return waitForSeconds;
         mainCube.IsMainCube = false;
         canMove = true;
         SpawnCube();
      }

      private void SpawnCube()
      {
         mainCube = cubeSpawner.Spawn(GenerateRandomNumber(), transform.position);
         mainCube.IsMainCube = true;

         // reset cubePos variable
         cubePos = mainCube.transform.position;
      }
      
      private int GenerateRandomNumber()
      {
         return (int)Mathf.Pow(2, Random.Range(1, 6));
      }

      private void OnDestroy()
      {
         //remove listeners:
         touchSlider.OnPointerDownEvent -= OnPointerDown;
         touchSlider.OnPointerDragEvent -= OnPointerDrag;
         touchSlider.OnPointerUpEvent -= OnPointerUp;
      }
   }
}