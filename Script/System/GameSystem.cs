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
        // ��Ϸ���������ܺ��� ��Mono�ű�����Ϊ��
        GameModel gameModel;
        RecipeSystem recipeSystem;
        UtilSystem utilSystem;
        RecipeModel recipeModel;
        public static bool isUseNewDragMode = true; 
        // ���忪ʼ�����嶩�ĵ���ק�б�
        public void AddDragListen(ICanDragComponentMono mono)
        {
            // Debug.Log("���յ�����");
            if (!gameModel.dragMonoList.Contains(mono))
                gameModel.dragMonoList.Add(mono);
        }
        // ���������קʱ���������ק�б��Ƴ�
        public void RemoveDragListen(ICanDragComponentMono mono)
        {
            if (gameModel.dragMonoList.Contains(mono))
                gameModel.dragMonoList.Remove(mono);
        }

        // ʹ����������Բ�����Ч
        public bool MonoStackCardToSlot(ITableElement elementMono, SlotMono slotMono, TableElementMonoType elementType)
        {
            bool result = StackCardToASlot(elementMono, slotMono);
            if (result)
            {
                // ���Ƶ��ųɹ�
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
                    MentionSystem.ShowMessage("���ܷ����������","������岻�ܷ���������ۣ����鿨�۶Է��뿨�Ƶ�Ҫ��");
                }
            }

            return result;
        }
        // �ַ������жϿ������Ͳ��ַ�����Ӧ�ĵ��ź�����
        // �������ڣ����ﴫ��Ŀ��Ʋ���֪���Լ���Ŀ���ǿ��ۻ�������
        public bool StackCardToASlot(ITableElement cardMono, SlotMono slotMono, bool isCanStackWithVerb = true)
        {
            if (slotMono == null || slotMono.slot == null) return false;
            if (slotMono.slot.isSlot)
            {
                return StackElementWithSlot(cardMono, slotMono, isCanStackWithVerb);
            }
            else
            {
                //Debug.Log(cardMono.GetGameobject().name + "����" + slotMono.name);
                return StackElementWithGrid(cardMono, slotMono, isCanStackWithVerb);
            }
        }

        // �����뿨�۶ѵ�ʱ
        public bool StackElementWithSlot(ITableElement elementMono,SlotMono slotMono, bool isCanStackWithVerb = true)
        {
            // ���۲������ж���
            if(elementMono is VerbMono verbMono)
            {
                StackCardToASlot(elementMono, verbMono.LastGridMono);
                Debug.Log("�߻��ж���");
                return false;
            }
            CardMono cardMono = (CardMono)elementMono;
            // �����elementһ���ǿ���
            // ����Ų���ȥ�򵯻�
            if(!RecipeSystem.IsCardMeetSlotsAspectRequire(cardMono.card,slotMono.slot))
            {
                MoveCardToClosestNullGrid(cardMono, cardMono.BeforeSlotMono);
                return false;

            }
            //Debug.Log("���Ե��ŵ�Ԫ������" + slotMono.slot.stackItemList.Count);
            // 3.2.1.1 ̰�����۲������滻����
            if (slotMono.slot.isGreedy && slotMono.slot.stackItemList.Count >= slotMono.slot.maxSlotItemCount)
            {
                Debug.Log(elementMono.GetGameobject().name + "�ܾ����Ƽ���̰������");
                MoveCardToClosestNullGrid(elementMono, elementMono.LastGridMono);
                return false;
            }
            // ��������п�λ��ֱ�ӷ��벢ע��
            if (slotMono.slot.stackItemList.Count < slotMono.slot.maxSlotItemCount)
            {
                RegisterStackElementToSlot(elementMono, slotMono);
                // ��������λ��
                ResetItemPositionToSlot(elementMono.GetGameobject(), slotMono.gameObject);
                return true;
            }
            else
            {

                // ���������������Ƶ����߼� 
                // ���ڿ��۶��ԣ���Զ���滻��ǰ��������
                ITableElement beforeElementMono = slotMono.slot.stackItemList[0];
                //������ܵ��ţ���ԭ�еĿ��Ƽ��ߵ�����Ŀ�λ���Լ���������
                SlotMono beforeElementBeforeSlotMono = beforeElementMono.BelongtoSlotMono;
                // ��ȡԭ���Ŀ��ƣ��������ſ��ƶ�������
                UnRegisterStackElementFromSlot(beforeElementMono, beforeElementMono.BelongtoSlotMono);
                // �¿��Ʒ����ڳ��Ŀռ�
                StackCardToASlot(elementMono, beforeElementBeforeSlotMono);
                // �ɿ��Ʋ���������õ�λ��
                MoveCardToClosestNullGrid(beforeElementMono, beforeElementMono.LastGridMono);
                return false;
            }
        }
        // ��������������ѵ�ʱ
        public bool StackElementWithGrid(ITableElement elementMono, SlotMono slotMono,bool isCanStackWithVerb = true)
        {
            // ����ǿ����ҷŲ���ȥ�򵯻�
            if (elementMono is CardMono preCardMono)
            {
                if (!RecipeSystem.IsCardMeetSlotsAspectRequire(preCardMono.card, slotMono.slot))
                {
                    MoveCardToClosestNullGrid(preCardMono, preCardMono.BeforeSlotMono);
                    return false;
                }
            }
            // ��������п�λ��ֱ�ӷ��벢ע��

            if (slotMono.slot.stackItemList.Count < slotMono.slot.maxSlotItemCount)
            {
                RegisterStackElementToSlot(elementMono, slotMono);
                ResetItemPositionToSlot(elementMono.GetGameobject(), slotMono.gameObject);
                return true;
            }
            else
            {
                // 3.2.1.1 ̰�����۲������滻����
                if (slotMono.slot.isGreedy)
                {
                    MoveCardToClosestNullGrid(elementMono, elementMono.LastGridMono);
                    return false;
                }
                // ���������������Ƶ����߼�
                ITableElement beforeElementMono = slotMono.slot.stackItemList[0];
                if (!isCanStackWithVerb)
                {
                    if (beforeElementMono is VerbMono verbMono)
                    {
                        MoveCardToClosestNullGrid(elementMono, elementMono.BelongtoSlotMono);
                        return true;
                    }
                }
                // 3.1.1.1�� ���⴦���Ƶ��ŵ��ж�����߼�
                if (elementMono is CardMono cardMono && beforeElementMono is VerbMono beforeVerbMono)
                {
                    if (beforeVerbMono.CanStackWith(elementMono))
                    {
                        beforeVerbMono.TryAddStack(elementMono);
                        // ����Ҫ�Ƴ�Ԫ��
                        //elementMono.DestroySelf();
                        return true;
                    }
                    else
                    {
                        // ������ȥ�����ֱ��������Ŀո�
                        MoveCardToClosestNullGrid(elementMono, elementMono.LastGridMono, 2);
                        return false;
                    }
                }

                // ��������ʣ�µ�����������ԣ�������ص�����ţ��������д���
                if ( beforeElementMono.CanStackWith(elementMono))
                {
                    beforeElementMono.TryAddStack(elementMono);
                    // Ȼ�������������
                    elementMono.DestroySelf();
                    return true;
                }
                else
                {
                    //������ܵ��ţ���ԭ�еĿ��Ƽ��ߵ�����Ŀ�λ���Լ���������
                    SlotMono beforeElementBeforeSlotMono = beforeElementMono.BelongtoSlotMono;
                    // ��ȡԭ���Ŀ��ƣ��������ſ��ƶ�������
                    UnRegisterStackElementFromSlot(beforeElementMono, beforeElementMono.BelongtoSlotMono);
                    // �¿��Ʒ����ڳ��Ŀռ�
                    StackCardToASlot(elementMono, beforeElementBeforeSlotMono);
                    int length = 1;
                    if (beforeElementMono is VerbMono verbMono)
                        length = 2;
                        // �ɿ��Ʋ���������õ�λ��
                        MoveCardToClosestNullGrid(beforeElementMono, beforeElementBeforeSlotMono, length);
                    return false;
                }
            }
        }
        // �������ƶ�������Ŀտ�����
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
        //        // y�����չ��0 �� +1 �� -1 �� +2 �� -2 �� ...
        //        int[] yCandidates = { y + yOffset, y - yOffset };
        //        foreach (int currentY in yCandidates)
        //        {
        //            if (currentY < 0 || currentY >= maxY) continue;

        //            for (int xOffset = 0; xOffset < maxX; xOffset += stepLength)
        //            {
        //                // x�����չ��0 �� +1 �� -1 �� +2 �� -2 �� ...
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

            // ---------- 1. ��ǰ�� ----------
            if (CheckRowForEmptySlot(mono, startX, startY, maxX))
                return;

            // ---------- 2. ������� ----------
            for (int y = startY - stepLength; y >= 0; y -= stepLength)
            {
                if (CheckRowForEmptySlot(mono, startX, y, maxX))
                    return;
            }

            // ---------- 3. ������� ----------
            for (int y = startY + stepLength; y < maxY; y += stepLength)
            {
                if (CheckRowForEmptySlot(mono, startX, y, maxX))
                    return;
            }
        }
        /// <summary>
        /// ��ָ�� Y �㣬�� startX ��ʼ��������ɨ�裬������ɨ��
        /// </summary>
        private bool CheckRowForEmptySlot(ITableElement mono, int startX, int y, int maxX)
        {
            // ������
            for (int x = startX; x < maxX; x += mono.slotSize)
            {
                var slot = gameModel.table.slotMonos[x, y];
                if (slot.slot.stackItemList.Count == 0)
                {
                    StackElementWithGrid(mono, slot);
                    return true;
                }
            }

            // ������
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


        // �����ƴӿ������Ƴ�ע�� ��һ�����ڴ���mono���������� �����Գ�������Ĵ���
        public void UnRegisterStackElementFromSlot(ITableElement stackMono,SlotMono slotMono,bool isUnregisterElement = true)
        {
            if (slotMono == null) return;
            slotMono.slot.stackItemList.Remove(stackMono);
            //cardMono.beforeSlotMono = null;
            stackMono.GetGameobject().GetComponent<ICanBelongToSlotMono>().BelongtoSlotMono = null;
            // �ڱ��ж������ʱ���Ƴ����ݷ�ֹ����
            // 3.2.1.2 �����������
            if (stackMono is CardMono item)
            {
                // 3.2.1.2 �����������
                UnRegisterCardFromSlot(item.card, slotMono.slot,isTriggerReCalculate : false , isUnregisterElement : isUnregisterElement);
                //3.2.1.1
                if(item.GetComponent<ICanDragComponentMono>().canvasGroup != null)
                item.GetComponent<ICanDragComponentMono>().canvasGroup.blocksRaycasts = true;

            }   
        }
        // ������ע�ᵽ���� ��һ�����ڴ���mono���������� �����Գ�������Ĵ���
        public void RegisterStackElementToSlot(ITableElement cardMono,SlotMono slotMono)
        {
            slotMono.slot.stackItemList.Add(cardMono);
            cardMono.BeforeSlotMono = slotMono;
            cardMono.BelongtoSlotMono = slotMono;
            //Debug.Log("��ӵ�" + slotMono.slot.label + "�Ƿ�Ϊ����" + slotMono.slot.isSlot + "ֵ" + cardMono.BelongtoSlotMono.slot.label);
            if (!slotMono.slot.isSlot)
                cardMono.LastGridMono = slotMono;

            if(cardMono is CardMono item)
            {
                // 3.2.1.2 �����������

                // ���뿨��ʱ���������Ƴ�
                if (slotMono.slot.isSlot)
                {
                    gameModel.RemoveCardMonoFromTableList(item);
                }
                else
                {
                    // ������������ʱ���¼������涩��
                    gameModel.AddCardMonoToTableList(item);
                    // ���Ҽ���ؿ�����(�������;������һ;���Ǳ��ж����������)
                    gameModel.AddCardMonoToLevelList(item);
                }
                RegisterCardToSlot(item.card, slotMono.slot);


            }
        }
        // ����������ע�ᵽ�������ݣ��������ݴ���
        public void RegisterCardToSlot(AbstractCard card,AbstractSlot slot/*CardMono cardMono,SlotMono slotMono*/)
        {
            //Debug.Log("��ӿ��Ƶ�����");
            //slotMono.slot.card = cardMono.card;
            slot.card = card;
            // ���ж����еĿ��۽���ע��
            if (slot.verb != null)
            {
                //Debug.Log(card.label + "���ƾ��п�������" + card.cardSlotList.Count);
                // ע�Ῠ��Ԫ�� ����Я���Ŀ���
                AddCardElementToVerb(card, slot.verb);
                foreach(AbstractSlot item in card.cardSlotList)
                {
                    if (isSlotCanPutInVerb(item, slot, slot.verb))
                    {
                        AddCardSlotToVerb(item, slot.verb);
                    }
                }
                // ������һ���¼�����
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
        // ���������ݴӿ����������Ƴ����������ݴ���
        public void UnRegisterCardFromSlot(AbstractCard card, AbstractSlot slot,bool isTriggerReCalculate = true, bool isUnregisterElement = true/*CardMono cardMono,SlotMono slotMono*/)
        {
            //slotMono.slot.card = null;
            slot.card = null;
            if (slot.verb != null)
            {
                // �Ƴ��������ࡢ���۲������¼������п��۴��ڵݹ����
                if(isUnregisterElement)
                RemoveCardElementFromVerb(card, slot.verb);
                RemoveCardSlotFromVerb(card, slot.verb, isUnregisterElement);
                if(isTriggerReCalculate)
                recipeSystem.ReCalculateVerbRecipeState(slot.verb);

            }
        }
        // ����������ע�ᵽ�ж���
        public void AddCardElementToVerb(AbstractCard card,AbstractVerb verb)
        {
            //Debug.Log("����Ԫ��Ӱ��");
            AddElementToVerb(card.stringIndex, 1, verb);
            foreach(var item in card.aspectDictionary)
            {
                AddElementToVerb(item.Key, item.Value,verb);
            }
            // ע�Ῠ�۵��ж���

        }
        // ������������ж������Ƴ�
        public void RemoveCardElementFromVerb(AbstractCard card,AbstractVerb verb)
        {
            AddElementToVerb(card.stringIndex, -1, verb);
            foreach (var item in card.aspectDictionary)
            {
                AddElementToVerb(item.Key, -item.Value, verb);
            }
        }
        // �����ƵĿ�����ӵ��ж���
        public void AddCardSlotToVerb(AbstractSlot slot,AbstractVerb verb)
        {
            //Debug.Log("����µĿ��ƿ���" + slot.label);
            verb.verbMono.situationWindowMono.AddSlotObjectToVerbDominion(slot);
            // 7.22 ������������ֻע�᲻����
            slot.verb = verb;
            verb.cardSlotList.Add(slot);
        }

        // �����ƵĿ��۴��ж������Ƴ�
        public void RemoveCardSlotFromVerb(AbstractCard card, AbstractVerb verb,bool isUnRegisterElement = true)
        {
            foreach (var slot in card.cardSlotList)
            {
                // �ݹ��Ƴ������п��ƵĶ���
                if (slot.card != null && slot.card.cardMono != null)
                {
                    // ���������׳�
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
        // ���¼�Я�����ж�����ע�ᵽverb�в�����ʵ��
        public void AddRecipeSlotToVerb(AbstractSlot slot,AbstractVerb verb)
        {
            verb.verbMono.situationWindowMono.AddSlotToRecipeExcutingSlotDominion(slot);

            // 7.22 ������������ֻע�᲻����
            slot.verb = verb;
            
        }
        public void RemoveRecipeSlotFromVerb(AbstractSlot slot,AbstractVerb verb)
        {
            verb.verbMono.situationWindowMono.RemoveSlotObjectFromVerbDominion(slot);
            slot.verb = null;
        }
        // �޸��ж����Ԫ�شʵ�
        public void AddElementToVerb(string key,int value, AbstractVerb verb)
        {
            verb.AddAspect(key, value);
        }
  

        // ������˥��Ϊ�µĿ���
        public void TranslateCardMonosCardToNewCard(CardMono cardMono)
        {
            AbstractCard oldCard = cardMono.card;
            string newCardIndex = oldCard.decayToCardStringIndex;
            SlotMono oldCardSlot = cardMono.LastGridMono;
            if (cardMono.gameObject != null)
            {
                cardMono.DestroySelf();
                // �����Ҫת�䵽�Ŀ��ƣ���Ϊ�¿��ƴ���������ʵ�嵽����Ŀ��ò�λ
                if (newCardIndex != null)
                {
                    GameObject ob = utilSystem.CreateCardGameObject(CardDataBase.TryGetCard(newCardIndex).GetNewCopy());
                    MoveCardToClosestNullGrid(ob.GetComponent<ITableElement>(), oldCardSlot);
                }
            }
        }
        #region Monoͨ�ù��ߺ���
        // ����һ�����ڱ������ק�Ŀ��Ƹ��ƣ��Ҽ̳���ק����
        public void CreateCardCopyToBeDrag(CardMono cardMono, PointerEventData eventData)
        {
            // ������Ϊ�µĿ��Ƹ������и�ֵ * TOSET ���õ����ĺ��������ƽű������ ��ֹ�����ڴ�������ʱ�޷�����ĸ�� *
            GameObject ob = this.GetSystem<UtilSystem>().CreateCardGameObject(cardMono.card.GetNewCopy());
            ICanDragComponentMono newMono = ob.GetComponent<ICanDragComponentMono>();
            newMono.transform.position = cardMono.transform.position;
            newMono.Start();
            newMono.textIndex++;
            newMono.transform.SetAsLastSibling();
            // �����Լ�����������
            cardMono.StackCount--;
            newMono.GetComponent<CardMono>().BeforeSlotMono = cardMono.BeforeSlotMono;
            newMono.GetComponent<CardMono>().LastGridMono = cardMono.LastGridMono;


            //�޸��¼�����Ŀ��Ϊ�µĿ���
            cardMono.StartCoroutine(TransferDragToCopyNextFrame(newMono, eventData));
            eventData.pointerDrag = null;
        }


        //���µ���ק�¼����ĵ��µĿ���
        IEnumerator TransferDragToCopyNextFrame(ICanDragComponentMono copy, PointerEventData eventData)
        {
            yield return null; // �ȴ�һ֡��ȷ��EventSystem�ͷ�������Ȩ

            eventData.pointerDrag = copy.gameObject;
            // ǿ�ƽ�����ע��Ϊ��קĿ��
            EventSystem.current.SetSelectedGameObject(copy.gameObject);

            // �ֶ����ø�������ק��ʼ�߼�
            ExecuteEvents.Execute(copy.gameObject, eventData, ExecuteEvents.beginDragHandler);
            ExecuteEvents.Execute(copy.gameObject, eventData, ExecuteEvents.dragHandler);

        }

        //������ק״̬ �����ڸ���������קʱ���ԭ�����ƵĲ����϶�״̬
        public void ResetDragState(ICanDragComponentMono mono)
        {
            mono.onEndDrag.RemoveListener(ResetDragState);
            mono.isCanBeDrag = true;
        }

        // ���������������굽������
        public void ResetItemPositionToSlot(GameObject item, GameObject slot)
        {

            SlotMono slotMono = slot.GetComponent<SlotMono>();
            if (item == null) return;
            // === ���������� isSlot ���� item �ĸ����� ===
            if (slotMono != null && slotMono.slot.isSlot)
            {
                //Debug.Log("���踸���嵽" + slotMono.gameObject.name);
                item.transform.SetParent(slot.transform, worldPositionStays: true); // ����
            }
            else
            {
                //Debug.Log("���踸���嵽��������" + slotMono.gameObject.name);
                item.transform.SetParent(UtilSystem.cardParent.transform, worldPositionStays: true); // ����
            }

            // mono �ĸ������� parent
            Transform parent = item.transform.parent;

            // �� slot ���������������ת��Ϊ mono ���ڿռ�ľֲ�����
            Vector3 localPos = parent.InverseTransformPoint(slot.transform.position);
            CardMono cardMono = item.GetComponent<CardMono>();
            if (cardMono != null)
            {
                cardMono.transform.SetSiblingIndex(slotMono.gameObject.transform.childCount - 2);

            }
            // ��ֵ�� mono �� RectTransform
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
        //    // === ���������� isSlot ���� item �ĸ����� ===
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

        //    // mono �ĸ������� parent
        //    // Transform parent = item.transform.parent;

        //    // �� slot ���������������ת��Ϊ mono ���ڿռ�ľֲ�����
        //    //Vector3 localPos = parent.InverseTransformPoint(slot.transform.position);
        //    CardMono cardMono = item.GetComponent<CardMono>();
        //    if (cardMono != null)
        //    {
        //        cardMono.transform.SetSiblingIndex(slotMono.gameObject.transform.childCount - 2);
        //    }

        //    // ƽ���ƶ�
        //    RectTransform rect = item.transform as RectTransform;
        //    Debug.Log("Ŀ����" + slot.name + "��" + rect.anchoredPosition + "λ��" + slot.GetComponent<RectTransform>().anchoredPosition + "������Ŀ��λ��" + targetPos);

        //    if (rect != null)
        //    {
        //        // ֹ֮ͣǰ���ƶ�Э�̣��������
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

        //// Э�̣��������Ժ㶨�ٶ��ƶ���Ŀ��λ��
        //private IEnumerator MoveToPosition(RectTransform rect, Vector2 targetPos, float speed)
        //{
        //    Debug.Log("�����Ѿ�����");
        //    yield return null;
        //    Debug.Log("�����Ѿ�����");
        //    while ((rect.anchoredPosition - targetPos).sqrMagnitude > 0.01f)
        //    {
        //        Debug.Log("�����ƶ�" + rect.anchoredPosition + "Ŀ��λ��" + targetPos);
        //        rect.anchoredPosition = Vector3.MoveTowards(
        //            rect.anchoredPosition,
        //            targetPos,
        //            speed * Time.deltaTime
        //        );
        //        yield return null;
        //    }
        //    Debug.Log("�Ѿ�����" + rect.anchoredPosition + "Ŀ��λ��" + targetPos);
        //    rect.anchoredPosition = targetPos; // ���վ�ȷ����
        //}

        // �����ࣺ��������Э��
        public class MonoBehaviourHelper : MonoBehaviour { }


        #endregion

        #region ����ϵͳʹ�õĺ����������漰��Mono���ָ����
        // ������ʵ�嵯����������õĿ���
        public void OutputCardToTable(ITableElement elementMono, SlotMono slotMono,string dropZoneID = "")
        {

            if (slotMono != null && slotMono.slot.isSlot == false)
            {
                //Debug.Log("����Ŀ��" + slotMono.slot.label);
                StackCardToASlot(elementMono, slotMono, false);
            }
            else
            {
                // �з���������
                if (dropZoneID != "")
                {
                    // ��Ϊ�ƶ���������
                    DropZoneDragHelper dropZone = GameModel.GetDropZone(dropZoneID);
                    // Debug.Log("�ƶ���������" + dropZoneID + "Ŀ��" + dropZone.targetSlot.name);

                    //StackCardToASlot(elementMono, dropZone.targetSlot, false);
                    MoveCardToClosestNullGrid(elementMono, dropZone.targetSlot);


                }
                else
                {
                    // Debug.Log("�ƶ����ո�");
                    MoveCardToClosestNullGrid(elementMono, slotMono);
                }
            }
        }
        // ֱ�ӽ�����ע�ᵽ�ж��򣬲���������
        public void DirectAddCardToVerb(AbstractCard card, AbstractVerb verb)
        {
            AddCardElementToVerb(card, verb);
            verb.verbCardList.Add(card);
        }// �����ƴӵ�ǰ�����ƶ�����һ������
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
            //Debug.Log("Ҫ������������" + gameModel.tableCardMonoList.Count);
            // �������������Ͽ��Ƶ����ż�����
            for (int i = gameModel.tableCardMonoList.Count - 1; i >= 0; i-- )
            {

                var item = gameModel.tableCardMonoList[i];
                if (item == null) continue;
                AbstractCard card = item.card;

                if (card == null) continue;

                //Debug.Log("����" + card.label + "״̬" + card.isCanDecayByTime + "ʣ��ʱ��" + card.lifeTime);

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
            //            //Debug.Log("����" + card.label + "״̬" + card.isCanDecayByTime + "ʣ��ʱ��" + card.lifeTime);

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