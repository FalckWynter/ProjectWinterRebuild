using UnityEngine;

namespace FlappyBird
{
    public class GameStart : MonoBehaviour
    {
        private void Awake()
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Bird"));
            new GameObject("Level").AddComponent<Level>();
            FlappyBirdContext.Interface.GetSystem<UISystem>().OpenUI(nameof(WaitingToStartWindow));
        }
    }
}