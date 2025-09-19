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
        // 基本数据订阅
        public GameSystem gameSystem;
        public RecipeSystem recipeSystem;
        public GameObject panelObject;
        // 行动框拥有的verb
        public AbstractVerb verb;
        // 行动框图片物体
        public Image artwork;
        // 从属卡槽关系

        // 行动框卡槽管理器
        public GameObject verbSlotManager;
        public List<SlotMono> slotMonoList = new List<SlotMono>();
        // 拖拽音效管理器
        public ICanDragPlayAudioComponentMono dragAudioMono;
        // 行动框拥有的卡槽面板
        public SituationWindowMono situationWindowMono;
        // 是否需要更新 以避免海量update降低性能
        public bool isNeedRefresh = false;

        public int slotSize => 2;

        public void LoadVerbData(AbstractVerb verb)
        {
            if(this.verb != null)
            // 移除之前的注册
            this.verb.verbMono = null;
            // 加入现在的注册
            this.verb = verb;
            artwork.sprite = verb.icon;
            verb.verbMono = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            // 订阅和设置变量 并刷新一次UI
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
            // 只在需要刷新时更新
            if(isNeedRefresh)
            {
                //Debug.Log("尝试更新Verb");
                isNeedRefresh = false;
                UtilSystem.RefreshVerbMonoUI(this);
            }
            else
            {

            }
            //if (verb.stringIndex == "AddTestVerb")
            //    Debug.Log("当前事件" + verb.situation.currentRecipe.label);
        }

        public bool IsInSlot()
        {
            return false;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            // 点击时切换事件面板的显示状态
            if (situationWindowMono.gameObject.activeSelf == false)
                situationWindowMono.EnableWindow();
            else
            {
                situationWindowMono.DisableWindow();
                situationWindowMono.PlayDisableAudio();
            }
            dragAudioMono.PlayGetClickAudio();
        }

        // 3.1.1.1节修改 判断能放入哪个卡槽
        public bool CanStackWith(ICanBeStack other)
        {
            if (other is CardMono cardMono)
            {
                if (verb.situation.situationState == AbstractSituation.SituationState.Prepare)
                {
                    foreach (AbstractSlot slot in verb.slotList)
                    {
                        //Debug.Log(cardMono.card.label + "检测卡槽" + slot.label + "当前物体数量" + slot.stackItemList.Count + "最大数量" + slot.maxSlotItemCount + "是否满足要求" + (RecipeSystem.IsCardMeetSlotsAspectRequire(cardMono.card, slot)));
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
        // 3.1.1.1修改 将卡牌直接放入卡槽并打开面板
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
            // 摧毁自身时解除订阅
            gameSystem.RemoveDragListen(this.GetComponent<ICanDragComponentMono>());
            UnRegisterFromSlotMono();
            Destroy(gameObject);
        }
        public void UnRegisterFromSlotMono()
        {
            // 解除对卡槽的注册
            if (BelongtoSlotMono != null)
                BelongtoSlotMono.slot.stackItemList.Remove(this);
            if (BeforeSlotMono != null)
                BeforeSlotMono.slot.stackItemList.Remove(this);
        }

        public GameObject GetGameobject()
        {
            // 给桌面元素用于获取游戏对象的接口
            return this.gameObject;
        }
        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
    }
}