using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBird
{
    public class GameOverWindow : BaseController
    {

        private Text scoreText;
        private Text highscoreText;

        private void Awake()
        {
            scoreText = transform.Find("scoreText").GetComponent<Text>();
            highscoreText = transform.Find("highscoreText").GetComponent<Text>();

            transform.Find("retryBtn").GetComponent<Button>().onClick
                .AddListener(Restart);

            transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        private void Start()
        {
            var score = this.GetModel<GameRuntimeModel>().Score;
            var highestScore = this.GetModel<PlayerModel>().HighestScore;
            if (score.Value > highestScore.Value)
            {
                highscoreText.text = $"NEW HIGHSCORE {score}";
            }
            else
            {
                highscoreText.text = $"HIGHSCORE: {highestScore}";
            }
            scoreText.text = $"SCORE: {score}";
        }

        private void Restart()
        {
            this.GetSystem<UISystem>().OpenUI(nameof(WaitingToStartWindow));
            this.GetSystem<UISystem>().CloseUI(nameof(GameOverWindow));
            this.GetModel<GameRuntimeModel>().GameState.Value = GameRuntimeModel.State.WaitingToStart;
        }
    }
}