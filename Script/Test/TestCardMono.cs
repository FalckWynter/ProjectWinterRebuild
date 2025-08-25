using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace PlentyFishFramework
{
    public class TestCardMono : MonoBehaviour, IController
    {
        public string cardID = "";
        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
        public void CreateCard()
        {
            // ����һ������ʵ�岢���䵽ָ������
            GameObject ob = this.GetSystem<UtilSystem>().CreateCardGameObject(CardDataBase.TryGetCard(cardID));
            foreach (AbstractSlot slot in CardDataBase.TryGetCard(cardID).cardSlotList)
            {
                Debug.Log(cardID + "���п���" + slot.label);
            }
            SlotMono[,] slotMonos = this.GetModel<GameModel>().table.slotMonos;
            int rowCount = slotMonos.GetLength(0); // �������߶ȣ�
            int colCount = slotMonos.GetLength(1); // ��������ȣ�

            int centerRow = rowCount / 2;
            int centerCol = colCount / 2;

            SlotMono centerSlot = slotMonos[centerRow, centerCol];
            this.GetSystem<GameSystem>().MoveCardToClosestNullGrid(ob.GetComponent<CardMono>(), centerSlot);
        }
        public void CreateLegacy()
        {
            this.GetSystem<UtilSystem>().LoadLegacy(cardID);
        }
        public void CreateEnding()
        {
            this.GetSystem<UtilSystem>().LoadEnding(cardID);
        }
        public void RestartGame()
        {
            this.GetSystem<GameSystem>().RestartGame();
        }
    }
}