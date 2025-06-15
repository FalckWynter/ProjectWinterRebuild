using QFramework;
using UnityEngine;

namespace FlappyBird
{
    public class PlayerModel : AbstractModel
    {
        public BindableProperty<int> HighestScore { get; } = new BindableProperty<int>();

        public void SaveScore(int score)
        {
            if (score > HighestScore.Value)
                HighestScore.Value = score;
        }
        
        protected override void OnInit()
        {
            HighestScore.Value = PlayerPrefs.GetInt(nameof(HighestScore));
            HighestScore.Register(score => PlayerPrefs.SetInt(nameof(HighestScore), score));
        }
    }
}