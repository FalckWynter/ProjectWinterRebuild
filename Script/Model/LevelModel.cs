using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlentyFishFramework
{
    public class LevelModel : AbstractModel
    {
        public GameSpeedExchangeEvent GameSpeedExchangeEvent = new GameSpeedExchangeEvent();
        protected override void OnInit()
        {
        }


    }
    public class GameSpeedExchangeArgs
    {
        public int ControlPriorityLevel { get; set; }
        public GameSpeed GameSpeed { get; set; }
        public bool WithSFX { get; set; }

        public GameSpeedExchangSource GameExchangeSource;

        public static GameSpeedExchangeArgs ArgsForPause()
        {
            //Always use ControlPriorityLevel 2 for code-based Pause, otherwise unpausing can get buggered up
            //REMEMBER THE MANSUS
            var args = new GameSpeedExchangeArgs { ControlPriorityLevel = 2, GameSpeed = GameSpeed.Paused };
            return args;
        }
    }
    public enum GameSpeed
    {
        Paused = 0,Normal = 1,Fast = 2
    }
    public enum GameSpeedExchangSource
    {
        UI,GameEvent,PauseButton,ClearSpeedRequest
    }
    public class GameSpeedExchangeEvent : UnityEvent<GameSpeedExchangeArgs>
    {

    }
}