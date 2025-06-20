using QFramework;

namespace FlappyBird
{
    public class BirdDeadCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetModel<GameRuntimeModel>().GameState.Value = GameRuntimeModel.State.BirdDead;
            this.GetModel<PlayerModel>().SaveScore(this.GetModel<GameRuntimeModel>().Score.Value);

        }
    }
}