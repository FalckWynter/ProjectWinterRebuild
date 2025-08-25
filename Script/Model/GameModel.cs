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
        public List<CardMono> tableCardMonoList = new List<CardMono>();
        public List<CardMono> levelCardMonoList = new List<CardMono>();
        

        protected override void OnInit()
        {
        }
        public void AddCardMonoToLevelList(CardMono cardMono)
        {
            if (levelCardMonoList.Contains(cardMono))
                return;
            levelCardMonoList.Add(cardMono);
        }
        public void RemoveCardMonoFromLevelList(CardMono cardMono)
        {
            if (levelCardMonoList.Contains(cardMono))
                levelCardMonoList.Remove(cardMono);
        }
        public void AddCardMonoToTableList(CardMono cardMono)
        {

            //Debug.Log(cardMono.card.label + "尝试添加" + (!tableCardMonoList.Contains(cardMono)) );
            if (tableCardMonoList.Contains(cardMono))
                return;
            tableCardMonoList.Add(cardMono);
        }
        public void RemoveCardMonoFromTableList(CardMono cardMono)
        {
            //Debug.Log(cardMono.card.label + "尝试移除" + (tableCardMonoList.Contains(cardMono)));

            if (tableCardMonoList.Contains(cardMono))
            tableCardMonoList.Remove(cardMono);
        }

    }
}