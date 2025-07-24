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
            // 创建一个卡牌实体并分配到指定网格
            GameObject ob = this.GetSystem<UtilSystem>().CreateCardGameObject(CardDataBase.TryGetCard(cardID));
            SlotMono[,] slotMonos = this.GetModel<GameModel>().table.slotMonos;
            int rowCount = slotMonos.GetLength(0); // 行数（高度）
            int colCount = slotMonos.GetLength(1); // 列数（宽度）

            int centerRow = rowCount / 2;
            int centerCol = colCount / 2;

            SlotMono centerSlot = slotMonos[centerRow, centerCol];
            this.GetSystem<GameSystem>().MoveCardToClosestNullGrid(ob.GetComponent<CardMono>(), centerSlot);
        }

    }
}