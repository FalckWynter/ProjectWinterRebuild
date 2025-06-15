using System.Collections;
using UnityEngine;

namespace QFramework.Example
{
    public class ArchitectureDeinitExample : MonoBehaviour
    {
        // Start is called before the first frame update
        IEnumerator Start()
        {
            Debug.Log("Start Init");
            var simpleArchitecture = SimpleArchitecture.Interface;
            yield return new WaitForSeconds(2.0f);
            Debug.Log("Start Deinit");
            SimpleArchitecture.Interface.Deinit();
        }


        public class SimpleArchitecture : Architecture<SimpleArchitecture>
        {
            protected override void Init()
            {
                Debug.Log("Simple Architecture Init");
                this.RegisterSystem(new MySystem());
                this.RegisterModel(new MyModel());
            }

            protected override void OnDeinit()
            {
                Debug.Log("Simple Architecture Deinit");
            }
        }

        public class MyModel : AbstractModel
        {
            protected override void OnInit()
            {
                Debug.Log("My Model Init");
            }

            protected override void OnDeinit()
            {
                Debug.Log("My Model Deinit");
            }
        }

        public class MySystem : AbstractSystem
        {
            protected override void OnInit()
            {
                Debug.Log("My System Init");
            }

            protected override void OnDeinit()
            {
                Debug.Log("My System Deinit");
            }

        }
    }
}
