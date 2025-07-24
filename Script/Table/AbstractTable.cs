using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    // �������ݽṹ ������ECS��
    public class AbstractTable
    {
        public static bool isShowGridShadow = true;
        [Header("��������")]
        // X Y�����������
        public int xCount = 25;
        public int yCount = 9;
        // ����Ԥ���� ���۸����弰·��
        public GameObject slotPrefab;
        public Transform slotParent;
        public static string slotPrefabPath = "Prefabs/LoadPrefab/SlotPrefab";
        // ÿ������ĳߴ�
        public float xRange = 75;
        public float yRange = 115;
        [Header("�߼��ṹ���Ƽ�������")]
        public SlotEntity[,] slotGroups;

        [Header("ֱ�ӷ��ʽṹ")]
        public AbstractSlot[,] abstractSlots;
        public SlotMono[,] slotMonos;

        // Ĭ�Ͽ�������
        public int defaultX, defaultY;
        // ��ʼ�����渳ֵ
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
        // ������������
        public void ClearTable()
        {
            if (slotParent == null) return;
            foreach (Transform child in slotParent)
            {
               GameObject.Destroy(child.gameObject);
            }
        }
        // ��ʼ�����ݽṹ��С
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
        // ���ɿ������岢��ֵ�����ݽṹ��
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
                        Debug.LogError($"SlotPrefab ȱ��������� {r}, �� {c}��");
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
                    // ����ӳ�ʼ��UI������slotMono.SetupUI(...) ����ˢ�±�����
                }
            }
        }
        // ����ض�λ�õĿ��ۻ�ű�
        public AbstractSlot GetAbstractSlot(int r, int c) => abstractSlots[r, c];
        public SlotMono GetSlotMono(int r, int c) => slotMonos[r, c];
    } 
}
