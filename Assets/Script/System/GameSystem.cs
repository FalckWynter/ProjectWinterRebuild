using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace PlentyFishFramework
{
    public class GameSystem : AbstractSystem
    {
        GameModel model;
        public void AddDragListen(ICanDragComponentMono mono)
        {
            // Debug.Log("Ω” ’µΩ∂©‘ƒ");
            if (!model.dragMonoList.Contains(mono))
                model.dragMonoList.Add(mono);
        }
        public void RemoveDragListen(ICanDragComponentMono mono)
        {
            if (model.dragMonoList.Contains(mono))
                model.dragMonoList.Remove(mono);
        }
        protected override void OnInit()
        {
        }
        public void LateInit()
        {
            model = this.GetModel<GameModel>();
        }
    }
}