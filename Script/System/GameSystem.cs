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
        GameModel model;
        RecipeSystem recipeSystem;
        public static bool isUseNewDragMode = true; 
        // ���忪ʼ�����嶩�ĵ���ק�б�
        public void AddDragListen(ICanDragComponentMono mono)
        {
            // Debug.Log("���յ�����");
            if (!model.dragMonoList.Contains(mono))
                model.dragMonoList.Add(mono);
        }
        // ���������קʱ���������ק�б��Ƴ�
        public void RemoveDragListen(ICanDragComponentMono mono)
        {
            if (model.dragMonoList.Contains(mono))
                model.dragMonoList.Remove(mono);
        }

        // �����뿨�۶ѵ�ʱ
        public bool StackElementWithSlot(ITableElement elementMono,SlotMono slotMono)
        {
            // ���۲������ж���
            if(elementMono is VerbMono verbMono)
            {
                StackCardToASlot(elementMono, verbMono.LastGridMono);
                Debug.Log("�߻��ж���");
                return false;
            }

            // ��������п�λ��ֱ�ӷ��벢ע��
            if(slotMono.slot.stackItemList.Count < slotMono.slot.maxSlotItemCount)
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
        public bool StackElementWithGrid(ITableElement elementMono, SlotMono slotMono)
        {
            // ��������п�λ��ֱ�ӷ��벢ע��

            if (slotMono.slot.stackItemList.Count < slotMono.slot.maxSlotItemCount)
            {
                RegisterStackElementToSlot(elementMono, slotMono);
                ResetItemPositionToSlot(elementMono.GetGameobject(), slotMono.gameObject);
                return true;
            }
            else
            {
                // ���������������Ƶ����߼�
                ITableElement beforeElementMono = slotMono.slot.stackItemList[0];
                // ����������ԣ�������ص�����ţ��������д���
                if (beforeElementMono.CanStackWith(elementMono))
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
                    // �ɿ��Ʋ���������õ�λ��
                     MoveCardToClosestNullGrid(beforeElementMono, beforeElementBeforeSlotMono);
                    return false;
                }
            }
        }
        // �������ƶ�������Ŀտ�����
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
                // y�����չ��0 �� +1 �� -1 �� +2 �� -2 �� ...
                int[] yCandidates = { y + yOffset, y - yOffset };
                foreach (int currentY in yCandidates)
                {
                    if (currentY < 0 || currentY >= maxY) continue;

                    for (int xOffset = 0; xOffset < maxX; xOffset++)
                    {
                        // x�����չ��0 �� +1 �� -1 �� +2 �� -2 �� ...
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
        // ʹ����������Բ�����Ч
        public bool MonoStackCardToSlot(ITableElement elementMono,SlotMono slotMono,TableElementMonoType elementType)
        {
            bool result = StackCardToASlot(elementMono, slotMono);
            if(result)
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
        // �ַ������жϿ������Ͳ��ַ�����Ӧ�ĵ��ź�����
        // �������ڣ����ﴫ��Ŀ��Ʋ���֪���Լ���Ŀ���ǿ��ۻ�������
        public bool StackCardToASlot(ITableElement cardMono,SlotMono slotMono)
        {
            if (slotMono == null || slotMono.slot == null) return false;
            if(slotMono.slot.isSlot)
            {
                return StackElementWithSlot(cardMono, slotMono);
            }
            else
            {
                //Debug.Log(cardMono.gameObject.name + "����" + slotMono.name) ;
                return StackElementWithGrid(cardMono, slotMono);
            }
        }
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

        // �����ƴӿ������Ƴ�ע�� �����Գ�������Ĵ���
        public void UnRegisterStackElementFromSlot(ITableElement stackMono,SlotMono slotMono,bool isUnregisterElement = true)
        {
            if (slotMono == null) return;
            slotMono.slot.stackItemList.Remove(stackMono);
            //cardMono.beforeSlotMono = null;
            stackMono.GetGameobject().GetComponent<ICanBelongToSlotMono>().BelongtoSlotMono = null;
            // �ڱ��ж������ʱ���Ƴ����ݷ�ֹ����
            if (isUnregisterElement)
            {
                if (stackMono is CardMono item)
                {
                    UnRegisterCardFromSlot(item.card, slotMono.slot);
                }
            }
        }
        // ������ע�ᵽ���� �����Գ�������Ĵ���
        public void RegisterStackElementToSlot(ITableElement cardMono,SlotMono slotMono)
        {
            //Debug.Log("��ӵ�" + slotMono.slot.label + "�Ƿ�Ϊ����" + slotMono.slot.isSlot);
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
        // ����������ע�ᵽ�������ݣ��������ݴ���
        public void RegisterCardToSlot(AbstractCard card,AbstractSlot slot/*CardMono cardMono,SlotMono slotMono*/)
        {
            //Debug.Log("��ӿ��Ƶ�����");
            //slotMono.slot.card = cardMono.card;
            slot.card = card;
            // ���ж����еĿ��۽���ע��
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
        // ���������ݴӿ����������Ƴ����������ݴ���

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
            verb.verbMono.situationWindowMono.AddSlotObjectToVerbDominion(slot);
            // 7.22 ������������ֻע�᲻����
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
                // �ݹ��Ƴ������п��ƵĶ���
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
            // 7.22 ������������ֻע�᲻����
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
  
        // ���������������굽������
        public void ResetItemPositionToSlot(GameObject item,GameObject slot)
        {

            SlotMono slotMono = slot.GetComponent<SlotMono>();
            if (item == null) return;
            // === ���������� isSlot ���� item �ĸ����� ===
            if (slotMono != null && slotMono.slot.isSlot)
            {
                item.transform.SetParent(slot.transform, worldPositionStays: false); // ����
            }
            else
            {
                item.transform.SetParent(UtilSystem.cardParent.transform, worldPositionStays: false); // ����
            }

            // mono �ĸ������� parent
            Transform parent = item.transform.parent;

            // �� slot ���������������ת��Ϊ mono ���ڿռ�ľֲ�����
            Vector3 localPos = parent.InverseTransformPoint(slot.transform.position);

            // ��ֵ�� mono �� RectTransform
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