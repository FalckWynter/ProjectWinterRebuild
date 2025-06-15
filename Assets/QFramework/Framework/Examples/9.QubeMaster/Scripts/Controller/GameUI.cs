using System;
using QFramework;
using UnityEngine;

namespace CubeMaster
{
    public class GameUI : MonoController
    {

        private int score;

        private void Start()
        {
            this.GetModel<RuntimeData>().Score.RegisterWithInitValue((val) => score = val);
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(100,100,200,100),$"Score : {score}");
        }
    }
}