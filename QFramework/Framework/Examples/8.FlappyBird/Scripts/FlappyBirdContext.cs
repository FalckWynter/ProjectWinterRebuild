using QFramework;

namespace FlappyBird
{
    public class FlappyBirdContext : Architecture<FlappyBirdContext>
    {
        protected override void Init()
        {
            RegisterModel(new PlayerModel());
            RegisterModel(new GameRuntimeModel());
            RegisterSystem(new UISystem());
            RegisterSystem(new AudioSystem());
        }
    }
}