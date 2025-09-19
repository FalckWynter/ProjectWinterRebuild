using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class VerbMono : BasicElementMono, IPointerClickHandler, ITableElement,IController
    {
        // �������ݶ���
        public GameSystem gameSystem;
        public RecipeSystem recipeSystem;
        public GameObject panelObject;
        // �ж���ӵ�е�verb
        public AbstractVerb verb;
        // �ж���ͼƬ����
        public Image artwork;
        // �������۹�ϵ

        // �ж��򿨲۹�����
        public GameObject verbSlotManager;
        public List<SlotMono> slotMonoList = new List<SlotMono>();
        // ��ק��Ч������
        public ICanDragPlayAudioComponentMono dragAudioMono;
        // �ж���ӵ�еĿ������
        public SituationWindowMono situationWindowMono;
        // �Ƿ���Ҫ���� �Ա��⺣��update��������
        public bool isNeedRefresh = false;

        public int slotSize => 2;

        public void LoadVerbData(AbstractVerb verb)
        {
            if(this.verb != null)
            // �Ƴ�֮ǰ��ע��
            this.verb.verbMono = null;
            // �������ڵ�ע��
            this.verb = verb;
            artwork.sprite = verb.icon;
            verb.verbMono = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            // ���ĺ����ñ��� ��ˢ��һ��UI
            gameSystem = this.GetSystem<GameSystem>();
            recipeSystem = this.GetSystem<RecipeSystem>();
            panelObject.GetComponent<RectTransform>().SetParent(UtilSystem.panelParent.transform);
            dragAudioMono = GetComponent<ICanDragPlayAudioComponentMono>();
            situationWindowMono.SetVerbMono(this);
            UtilSystem.RefreshVerbMonoUI(this);
        }

        // Update is called once per frame
        void Update()
        {
            // ֻ����Ҫˢ��ʱ����
            if(isNeedRefresh)
            {
                //Debug.Log("���Ը���Verb");
                isNeedRefresh = false;
                UtilSystem.RefreshVerbMonoUI(this);
            }
            else
            {

            }
            //if (verb.stringIndex == "AddTestVerb")
            //    Debug.Log("��ǰ�¼�" + verb.situation.currentRecipe.label);
        }

        public bool IsInSlot()
        {
            return false;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            // ���ʱ�л��¼�������ʾ״̬
            if (situationWindowMono.gameObject.activeSelf == false)
                situationWindowMono.EnableWindow();
            else
            {
                situationWindowMono.DisableWindow();
                situationWindowMono.PlayDisableAudio();
            }
            dragAudioMono.PlayGetClickAudio();
        }

        // 3.1.1.1���޸� �ж��ܷ����ĸ�����
        public bool CanStackWith(ICanBeStack other)
        {
            if (other is CardMono cardMono)
            {
                if (verb.situation.situationState == AbstractSituation.SituationState.Prepare)
                {
                    foreach (AbstractSlot slot in verb.slotList)
                    {
                        //Debug.Log(cardMono.card.label + "��⿨��" + slot.label + "��ǰ��������" + slot.stackItemList.Count + "�������" + slot.maxSlotItemCount + "�Ƿ�����Ҫ��" + (RecipeSystem.IsCardMeetSlotsAspectRequire(cardMono.card, slot)));
                        if (slot.stackItemList.Count < slot.maxSlotItemCount)
                        //if (RecipeSystem.IsCardMeetSlotsAspectRequire(cardMono.card, slot))
                        {
                            if (RecipeSystem.IsCardMeetSlotsAspectRequire(cardMono.card, slot))
                                return true;

                        }
                    }
                    return false;
                }
                else if (verb.situation.situationState == AbstractSituation.SituationState.Excuting)
                {
                    foreach (AbstractSlot slot in verb.verbRecipeSlotList)
                    {
                        if (slot.stackItemList.Count < slot.maxSlotItemCount)
                        //if (RecipeSystem.IsCardMeetSlotsAspectRequire(cardMono.card, slot))
                        {
                            if (RecipeSystem.IsCardMeetSlotsAspectRequire(cardMono.card, slot))
                                return true;

                        }
                    }
                    return false;
                }
            }
            return false;
        }
        // 3.1.1.1�޸� ������ֱ�ӷ��뿨�۲������
        public bool TryAddStack(ICanBeStack other)
        {
            if (other is CardMono cardMono)
            {
                if (verb.situation.situationState == AbstractSituation.SituationState.Prepare)
                {
                    foreach (AbstractSlot slot in verb.slotList)
                    {
                        if (slot.stackItemList.Count < slot.maxSlotItemCount)
                        {
                            if (RecipeSystem.IsCardMeetSlotsAspectRequire(cardMono.card, slot))
                            {
                                gameSystem.MonoStackCardToSlot(cardMono, slot.slotMono, TableElementMonoType.Slot);
                                if (situationWindowMono.gameObject.activeSelf == false)
                                {
                                    situationWindowMono.EnableWindow();
                                }
                                return true;
                            }

                        }
                    }
                }
                else if (verb.situation.situationState == AbstractSituation.SituationState.Excuting)
                {
                    foreach (AbstractSlot slot in verb.verbRecipeSlotList)
                    {
                        if (slot.stackItemList.Count < slot.maxSlotItemCount)
                        {
                            if (RecipeSystem.IsCardMeetSlotsAspectRequire(cardMono.card, slot))
                            {
                                gameSystem.MonoStackCardToSlot(cardMono, slot.slotMono, TableElementMonoType.Slot);
                                if (situationWindowMono.gameObject.activeSelf == false)
                                {
                                    situationWindowMono.EnableWindow();
                                }
                                return true;
                            }

                        }
                    }
                }
            }
            return false;

        }

        public bool TrySubStack(ICanBeStack other)
        {
            return true;
        }

        public void DestroySelf()
        {
            // �ݻ�����ʱ�������
            gameSystem.RemoveDragListen(this.GetComponent<ICanDragComponentMono>());
            UnRegisterFromSlotMono();
            Destroy(gameObject);
        }
        public void UnRegisterFromSlotMono()
        {
            // ����Կ��۵�ע��
            if (BelongtoSlotMono != null)
                BelongtoSlotMono.slot.stackItemList.Remove(this);
            if (BeforeSlotMono != null)
                BeforeSlotMono.slot.stackItemList.Remove(this);
        }

        public GameObject GetGameobject()
        {
            // ������Ԫ�����ڻ�ȡ��Ϸ����Ľӿ�
            return this.gameObject;
        }
        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
    }
}