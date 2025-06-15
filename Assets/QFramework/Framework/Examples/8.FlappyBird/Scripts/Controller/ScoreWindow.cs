using System.Collections.Generic;
using QFramework;
using UnityEngine.UI;

namespace FlappyBird
{
    public class ScoreWindow : BaseController , IUnRegisterList
    {
        private Text highscoreText;
        private Text scoreText;

        private void Awake()
        {
            scoreText = transform.Find("scoreText").GetComponent<Text>();
            highscoreText = transform.Find("highscoreText").GetComponent<Text>();
        }

        private void Start()
        {
            this.GetModel<PlayerModel>().HighestScore.RegisterWithInitValue((val) =>
            {
                highscoreText.text = $"HIGHSCORE: {val};";
            }).AddToUnregisterList(this);
            
            this.GetModel<GameRuntimeModel>().Score.RegisterWithInitValue((val) =>
            {
                scoreText.text = $"SCORE: {val}";
            }).AddToUnregisterList(this);
        }

        private void OnDestroy()
        {
            this.UnRegisterAll();
        }

        public List<IUnRegister> UnregisterList { get; } = new List<IUnRegister>();
    }
}