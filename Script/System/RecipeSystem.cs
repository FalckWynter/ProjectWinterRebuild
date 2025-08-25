using QFramework;
using QFramework.PointGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace PlentyFishFramework
{
    public class RecipeSystem : AbstractSystem
    {
        // �¼�ϵͳ
        public RecipeModel recipeModel;
        public GameSystem gameSystem;
        public GameModel gameModel;
        public UtilSystem utilSystem;


        // �����ж�����¼�ִ��ģʽ��ֻ��ʼһ��
        public void StartVerbSituation(AbstractVerb verb)
        {
            // ����ǰ�¼�����Ϊʵ�ʸ��� ���޸��¼�����״̬Ϊִ����
            AbstractRecipe currentRecipe = verb.situation.possibleRecipe.GetNewCopy();
            //Debug.Log("Ŀ���¼� " + currentRecipe.createIndex + "�Ƿ�Ϊ��" + (currentRecipe == null) + "�¼�ִ��״̬" + currentRecipe.isExcuting + "���״̬" + currentRecipe.isFinished);
            // ���岻�����ſ�
            if (currentRecipe == null) return;
            verb.situation.situationState = AbstractSituation.SituationState.Excuting;
            // >>����׼��״̬�µ���Ϣ
            // ��¼�¼�׼�������е����п���
            verb.verbCardList = new List<AbstractCard>();
            //foreach(AbstractSlot slot in verb.slotList)
            // �ս��ж���׼�������е����п���
            for(int i = verb.slotList.Count - 1; i >= 0;i --)
            {
                AbstractSlot slot = verb.slotList[i];
                //Debug.Log(slot.label + "׼���޳�Ԫ��" + (slot.card == null ? "û�п���" : slot.card.label));
                GetCardToVerbCardList(verb, slot);
                if (slot.card != null)
                {
                    
                    gameSystem.UnRegisterStackElementFromSlot(slot.card.cardMono, slot.card.cardMono.BelongtoSlotMono, false);
                }

            }
            // ������ǰ�¼��������úͶ���Recipe����ز���
            SetVerbRecipeStarted(verb, currentRecipe);
            // >>
            //Debug.Log(verb.stringIndex + "�ж���" + verb.situation.currentRecipe.label + "�¼�" + "��������" + verb.situation.currentRecipe.recipeSlots.Count);
            // �޸���ɣ�������Ӧ�޸��¼�
            verb.OnVerbDataChanged.Invoke(verb,AbstractVerb.VerbExchangeReason.RecipeStarted);
        }
        // ���¼��л�Ϊִ��״̬
        public void SetVerbRecipeStarted(AbstractVerb verb, AbstractRecipe currentRecipe)
        {
            // �޸��¼�Ϊ��ǰ�¼�
            verb.situation.currentRecipe = currentRecipe;
            // ���õ�ǰ�¼���״̬�����ü�ʱ��
            currentRecipe.isExcuting = true;
            currentRecipe.warpup = currentRecipe.maxWarpup;
            // ����ǰ�¼�������ע������
            foreach (var pair in currentRecipe.recipeAspectDictionary)
            {
                //Debug.Log("�������" + pair);
                verb.AddAspect(pair.Key, pair.Value);
            }
            // ���ĵ�ǰ�¼��Ŀ���
            foreach (AbstractSlot slot in verb.situation.currentRecipe.recipeSlots)
            {
                gameSystem.AddRecipeSlotToVerb(slot, verb);
            }
            // �����¼���Ϣ
            verb.situation.recipeTextList.Add(new AbstractSituation.RecipeTextElement(currentRecipe.excutingLabel, currentRecipe.excutingDescription));
            verb.OnVerbDataChanged.Invoke(verb, AbstractVerb.VerbExchangeReason.RecipeStarted);

            // ���¼���Ϣ�󶨵��¼�������

        }
        // �����ƴӿ�����ת�Ƶ��ж�����
        public void GetCardToVerbCardList(AbstractVerb verb,AbstractSlot slot)
        {
            if (slot.card != null)
            {
                verb.verbCardList.Add(slot.card);
                // 3.2.1.2 �������������� �ڴ˴������ƴӹؿ����Ƴ�����Ϊ����������Ϸ�е�ʵ����
                gameModel.RemoveCardMonoFromLevelList(slot.card.cardMono);
                if (slot.isConsumes)
                {
                    //slot.card.isDisposed = true;
                    slot.card.isInVerbsByConsumeSlot = true;
                }
            }
        }
        #region �ж���Recipeѡ�񲿷�
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
        #endregion
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
            //Debug.Log("���Բ��ҵĿ�������" + verb.verbRecipeSlotList);
            foreach (AbstractSlot slot in verb.verbRecipeSlotList)
            {
                AbstractCard card = slot.card;
                if (card != null)
                {
                    //Debug.Log("�սɿ���" + card.label + card.createIndex);
                    // ������Щ���ƣ���Ϊ���ٻᱻ�޸� Ҳ�������ٿ���
                    //verb.verbCardList.Add(card);
                    GetCardToVerbCardList(verb, slot);
                    CardMono cardMono = card.cardMono;
                    // ���岻���Ĵ���ʵ�庯�� ��������Щ���ƶ�����ʵ���
                    if (card.cardMono == null)
                    {
                        cardMono = utilSystem.CreateCardGameObject(card).GetComponent<CardMono>();
                        card.cardMono = cardMono;
                    }
                    // �Ƴ��Կ��۵İ󶨹�ϵ
                    gameSystem.UnRegisterStackElementFromSlot(card.cardMono, card.cardMono.BelongtoSlotMono, false);
                    // �ƶ��������������
                    verb.verbMono.situationWindowMono.AddCardToRewardCollecter(card);
                    // ��������Ϊ�����٣�����ݻ� ��������Ƴ����ۺ����ֹע����� * û��ʵ���ô������Ǳ���
                    if (card.isDisposed)
                    {
                        //Debug.Log("Ҫ�ݻٵ�Ŀ�꿨������" + card.cardMono.name);
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


            // ���������¼� ����������Ǿ���Ҫ��Ϊ����Ŀ����¼�
            CalculateVerbRecipeLinkEvent(verb);
           // 7.25�������������������Recipe������


            recipe = verb.situation.currentRecipe;
            // �޸ı���
            recipe.isFinished = true;
            recipe.isExcuting = true;
            // �󶨱���
            verb.situation.recipeTextList.Add(new AbstractSituation.RecipeTextElement(recipe.finishedLabel, recipe.finishedDescription));
            // ͬ���޸�
            verb.OnVerbDataChanged.Invoke(verb, AbstractVerb.VerbExchangeReason.RecipeFinished);
            // �����¼��Կ��Ƶ��޸�
            CalculateVerbRecipeEffectReward(verb);

            // ������
            if(recipe.ending != null)
            {
                utilSystem.LoadEnding(EndingDataBase.TryGetEnding(recipe.ending));
            }


        }
        // 2.3 �¼�״̬��������ֳ���
        // �����¼�����������Ա�����ܵ��ж���δ�������൱�ڽ���һ�������PossibleRecipeCheck
        public void CalculateVerbRecipeLinkEvent(AbstractVerb verb)
        {
            AbstractRecipe recipe = verb.situation.currentRecipe;

            // ���Խ���������
            RecipeTriggerNode node = null;
            if (recipe.recipeLinker != null) node = recipe.recipeLinker.ResolveFirstTrigger(verb,false);
            // �������Ϊ��ʱ֤�������������¼� ֱ���滻possibleRecipe
            if (node != null && node.isAdditional == false)
            {
                //Debug.Log("Ŀ��" + node.targetRecipeGroup + "," + node.targetRecipe);
                verb.situation.possibleRecipe = RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup);
            }

            // �����Ҫ�������¼���ͬ ���Ϊ���������������¼� �Կ��۵Ĵ����Ѿ���֮ǰ������ ���������Recipe����ʽ
            // 7.26 ��Ϊֻ�����л������µ������� ��ͬ�¼��Ľ���ŵ�����ȥ
            if (!verb.situation.possibleRecipe.IsEqualTo(verb.situation.currentRecipe))
                ExchangeVerbCurrentRecipe(verb.situation.possibleRecipe.GetNewCopy(), verb);

            //if (verb.situation.possibleRecipe.IsEqualTo(verb.situation.currentRecipe))
            //{
            //    //foreach (var pair in verb.situation.currentRecipe.recipeAspectDictionary)
            //    //{
            //    //    // Debug.Log("�����޸�" + pair);
            //    //    // ȡ��������ΪҪȡ������
            //    //    verb.AddAspect(pair.Key, -pair.Value);
            //    //}
            //}
            //else
            //{
            //    // ��ô���ǶԵģ���Ϊ��ʱ����ģ�¿����¼�����˵�ǰ�¼���Ϊ����Ľ����������Ҫ�޸Ķ���
            //    ExchangeVerbCurrentRecipe(verb.situation.possibleRecipe.GetNewCopy(), verb);

            //}
        }
        #region �ж���ǰ�¼��л�����
        // ���ж���ĵ�ǰ�¼��л�Ϊ���¼�
        public void ExchangeVerbCurrentRecipe(AbstractRecipe newRecipe,AbstractVerb verb)
        {
            UnRegisterCurrentRecipeFromVerb(verb);
            RegisterRecipeToVerb(newRecipe, verb);

        }
        // һ�鷽�� �Ƴ��Ե�ǰ�¼���ע��
        public void UnRegisterCurrentRecipeFromVerb(AbstractVerb verb)
        {
            UnRegisterRecipeFromVerb(verb.situation.currentRecipe, verb);
        }

        public void UnRegisterRecipeFromVerb(AbstractRecipe recipe,AbstractVerb verb)
        {
            foreach (var pair in recipe.recipeAspectDictionary)
            {
                Debug.Log("�����޸�" + pair);
                // ȡ��������ΪҪȡ������
                verb.AddAspect(pair.Key, -pair.Value);
            }
        }
        //һ�鷽�� ���������¼���ע��
        public void RegisterCurrentRecipeToVerb(AbstractRecipe recipe, AbstractVerb verb)
        {
            RegisterRecipeToVerb(verb.situation.currentRecipe, verb);
        }
        public void RegisterRecipeToVerb(AbstractRecipe recipe, AbstractVerb verb)
        {
            verb.situation.currentRecipe = recipe;
            // ȡ����ֵ���˴��Ѿ��滻Ϊ��ֵ��
            foreach (var pair in recipe.recipeAspectDictionary)
            {
                Debug.Log("�����޸�" + pair);
                verb.AddAspect(pair.Key, pair.Value);
            }
        }
        #endregion
        // 2.3.2 �����ܽ����ջ�
        // �����¼��ջ������Ӱ��
        public void CalculateVerbRecipeEffectReward(AbstractVerb verb)
        {
            AbstractRecipe recipe = verb.situation.possibleRecipe;
            foreach(CardEffect effect in recipe.effects)
            {
                effect.Apply(verb.verbCardList);
            }
            //3.2.1.2 ����Կ��Ƶĺľ�
            TranslateBurrnedCardToNewCard(verb.verbCardList, verb);
            // ����Ҫ����Ŀ���
            CalculateVerbRecipeDeckDraws(verb);
            //PrintVerbCardList(verb);

        }
        public void CalculateVerbRecipeDeckDraws(AbstractVerb verb)
        {
            AbstractRecipe recipe = verb.situation.possibleRecipe;
            foreach(var item in recipe.deckeffects)
            {
                AbstractDeck deck = DeckDataBase.TryGetDeck(item.Key);
                for(int i = 0;i < item.Value;i++)
                {
                    gameSystem.DirectAddCardToVerb(CardDataBase.TryGetCard(deck.DrawCard()), verb);
                }
            }
            foreach(var item in recipe.recipeDecks)
            {
                for (int i = 0; i < item.cardDraws; i++)
                {
                    gameSystem.DirectAddCardToVerb(CardDataBase.TryGetCard(item.DrawCard()), verb);
                }
            }
        }

        // 2.3 �¼�״̬��������ֳ���
        //������ȷ���ж����Ƿ�Ҫ�л�������ģʽ
        public void CalculateVerbRecipeState(AbstractVerb verb)
        {
            CalculateNextRecipe(verb);
            // �����Ȼ�����״̬����û���µ�����ִ�л��ߵȴ�ִ�е��¼�
            if (verb.situation.linkRecipeList.Count <= 0 && verb.situation.currentRecipe.recipeExcutingState == AbstractRecipe.RecipeExcutingState.Finished)
            {
                verb.situation.situationState = AbstractSituation.SituationState.WaitingForCollect;
                // ���������޸�
                CalculateVerbRecipeMonoReward(verb);
                // ������Ч * TOSET ����Ӧ��Ų�������ط�ȥ
                UtilSystem.PlayAudio("SituationComplete");
            }
            else
            {
                // 3.2.3�¼���
                LoadVerbSituationNextLinkRecipe(verb);
            }

        }
        // �����ж�����¸������¼����趨���״̬
        public void CalculateNextRecipe(AbstractVerb verb)
        {
            AbstractRecipe recipe = verb.situation.currentRecipe;

            // ���Խ���������
            RecipeTriggerNode node = null;
            if (recipe.recipeLinker != null) node = recipe.recipeLinker.ResolveFirstTrigger   (verb,true);
            //Debug.Log(recipe.label + "ȡ���Ľ��" + (node == null) + "�Ƿ���������" + (recipe.recipeLinker == null)/* + "��������" + recipe.recipeLinker.triggerNodes.Count*/);
            if (node == null) return;
            // �������Ϊ��ʱ֤�������������¼� ֱ���滻possibleRecipe
            //Debug.Log("������" + node.additionalVerb + "��ǰ�ж���" + verb.stringIndex);

            if (node.additionalVerb != null && node.additionalVerb != verb.stringIndex)
            {
                VerbMono verbMono = recipeModel.verbMonoList.Find(x => x.verb.stringIndex == node.additionalVerb);
                if(verbMono == null)
                {
                    AbstractVerb newVerb = VerbDataBase.TryGetVerb(node.additionalVerb);
                    if(newVerb != null)
                    {
                        GameObject ob = this.GetSystem<UtilSystem>().CreateVerbGameObject(newVerb);
                        VerbMono newVerbMono = ob.GetComponent<VerbMono>();
                        newVerb = newVerbMono.verb;
                        AbstractRecipe newRecipe = RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup).GetNewCopy();
                        newVerb.situation.possibleRecipe = newRecipe;
                        StartVerbSituation(newVerb);
                        gameSystem.MoveCardToClosestNullGrid(newVerbMono, null, 2);
                    }
                }

            }
            else
            {
                if (node != null && node.isAdditional == true)
                {
                    Debug.Log("Ŀ��" + node.targetRecipeGroup + "," + node.targetRecipe);
                    verb.situation.possibleRecipe = RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup);
                    // �л�recipe
                    UnRegisterCurrentRecipeFromVerb(verb);
                    verb.situation.currentRecipe = verb.situation.possibleRecipe.GetNewCopy();
                    SetVerbRecipeStarted(verb, verb.situation.currentRecipe);
                    UtilSystem.PlayAudio("SituationLoop");
                }
                else
                {
                    // �����Ƴ���ǰ�¼�������
                    UnRegisterCurrentRecipeFromVerb(verb);

                }
            }
        }


        // �������¼����Դ���������¸�Ϊ���¼�ע�뵽�б� * ��δ����
        #region δ���õ��¹���
        public void CalculateListNextRecipe(AbstractVerb verb)
        {
            AbstractRecipe recipe = verb.situation.currentRecipe;

            // ���Խ���������
            List<RecipeTriggerNode> nodeList = null;
            if (recipe.recipeLinker != null) nodeList = recipe.recipeLinker.ResolveAllTrigger(verb, true);
            //Debug.Log(recipe.label + "ȡ���Ľ��" + (node == null) + "�Ƿ���������" + (recipe.recipeLinker == null)/* + "��������" + recipe.recipeLinker.triggerNodes.Count*/);
            if (nodeList == null) return;
            // �������Ϊ��ʱ֤�������������¼� ֱ���滻possibleRecipe
            //Debug.Log("������" + node.additionalVerb + "��ǰ�ж���" + verb.stringIndex);
            foreach (var node in nodeList)
            {
                // �������¼���������¼�
                if (node.additionalVerb != null && node.additionalVerb != verb.stringIndex)
                {
                    // ����ж��򲻴���ʱ�������ж���ʵ��
                    VerbMono targetVerbMono = recipeModel.verbMonoList.Find(x => x.verb.stringIndex == node.additionalVerb);
                    if (targetVerbMono == null)
                    {
                        AbstractVerb newVerb = VerbDataBase.TryGetVerb(node.additionalVerb);
                        if (newVerb != null)
                        {
                            // �����ж�������Ϊ���¼������俪ʼ ������汾��ֻ����ʵ�� ���������ں���ͳһ����
                            GameObject ob = this.GetSystem<UtilSystem>().CreateVerbGameObject(newVerb);
                            targetVerbMono = ob.GetComponent<VerbMono>();
                            newVerb = targetVerbMono.verb;
                            //AbstractRecipe newRecipe = RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup).GetNewCopy();
                            //newVerb.situation.possibleRecipe = newRecipe;
                            //StartVerbSituation(newVerb);
                            gameSystem.MoveCardToClosestNullGrid(targetVerbMono, null, 2);
                        }
                    }
                    AddLinkRecipeToVerb(RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup), targetVerbMono.verb);
                    //else
                    //{
                    //    // �������¼���ӵ����ж���ĵȴ��б�
                    //    verbMono.verb.situation.linkRecipeList.Add(RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup));
                    //}
                }
                else
                {
                    // û�б�עĿ���ж�������ǵ�ǰ�ж���ʱ
                    if (node != null && node.isAdditional == true)
                    {
                        // �滻����ʼ�µ��¼�
                        Debug.Log("Ŀ��" + node.targetRecipeGroup + "," + node.targetRecipe);
                        //verb.situation.possibleRecipe = RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup);
                        //// �л�recipe
                        //UnRegisterCurrentRecipeFromVerb(verb);
                        //verb.situation.currentRecipe = verb.situation.possibleRecipe.GetNewCopy();
                        //SetVerbRecipeStarted(verb, verb.situation.currentRecipe);
                        AddLinkRecipeToVerb(RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup), verb);
                        //UtilSystem.PlayAudio("SituationLoop");
                    }
                    else
                    {
                        // ����Ĭ����������������һ���¼�
                        // �����Ƴ���ǰ�¼�������
                        UnRegisterCurrentRecipeFromVerb(verb);

                    }
                }
            }
        }
        // Ϊ�ض����ж������ָ���¼����ȴ��б� * δ����
        public void AddLinkRecipeToVerb(AbstractRecipe recipe,AbstractVerb verb)
        {
            // ��������״̬����ȡ���п��Ʋ�ִ�п�ʼ�¼�
            if(verb.situation.situationState == AbstractSituation.SituationState.WaitingForCollect)
            {
                CollectVerbRecipe(verb);
            }
            // �����׼��״̬���Ƴ��������п��Ʋ���ʼ�¼�
            if (verb.situation.situationState == AbstractSituation.SituationState.Prepare)
            {
                //verb.situation.possibleRecipe = recipe;
                //// �л�recipe
                //UnRegisterCurrentRecipeFromVerb(verb);
                //RegisterCurrentRecipeToVerb(verb.situation.possibleRecipe.GetNewCopy(), verb);
                LoadPossibleRecipeAspect(recipe, verb);

                SetVerbRecipeStarted(verb, verb.situation.currentRecipe);
            }
            else if(verb.situation.situationState == AbstractSituation.SituationState.Excuting)
            {
                verb.situation.linkRecipeList.Add(recipe);
            }
            // �����ִ��״̬������ȴ��б�
        }
        public void LoadPossibleRecipeAspect(AbstractRecipe recipe,AbstractVerb verb)
        {
            verb.situation.possibleRecipe = recipe;
            UnRegisterCurrentRecipeFromVerb(verb);
            RegisterCurrentRecipeToVerb(verb.situation.possibleRecipe.GetNewCopy(), verb);
        }
        // �����ж�����¸��¼�
        public void LoadVerbSituationNextLinkRecipe(AbstractVerb verb)
        {
            // �л�recipe
            if (verb.situation.linkRecipeList.Count > 0)
            {
                AbstractRecipe nextRecipe = verb.situation.linkRecipeList[0].GetNewCopy();
                LoadPossibleRecipeAspect(nextRecipe, verb);
                SetVerbRecipeStarted(verb, verb.situation.currentRecipe);
                UtilSystem.PlayAudio("SituationLoop");

            }

        }
        #endregion
        // �����ж���ʱ����ɺ��Monoʵ����޸�
        public void CalculateVerbRecipeMonoReward(AbstractVerb verb)
        {
            SituationWindowMono situationWindow = verb.verbMono.situationWindowMono;
            VerbMono verbMono = verb.verbMono;
            // �ȼ����뿪ʱ��CardXtrigger
            for (int i = verb.verbCardList.Count - 1; i >= 0; i--)
            {
                AbstractCard targetCard = verb.verbCardList[i];
                if (targetCard == null)
                    continue;
                if (targetCard.cardXtriggersList != null && targetCard.cardXtriggersList.Count > 0)
                {
                    foreach (var trigger in targetCard.cardXtriggersList)
                    {
                        if (IsVerbHaveEnoughElement(verb, trigger.requireAspect, trigger.requireCount))
                        {
                            // �����仯 �Ƴ�ԭ�еĿ���
                            CardMono cardMono = targetCard.cardMono;
                            if (cardMono != null)
                            {
                                gameSystem.UnRegisterStackElementFromSlot(cardMono, cardMono.BelongtoSlotMono, false);
                            }
                            gameSystem.RemoveCardElementFromVerb(targetCard, verb);
                            verb.verbCardList.Remove(targetCard);
                            // ���һ���¿����뿪
                            verb.verbCardList.Add(CardDataBase.TryGetCard(trigger.triggerToCardStringid));
                            break;

                        }

                    }
                }
                foreach(var pair in targetCard.aspectDictionary)
                {

                    AbstractAspect aspect = AspectDataBase.TryGetAspect(pair.Key);
                    //Debug.Log("���Ի�ȡ����" + pair.Key + "�Ƿ����" + (aspect == null ) + "����������" + aspect.cardXtriggersList.Count);

                    if (aspect != null)
                    {
                        foreach(var trigger in aspect.cardXtriggersList)
                        {
                            //Debug.Log(">>>>>>>>>>>���Ҫ��" + trigger.requireAspect + "����" + trigger.requireCount + "�Ƿ�ɹ�" + IsVerbHaveEnoughElement(verb, trigger.requireAspect, trigger.requireCount));
                            if (IsVerbHaveEnoughElement(verb, trigger.requireAspect, trigger.requireCount))
                            {
                                // �����仯 �Ƴ�ԭ�еĿ���
                                CardMono cardMono = targetCard.cardMono;
                                if (cardMono != null)
                                {
                                    gameSystem.UnRegisterStackElementFromSlot(cardMono, cardMono.BelongtoSlotMono, false);
                                }
                                gameSystem.RemoveCardElementFromVerb(targetCard, verb);
                                verb.verbCardList.Remove(targetCard);
                                // ���һ���¿����뿪
                                verb.verbCardList.Add(CardDataBase.TryGetCard(trigger.triggerToCardStringid));
                                goto nextCard;

                            }
                        }
                    }
                }
                nextCard:;

            }
            //Debug.Log("�����б�����>>>>>>>>>" + verb.verbCardList.Count);
            // �����п��ƴӿ�����ȡ����
            foreach (AbstractCard card in verb.verbCardList)
            {
                if (card == null)
                    continue;
                //Debug.Log("������>>>>>>>" + card.label + card.createIndex + "�Ƿ�ݻ�" + card.isDisposed);
                CardMono cardMono = card.cardMono;
                if (card.cardMono == null)
                {
                   // Debug.Log("����û��ʵ��");
                    cardMono = utilSystem.CreateCardGameObject(card).GetComponent<CardMono>();
                    card.cardMono = cardMono;
                }
                else
                {
                    //Debug.Log("���ƴ���ʵ��");

                    gameSystem.UnRegisterStackElementFromSlot(card.cardMono, card.cardMono.BelongtoSlotMono,false);
                }
                gameSystem.RemoveCardElementFromVerb(card, verb);

                // �ƶ��������������
                verb.verbMono.situationWindowMono.AddCardToRewardCollecter(card);
                // �ÿ������»ص��ؿ�����Ҽ��뵽����ĸ����б���
                gameModel.AddCardMonoToTableList(cardMono);
                // ��������Ϊ�����٣�����ݻ� ��������Ƴ����ۺ����ֹע�����
                if (card.isDisposed)
                {
                    //Debug.Log("Ҫ�ݻٵ�Ŀ�꿨������" + card.cardMono.name);
                    GameObject.Destroy(card.cardMono.gameObject);
                }
            }

        }
        // ��ȡ�¼����� �����Ʒ��͵��洢����
        public void CollectVerbRecipe(AbstractVerb verb)
        {
            GameObject root = verb.verbMono.situationWindowMono.rewardStorageDominionManager.gameObject;
            Transform parent = root.transform;
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                GameObject child = parent.GetChild(i).gameObject;
                CardMono cardMono = child.GetComponent<CardMono>();
                if (cardMono == null) continue;
                gameSystem.OutputCardToTable(cardMono, cardMono.LastGridMono);
                cardMono.RestoreOriginalRectTransform();
            }

            //foreach(AbstractCard card in verb.verbCardList)
            //{
            //    if (card.isDisposed)
            //        continue;
            //    CardMono cardMono = card.cardMono;
            //    if(card.cardMono == null)
            //    {
            //        cardMono =  UtilSystem.StaticCreateCardGameObject(card).GetComponent<CardMono>();
            //        card.cardMono = cardMono;
            //    }
            //    //gameSystem.StackCardToASlot(card.cardMono,card.cardMono.LastGridMono);
            //    // ���������������
            //    gameSystem.OutputCardToTable(cardMono, cardMono.LastGridMono);
            //    cardMono.RestoreOriginalRectTransform();
            //}
            verb.verbCardList.Clear();
            ReloadVerbDefaultSituation(verb);
        }
        // Ϊverb��������Ĭ���¼�����
        public void ReloadVerbDefaultSituation(AbstractVerb verb)
        {
            verb.situation = SituationDataBase.TryGetSituation(verb.defaultSituationKey).GetNewCopy();
        }

        // ��ʼ����ֵ
        public void LateInit()
        {
            gameModel = this.GetModel<GameModel>();
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
                    CheckSlotGreedyEvent(mono.verb);
                }
                // ��ʼĳЩ��ʼ״̬�¾���ִ�е��¼�
                if(mono.verb.situation.currentRecipe.isInitExcuting && mono.verb.situation.situationState == AbstractSituation.SituationState.Prepare)
                {
                    StartVerbSituation(mono.verb);
                }
            }
        }
        // ����ж����е�̰�������Ƿ��п��õĿ���
        public void CheckSlotGreedyEvent(AbstractVerb verb)
        {
            // ׼��״̬ʱ���׼���������Ƿ��д�������
            if(verb.situation.situationState == AbstractSituation.SituationState.Prepare)
            {
                foreach(AbstractSlot slot in verb.slotList)
                {
                    if(slot.isGreedy && slot.card == null)
                    {
                        //Debug.Log("���۳��Ի�ȡ" + slot.slotMono.slotLabel.text);
                        for(int i = gameModel.tableCardMonoList.Count - 1;i >= 0;i--)
                        {
                            CardMono cardMono = gameModel.tableCardMonoList[i];
                            AbstractCard card = cardMono.card;
                            if (IsCardMeetSlotsAspectRequire(card, slot))
                            {
                                //Debug.Log("ץȡ����ЧԪ��" + slot.slotMono.slotLabel.text);
                                //gameSystem.StackCardToASlot(cardMono, slot.slotMono);
                                gameSystem.MoveCardFromSlotToAnotherSlot(cardMono, slot.slotMono);
                                break;
                            }
                        }
                    }
                }
            }
            else if ( verb.situation.situationState == AbstractSituation.SituationState.Excuting)
            {
                foreach (AbstractSlot slot in verb.verbRecipeSlotList)
                {
                    if (slot.isGreedy && slot.card == null)
                    {
                        for (int i = gameModel.tableCardMonoList.Count - 1; i >= 0; i--)
                        {
                            CardMono cardMono = gameModel.tableCardMonoList[i];
                            AbstractCard card = cardMono.card;
                            if (IsCardMeetSlotsAspectRequire(card, slot))
                            {
                                gameSystem.StackCardToASlot(cardMono, slot.slotMono);
                                break;
                            }
                        }
                    }
                }
            }
        }
        protected override void OnInit()
        {
            recipeModel = this.GetModel<RecipeModel>();
            gameSystem = this.GetSystem<GameSystem>();
            utilSystem = this.GetSystem<UtilSystem>();
        }
        // ��ӡ�ж����еĿ����б�
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

        public static bool IsCardMeetSlotsAspectRequire(AbstractCard card , AbstractSlot slot)
        {
            Dictionary<string, int> cardAspects = card.aspectDictionary;
            // 1. ����������ࣨforbidden - AND��
            foreach (var kvp in slot.forbiddenAspectsDictionary)
            {
                string aspect = kvp.Key;
                int maxAllowed = kvp.Value;

                if (cardAspects.TryGetValue(aspect, out int actualAmount) && actualAmount >= maxAllowed)
                {
                    return false; // ���ڳ������Ƶ�����
                }
            }

            // 2. ���������һ��requipred - OR��
            if (slot.requipredAspectsDictionary.Count > 0)
            {
                bool hasAtLeastOne = false;
                foreach (var kvp in slot.requipredAspectsDictionary)
                {
                    string aspect = kvp.Key;
                    int minAmount = kvp.Value;

                    if (cardAspects.TryGetValue(aspect, out int actualAmount) && actualAmount >= minAmount)
                    {
                        hasAtLeastOne = true;
                        break;
                    }
                }

                if (!hasAtLeastOne)
                {
                    return false; // û����������һ��������
                }
            }

            // 3. ���������ࣨessential - AND��
            foreach (var kvp in slot.essentialAspectsDictionary)
            {
                string aspect = kvp.Key;
                int requiredAmount = kvp.Value;

                if (!cardAspects.TryGetValue(aspect, out int actualAmount) || actualAmount < requiredAmount)
                {
                    return false; // ȱ�ٱ����������������
                }
            }

            // ͨ������Ҫ��
            return true;
        }
        // ��鿨���б��б����Ϊȼ���Ŀ��Ʋ�ת��
        public void TranslateBurrnedCardToNewCard(List<AbstractCard> cardList,AbstractVerb verb)
        {
            for (int i = cardList.Count - 1; i >= 0; i--)
            {
                AbstractCard oldCard = cardList[i];
                // ���Ǵ�ȼ�տ����н���ģ���ֱ����������
                if (oldCard.isInVerbsByConsumeSlot == false) continue;
                // ���Ϊ�������ȴ�����
                oldCard.isDisposed = true;
                string newCardIndex = oldCard.burnToCardStringIndex;
                if (newCardIndex == null)
                    newCardIndex = oldCard.decayToCardStringIndex;
                // ��ԭ�п��Ƶ�Ԫ���Ƴ��ж���
                gameSystem.RemoveCardElementFromVerb(oldCard, verb);
                // Ȼ���Ƴ������б�
                // cardList.Remove(oldCard);
                // ������ ����һ���¶���
                // �����Ҫת�䵽�Ŀ��ƣ��򴴽�����ʵ�����滻��ֵ
                if (newCardIndex != null)
                {
                    AbstractCard newCard = CardDataBase.TryGetCard(newCardIndex).GetNewCopy();
                    gameSystem.AddCardElementToVerb(newCard, verb);
                    verb.verbCardList.Add(newCard);
                    //if(oldCard.cardMono != null)
                    //oldCard.cardMono.LoadCardData(newCard);
                    //GameObject ob = utilSystem.CreateCardGameObject(CardDataBase.TryGetCard(newCardIndex).GetNewCopy());
                    //MoveCardToClosestNullGrid(ob.GetComponent<ITableElement>(), oldCardSlot);
                }
                else
                {
                    // �����������ſ��Ƶ�ʵ��
                }

            }

        }

    }
}