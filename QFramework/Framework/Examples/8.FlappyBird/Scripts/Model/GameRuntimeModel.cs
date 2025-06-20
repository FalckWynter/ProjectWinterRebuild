using System;
using QFramework;

namespace FlappyBird
{
    public class GameRuntimeModel : AbstractModel
    {
        public BindableProperty<int> Score { get; } = new BindableProperty<int>();
        public BindableProperty<State> GameState { get; } = new BindableProperty<State>(State.WaitingToStart);
        protected override void OnInit()
        {
            GameState.Register(state =>
            {
                if (state == State.Playing)
                    Score.Value = 0;
            });
        }
        
        public enum State
        {
            WaitingToStart,
            Playing,
            BirdDead,
        }
    }
}