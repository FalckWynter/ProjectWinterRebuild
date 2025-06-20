using QFramework;

namespace FlappyBird
{
    public class GainScoreCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetModel<GameRuntimeModel>().Score.Value++;
        }
    }
}