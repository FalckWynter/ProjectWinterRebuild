using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class RecipeModel : AbstractModel
    {
        // 行动框列表 用于每帧更新，没有mono存在于游戏内的行动框视为不存在
        public List<VerbMono> verbMonoList = new List<VerbMono>();
        public void AddVerbMono(VerbMono mono)
        {
            if (verbMonoList.Contains(mono)) return;
            verbMonoList.Add(mono);
        }
        protected override void OnInit()
        {
        }


    }
}