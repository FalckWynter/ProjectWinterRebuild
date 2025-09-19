using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace PlentyFishFramework
{
    public class GameModel : AbstractModel
    {   
        // �洢ս���ڵ���Ϸ����
        // ���ڱ���ק������б�
        public List<ICanDragComponentMono> dragMonoList = new List<ICanDragComponentMono>();
        // �������ݽṹ
        public AbstractTable table;
        public List<CardMono> tableCardMonoList = new List<CardMono>();
        public List<CardMono> levelCardMonoList = new List<CardMono>();
        public static Dictionary<string, DropZoneDragHelper> dropZoneList = new Dictionary<string, DropZoneDragHelper>();
        public static Transform dropZoneParent;
        public static string DefaultCardDropZoneID = "CardDefault", DefaultVerbDropZoneID = "VerbDefault";
        public static void RegisterDropZone(string key, DropZoneDragHelper ob)
        {
            if (dropZoneList.ContainsKey(key))
                dropZoneList[key] = ob;
            else
                dropZoneList.Add(key, ob);

        }
        public static void UnRegisterDropZone(string key)
        {
            if (dropZoneList.ContainsKey(key))
                dropZoneList.Remove(key);
        }
        public static DropZoneDragHelper GetDropZone(string key)
        {
            if (dropZoneList.ContainsKey(key))
                return dropZoneList[key];
            else
                return dropZoneList[DefaultCardDropZoneID];
        }

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

            //Debug.Log(cardMono.card.label + "�������" + (!tableCardMonoList.Contains(cardMono)) );
            if (tableCardMonoList.Contains(cardMono))
                return;
            tableCardMonoList.Add(cardMono);
        }
        public void RemoveCardMonoFromTableList(CardMono cardMono)
        {
            //Debug.Log(cardMono.card.label + "�����Ƴ�" + (tableCardMonoList.Contains(cardMono)));

            if (tableCardMonoList.Contains(cardMono))
            tableCardMonoList.Remove(cardMono);
        }

    }
}