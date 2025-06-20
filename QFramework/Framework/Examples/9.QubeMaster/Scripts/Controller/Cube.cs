using UnityEngine ;
using TMPro ;

namespace CubeMaster
{
   public class Cube : MonoBehaviour
   {
      static int staticID = 0;
      private TMP_Text[] numbersText;

      public int CubeID { get; private set; }
      public Color CubeColor{ get; private set; }
      public int CubeNumber{ get; private set; }
      public Rigidbody CubeRigidbody{ get; private set; }
      [HideInInspector] public bool IsMainCube;

      private MeshRenderer cubeMeshRenderer;

      private void Awake()
      {
         numbersText = GetComponentsInChildren<TMP_Text>();
         CubeID = staticID++;
         cubeMeshRenderer = GetComponent<MeshRenderer>();
         CubeRigidbody = GetComponent<Rigidbody>();
      }

      public void SetColor(Color color)
      {
         CubeColor = color;
         cubeMeshRenderer.material.color = color;
      }

      public void SetNumber(int number)
      {
         CubeNumber = number;
         for (int i = 0; i < 6; i++)
         {
            numbersText[i].text = number.ToString();
         }
      }
   }
}