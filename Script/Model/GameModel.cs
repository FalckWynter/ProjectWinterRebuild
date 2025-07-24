using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace PlentyFishFramework
{
    public class GameModel : AbstractModel
    {
        // 正在被拖拽物体的列表
        public List<ICanDragComponentMono> dragMonoList = new List<ICanDragComponentMono>();
        // 桌子数据结构
        public AbstractTable table;
        protected override void OnInit()
        {
        }


    }
}