using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace PlentyFishFramework
{
    public class GameModel : AbstractModel
    {
        // ���ڱ���ק������б�
        public List<ICanDragComponentMono> dragMonoList = new List<ICanDragComponentMono>();
        // �������ݽṹ
        public AbstractTable table;
        protected override void OnInit()
        {
        }


    }
}