using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class RecipeSystem : AbstractSystem
    {
        // �¼�ϵͳ
        public RecipeModel recipeModel;
        public GameSystem gameSystem;

        // ���¼����ж����¼��Ŀ����¼�
        public void ReCalculateVerbRecipeState(AbstractVerb verb)
        {
            // ����н�� ������Ϊ��Ӧ�¼�
            AbstractRecipe possibleRecipe = CalculateVerbRecipePossibleRecipe(verb);
            if (possibleRecipe != null)
            {
                verb.situation.possibleRecipe = possibleRecipe;
                verb.OnVerbDataChanged.Invoke(verb, AbstractVerb.VerbExchangeReason.PossibleRecipeExchange);
            }
            Debug.Log("���Ը���verb���¼�" + (possibleRecipe == null ? "û�н��" : possibleRecipe.label));

        }

        // ��ʼ�ж����е��¼�
        public void StartVerbRecipe(AbstractVerb verb)
        {
            // ����ǰ�¼�����Ϊʵ�ʸ��� ���޸��¼�����״̬Ϊִ����
            AbstractRecipe currentRecipe = verb.situation.possibleRecipe.GetNewCopy();
            verb.situation.situationState = AbstractSituation.SituationState.Excuting;
            //Debug.Log("Ŀ���¼� " + currentRecipe.createIndex + "�Ƿ�Ϊ��" + (currentRecipe == null) + "�¼�ִ��״̬" + currentRecipe.isExcuting + "���״̬" + currentRecipe.isFinished);
            // ���岻�����ſ�
            if (currentRecipe == null) return;
            // ���õ�ǰ�¼���״̬�����ü�ʱ��
            currentRecipe.isExcuting = true;
            currentRecipe.warpup = currentRecipe.maxWarpup;

            // >>���뵱ǰ�¼�����Ϣ
            // ��¼�¼�׼�������е����п���
            verb.verbCardList = new List<AbstractCard>();
            foreach(AbstractSlot slot in verb.slotList)
            {
                if (slot.card != null)
                    verb.verbCardList.Add(slot.card);
            }

            // ����ǰ�¼�������ע������
            foreach(var pair in currentRecipe.recipeAspectDictionary)
            {
                Debug.Log("�������" + pair);
                verb.AddAspect(pair.Key, pair.Value);
            }
            // ���¼���Ϣ�󶨵��¼�������
            verb.situation.currentRecipe = currentRecipe;
            // >>

            Debug.Log(verb.situation.currentRecipe.label + "�¼�" + "��������" + verb.situation.currentRecipe.recipeSlots.Count);
            // ��������ִ���¼�����Ϣ
            //��������ִ���п��۵Ŀ���
            foreach (AbstractSlot slot in verb.situation.currentRecipe.recipeSlots)
            {
                gameSystem.AddRecipeSlotToVerb(slot, verb);
            }

            verb.situation.recipeTextList.Add(new AbstractSituation.RecipeTextElement(currentRecipe.label,currentRecipe.excutingDescription));
            // �޸���ɣ�������Ӧ�޸��¼�
            verb.OnVerbDataChanged.Invoke(verb,AbstractVerb.VerbExchangeReason.RecipeStarted);
        }

        // ��ȡ�ж���ǰ����״̬���ʺϵ��¼�
        public AbstractRecipe CalculateVerbRecipePossibleRecipe(AbstractVerb verb)
        {
            AbstractRecipe recipe = null;
            foreach(var recipeGroup in verb.situation.possibleRecipeGroupKeyList)
            {
                foreach(var recipePair in RecipeDataBase.TryGetRecipeGroup(recipeGroup))
                {
                    if(recipePair.Value.isCreatable && IsVerbHaveEnoughRecipeElement(verb,recipePair.Value))
                    {
                        recipe = recipePair.Value;
                    }
                }
            }
            return recipe;
        }
        // �Ƿ��ж����������¼�Ҫ�����������
        public bool IsVerbHaveEnoughRecipeElement(AbstractVerb verb, AbstractRecipe recipe)
        {
            foreach(var elementPair in recipe.requireElementDictionary)
            {
                if (!IsVerbHaveEnoughElement(verb, elementPair.Key, elementPair.Value))
                    return false;
            }
            return true;
        }
        // �ж����Ƿ�����ض�������ĳ������
        public bool IsVerbHaveEnoughElement(AbstractVerb verb,string elementKey,int count)
        {
            // ������ʾ����ӵ�У���Ҫ���ڸ����󲢱Ƚ�����
            if(count > 0)
            {
                if (verb.aspectDictionary.ContainsKey(elementKey) && verb.aspectDictionary[elementKey] >= count)
                    return true;
            }
            // ������ʾ���ܶ���ĳ����
            else if(count <= 0)
            {
                int maxCount = -count;
                // ����������ȻС��
                if (!verb.aspectDictionary.ContainsKey(elementKey))
                    return true;
                // ���ڵ������������ֵ��ͨ��
                if (verb.aspectDictionary.ContainsKey(elementKey) && verb.aspectDictionary[elementKey] < maxCount) 
                    return true;
            }
            // �����ж�Ϊ��ͨ��
            return false;
        }

        // �ƽ��ж����ִ��ʱ��
        public void AddVerbWarpUp(AbstractVerb verb,float time)
        {
            AbstractRecipe recipe = verb.situation.currentRecipe;
            //Debug.Log("���Ŀ��" + verb.situation.createIndex + "�Ƿ���ִ��" + (recipe.isExcuting) +"�Ƿ��Ѿ����" + (recipe.isFinished ));
            if (recipe.isExcuting == false || recipe.isFinished == true)
                return;
                recipe.warpup -= time;
            if (recipe.warpup <= 0 && recipe.isFinished == false)
            {
                recipe.warpup = 0;
                DoVerbRecipeFinishCalculate(verb);

            }
        }
        // 2.2������ 2.3���޸�
        // �����ж����¼���ɵ�����
        public void DoVerbRecipeFinishCalculate(AbstractVerb verb)
        {
            // ��ȡ��������
            CashVerbRecipeSlots(verb);
            // �����������Ƶ�����
            // ���㿨���¼�����������
            CalculateVerbRecipeReward(verb);
            // ����״̬�л� �����ж���Ҫ��Ҫ��������
            CalculateVerbRecipeState(verb);

        }
        // 2.3 �¼�״̬������
        // ���ж����е��¼����۽�����ȡ
        public void CashVerbRecipeSlots(AbstractVerb verb)
        {
            //���㵱ǰ�¼����޸�
            //���ս����е�ǰ�¼���Ԫ��
            foreach (AbstractSlot slot in verb.verbRecipeSlotList)
            {
                AbstractCard card = slot.card;
                if (card != null)
                {
                    // ������Щ���ƣ���Ϊ���ٻᱻ�޸� Ҳ�������ٿ���
                    verb.verbCardList.Add(card);
                    CardMono cardMono = card.cardMono;
                    // ���岻���Ĵ���ʵ�庯�� ��������Щ���ƶ�����ʵ���
                    if (card.cardMono == null)
                    {
                        cardMono = UtilSystem.StaticCreateCardGameObject(card).GetComponent<CardMono>();
                        card.cardMono = cardMono;
                    }
                    // �Ƴ��Կ��۵İ󶨹�ϵ
                    gameSystem.UnRegisterStackElementFromSlot(card.cardMono, card.cardMono.BelongtoSlotMono, false);
                    // �ƶ��������������
                    verb.verbMono.situationWindowMono.AddCardToRewardCollecter(card);
                    // ��������Ϊ�����٣�����ݻ� ��������Ƴ����ۺ����ֹע����� * û��ʵ���ô������Ǳ���
                    if (card.isDisposed)
                    {
                        Debug.Log("Ҫ�ݻٵ�Ŀ�꿨������" + card.cardMono.name);
                        GameObject.Destroy(card.cardMono.gameObject);
                    }
                }
                // ���ټ�����ϵĿ�������
                verb.verbMono.situationWindowMono.RemoveSlotObjectFromRecipeExcutingSlotDominion(slot);
            }
            // �Ƴ������¼����еĿ�������
            verb.verbRecipeSlotList.Clear();
        }
        // �����¼����� ����ֻ�������棬���ж���״̬�Ľ���ŵ�����ȥ
        // 2.3 �¼�״̬�������ع������״̬�л������ݼ��㹦��
        public void CalculateVerbRecipeReward(AbstractVerb verb)
        {
            // ��ȡ����
            AbstractRecipe recipe = verb.situation.currentRecipe;


            // ���������¼�
            CalculateVerbRecipeLinkEvent(verb);
           // 7.25�������������������Recipe������


            recipe = verb.situation.currentRecipe;
            // �޸ı���
            recipe.isFinished = true;
            // �󶨱���
            verb.situation.recipeTextList.Add(new AbstractSituation.RecipeTextElement(recipe.finishedLabel, recipe.finishedDescription));
            // ͬ���޸�
            verb.OnVerbDataChanged.Invoke(verb, AbstractVerb.VerbExchangeReason.RecipeFinished);
            // �����¼��Կ��Ƶ��޸�
            CalculateVerbRecipeEffectReward(verb);


        }
        // 2.3 �¼�״̬��������ֳ���
        // �����¼�����������Ա�����ܵ��ж���δ����
        public void CalculateVerbRecipeLinkEvent(AbstractVerb verb)
        {
            AbstractRecipe recipe = verb.situation.currentRecipe;

            // ���Խ���������
            RecipeTriggerNode node = null;
            if (recipe.recipeLinker != null) node = recipe.recipeLinker.ResolveTrigger(verb);
            // �������Ϊ��ʱ֤�������������¼� ֱ���滻possibleRecipe
            if (node != null && node.isAdditional == false)
            {
                Debug.Log("Ŀ��" + node.targetRecipeGroup + "," + node.targetRecipe);
                verb.situation.possibleRecipe = RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup);
            }

            // �����Ҫ�������¼���ͬ ���Ϊ���������������¼� �Կ��۵Ĵ����Ѿ���֮ǰ������ ���������Recipe����ʽ
            if (verb.situation.possibleRecipe.IsEqualTo(verb.situation.currentRecipe))
            {
                foreach (var pair in verb.situation.currentRecipe.recipeAspectDictionary)
                {
                    // Debug.Log("�����޸�" + pair);
                    // ȡ��������ΪҪȡ������
                    verb.AddAspect(pair.Key, -pair.Value);
                }
            }
            else
            {
                ExchangeVerbCurrentRecipe(verb.situation.possibleRecipe.GetNewCopy(), verb);

            }
        }
        public void ExchangeVerbCurrentRecipe(AbstractRecipe newRecipe,AbstractVerb verb)
        {
            foreach(var pair in verb.situation.currentRecipe.recipeAspectDictionary)
            {
                Debug.Log("�����޸�" + pair);
                // ȡ��������ΪҪȡ������
                verb.AddAspect(pair.Key, -pair.Value);
            }
            verb.situation.currentRecipe = newRecipe;
            // ȡ����ֵ���˴��Ѿ��滻Ϊ��ֵ��
            foreach (var pair in verb.situation.currentRecipe.recipeAspectDictionary)
            {
                Debug.Log("�����޸�" + pair);
                verb.AddAspect(pair.Key, pair.Value);
            }
        }
        // 2.3.2 �����ܽ����ջ�
        // �����¼��ջ������Ӱ��
        public void CalculateVerbRecipeEffectReward(AbstractVerb verb)
        {
            AbstractRecipe recipe = verb.situation.possibleRecipe;
            foreach(CardEffect effect in recipe.effects)
            {
                effect.Apply(verb.verbCardList);
            }
            PrintVerbCardList(verb);

        }

        // 2.3 �¼�״̬��������ֳ���
        //������ȷ���ж����Ƿ�Ҫ�л�������ģʽ
        public void CalculateVerbRecipeState(AbstractVerb verb)
        {
            // �����Ȼ�����״̬����û���µ�����ִ�л��ߵȴ�ִ�е��¼�
            if (verb.situation.currentRecipe.recipeExcutingState == AbstractRecipe.RecipeExcutingState.Finished)
            {
                verb.situation.situationState = AbstractSituation.SituationState.WaitingForCollect;
                // ���������޸�
                CalculateVerbRecipeMonoReward(verb);
                // ������Ч * TOSET ����Ӧ��Ų�������ط�ȥ
                UtilSystem.PlayAudio("SituationComplete");
            }

        }
        public void CalculateVerbRecipeMonoReward(AbstractVerb verb)
        {
            SituationWindowMono situationWindow = verb.verbMono.situationWindowMono;
            VerbMono verbMono = verb.verbMono;
            // �����п��ƴӿ�����ȡ����
            foreach(AbstractCard card in verb.verbCardList)
            {
                if (card == null)
                    continue;
                CardMono cardMono = card.cardMono;
                if (card.cardMono == null)
                {
                    cardMono = UtilSystem.StaticCreateCardGameObject(card).GetComponent<CardMono>();
                    card.cardMono = cardMono;
                }
                gameSystem.UnRegisterStackElementFromSlot(card.cardMono, card.cardMono.BelongtoSlotMono);
                // �ƶ��������������
                verb.verbMono.situationWindowMono.AddCardToRewardCollecter(card);
                // ��������Ϊ�����٣�����ݻ� ��������Ƴ����ۺ����ֹע�����
                if (card.isDisposed)
                {
                    Debug.Log("Ҫ�ݻٵ�Ŀ�꿨������" + card.cardMono.name);
                    GameObject.Destroy(card.cardMono.gameObject);
                }
            }

        }
        // ��ȡ�¼�����
        public void CollectVerbRecipe(AbstractVerb verb)
        {
            foreach(AbstractCard card in verb.verbCardList)
            {
                if (card.isDisposed)
                    continue;
                CardMono cardMono = card.cardMono;
                if(card.cardMono == null)
                {
                    cardMono =  UtilSystem.StaticCreateCardGameObject(card).GetComponent<CardMono>();
                    card.cardMono = cardMono;
                }
                //gameSystem.StackCardToASlot(card.cardMono,card.cardMono.LastGridMono);
                // ���������������
                gameSystem.OutputCardToTable(cardMono, cardMono.LastGridMono);
                cardMono.RestoreOriginalRectTransform();
            }
            verb.verbCardList.Clear();
            ReloadVerbDefaultSituation(verb);
        }
        // Ϊverb��������Ĭ���¼�����
        public void ReloadVerbDefaultSituation(AbstractVerb verb)
        {
            verb.situation = SituationDataBase.TryGetSituation(verb.defaultSituationKey).GetNewCopy();
        }
        public void OutputCardFromSlot(AbstractCard card , AbstractSlot slot)
        {

        }

        // ��ǰ֡����
        public void PreUpdate()
        {
            UpdateVerb();

        }
        // �����ж���
        public void UpdateVerb()
        {
            List<VerbMono> monoList = recipeModel.verbMonoList;
            for (int i = monoList.Count - 1; i >= 0; i--)
            {
                VerbMono mono = monoList[i];
                if (mono == null)
                {
                    monoList.RemoveAt(i);
                }
                else
                {
                    AddVerbWarpUp(mono.verb, UtilModel.GameLogicDeltaTime);
                }
            }
        }

        protected override void OnInit()
        {
            recipeModel = this.GetModel<RecipeModel>();
            gameSystem = this.GetSystem<GameSystem>();
        }
        public void PrintVerbCardList(AbstractVerb verb)
        {
            if (verb == null || verb.verbCardList == null)
            {
                Debug.Log("verb �� verbCardList �ǿյģ�");
                return;
            }

            Debug.Log($"verbCardList ������ {verb.verbCardList.Count} �ſ���:");

            foreach (var card in verb.verbCardList)
            {
                if (card == null)
                {
                    Debug.Log("�����տ��ƶ���");
                    continue;
                }

                // ���������ַ���
                string aspects = "";
                if (card.aspectDictionary != null && card.aspectDictionary.Count > 0)
                {
                    foreach (var kvp in card.aspectDictionary)
                    {
                        aspects += $"{kvp.Key}={kvp.Value}, ";
                    }
                    aspects = aspects.TrimEnd(',', ' ');
                }
                else
                {
                    aspects = "������";
                }

                Debug.Log($"����: stringIndex={card.stringIndex}, label={card.label}, aspects=[{aspects}], isDisposed = {card.isDisposed}");
            }
        }

    }
}