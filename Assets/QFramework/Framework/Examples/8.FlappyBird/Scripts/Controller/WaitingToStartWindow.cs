using QFramework;
using UnityEngine;

namespace FlappyBird
{
    public class WaitingToStartWindow : BaseController
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.GetSystem<UISystem>().OpenUI(nameof(ScoreWindow));
                this.GetSystem<UISystem>().CloseUI(nameof(WaitingToStartWindow));
                this.GetModel<GameRuntimeModel>().GameState.Value = GameRuntimeModel.State.Playing;
            }
        }
    }
}