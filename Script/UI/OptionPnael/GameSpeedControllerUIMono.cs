using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class GameSpeedControllerUIMono : MonoBehaviour,IController
    {
        [SerializeField] private PauseButtonMono pauseButton;
        [SerializeField] private Button normalSpeedButton;
        [SerializeField] private Button fastForwardButton;


        private readonly Color activeSpeedColor = new Color32(147, 225, 239, 255);
        private readonly Color inactiveSpeedColor = Color.white;


        public void Start()
        {
            normalSpeedButton.GetComponent<Image>().color = inactiveSpeedColor;
            this.GetModel<LevelModel>().GameSpeedExchangeEvent.AddListener(RespondToSpeedControlCommand);
            pauseButton.GetComponent<Button>().onClick.AddListener(PauseButton_OnClick);
            normalSpeedButton.GetComponent<Button>().onClick.AddListener(NormalSpeedButton_OnClick);
            fastForwardButton.GetComponent<Button>().onClick.AddListener(FastSpeedButtonOnClick);

        }


        public void AttractAttention()
        {
        }

        public void PauseButton_OnClick()
        {
            if (UtilModel.gameSpeedManager.IsCurrentControlledBy(GameSpeedExchangSource.PauseButton) )
                UtilModel.gameSpeedManager.ClearSource(GameSpeedExchangSource.PauseButton);
            else
                UtilModel.gameSpeedManager.RequestSpeed(new GameSpeedExchangeArgs() { ControlPriorityLevel = 1, GameSpeed = GameSpeed.Paused, GameExchangeSource = GameSpeedExchangSource.PauseButton, WithSFX = true });
        }

        public void NormalSpeedButton_OnClick()
        {
            UtilModel.gameSpeedManager.RequestSpeed(new GameSpeedExchangeArgs() { ControlPriorityLevel = 1, GameSpeed = GameSpeed.Normal, GameExchangeSource = GameSpeedExchangSource.UI, WithSFX = true });

        }

        public void FastSpeedButtonOnClick()
        {
            UtilModel.gameSpeedManager.RequestSpeed(new GameSpeedExchangeArgs() { ControlPriorityLevel = 1, GameSpeed = GameSpeed.Fast, GameExchangeSource = GameSpeedExchangSource.UI, WithSFX = true });

        }


        public void RespondToSpeedControlCommand(GameSpeedExchangeArgs args)
        {

            // uiShowsGameSpeed.SetGameSpeedCommand(args.ControlPriorityLevel, args.GameSpeed);


            if (UtilModel.gameSpeedManager.CurrentSpeed == GameSpeed.Paused)
            {
                pauseButton.SetColor(activeSpeedColor);
                pauseButton.SetPausedText(true);
                normalSpeedButton.GetComponent<Image>().color = inactiveSpeedColor;
                fastForwardButton.GetComponent<Image>().color = inactiveSpeedColor;
            }
            else
            {
                pauseButton.SetPausedText(false);
                pauseButton.SetColor(inactiveSpeedColor);

                if (UtilModel.gameSpeedManager.CurrentSpeed == GameSpeed.Fast)
                {
                    normalSpeedButton.GetComponent<Image>().color = inactiveSpeedColor;
                    fastForwardButton.GetComponent<Image>().color = activeSpeedColor;
                }
                else if (UtilModel.gameSpeedManager.CurrentSpeed == GameSpeed.Normal)
                {
                    normalSpeedButton.GetComponent<Image>().color = activeSpeedColor;
                    fastForwardButton.GetComponent<Image>().color = inactiveSpeedColor;
                }
                else
                {
                    Debug.Log("未知的游戏速度" + UtilModel.gameSpeedManager.CurrentSpeed);
                    //NoonUtility.Log("Unknown effective game speed: " + uiShowsGameSpeed.GetEffectiveGameSpeed());
                }
            }

        }

        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
    }
}