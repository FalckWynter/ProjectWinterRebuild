using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    // 桌面数据结构 懒得用ECS了
    public class AbstractTable
    {
        public static bool isShowGridShadow = true;
        [Header("桌面配置")]
        // X Y轴的网格数量
        public int xCount = 25;
        public int yCount = 9;
        // 卡槽预制体 卡槽父物体及路径
        public GameObject slotPrefab;
        public Transform slotParent;
        public static string slotPrefabPath = "Prefabs/LoadPrefab/SlotPrefab";
        // 每个网格的尺寸
        public float xRange = 75;
        public float yRange = 115;
        [Header("逻辑结构（推荐保留）")]
        public SlotEntity[,] slotGroups;

        [Header("直接访问结构")]
        public AbstractSlot[,] abstractSlots;
        public SlotMono[,] slotMonos;

        // 默认卡牌中心
        public int defaultX, defaultY;
        // 初始化桌面赋值
        public void InitTable()
        {
            if(slotPrefab == null)
            {
                slotPrefab = PrefabDataBase.TryGetPrefab("SlotPrefab");
            }
            if (slotParent == null)
                slotParent = GameObject.Find("MainCanvas/SceneParent/TableGameobject").transform;
            ClearTable();
            InitStructures();
            GenerateUISlots();
            defaultX = xCount / 2;
            defaultY = yCount / 2;
        }
        // 清理桌面内容
        public void ClearTable()
        {
            if (slotParent == null) return;
            foreach (Transform child in slotParent)
            {
               GameObject.Destroy(child.gameObject);
            }
        }
        // 初始化数据结构大小
        private void InitStructures()
        {
            slotGroups = new SlotEntity[xCount, yCount];
            abstractSlots = new AbstractSlot[xCount, yCount];
            slotMonos = new SlotMono[xCount, yCount];
            for (int r = 0; r < xCount; r++)
            {
                for (int c = 0; c < yCount; c++)
                {
                    slotGroups[r, c] = new SlotEntity(r, c);
                }
            }
        }
        // 生成卡槽物体并赋值到数据结构中
        private void GenerateUISlots()
        {
            for (int r = 0; r < xCount; r++)
            {
                for (int c = 0; c < yCount; c++)
                {
                    var uiSlot = GameObject.Instantiate(slotPrefab, slotParent);

                    AbstractSlot abstractSlot = new AbstractSlot();
                    abstractSlot.isSlot = false;
                    SlotMono slotMono = uiSlot.GetComponent<SlotMono>();
                    slotMono.slot = abstractSlot;
                    if (abstractSlot == null || slotMono == null)
                    {
                        Debug.LogError($"SlotPrefab 缺少组件（行 {r}, 列 {c}）");
                        continue;
                    }

                    //abstractSlot.Initialize(slotGroups[r, c]);
                    slotGroups[r, c].slot = abstractSlot;
                    slotGroups[r, c].slotMono = slotMono;

                    abstractSlots[r, c] = abstractSlot;
                    slotMonos[r, c] = slotMono;
                    slotMono.x = r;
                    slotMono.y = c;
                    uiSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(60f + xRange * r, 80f + yRange * c);
                    uiSlot.name = "Grid[" + r + "," + c + "]";
                    // 可添加初始化UI函数：slotMono.SetupUI(...) 例如刷新背景等
                }
            }
        }
        // 获得特定位置的卡槽或脚本
        public AbstractSlot GetAbstractSlot(int r, int c) => abstractSlots[r, c];
        public SlotMono GetSlotMono(int r, int c) => slotMonos[r, c];
    } 
}
