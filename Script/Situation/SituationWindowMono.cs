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
        // ��������޸ĵĶ���
        public VerbMono verbMono { set { verbmono = value; } get { return verbmono; } }
        private VerbMono verbmono;
        // ��ݷ�ʽ
        public AbstractSituation situation => verbMono.verb.situation;
        public AbstractRecipe possibleRecipe => verbMono.verb.situation.possibleRecipe;
        // �ж����������Ʊ�ǩ��������ǩ
        //public TextMeshProUGUI situationLabel,situationDescription;
        // ���Ͻ�ͼƬ
        public Image artWork;
        // ��ǰҪ��ʾ���¼�����ҳ��
        public int recipeCounter = 0;
        // ����ҳ�����ҷ�ҳ��ť
        public GameObject leftButton, rightButton;
        // ������СԪ��
        public LayoutElement layoutElement;
        // �ж���δ��ʼ�¼�ʱ�Ŀ��۸����弰������
        public GameObject verbThresholdsDominion, verbThresholdsDominionManager;
        // ����������
        public List<SlotMono> verbThresholdsDominionSlotMonoList = new List<SlotMono>();
        // �¼���ʼ�󿨲۸����弰������
        public GameObject recipeThresholdsDominion;
        public RecipeThresholdsDominionMono recipeThresholdsDominionMono;
        // �¼��������¼�������
        public GameObject storageDominion, rewardStorageDominionManager;
        // �¼������Ŀ��ƴ洢��
        public GameObject rewardCardStorageDominion;
        public GameObject outputDominion;
        // �ж����ƹ�����
        //public GameObject cardManager;
        // �����б�����
        public GameObject aspectDisplay;
        // �����б������������������
        public List<ElementFrameMono> elementFrameMonoList = new List<ElementFrameMono>();
        // ��ʼ�ж���ť
        public Button startButton;
        // �¼����ݱ�ǩ���¼�������ǩ
        public TextMeshProUGUI recipeLabel, recipeLore;
        // �ռ����ư�ť
        public Button finishCollectButton;

        // 2.3�ڼ���
        // ��¼���ƿ��۸����弰������
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
            Debug.Log("����" + slotMono.slot.label + "�Ƿ�Ϊ����" + slotMono.slot.isRecipeSlot);
            // ����������text
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
        // ���һ�����۵��ж����У����������ϵ�����ÿ��۵�����
        public void AddSlotObjectToVerbDominion(AbstractSlot slot)
        {
            GameObject ob = UtilSystem.CreateSlotInstantGameObject(slot);
            ob.transform.SetParent(verbThresholdsDominionManager.transform);
            SlotMono slotMono = ob.GetComponent<SlotMono>();
            slotMono.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            //Debug.Log("����" + slotMono.slot.label + "�Ƿ�Ϊ����" + slotMono.slot.isSlot);
            // ����������text
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
        // �޸ĵ�ǰ�¼�ҳ��
        public void OffsetRecipeCount(int value)
        {
            // û������ʱ֤����׼��״̬ ��ʱ���������޸�
            if (verbMono.verb.situation.recipeTextList.Count == 0)
                recipeCounter = 0;
            // ��ȡ�������ֵ
            recipeCounter = recipeCounter + value;
            // ��������ѹ�ƻ���
            if (recipeCounter > verbMono.verb.situation.recipeTextList.Count)
                recipeCounter = verbMono.verb.situation.recipeTextList.Count;
            // ��������ͬ��ѹ�ƻ���
            if (recipeCounter < 0) recipeCounter = 0;
            // ��鰴ť�������
            CheckRecipeButtonState();
            // ˢ���ı�����
            SetWindowRecipeUIText();
        }
        // ��鲢���÷�ҳ��ť��״̬
        public void CheckRecipeButtonState()
        {
            int count = verbMono.verb.situation.recipeTextList.Count;
            // ׼��״ֱ̬������ �������л�ҳ��(Ҳû���л�ҳ��)
            if (verbMono.verb.situation.situationState == AbstractSituation.SituationState.Prepare)
            {
                leftButton.GetComponent<Button>().interactable = false;
                rightButton.GetComponent<Button>().interactable = false;
                return;
            }
            // �жϿ����¼��뵱ǰ�¼��Ƿ�Ϊͬһ��
            bool isSameRecipe = situation.currentRecipe.stringIndex == situation.possibleRecipe.stringIndex;
            // ����ִ�� ��û��ִ����� �ҿ��ܵ��¼��뵱ǰ�¼�����ͬһ��ʱ������л�����Ч
            //if(situation.currentRecipe.isExcuting && !situation.currentRecipe.isFinished && !isSameRecipe)
            if (situation.currentRecipe.recipeExcutingState == AbstractRecipe.RecipeExcutingState.Excuting && !isSameRecipe)
            {
                // ������ͬ�¼�ʱ����possibleRecipe��Ϊ�¼��б�β�����м���
                // û���ı�ʱֻ��1��possibleRecipe�������л�
                if (count <= 0)
                {
                    leftButton.GetComponent<Button>().interactable = false;
                    rightButton.GetComponent<Button>().interactable = false;
                    return;
                }
                // ʡ���˶�count���жϣ���ʱĬ���ж���2����list1�� possible1��)�¼������л�
                // ��������߼����л�
                if (recipeCounter > 0)
                    leftButton.GetComponent<Button>().interactable = true;
                else
                    leftButton.GetComponent<Button>().interactable = false;

                // ���ڴ�ʱ����Ϊ 0 - index������ֻҪ����index�Ϳ��������л�
                if (recipeCounter < count)
                    rightButton.GetComponent<Button>().interactable = true;
                else
                    rightButton.GetComponent<Button>().interactable = false;
            }
            // ����ֻ��list���л�
            else
            {
                // ����ͬ�¼�ʱ��ֻ����list���л�
                // ֻ��1���ı���û���ı�ʱ�޷��л�
                if (count <= 1)
                {
                    leftButton.GetComponent<Button>().interactable = false;
                    rightButton.GetComponent<Button>().interactable = false;
                    return;
                }
                // ���������ʱ���������л�
                if (recipeCounter > 0 && count > 1)
                    leftButton.GetComponent<Button>().interactable = true;
                else
                    leftButton.GetComponent<Button>().interactable = false;
                // �������ұ�ʱ���������л�
                if (recipeCounter < count - 1 && count > 1)
                    rightButton.GetComponent<Button>().interactable = true;
                else
                    rightButton.GetComponent<Button>().interactable = false;
            }
            // �ɰ汾
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
        // �����ж������
        public void EnableWindow()
        {
            this.gameObject.SetActive(true);
            transform.SetAsLastSibling(); // �����ŵ�ͬ�� UI �е����ϲ�
            recipeCounter = 0;
        }
        // �����ж������
        public void DisableWindow()
        {
            if (!Application.isPlaying) return;
            this.gameObject.SetActive(false);
            //GetComponent<ICanDragPlayAudioComponentMono>().PlayGameobjectDisableAudio();
        }
        // ���Ž�����Ч
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
        // �����ж���������ݺ���ʾ
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
        // ��鲢�����¼�UI״̬
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
        // �ر������¼�״̬�ؼ�
        public void CloseAllUI()
        {
            verbThresholdsDominion.SetActive(false);
            recipeThresholdsDominion.SetActive(false);
            storageDominion.SetActive(false);
            outputDominion.SetActive(false);
            startButton.gameObject.SetActive(false);


        }
        // ���ݵ�ǰ�¼�״̬������UI����״̬������
        public void UpdateExcutingUI()
        {
            recipeThresholdsDominion.SetActive(true);
            AbstractRecipe recipe = situation.currentRecipe;
            // 2.2�ڴ��Ϣ�������
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
            // 2.2�ڴ��Ϣ�������
            //string willRecipeLabel,willRecipeLore;
            if (situation.possibleRecipe != null)
            {
                //willRecipeLabel = situation.possibleRecipe.label;
                //willRecipeLore = situation.possibleRecipe.description;
                startButton.interactable = situation.possibleRecipe.isStartable;
                //Debug.Log(situation.possibleRecipe.label + "�¼��������" + situation.possibleRecipe.isStartable);
            }
            else
            {
                //willRecipeLabel = verbMono.verb.label;
                //willRecipeLore = verbMono.verb.lore;
                //Debug.Log("û�п��Ի������¼�");
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
            // 2.2�ڴ��Ϣ�������
            //string willRecipeLabel, willRecipeLore;
            //willRecipeLabel = recipe.excutingLabel;
            //willRecipeLore = recipe.excutingDescription;
            //recipeLabel.text = willRecipeLabel;
            //recipeLore.text = willRecipeLore;
            // ��ʾ��ʼ��ť��������
            startButton.gameObject.SetActive(true);
            startButton.interactable = false;
        }
        // �����¼�UI�ı� Ĭ�ϴ�ʱ�ж���һ����ִ�л���ִ����ϵ�״̬
        public void SetWindowRecipeUIText()
        {
            List<AbstractSituation.RecipeTextElement> textList = situation.recipeTextList;
            string willRecipeLabel, willRecipeLore;
            // Debug.Log("�ж����¼�" + situation.currentRecipe.label + "״̬" + situation.currentRecipe.isExcuting + "���" + situation.currentRecipe.isFinished + "���" + situation.currentRecipe.recipeExcutingState);
            // ִ��״̬ �ҵ�ǰ�¼���ͬ
            //if (situation.currentRecipe.isExcuting && !situation.currentRecipe.isFinished && situation.possibleRecipe.stringIndex != situation.currentRecipe.stringIndex)
            if (situation.currentRecipe.recipeExcutingState == AbstractRecipe.RecipeExcutingState.Excuting && !situation.currentRecipe.IsEqualTo(situation.possibleRecipe))
            {
                // ���ӶԿ��ܵĵ�ǰ�¼����ж�
                // ������������б����������������Ϊ�����¼����ı�
                if (recipeCounter >= situation.recipeTextList.Count)
                {
                    willRecipeLabel = situation.possibleRecipe.excutingLabel;
                    willRecipeLore = situation.possibleRecipe.excutingDescription;
                }
                // ����Ϊ�ı������е�����
                else
                {
                    willRecipeLabel = textList[recipeCounter].recipeLabel;
                    willRecipeLore = textList[recipeCounter].recipeDescription;
                }
            }
            // ִ���л�ִ����ɣ����ڿ����¼��뵱ǰ�¼�һ����ͬ
            else if (situation.currentRecipe.isExcuting || situation.currentRecipe.recipeExcutingState == AbstractRecipe.RecipeExcutingState.Finished || situation.situationState == AbstractSituation.SituationState.WaitingForCollect)
            {
                if (recipeCounter >= textList.Count)
                    recipeCounter = textList.Count - 1;
                // Debug.Log("�¼����");
                willRecipeLabel = textList[recipeCounter].recipeLabel;
                willRecipeLore = textList[recipeCounter].recipeDescription;

            }
            // ׼��״̬
            else
            {
                // �п����¼�����ÿ����¼���
                if (situation.possibleRecipe != null)
                {
                    willRecipeLabel = situation.possibleRecipe.label;
                    willRecipeLore = situation.possibleRecipe.description;
                    startButton.interactable = situation.possibleRecipe.isStartable;
                    //Debug.Log(situation.possibleRecipe.label + "�¼��������" + situation.possibleRecipe.isStartable);
                }
                // ��������ж����Դ�������
                else
                {
                    willRecipeLabel = verbMono.verb.label;
                    willRecipeLore = verbMono.verb.lore;
                    //Debug.Log("û�п��Ի������¼�");
                    startButton.interactable = false;
                }

            }
            recipeLabel.text = willRecipeLabel;
            recipeLore.text = willRecipeLore;
        }
        // �ж������ݸı�ʱ����ĳЩ�����Զ�ˢ�µı��� * ���Ǻܱ�Ҫ ���Ըĳ���������
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
        // ��ť�¼� �ջ�ǰ�¼�
        public void CollectCurrentRecipe()
        {
            this.verbMono.recipeSystem.CollectVerbRecipe(verbMono.verb);
            Update();
        }
        // ��ť�¼� ��ʼ��ǰ�¼�

        public void StartCurrentRecipe()
        {
            // Debug.Log("���Կ�ʼ�¼�");
            this.verbMono.recipeSystem.StartVerbSituation(this.verbMono.verb);
        }

        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
    }
}