using PlentyFishFramework;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class SituationWindowMono : MonoBehaviour, IEndDragHandler,IController
    {
        // 添加数据修改的订阅
        public VerbMono verbMono { set { verbmono = value; } get { return verbmono; } }
        private VerbMono verbmono;
        // 快捷方式
        public AbstractSituation situation => verbMono.verb.situation;
        public AbstractRecipe possibleRecipe => verbMono.verb.situation.possibleRecipe;
        // 行动框面板的名称标签和描述标签
        //public TextMeshProUGUI situationLabel,situationDescription;
        // 左上角图片
        public Image artWork;
        // 当前要表示的事件内容页码
        public int recipeCounter = 0;
        // 向左翻页和向右翻页按钮
        public GameObject leftButton, rightButton;
        // 容器大小元素
        public LayoutElement layoutElement;
        // 行动框未开始事件时的卡槽父物体及管理器
        public GameObject verbThresholdsDominion, verbThresholdsDominionManager;
        // 卡槽物体们
        public List<SlotMono> verbThresholdsDominionSlotMonoList = new List<SlotMono>();
        // 事件开始后卡槽父物体及管理器
        public GameObject recipeThresholdsDominion;
        public RecipeThresholdsDominionMono recipeThresholdsDominionMono;
        // 事件结束的事件管理器
        public GameObject storageDominion, rewardStorageDominionManager;
        // 事件结束的卡牌存储器
        public GameObject rewardCardStorageDominion;
        public GameObject outputDominion;
        // 行动框卡牌管理器
        //public GameObject cardManager;
        // 性相列表父物体
        public GameObject aspectDisplay;
        // 性相列表中性相子物体管理器
        public List<ElementFrameMono> elementFrameMonoList = new List<ElementFrameMono>();
        // 开始行动按钮
        public Button startButton;
        // 事件内容标签，事件描述标签
        public TextMeshProUGUI recipeLabel, recipeLore;
        // 收集卡牌按钮
        public Button finishCollectButton;

        // 2.3节加入
        // 记录卡牌卡槽父物体及卡槽们
        public GameObject recipeExcutingSlotParent;
        public List<SlotMono> recipeExcutingSlotList = new List<SlotMono>();
        private void OnEnable()
        {

        }
        public void SetVerbMono(VerbMono newVerbMono)
        {
            if (verbMono != null)
                verbMono.verb.OnVerbDataChanged.RemoveListener(OnVerbDataChanged);
            verbMono = newVerbMono;
            verbMono.verb.OnVerbDataChanged.AddListener(OnVerbDataChanged);
        }
        public void AddCardToRewardCollecter(AbstractCard card)
        {
            CardMono cardMono = card.cardMono;
            if (cardMono == null)
            {
                cardMono = this.GetSystem<UtilSystem>().CreateCardGameObject(card).GetComponent<CardMono>();
                cardMono.GetComponent<ICanDragComponentMono>().isCanBeDrag = true;
                card.cardMono = cardMono;
            }
            cardMono.transform.SetParent(rewardStorageDominionManager.transform, false);
        }
        public void AddSlotToRecipeExcutingSlotDominion(AbstractSlot slot)
        {
            GameObject ob = UtilSystem.CreateSlotInstantGameObject(slot);
            ob.transform.SetParent(recipeExcutingSlotParent.transform);
            SlotMono slotMono = ob.GetComponent<SlotMono>();
            slotMono.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            Debug.Log("卡槽" + slotMono.slot.label + "是否为卡槽" + slotMono.slot.isRecipeSlot);
            // 启用其名称text
            slotMono.slotLabel.gameObject.SetActive(true);
            recipeExcutingSlotList.Add(slotMono);
        }
        public void RemoveSlotObjectFromRecipeExcutingSlotDominion(AbstractSlot slot)
        {
            for (int i = recipeExcutingSlotList.Count - 1; i >= 0; i--)
            {
                if (recipeExcutingSlotList[i].slot == slot)
                {
                    GameObject ob = recipeExcutingSlotList[i].gameObject;
                    recipeExcutingSlotList.RemoveAt(i);
                    Destroy(ob);

                }
            }
        }
        // 添加一个卡槽到行动框中，设置物体关系并启用卡槽的名称
        public void AddSlotObjectToVerbDominion(AbstractSlot slot)
        {
            GameObject ob = UtilSystem.CreateSlotInstantGameObject(slot);
            ob.transform.SetParent(verbThresholdsDominionManager.transform);
            SlotMono slotMono = ob.GetComponent<SlotMono>();
            slotMono.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            //Debug.Log("卡槽" + slotMono.slot.label + "是否为卡槽" + slotMono.slot.isSlot);
            // 启用其名称text
            slotMono.slotLabel.gameObject.SetActive(true);
            verbThresholdsDominionSlotMonoList.Add(slotMono);
        }
        public void RemoveSlotObjectFromVerbDominion(AbstractSlot slot)
        {
            for (int i = verbThresholdsDominionSlotMonoList.Count - 1; i >= 0; i--)
            {
                if (verbThresholdsDominionSlotMonoList[i].slot == slot)
                {
                    GameObject ob = verbThresholdsDominionSlotMonoList[i].gameObject;
                    verbThresholdsDominionSlotMonoList.RemoveAt(i);
                    Destroy(ob);

                }
            }
        }
        // 修改当前事件页码
        public void OffsetRecipeCount(int value)
        {
            // 没有内容时证明在准备状态 此时无视所有修改
            if (verbMono.verb.situation.recipeTextList.Count == 0)
                recipeCounter = 0;
            // 获取修正后的值
            recipeCounter = recipeCounter + value;
            // 超过上现压制回来
            if (recipeCounter > verbMono.verb.situation.recipeTextList.Count)
                recipeCounter = verbMono.verb.situation.recipeTextList.Count;
            // 低于下限同样压制回来
            if (recipeCounter < 0) recipeCounter = 0;
            // 检查按钮可用情况
            CheckRecipeButtonState();
            // 刷新文本内容
            SetWindowRecipeUIText();
        }
        // 检查并设置翻页按钮的状态
        public void CheckRecipeButtonState()
        {
            int count = verbMono.verb.situation.recipeTextList.Count;
            // 准备状态直接跳过 不允许切换页码(也没法切换页码)
            if (verbMono.verb.situation.situationState == AbstractSituation.SituationState.Prepare)
            {
                leftButton.GetComponent<Button>().interactable = false;
                rightButton.GetComponent<Button>().interactable = false;
                return;
            }
            // 判断可能事件与当前事件是否为同一个
            bool isSameRecipe = situation.currentRecipe.stringIndex == situation.possibleRecipe.stringIndex;
            // 正在执行 且没有执行完毕 且可能的事件与当前事件不是同一个时，向后切换才有效
            //if(situation.currentRecipe.isExcuting && !situation.currentRecipe.isFinished && !isSameRecipe)
            if (situation.currentRecipe.recipeExcutingState == AbstractRecipe.RecipeExcutingState.Excuting && !isSameRecipe)
            {
                // 不是相同事件时，将possibleRecipe视为事件列表尾部进行计算
                // 没有文本时只有1个possibleRecipe，不能切换
                if (count <= 0)
                {
                    leftButton.GetComponent<Button>().interactable = false;
                    rightButton.GetComponent<Button>().interactable = false;
                    return;
                }
                // 省略了对count的判断，此时默认有多余2个（list1个 possible1个)事件可以切换
                // 不是最左边即可切换
                if (recipeCounter > 0)
                    leftButton.GetComponent<Button>().interactable = true;
                else
                    leftButton.GetComponent<Button>().interactable = false;

                // 由于此时索引为 0 - index，所以只要不是index就可以向右切换
                if (recipeCounter < count)
                    rightButton.GetComponent<Button>().interactable = true;
                else
                    rightButton.GetComponent<Button>().interactable = false;
            }
            // 否则只在list里切换
            else
            {
                // 是相同事件时，只能在list里切换
                // 只有1个文本或没有文本时无法切换
                if (count <= 1)
                {
                    leftButton.GetComponent<Button>().interactable = false;
                    rightButton.GetComponent<Button>().interactable = false;
                    return;
                }
                // 不是最左边时可以向左切换
                if (recipeCounter > 0 && count > 1)
                    leftButton.GetComponent<Button>().interactable = true;
                else
                    leftButton.GetComponent<Button>().interactable = false;
                // 不是最右边时可以向右切换
                if (recipeCounter < count - 1 && count > 1)
                    rightButton.GetComponent<Button>().interactable = true;
                else
                    rightButton.GetComponent<Button>().interactable = false;
            }
            // 旧版本
            //if (count <= 1)
            //{
            //    leftButton.GetComponent<Button>().interactable = false;
            //    rightButton.GetComponent<Button>().interactable = false;
            //    return;
            //}
            //if (recipeCounter > 0 && count > 1)
            //    leftButton.GetComponent<Button>().interactable = true;
            //else
            //    leftButton.GetComponent<Button>().interactable = false;

            //if (recipeCounter < count - 1 && count > 1)
            //    rightButton.GetComponent<Button>().interactable = true;
            //else
            //    rightButton.GetComponent<Button>().interactable = false;
        }
        // 启用行动框面板
        public void EnableWindow()
        {
            this.gameObject.SetActive(true);
            transform.SetAsLastSibling(); // 把它放到同级 UI 中的最上层
            recipeCounter = 0;
        }
        // 禁用行动框面板
        public void DisableWindow()
        {
            if (!Application.isPlaying) return;
            this.gameObject.SetActive(false);
            //GetComponent<ICanDragPlayAudioComponentMono>().PlayGameobjectDisableAudio();
        }
        // 播放禁用音效
        public void PlayDisableAudio()
        {
            GetComponent<ICanDragPlayAudioComponentMono>().PlayGameobjectDisableAudio();

        }
        private void OnDisable()
        {

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateSituationWindow();
            CheckRecipeUI();
            CheckRecipeButtonState();
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(UtilSystem.panelParent.transform);
        }
        // 更新行动框面板数据和显示
        public void UpdateSituationWindow()
        {
            if (verbMono == null || verbMono.verb == null) return;
            //Debug.Log(verbMono.verb.situation.label);
            AbstractSituation situation = verbMono.verb.situation;
            if (situation == null) return;
            artWork.sprite = situation.icon;

            //if (situation.possibleRecipe != null)
            //{
            //    situationLabel.text = situation.possibleRecipe.label;
            //}
            //else
            //{
            //    situationLabel.text = verbMono.verb.label;
            //    situationDescription.text = verbMono.verb.lore;
            //}

            if (verbMono.verb.slotList.Count > 6)
            {
                int count = verbMono.verb.slotList.Count;
                int rowCount = Mathf.CeilToInt(count / 3f);

                int height = 470;
                if (rowCount > 2)
                {
                    height += (rowCount - 2) * 150;
                }

                layoutElement.preferredHeight = height;
            }
            else
            {
                layoutElement.preferredHeight = 470;
            }

        }
        // 检查并设置事件UI状态
        public void CheckRecipeUI()
        {
            AbstractSituation situation = verbMono.verb.situation;

            CloseAllUI();
            if (situation.situationState == AbstractSituation.SituationState.Prepare)
            {
                UpdatePrepareUI();
            }
            else if (situation.situationState == AbstractSituation.SituationState.Excuting)
            {
                UpdateExcutingUI();
            }
            else if (situation.situationState == AbstractSituation.SituationState.WaitingForCollect)
            {
                UpdateCollectUI();
            }

        }
        // 关闭所有事件状态控件
        public void CloseAllUI()
        {
            verbThresholdsDominion.SetActive(false);
            recipeThresholdsDominion.SetActive(false);
            storageDominion.SetActive(false);
            outputDominion.SetActive(false);
            startButton.gameObject.SetActive(false);


        }
        // 根据当前事件状态设置其UI启用状态和内容
        public void UpdateExcutingUI()
        {
            recipeThresholdsDominion.SetActive(true);
            AbstractRecipe recipe = situation.currentRecipe;
            // 2.2节搭建信息面板重置
            //string willRecipeLabel, willRecipeLore;
            //willRecipeLabel = recipe.excutingLabel;
            //willRecipeLore = recipe.excutingDescription;
            //recipeLabel.text = willRecipeLabel;
            //recipeLore.text = willRecipeLore;
            SetWindowRecipeUIText();
            recipeThresholdsDominion.SetActive(true);
            recipeThresholdsDominionMono.SetTimer(recipe);
        }
        public void UpdatePrepareUI()
        {
            verbThresholdsDominion.SetActive(true);
            startButton.gameObject.SetActive(true);

            SetWindowRecipeUIText();
            // 2.2节搭建信息面板重置
            //string willRecipeLabel,willRecipeLore;
            if (situation.possibleRecipe != null)
            {
                //willRecipeLabel = situation.possibleRecipe.label;
                //willRecipeLore = situation.possibleRecipe.description;
                startButton.interactable = situation.possibleRecipe.isStartable;
                //Debug.Log(situation.possibleRecipe.label + "事件互动情况" + situation.possibleRecipe.isStartable);
            }
            else
            {
                //willRecipeLabel = verbMono.verb.label;
                //willRecipeLore = verbMono.verb.lore;
                //Debug.Log("没有可以互动的事件");
                startButton.interactable = false;
            }
            //recipeLabel.text = willRecipeLabel;
            //recipeLore.text = willRecipeLore;


        }
        public void UpdateCollectUI()
        {
            outputDominion.SetActive(true);
            storageDominion.SetActive(true);
            AbstractRecipe recipe = situation.currentRecipe;
            // 2.2节搭建信息面板重置
            //string willRecipeLabel, willRecipeLore;
            //willRecipeLabel = recipe.excutingLabel;
            //willRecipeLore = recipe.excutingDescription;
            //recipeLabel.text = willRecipeLabel;
            //recipeLore.text = willRecipeLore;
            // 显示开始按钮但不可用
            startButton.gameObject.SetActive(true);
            startButton.interactable = false;
        }
        // 设置事件UI文本 默认此时行动框一定是执行或者执行完毕的状态
        public void SetWindowRecipeUIText()
        {
            List<AbstractSituation.RecipeTextElement> textList = situation.recipeTextList;
            string willRecipeLabel, willRecipeLore;
            // Debug.Log("行动框事件" + situation.currentRecipe.label + "状态" + situation.currentRecipe.isExcuting + "完成" + situation.currentRecipe.isFinished + "结果" + situation.currentRecipe.recipeExcutingState);
            // 执行状态 且当前事件不同
            //if (situation.currentRecipe.isExcuting && !situation.currentRecipe.isFinished && situation.possibleRecipe.stringIndex != situation.currentRecipe.stringIndex)
            if (situation.currentRecipe.recipeExcutingState == AbstractRecipe.RecipeExcutingState.Excuting && !situation.currentRecipe.IsEqualTo(situation.possibleRecipe))
            {
                // 增加对可能的当前事件的判断
                // 如果数量超过列表最大数量，则设置为可能事件的文本
                if (recipeCounter >= situation.recipeTextList.Count)
                {
                    willRecipeLabel = situation.possibleRecipe.excutingLabel;
                    willRecipeLore = situation.possibleRecipe.excutingDescription;
                }
                // 否则为文本变量中的内容
                else
                {
                    willRecipeLabel = textList[recipeCounter].recipeLabel;
                    willRecipeLore = textList[recipeCounter].recipeDescription;
                }
            }
            // 执行中或执行完成，现在可能事件与当前事件一定相同
            else if (situation.currentRecipe.isExcuting || situation.currentRecipe.recipeExcutingState == AbstractRecipe.RecipeExcutingState.Finished || situation.situationState == AbstractSituation.SituationState.WaitingForCollect)
            {
                if (recipeCounter >= textList.Count)
                    recipeCounter = textList.Count - 1;
                // Debug.Log("事件完成");
                willRecipeLabel = textList[recipeCounter].recipeLabel;
                willRecipeLore = textList[recipeCounter].recipeDescription;

            }
            // 准备状态
            else
            {
                // 有可能事件则调用可能事件的
                if (situation.possibleRecipe != null)
                {
                    willRecipeLabel = situation.possibleRecipe.label;
                    willRecipeLore = situation.possibleRecipe.description;
                    startButton.interactable = situation.possibleRecipe.isStartable;
                    //Debug.Log(situation.possibleRecipe.label + "事件互动情况" + situation.possibleRecipe.isStartable);
                }
                // 否则调用行动框自带的内容
                else
                {
                    willRecipeLabel = verbMono.verb.label;
                    willRecipeLore = verbMono.verb.lore;
                    //Debug.Log("没有可以互动的事件");
                    startButton.interactable = false;
                }

            }
            recipeLabel.text = willRecipeLabel;
            recipeLore.text = willRecipeLore;
        }
        // 行动框数据改变时修正某些不会自动刷新的变量 * 不是很必要 可以改成滞留变量
        public void OnVerbDataChanged(AbstractVerb verb, AbstractVerb.VerbExchangeReason reason)
        {
            if (reason == AbstractVerb.VerbExchangeReason.RecipeStarted || reason == AbstractVerb.VerbExchangeReason.RecipeFinished)
            {
                recipeCounter = situation.recipeTextList.Count - 1;
                SetWindowRecipeUIText();
            }
            if (reason == AbstractVerb.VerbExchangeReason.PossibleRecipeExchange)
            {
                recipeCounter = situation.recipeTextList.Count;
                SetWindowRecipeUIText();
            }
            //if (reason != AbstractVerb.VerbExchangeReason.PossibleRecipeExchange && reason != AbstractVerb.VerbExchangeReason.RecipeFinished && reason != AbstractVerb.VerbExchangeReason.All)
            //    return;
            ////if (reason == AbstractVerb.VerbExchangeReason.All)
            //    if (situation.currentRecipe.isFinished)
            //        recipeCounter = situation.recipeTextList.Count - 1;
            //    else if (situation.currentRecipe.isExcuting)
            //        recipeCounter = situation.recipeTextList.Count;
            //SetWindowRecipeUIText();
        }
        // 按钮事件 收获当前事件
        public void CollectCurrentRecipe()
        {
            this.verbMono.recipeSystem.CollectVerbRecipe(verbMono.verb);
            Update();
        }
        // 按钮事件 开始当前事件

        public void StartCurrentRecipe()
        {
            // Debug.Log("尝试开始事件");
            this.verbMono.recipeSystem.StartVerbSituation(this.verbMono.verb);
        }

        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
    }
}