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
        GameModel model;
        RecipeSystem recipeSystem;
        public static bool isUseNewDragMode = true; 
        // 物体开始将物体订阅到拖拽列表
        public void AddDragListen(ICanDragComponentMono mono)
        {
            // Debug.Log("接收到订阅");
            if (!model.dragMonoList.Contains(mono))
                model.dragMonoList.Add(mono);
        }
        // 物体结束拖拽时将物体从拖拽列表移除
        public void RemoveDragListen(ICanDragComponentMono mono)
        {
            if (model.dragMonoList.Contains(mono))
                model.dragMonoList.Remove(mono);
        }

        // 卡牌与卡槽堆叠时
        public bool StackElementWithSlot(ITableElement elementMono,SlotMono slotMono)
        {
            // 卡槽不承载行动框
            if(elementMono is VerbMono verbMono)
            {
                StackCardToASlot(elementMono, verbMono.LastGridMono);
                Debug.Log("踢回行动框");
                return false;
            }

            // 如果卡槽有空位，直接放入并注册
            if(slotMono.slot.stackItemList.Count < slotMono.slot.maxSlotItemCount)
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
        public bool StackElementWithGrid(ITableElement elementMono, SlotMono slotMono)
        {
            // 如果卡槽有空位，直接放入并注册

            if (slotMono.slot.stackItemList.Count < slotMono.slot.maxSlotItemCount)
            {
                RegisterStackElementToSlot(elementMono, slotMono);
                ResetItemPositionToSlot(elementMono.GetGameobject(), slotMono.gameObject);
                return true;
            }
            else
            {
                // 卡槽已满，处理卡牌叠放逻辑
                ITableElement beforeElementMono = slotMono.slot.stackItemList[0];
                // 对于网格而言，如果能重叠则叠放，否则另行处理
                if (beforeElementMono.CanStackWith(elementMono))
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
                    // 旧卡牌查找最近可用的位置
                     MoveCardToClosestNullGrid(beforeElementMono, beforeElementBeforeSlotMono);
                    return false;
                }
            }
        }
        // 将卡牌移动到最近的空卡槽中
        public void MoveCardToClosestNullGrid(ITableElement mono,SlotMono beforeSlotMono)
        {
            int x, y;
            if (beforeSlotMono == null)
            {
                x = model.table.defaultX;
                y = model.table.defaultY;
            }
            else
            {
                x = beforeSlotMono.x;
                y = beforeSlotMono.y;
            }
            int maxX = model.table.slotMonos.GetLength(0);
            int maxY = model.table.slotMonos.GetLength(1);

            for (int yOffset = 0; yOffset < maxY; yOffset++)
            {
                // y层次扩展：0 → +1 → -1 → +2 → -2 → ...
                int[] yCandidates = { y + yOffset, y - yOffset };
                foreach (int currentY in yCandidates)
                {
                    if (currentY < 0 || currentY >= maxY) continue;

                    for (int xOffset = 0; xOffset < maxX; xOffset++)
                    {
                        // x层次扩展：0 → +1 → -1 → +2 → -2 → ...
                        int[] xCandidates = { x - xOffset, x + xOffset };
                        foreach (int currentX in xCandidates)
                        {
                            if (currentX < 0 || currentX >= maxX) continue;

                            var slot = model.table.slotMonos[currentX, currentY];
                            if (slot.slot.stackItemList.Count == 0)
                            {
                                StackElementWithGrid(mono, slot);
                                return;
                            }
                        }
                    }
                }
            }
        }
        // 使用这个函数以播放音效
        public bool MonoStackCardToSlot(ITableElement elementMono,SlotMono slotMono,TableElementMonoType elementType)
        {
            bool result = StackCardToASlot(elementMono, slotMono);
            if(result)
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
                    UtilSystem.PlayAudio("card_drag_fail");
            }

                return result;
        }
        public void OutputCardToTable(ITableElement elementMono,SlotMono slotMono)
        {
            if (slotMono != null)
                StackCardToASlot(elementMono, slotMono);
            else
                MoveCardToClosestNullGrid(elementMono, slotMono);
        }
        // 分发器，判断卡槽类型并分发到对应的叠放函数中
        // 区别在于，这里传入的卡牌并不知道自己的目标是卡槽还是网格
        public bool StackCardToASlot(ITableElement cardMono,SlotMono slotMono)
        {
            if (slotMono == null || slotMono.slot == null) return false;
            if(slotMono.slot.isSlot)
            {
                return StackElementWithSlot(cardMono, slotMono);
            }
            else
            {
                //Debug.Log(cardMono.gameObject.name + "调用" + slotMono.name) ;
                return StackElementWithGrid(cardMono, slotMono);
            }
        }
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

        // 将卡牌从卡槽中移除注册 包含对场景物体的处理
        public void UnRegisterStackElementFromSlot(ITableElement stackMono,SlotMono slotMono,bool isUnregisterElement = true)
        {
            if (slotMono == null) return;
            slotMono.slot.stackItemList.Remove(stackMono);
            //cardMono.beforeSlotMono = null;
            stackMono.GetGameobject().GetComponent<ICanBelongToSlotMono>().BelongtoSlotMono = null;
            // 在被行动框调用时不移除数据防止出错
            if (isUnregisterElement)
            {
                if (stackMono is CardMono item)
                {
                    UnRegisterCardFromSlot(item.card, slotMono.slot);
                }
            }
        }
        // 将卡牌注册到卡槽 包含对场景物体的处理
        public void RegisterStackElementToSlot(ITableElement cardMono,SlotMono slotMono)
        {
            //Debug.Log("添加到" + slotMono.slot.label + "是否为卡槽" + slotMono.slot.isSlot);
            slotMono.slot.stackItemList.Add(cardMono);
            cardMono.BeforeSlotMono = slotMono;
            cardMono.BelongtoSlotMono = slotMono;
            if (!slotMono.slot.isSlot)
                cardMono.LastGridMono = slotMono;

            if(cardMono is CardMono item)
            {
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
                AddCardElementToVerb(card, slot.verb);
                foreach(AbstractSlot item in card.cardSlotList)
                {
                    if (isSlotCanPutInVerb(item, slot, slot.verb))
                    {
                        AddCardSlotToVerb(item, slot.verb);
                    }
                }
                recipeSystem.ReCalculateVerbRecipeState(slot.verb);
            }

        }
        // 将卡牌数据从卡槽数据中移除，抽象数据处理

        public void UnRegisterCardFromSlot(AbstractCard card, AbstractSlot slot,bool isTriggerReCalculate = true/*CardMono cardMono,SlotMono slotMono*/)
        {
            //slotMono.slot.card = null;
            slot.card = null;
            if (slot.verb != null)
            {
                RemoveCardElementFromVerb(card, slot.verb);
                RemoveCardSlotFromVerb(card, slot.verb);
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
            verb.verbMono.situationWindowMono.AddSlotObjectToVerbDominion(slot);
            // 7.22 可能有隐患，只注册不消除
            slot.verb = verb;
            verb.cardSlotList.Add(slot);
        }
        public bool isSlotCanPutInVerb(AbstractSlot addSlot, AbstractSlot putInSlot,AbstractVerb verb)
        {
            if (putInSlot.isVerbSlot == false) return false;
            foreach (string str in addSlot.slotPossibleShowVerbList)
            {
                if (str == "All")
                    return true;
                if (str == verb.stringIndex)
                    return true;
            }
            return false;
        }
        public void RemoveCardSlotFromVerb(AbstractCard card, AbstractVerb verb)
        {
            foreach (var slot in card.cardSlotList)
            {
                // 递归移除卡槽中卡牌的订阅
                if (slot.card != null && slot.card.cardMono != null)
                {
                    CardMono mono = slot.card.cardMono;
                    mono.transform.SetParent(UtilSystem.cardParent.transform, true);
                    UnRegisterStackElementFromSlot(slot.card.cardMono, slot.card.cardMono.BelongtoSlotMono);
                    //StackCardToASlot(mono, mono.LastGridMono);
                }
                verb.verbMono.situationWindowMono.RemoveSlotObjectFromVerbDominion(slot);
                verb.cardSlotList.Remove(slot);
            }
        }
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
        public void AddElementToVerb(string key,int value, AbstractVerb verb)
        {
            verb.AddAspect(key, value);
        }
  
        // 重新设置物体坐标到父物体
        public void ResetItemPositionToSlot(GameObject item,GameObject slot)
        {

            SlotMono slotMono = slot.GetComponent<SlotMono>();
            if (item == null) return;
            // === 新增：根据 isSlot 设置 item 的父物体 ===
            if (slotMono != null && slotMono.slot.isSlot)
            {
                item.transform.SetParent(slot.transform, worldPositionStays: false); // 新增
            }
            else
            {
                item.transform.SetParent(UtilSystem.cardParent.transform, worldPositionStays: false); // 新增
            }

            // mono 的父物体是 parent
            Transform parent = item.transform.parent;

            // 将 slot 的坐标从世界坐标转换为 mono 所在空间的局部坐标
            Vector3 localPos = parent.InverseTransformPoint(slot.transform.position);

            // 赋值给 mono 的 RectTransform
            RectTransform rect = item.transform as RectTransform;
            if (rect != null)
                rect.anchoredPosition = localPos;


        }

        public void ReCalculateVerbContains(AbstractVerb verb)
        {

        }
        public void InitTable()
        {
            model.table = new AbstractTable();
            model.table.InitTable();
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
            model = this.GetModel<GameModel>();
        }
    }
    public enum TableElementMonoType
    {
        Card,Slot,Verb,SituationWindows,None
    }
}