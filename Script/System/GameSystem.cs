using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.EventSystems;
using System;
namespace PlentyFishFramework
{
    public class GameSystem : AbstractSystem
    {
        // 游戏动作及功能函数 以Mono脚本互动为主
        GameModel gameModel;
        RecipeSystem recipeSystem;
        UtilSystem utilSystem;
        RecipeModel recipeModel;
        public static bool isUseNewDragMode = true; 
        // 物体开始将物体订阅到拖拽列表
        public void AddDragListen(ICanDragComponentMono mono)
        {
            // Debug.Log("接收到订阅");
            if (!gameModel.dragMonoList.Contains(mono))
                gameModel.dragMonoList.Add(mono);
        }
        // 物体结束拖拽时将物体从拖拽列表移除
        public void RemoveDragListen(ICanDragComponentMono mono)
        {
            if (gameModel.dragMonoList.Contains(mono))
                gameModel.dragMonoList.Remove(mono);
        }

        // 使用这个函数以播放音效
        public bool MonoStackCardToSlot(ITableElement elementMono, SlotMono slotMono, TableElementMonoType elementType)
        {
            bool result = StackCardToASlot(elementMono, slotMono);
            if (result)
            {
                // 卡牌叠放成功
                if (elementType == TableElementMonoType.Card)
                    UtilSystem.PlayAudio("card_table_leave");
                if (elementType == TableElementMonoType.Slot)
                    UtilSystem.PlayAudio("card_drop");
                if (elementType == TableElementMonoType.Verb)
                    UtilSystem.PlayAudio("SituationAvailable");
                if (elementType == TableElementMonoType.SituationWindows)
                    UtilSystem.PlayAudio("card_drop");


            }
            else
            {
                if (elementType == TableElementMonoType.Slot)
                {
                    UtilSystem.PlayAudio("card_drag_fail");
                    MentionSystem.ShowMessage("不能放入这个卡槽","这件物体不能放入这个卡槽，请检查卡槽对放入卡牌的要求");
                }
            }

            return result;
        }
        // 分发器，判断卡槽类型并分发到对应的叠放函数中
        // 区别在于，这里传入的卡牌并不知道自己的目标是卡槽还是网格
        public bool StackCardToASlot(ITableElement cardMono, SlotMono slotMono, bool isCanStackWithVerb = true)
        {
            if (slotMono == null || slotMono.slot == null) return false;
            if (slotMono.slot.isSlot)
            {
                return StackElementWithSlot(cardMono, slotMono, isCanStackWithVerb);
            }
            else
            {
                //Debug.Log(cardMono.GetGameobject().name + "调用" + slotMono.name);
                return StackElementWithGrid(cardMono, slotMono, isCanStackWithVerb);
            }
        }

        // 卡牌与卡槽堆叠时
        public bool StackElementWithSlot(ITableElement elementMono,SlotMono slotMono, bool isCanStackWithVerb = true)
        {
            // 卡槽不承载行动框
            if(elementMono is VerbMono verbMono)
            {
                StackCardToASlot(elementMono, verbMono.LastGridMono);
                Debug.Log("踢回行动框");
                return false;
            }
            CardMono cardMono = (CardMono)elementMono;
            // 这里的element一定是卡牌
            // 如果放不进去则弹回
            if(!RecipeSystem.IsCardMeetSlotsAspectRequire(cardMono.card,slotMono.slot))
            {
                MoveCardToClosestNullGrid(cardMono, cardMono.BeforeSlotMono);
                return false;

            }
            //Debug.Log("尝试叠放的元素数量" + slotMono.slot.stackItemList.Count);
            // 3.2.1.1 贪婪卡槽不允许替换内容
            if (slotMono.slot.isGreedy && slotMono.slot.stackItemList.Count >= slotMono.slot.maxSlotItemCount)
            {
                Debug.Log(elementMono.GetGameobject().name + "拒绝卡牌加入贪婪卡槽");
                MoveCardToClosestNullGrid(elementMono, elementMono.LastGridMono);
                return false;
            }
            // 如果卡槽有空位，直接放入并注册
            if (slotMono.slot.stackItemList.Count < slotMono.slot.maxSlotItemCount)
            {
                RegisterStackElementToSlot(elementMono, slotMono);
                // 重设物体位置
                ResetItemPositionToSlot(elementMono.GetGameobject(), slotMono.gameObject);
                return true;
            }
            else
            {

                // 卡槽已满，处理卡牌叠放逻辑 
                // 对于卡槽而言，永远是替换当前卡槽内容
                ITableElement beforeElementMono = slotMono.slot.stackItemList[0];
                //如果不能叠放，则将原有的卡牌挤走到最近的空位，自己放入其中
                SlotMono beforeElementBeforeSlotMono = beforeElementMono.BelongtoSlotMono;
                // 提取原来的卡牌，现在两张卡牌都在天上
                UnRegisterStackElementFromSlot(beforeElementMono, beforeElementMono.BelongtoSlotMono);
                // 新卡牌放入腾出的空间
                StackCardToASlot(elementMono, beforeElementBeforeSlotMono);
                // 旧卡牌查找最近可用的位置
                MoveCardToClosestNullGrid(beforeElementMono, beforeElementMono.LastGridMono);
                return false;
            }
        }
        // 卡牌与桌面网格堆叠时
        public bool StackElementWithGrid(ITableElement elementMono, SlotMono slotMono,bool isCanStackWithVerb = true)
        {
            // 如果是卡牌且放不进去则弹回
            if (elementMono is CardMono preCardMono)
            {
                if (!RecipeSystem.IsCardMeetSlotsAspectRequire(preCardMono.card, slotMono.slot))
                {
                    MoveCardToClosestNullGrid(preCardMono, preCardMono.BeforeSlotMono);
                    return false;
                }
            }
            // 如果卡槽有空位，直接放入并注册

            if (slotMono.slot.stackItemList.Count < slotMono.slot.maxSlotItemCount)
            {
                RegisterStackElementToSlot(elementMono, slotMono);
                ResetItemPositionToSlot(elementMono.GetGameobject(), slotMono.gameObject);
                return true;
            }
            else
            {
                // 3.2.1.1 贪婪卡槽不允许替换内容
                if (slotMono.slot.isGreedy)
                {
                    MoveCardToClosestNullGrid(elementMono, elementMono.LastGridMono);
                    return false;
                }
                // 卡槽已满，处理卡牌叠放逻辑
                ITableElement beforeElementMono = slotMono.slot.stackItemList[0];
                if (!isCanStackWithVerb)
                {
                    if (beforeElementMono is VerbMono verbMono)
                    {
                        MoveCardToClosestNullGrid(elementMono, elementMono.BelongtoSlotMono);
                        return true;
                    }
                }
                // 3.1.1.1节 特殊处理卡牌叠放到行动框的逻辑
                if (elementMono is CardMono cardMono && beforeElementMono is VerbMono beforeVerbMono)
                {
                    if (beforeVerbMono.CanStackWith(elementMono))
                    {
                        beforeVerbMono.TryAddStack(elementMono);
                        // 不需要移除元素
                        //elementMono.DestroySelf();
                        return true;
                    }
                    else
                    {
                        // 叠不上去的情况直接找最近的空格
                        MoveCardToClosestNullGrid(elementMono, elementMono.LastGridMono, 2);
                        return false;
                    }
                }

                // 对于网格剩下的三种情况而言，如果能重叠则叠放，否则另行处理
                if ( beforeElementMono.CanStackWith(elementMono))
                {
                    beforeElementMono.TryAddStack(elementMono);
                    // 然后销毁这个物体
                    elementMono.DestroySelf();
                    return true;
                }
                else
                {
                    //如果不能叠放，则将原有的卡牌挤走到最近的空位，自己放入其中
                    SlotMono beforeElementBeforeSlotMono = beforeElementMono.BelongtoSlotMono;
                    // 提取原来的卡牌，现在两张卡牌都在天上
                    UnRegisterStackElementFromSlot(beforeElementMono, beforeElementMono.BelongtoSlotMono);
                    // 新卡牌放入腾出的空间
                    StackCardToASlot(elementMono, beforeElementBeforeSlotMono);
                    int length = 1;
                    if (beforeElementMono is VerbMono verbMono)
                        length = 2;
                        // 旧卡牌查找最近可用的位置
                        MoveCardToClosestNullGrid(beforeElementMono, beforeElementBeforeSlotMono, length);
                    return false;
                }
            }
        }
        // 将卡牌移动到最近的空卡槽中
        //public void MoveCardToClosestNullGrid(ITableElement mono, SlotMono beforeSlotMono, int stepLength = 1,int posX = -1,int posY = -1)
        //{
        //    stepLength = mono.slotSize;
        //    int x, y;
        //    if (beforeSlotMono == null || beforeSlotMono.slot.isSlot)
        //    {
        //        x = gameModel.table.defaultX;
        //        y = gameModel.table.defaultY;
        //    }
        //    else
        //    {
        //        x = beforeSlotMono.x;
        //        y = beforeSlotMono.y;
        //    }
        //    int maxX = gameModel.table.slotMonos.GetLength(0);
        //    int maxY = gameModel.table.slotMonos.GetLength(1);

        //    for (int yOffset = 0; yOffset < maxY; yOffset += stepLength)
        //    {
        //        // y层次扩展：0 → +1 → -1 → +2 → -2 → ...
        //        int[] yCandidates = { y + yOffset, y - yOffset };
        //        foreach (int currentY in yCandidates)
        //        {
        //            if (currentY < 0 || currentY >= maxY) continue;

        //            for (int xOffset = 0; xOffset < maxX; xOffset += stepLength)
        //            {
        //                // x层次扩展：0 → +1 → -1 → +2 → -2 → ...
        //                int[] xCandidates = { x - xOffset, x + xOffset };
        //                foreach (int currentX in xCandidates)
        //                {
        //                    if (currentX < 0 || currentX >= maxX) continue;

        //                    var slot = gameModel.table.slotMonos[currentX, currentY];
        //                    if (slot.slot.stackItemList.Count == 0)
        //                    {
        //                        StackElementWithGrid(mono, slot);
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public void MoveCardToClosestNullGrid(
            ITableElement mono,
            SlotMono beforeSlotMono,
            int stepLength = 1,
            int posX = -1,
            int posY = -1)
        {
            stepLength = mono.slotSize;
            int startX, startY;
            if (beforeSlotMono == null || beforeSlotMono.slot.isSlot)
            {
                startX = gameModel.table.defaultX;
                startY = gameModel.table.defaultY;
            }
            else
            {
                startX = beforeSlotMono.x;
                startY = beforeSlotMono.y;
            }

            int maxX = gameModel.table.slotMonos.GetLength(0);
            int maxY = gameModel.table.slotMonos.GetLength(1);

            // ---------- 1. 当前层 ----------
            if (CheckRowForEmptySlot(mono, startX, startY, maxX))
                return;

            // ---------- 2. 向下逐层 ----------
            for (int y = startY - stepLength; y >= 0; y -= stepLength)
            {
                if (CheckRowForEmptySlot(mono, startX, y, maxX))
                    return;
            }

            // ---------- 3. 向上逐层 ----------
            for (int y = startY + stepLength; y < maxY; y += stepLength)
            {
                if (CheckRowForEmptySlot(mono, startX, y, maxX))
                    return;
            }
        }
        /// <summary>
        /// 在指定 Y 层，从 startX 开始，先向右扫描，再向左扫描
        /// </summary>
        private bool CheckRowForEmptySlot(ITableElement mono, int startX, int y, int maxX)
        {
            // 先向右
            for (int x = startX; x < maxX; x += mono.slotSize)
            {
                var slot = gameModel.table.slotMonos[x, y];
                if (slot.slot.stackItemList.Count == 0)
                {
                    StackElementWithGrid(mono, slot);
                    return true;
                }
            }

            // 再向左
            for (int x = startX - mono.slotSize; x >= 0; x -= mono.slotSize)
            {
                var slot = gameModel.table.slotMonos[x, y];
                if (slot.slot.stackItemList.Count == 0)
                {
                    StackElementWithGrid(mono, slot);
                    return true;
                }
            }

            return false;
        }


        // 将卡牌从卡槽中移除注册 这一层用于处理mono降本的数据 包含对场景物体的处理
        public void UnRegisterStackElementFromSlot(ITableElement stackMono,SlotMono slotMono,bool isUnregisterElement = true)
        {
            if (slotMono == null) return;
            slotMono.slot.stackItemList.Remove(stackMono);
            //cardMono.beforeSlotMono = null;
            stackMono.GetGameobject().GetComponent<ICanBelongToSlotMono>().BelongtoSlotMono = null;
            // 在被行动框调用时不移除数据防止出错
            // 3.2.1.2 检查桌面内容
            if (stackMono is CardMono item)
            {
                // 3.2.1.2 检查桌面内容
                UnRegisterCardFromSlot(item.card, slotMono.slot,isTriggerReCalculate : false , isUnregisterElement : isUnregisterElement);
                //3.2.1.1
                if(item.GetComponent<ICanDragComponentMono>().canvasGroup != null)
                item.GetComponent<ICanDragComponentMono>().canvasGroup.blocksRaycasts = true;

            }   
        }
        // 将卡牌注册到卡槽 这一层用于处理mono降本的数据 包含对场景物体的处理
        public void RegisterStackElementToSlot(ITableElement cardMono,SlotMono slotMono)
        {
            slotMono.slot.stackItemList.Add(cardMono);
            cardMono.BeforeSlotMono = slotMono;
            cardMono.BelongtoSlotMono = slotMono;
            //Debug.Log("添加到" + slotMono.slot.label + "是否为卡槽" + slotMono.slot.isSlot + "值" + cardMono.BelongtoSlotMono.slot.label);
            if (!slotMono.slot.isSlot)
                cardMono.LastGridMono = slotMono;

            if(cardMono is CardMono item)
            {
                // 3.2.1.2 检查桌面内容

                // 放入卡槽时从桌面上移除
                if (slotMono.slot.isSlot)
                {
                    gameModel.RemoveCardMonoFromTableList(item);
                }
                else
                {
                    // 放入桌面网格时重新加入桌面订阅
                    gameModel.AddCardMonoToTableList(item);
                    // 并且加入关卡订阅(常规添加途径，另一途径是被行动框制造出来)
                    gameModel.AddCardMonoToLevelList(item);
                }
                RegisterCardToSlot(item.card, slotMono.slot);


            }
        }
        // 将卡牌数据注册到卡槽数据，抽象数据处理
        public void RegisterCardToSlot(AbstractCard card,AbstractSlot slot/*CardMono cardMono,SlotMono slotMono*/)
        {
            //Debug.Log("添加卡牌到卡槽");
            //slotMono.slot.card = cardMono.card;
            slot.card = card;
            // 对行动框中的卡槽进行注册
            if (slot.verb != null)
            {
                //Debug.Log(card.label + "卡牌具有卡槽数量" + card.cardSlotList.Count);
                // 注册卡牌元素 卡牌携带的卡槽
                AddCardElementToVerb(card, slot.verb);
                foreach(AbstractSlot item in card.cardSlotList)
                {
                    if (isSlotCanPutInVerb(item, slot, slot.verb))
                    {
                        AddCardSlotToVerb(item, slot.verb);
                    }
                }
                // 并触发一次事件重算
                recipeSystem.ReCalculateVerbRecipeState(slot.verb);
            }

        }
        public bool isSlotCanPutInVerb(AbstractSlot addSlot, AbstractSlot putInSlot, AbstractVerb verb)
        {
            if (putInSlot.isVerbSlot == false) return false;
            if (addSlot.slotPossibleShowVerbList == null || addSlot.slotPossibleShowVerbList.Count == 0)
                return true;
            foreach (string str in addSlot.slotPossibleShowVerbList)
            {
                if (str == "All")
                    return true;
                if (str == verb.stringIndex)
                    return true;
            }
            return false;
        }
        // 将卡牌数据从卡槽数据中移除，抽象数据处理
        public void UnRegisterCardFromSlot(AbstractCard card, AbstractSlot slot,bool isTriggerReCalculate = true, bool isUnregisterElement = true/*CardMono cardMono,SlotMono slotMono*/)
        {
            //slotMono.slot.card = null;
            slot.card = null;
            if (slot.verb != null)
            {
                // 移除卡牌性相、卡槽并重算事件，其中卡槽存在递归情况
                if(isUnregisterElement)
                RemoveCardElementFromVerb(card, slot.verb);
                RemoveCardSlotFromVerb(card, slot.verb, isUnregisterElement);
                if(isTriggerReCalculate)
                recipeSystem.ReCalculateVerbRecipeState(slot.verb);

            }
        }
        // 将卡牌性相注册到行动框
        public void AddCardElementToVerb(AbstractCard card,AbstractVerb verb)
        {
            //Debug.Log("计算元素影响");
            AddElementToVerb(card.stringIndex, 1, verb);
            foreach(var item in card.aspectDictionary)
            {
                AddElementToVerb(item.Key, item.Value,verb);
            }
            // 注册卡槽到行动框

        }
        // 将卡牌性相从行动框中移除
        public void RemoveCardElementFromVerb(AbstractCard card,AbstractVerb verb)
        {
            AddElementToVerb(card.stringIndex, -1, verb);
            foreach (var item in card.aspectDictionary)
            {
                AddElementToVerb(item.Key, -item.Value, verb);
            }
        }
        // 将卡牌的卡槽添加到行动框
        public void AddCardSlotToVerb(AbstractSlot slot,AbstractVerb verb)
        {
            //Debug.Log("添加新的卡牌卡槽" + slot.label);
            verb.verbMono.situationWindowMono.AddSlotObjectToVerbDominion(slot);
            // 7.22 可能有隐患，只注册不消除
            slot.verb = verb;
            verb.cardSlotList.Add(slot);
        }

        // 将卡牌的卡槽从行动框中移除
        public void RemoveCardSlotFromVerb(AbstractCard card, AbstractVerb verb,bool isUnRegisterElement = true)
        {
            foreach (var slot in card.cardSlotList)
            {
                // 递归移除卡槽中卡牌的订阅
                if (slot.card != null && slot.card.cardMono != null)
                {
                    // 并将卡牌抛出
                    CardMono mono = slot.card.cardMono;
                    mono.transform.SetParent(UtilSystem.cardParent.transform, true);
                    UnRegisterStackElementFromSlot(slot.card.cardMono, slot.card.cardMono.BelongtoSlotMono, isUnRegisterElement);
                    MoveCardToClosestNullGrid(mono, mono.LastGridMono);
                    //StackCardToASlot(mono, mono.LastGridMono);
                }
                verb.verbMono.situationWindowMono.RemoveSlotObjectFromVerbDominion(slot);
                verb.cardSlotList.Remove(slot);
            }
        }
        // 将事件携带的行动框中注册到verb中并产生实体
        public void AddRecipeSlotToVerb(AbstractSlot slot,AbstractVerb verb)
        {
            verb.verbMono.situationWindowMono.AddSlotToRecipeExcutingSlotDominion(slot);

            // 7.22 可能有隐患，只注册不消除
            slot.verb = verb;
            
        }
        public void RemoveRecipeSlotFromVerb(AbstractSlot slot,AbstractVerb verb)
        {
            verb.verbMono.situationWindowMono.RemoveSlotObjectFromVerbDominion(slot);
            slot.verb = null;
        }
        // 修改行动框的元素词典
        public void AddElementToVerb(string key,int value, AbstractVerb verb)
        {
            verb.AddAspect(key, value);
        }
  

        // 将卡牌衰变为新的卡牌
        public void TranslateCardMonosCardToNewCard(CardMono cardMono)
        {
            AbstractCard oldCard = cardMono.card;
            string newCardIndex = oldCard.decayToCardStringIndex;
            SlotMono oldCardSlot = cardMono.LastGridMono;
            if (cardMono.gameObject != null)
            {
                cardMono.DestroySelf();
                // 如果有要转变到的卡牌，则为新卡牌创建并分配实体到最近的可用槽位
                if (newCardIndex != null)
                {
                    GameObject ob = utilSystem.CreateCardGameObject(CardDataBase.TryGetCard(newCardIndex).GetNewCopy());
                    MoveCardToClosestNullGrid(ob.GetComponent<ITableElement>(), oldCardSlot);
                }
            }
        }
        #region Mono通用工具函数
        // 创建一个用于被玩家拖拽的卡牌复制，且继承拖拽订阅
        public void CreateCardCopyToBeDrag(CardMono cardMono, PointerEventData eventData)
        {
            // 创建并为新的卡牌副本进行赋值 * TOSET 设置单独的函数处理复制脚本的情况 防止类似于创建副本时无法叠回母本 *
            GameObject ob = this.GetSystem<UtilSystem>().CreateCardGameObject(cardMono.card.GetNewCopy());
            ICanDragComponentMono newMono = ob.GetComponent<ICanDragComponentMono>();
            newMono.transform.position = cardMono.transform.position;
            newMono.Start();
            newMono.textIndex++;
            newMono.transform.SetAsLastSibling();
            // 减少自己的样本数量
            cardMono.StackCount--;
            newMono.GetComponent<CardMono>().BeforeSlotMono = cardMono.BeforeSlotMono;
            newMono.GetComponent<CardMono>().LastGridMono = cardMono.LastGridMono;


            //修改事件订阅目标为新的卡牌
            cardMono.StartCoroutine(TransferDragToCopyNextFrame(newMono, eventData));
            eventData.pointerDrag = null;
        }


        //将新的拖拽事件订阅到新的卡牌
        IEnumerator TransferDragToCopyNextFrame(ICanDragComponentMono copy, PointerEventData eventData)
        {
            yield return null; // 等待一帧，确保EventSystem释放鼠标控制权

            eventData.pointerDrag = copy.gameObject;
            // 强制将副本注册为拖拽目标
            EventSystem.current.SetSelectedGameObject(copy.gameObject);

            // 手动调用副本的拖拽开始逻辑
            ExecuteEvents.Execute(copy.gameObject, eventData, ExecuteEvents.beginDragHandler);
            ExecuteEvents.Execute(copy.gameObject, eventData, ExecuteEvents.dragHandler);

        }

        //重置拖拽状态 用于在副本结束拖拽时解除原来卡牌的不可拖动状态
        public void ResetDragState(ICanDragComponentMono mono)
        {
            mono.onEndDrag.RemoveListener(ResetDragState);
            mono.isCanBeDrag = true;
        }

        // 重新设置物体坐标到父物体
        public void ResetItemPositionToSlot(GameObject item, GameObject slot)
        {

            SlotMono slotMono = slot.GetComponent<SlotMono>();
            if (item == null) return;
            // === 新增：根据 isSlot 设置 item 的父物体 ===
            if (slotMono != null && slotMono.slot.isSlot)
            {
                //Debug.Log("重设父物体到" + slotMono.gameObject.name);
                item.transform.SetParent(slot.transform, worldPositionStays: true); // 新增
            }
            else
            {
                //Debug.Log("重设父物体到网格父物体" + slotMono.gameObject.name);
                item.transform.SetParent(UtilSystem.cardParent.transform, worldPositionStays: true); // 新增
            }

            // mono 的父物体是 parent
            Transform parent = item.transform.parent;

            // 将 slot 的坐标从世界坐标转换为 mono 所在空间的局部坐标
            Vector3 localPos = parent.InverseTransformPoint(slot.transform.position);
            CardMono cardMono = item.GetComponent<CardMono>();
            if (cardMono != null)
            {
                cardMono.transform.SetSiblingIndex(slotMono.gameObject.transform.childCount - 2);

            }
            // 赋值给 mono 的 RectTransform
            RectTransform rect = item.transform as RectTransform;
            if (rect != null)
                rect.anchoredPosition = localPos;
        }
        //public void ResetItemPositionToSlot(GameObject item, GameObject slot, float moveSpeed = 500f)
        //{
        //    SlotMono slotMono = slot.GetComponent<SlotMono>();
        //    if (item == null) return;
        //    Vector2 targetPos;
        //    RectTransform targetRect = null;
        //    // === 新增：根据 isSlot 设置 item 的父物体 ===
        //    if (slotMono != null && slotMono.slot.isSlot)
        //    {
        //        item.transform.SetParent(slot.transform, worldPositionStays: false);
        //        targetPos = new Vector2(0.01f, 0.01f);
        //    }
        //    else
        //    {   
        //        item.transform.SetParent(UtilSystem.cardParent.transform, worldPositionStays: false);
        //        targetPos = slot.GetComponent<RectTransform>().anchoredPosition;
        //        targetRect = slot.GetComponent<RectTransform>();
        //    }

        //    // mono 的父物体是 parent
        //    // Transform parent = item.transform.parent;

        //    // 将 slot 的坐标从世界坐标转换为 mono 所在空间的局部坐标
        //    //Vector3 localPos = parent.InverseTransformPoint(slot.transform.position);
        //    CardMono cardMono = item.GetComponent<CardMono>();
        //    if (cardMono != null)
        //    {
        //        cardMono.transform.SetSiblingIndex(slotMono.gameObject.transform.childCount - 2);
        //    }

        //    // 平滑移动
        //    RectTransform rect = item.transform as RectTransform;
        //    Debug.Log("目标是" + slot.name + "从" + rect.anchoredPosition + "位置" + slot.GetComponent<RectTransform>().anchoredPosition + "计算后的目标位置" + targetPos);

        //    if (rect != null)
        //    {
        //        // 停止之前的移动协程，避免叠加
        //        if (item.TryGetComponent<MonoBehaviourHelper>(out var helper))
        //        {
        //            helper.StopAllCoroutines();
        //        }
        //        else
        //        {
        //            helper = item.AddComponent<MonoBehaviourHelper>();
        //        }

        //        helper.StartCoroutine(MoveToPosition(rect, targetPos, moveSpeed));
        //    }
        //}

        //// 协程：让物体以恒定速度移动到目标位置
        //private IEnumerator MoveToPosition(RectTransform rect, Vector2 targetPos, float speed)
        //{
        //    Debug.Log("进程已经启动");
        //    yield return null;
        //    Debug.Log("进程已经启动");
        //    while ((rect.anchoredPosition - targetPos).sqrMagnitude > 0.01f)
        //    {
        //        Debug.Log("尝试移动" + rect.anchoredPosition + "目标位置" + targetPos);
        //        rect.anchoredPosition = Vector3.MoveTowards(
        //            rect.anchoredPosition,
        //            targetPos,
        //            speed * Time.deltaTime
        //        );
        //        yield return null;
        //    }
        //    Debug.Log("已经对齐" + rect.anchoredPosition + "目标位置" + targetPos);
        //    rect.anchoredPosition = targetPos; // 最终精确对齐
        //}

        // 辅助类：用来挂载协程
        public class MonoBehaviourHelper : MonoBehaviour { }


        #endregion

        #region 其他系统使用的函数，由于涉及到Mono被分割到这里
        // 将卡牌实体弹出到最近可用的卡槽
        public void OutputCardToTable(ITableElement elementMono, SlotMono slotMono,string dropZoneID = "")
        {

            if (slotMono != null && slotMono.slot.isSlot == false)
            {
                //Debug.Log("叠放目标" + slotMono.slot.label);
                StackCardToASlot(elementMono, slotMono, false);
            }
            else
            {
                // 有放置区可用
                if (dropZoneID != "")
                {
                    // 改为移动到放置区
                    DropZoneDragHelper dropZone = GameModel.GetDropZone(dropZoneID);
                    // Debug.Log("移动到放置区" + dropZoneID + "目标" + dropZone.targetSlot.name);

                    //StackCardToASlot(elementMono, dropZone.targetSlot, false);
                    MoveCardToClosestNullGrid(elementMono, dropZone.targetSlot);


                }
                else
                {
                    // Debug.Log("移动到空格");
                    MoveCardToClosestNullGrid(elementMono, slotMono);
                }
            }
        }
        // 直接将卡牌注册到行动框，并输入性相
        public void DirectAddCardToVerb(AbstractCard card, AbstractVerb verb)
        {
            AddCardElementToVerb(card, verb);
            verb.verbCardList.Add(card);
        }// 将卡牌从当前卡槽移动到另一个卡槽
        public void MoveCardFromSlotToAnotherSlot(CardMono cardMono, SlotMono slotMono)
        {
            if (cardMono.BelongtoSlotMono != null)
            {
                UnRegisterStackElementFromSlot(cardMono, cardMono.BelongtoSlotMono);
            }
            StackCardToASlot(cardMono, slotMono);
        }
        #endregion
        public void ReCalculateVerbContains(AbstractVerb verb)
        {

        }
        public void RestartGame()
        {
            foreach(var item in recipeModel.verbMonoList)
            {
                GameObject.Destroy(item.gameObject);
            }
            foreach(var item in gameModel.levelCardMonoList)
            {
                GameObject.Destroy(item.gameObject);
            }
        }

        public void InitTable()
        {
            gameModel.table = new AbstractTable();
            gameModel.table.InitTable();
        }
        public void Update()
        {
            //Debug.Log("要检查的内容数量" + gameModel.tableCardMonoList.Count);
            // 更新所有桌面上卡牌的流逝计数器
            for (int i = gameModel.tableCardMonoList.Count - 1; i >= 0; i-- )
            {

                var item = gameModel.tableCardMonoList[i];
                if (item == null) continue;
                AbstractCard card = item.card;

                if (card == null) continue;

                //Debug.Log("卡牌" + card.label + "状态" + card.isCanDecayByTime + "剩余时间" + card.lifeTime);

                if (card.isCanDecayByTime == false) continue;
                if (card.lifeTime > 0)
                {
                    card.lifeTime -= UtilModel.GameLogicDeltaTime;
                    if (card.cardMono != null)
                    {
                        card.cardMono.UpdateCardDecayView();
                    }
                }
                if (card.lifeTime <= 0)
                {
                    TranslateCardMonosCardToNewCard(item);
                    card.lifeTime = 0;
                }


                if (item.gameObject == null)
                    gameModel.tableCardMonoList.RemoveAt(i);
            }
            
            //for(int i = gameModel.dragMonoList.Count - 1; i >= 0;i --)
            //{
            //    if (gameModel.dragMonoList[i] is CardICanDragComponentMono dragMono )
            //    {
            //        if(dragMono.isDragging)
            //        {
            //            var item = gameModel.dragMonoList[i].GetComponent<CardMono>();
            //            if (item == null) continue;
            //            AbstractCard card = item.card;
            //            if (card == null) continue;
            //            //Debug.Log("卡牌" + card.label + "状态" + card.isCanDecayByTime + "剩余时间" + card.lifeTime);

            //            if (card.isCanDecayByTime == false) continue;
            //            if (card.lifeTime > 0)
            //            {
            //                card.lifeTime -= UtilModel.GameLogicDeltaTime;
            //                continue;
            //            }
            //            if (card.lifeTime <= 0)
            //                card.lifeTime = 0;
            //            TranslateCardMonosCardToNewCard(item);
            //        }
            //    }
            //}
        }
        public void LateUpdate()
        {
            //model.dragMonoList.Clear();
        }
        protected override void OnInit()
        {
            recipeSystem = this.GetSystem<RecipeSystem>();
        }
        public void LateInit()
        {
            gameModel = this.GetModel<GameModel>();
            utilSystem = this.GetSystem<UtilSystem>();
            recipeModel = this.GetModel<RecipeModel>();
        }
    }
    public enum TableElementMonoType
    {
        Card,Slot,Verb,SituationWindows,None
    }
}