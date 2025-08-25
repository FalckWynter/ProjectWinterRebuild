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
        // 事件系统
        public RecipeModel recipeModel;
        public GameSystem gameSystem;
        public GameModel gameModel;
        public UtilSystem utilSystem;


        // 启动行动框的事件执行模式，只开始一次
        public void StartVerbSituation(AbstractVerb verb)
        {
            // 将当前事件设置为实际副本 并修改事件容器状态为执行中
            AbstractRecipe currentRecipe = verb.situation.possibleRecipe.GetNewCopy();
            //Debug.Log("目标事件 " + currentRecipe.createIndex + "是否为空" + (currentRecipe == null) + "事件执行状态" + currentRecipe.isExcuting + "完成状态" + currentRecipe.isFinished);
            // 意义不明的排空
            if (currentRecipe == null) return;
            verb.situation.situationState = AbstractSituation.SituationState.Excuting;
            // >>载入准备状态下的信息
            // 记录事件准备卡槽中的所有卡牌
            verb.verbCardList = new List<AbstractCard>();
            //foreach(AbstractSlot slot in verb.slotList)
            // 收缴行动框准备卡槽中的所有卡牌
            for(int i = verb.slotList.Count - 1; i >= 0;i --)
            {
                AbstractSlot slot = verb.slotList[i];
                //Debug.Log(slot.label + "准备剔除元素" + (slot.card == null ? "没有卡牌" : slot.card.label));
                GetCardToVerbCardList(verb, slot);
                if (slot.card != null)
                {
                    
                    gameSystem.UnRegisterStackElementFromSlot(slot.card.cardMono, slot.card.cardMono.BelongtoSlotMono, false);
                }

            }
            // 启动当前事件，以设置和订阅Recipe的相关参数
            SetVerbRecipeStarted(verb, currentRecipe);
            // >>
            //Debug.Log(verb.stringIndex + "行动框" + verb.situation.currentRecipe.label + "事件" + "卡槽数量" + verb.situation.currentRecipe.recipeSlots.Count);
            // 修改完成，发起响应修改事件
            verb.OnVerbDataChanged.Invoke(verb,AbstractVerb.VerbExchangeReason.RecipeStarted);
        }
        // 将事件切换为执行状态
        public void SetVerbRecipeStarted(AbstractVerb verb, AbstractRecipe currentRecipe)
        {
            // 修改事件为当前事件
            verb.situation.currentRecipe = currentRecipe;
            // 设置当前事件的状态和重置计时器
            currentRecipe.isExcuting = true;
            currentRecipe.warpup = currentRecipe.maxWarpup;
            // 将当前事件的性相注入其中
            foreach (var pair in currentRecipe.recipeAspectDictionary)
            {
                //Debug.Log("添加性相" + pair);
                verb.AddAspect(pair.Key, pair.Value);
            }
            // 订阅当前事件的卡槽
            foreach (AbstractSlot slot in verb.situation.currentRecipe.recipeSlots)
            {
                gameSystem.AddRecipeSlotToVerb(slot, verb);
            }
            // 订阅事件信息
            verb.situation.recipeTextList.Add(new AbstractSituation.RecipeTextElement(currentRecipe.excutingLabel, currentRecipe.excutingDescription));
            verb.OnVerbDataChanged.Invoke(verb, AbstractVerb.VerbExchangeReason.RecipeStarted);

            // 将事件信息绑定到事件容器中

        }
        // 将卡牌从卡槽中转移到行动框里
        public void GetCardToVerbCardList(AbstractVerb verb,AbstractSlot slot)
        {
            if (slot.card != null)
            {
                verb.verbCardList.Add(slot.card);
                // 3.2.1.2 检查桌面内容添加 在此处将卡牌从关卡中移除，因为它不再有游戏中的实体了
                gameModel.RemoveCardMonoFromLevelList(slot.card.cardMono);
                if (slot.isConsumes)
                {
                    //slot.card.isDisposed = true;
                    slot.card.isInVerbsByConsumeSlot = true;
                }
            }
        }
        #region 行动框Recipe选择部分
        // 重新计算行动框事件的可能事件
        public void ReCalculateVerbRecipeState(AbstractVerb verb)
        {
            // 如果有结果 则设置为对应事件
            AbstractRecipe possibleRecipe = CalculateVerbRecipePossibleRecipe(verb);
            if (possibleRecipe != null)
            {
                verb.situation.possibleRecipe = possibleRecipe;
                verb.OnVerbDataChanged.Invoke(verb, AbstractVerb.VerbExchangeReason.PossibleRecipeExchange);
            }
            Debug.Log("尝试更新verb的事件" + (possibleRecipe == null ? "没有结果" : possibleRecipe.label));

        }
        // 获取行动框当前性相状态最适合的事件
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
        // 是否行动框有满足事件要求的所有性相
        public bool IsVerbHaveEnoughRecipeElement(AbstractVerb verb, AbstractRecipe recipe)
        {
            foreach(var elementPair in recipe.requireElementDictionary)
            {
                if (!IsVerbHaveEnoughElement(verb, elementPair.Key, elementPair.Value))
                    return false;
            }
            return true;
        }
        // 行动框是否具有特定数量的某种性相
        public bool IsVerbHaveEnoughElement(AbstractVerb verb,string elementKey,int count)
        {
            // 正数表示至少拥有，需要存在该形象并比较数量
            if(count > 0)
            {
                if (verb.aspectDictionary.ContainsKey(elementKey) && verb.aspectDictionary[elementKey] >= count)
                    return true;
            }
            // 负数表示不能多于某个数
            else if(count <= 0)
            {
                int maxCount = -count;
                // 不存在则显然小于
                if (!verb.aspectDictionary.ContainsKey(elementKey))
                    return true;
                // 存在但数量不足绝对值则通过
                if (verb.aspectDictionary.ContainsKey(elementKey) && verb.aspectDictionary[elementKey] < maxCount) 
                    return true;
            }
            // 否则判定为不通过
            return false;
        }
        #endregion
        // 推进行动框的执行时间
        public void AddVerbWarpUp(AbstractVerb verb,float time)
        {
            AbstractRecipe recipe = verb.situation.currentRecipe;
            //Debug.Log("检测目标" + verb.situation.createIndex + "是否在执行" + (recipe.isExcuting) +"是否已经完成" + (recipe.isFinished ));
            if (recipe.isExcuting == false || recipe.isFinished == true)
                return;
                recipe.warpup -= time;
            if (recipe.warpup <= 0 && recipe.isFinished == false)
            {
                recipe.warpup = 0;
                DoVerbRecipeFinishCalculate(verb);

            }
        }
        // 2.2节增加 2.3节修改
        // 计算行动框事件完成的收益
        public void DoVerbRecipeFinishCalculate(AbstractVerb verb)
        {
            // 收取卡牌数据
            CashVerbRecipeSlots(verb);
            // 计算收益上移到这里
            // 计算卡牌事件收益主函数
            CalculateVerbRecipeReward(verb);
            // 计算状态切换 决定行动框要不要结束计算
            CalculateVerbRecipeState(verb);

        }
        // 2.3 事件状态与连锁
        // 对行动框中的事件卡槽进行收取
        public void CashVerbRecipeSlots(AbstractVerb verb)
        {
            //计算当前事件的修改
            //先收缴所有当前事件的元素
            //Debug.Log("可以查找的卡槽数量" + verb.verbRecipeSlotList);
            foreach (AbstractSlot slot in verb.verbRecipeSlotList)
            {
                AbstractCard card = slot.card;
                if (card != null)
                {
                    //Debug.Log("收缴卡牌" + card.label + card.createIndex);
                    // 订阅这些卡牌，因为不再会被修改 也便于销毁卡槽
                    //verb.verbCardList.Add(card);
                    GetCardToVerbCardList(verb, slot);
                    CardMono cardMono = card.cardMono;
                    // 意义不明的创建实体函数 理论上这些卡牌都是有实体的
                    if (card.cardMono == null)
                    {
                        cardMono = utilSystem.CreateCardGameObject(card).GetComponent<CardMono>();
                        card.cardMono = cardMono;
                    }
                    // 移除对卡槽的绑定关系
                    gameSystem.UnRegisterStackElementFromSlot(card.cardMono, card.cardMono.BelongtoSlotMono, false);
                    // 移动到结果管理者中
                    verb.verbMono.situationWindowMono.AddCardToRewardCollecter(card);
                    // 如果被标记为已销毁，则将其摧毁 必须放在移除卡槽后面防止注册出错 * 没有实际用处，但是保留
                    if (card.isDisposed)
                    {
                        //Debug.Log("要摧毁的目标卡牌名字" + card.cardMono.name);
                        GameObject.Destroy(card.cardMono.gameObject);
                    }
                }
                // 销毁计算完毕的卡槽物体
                verb.verbMono.situationWindowMono.RemoveSlotObjectFromRecipeExcutingSlotDominion(slot);
            }
            // 移除所有事件带有的卡槽数据
            verb.verbRecipeSlotList.Clear();
        }
        // 计算事件收益 这里只计算收益，对行动框状态的结算放到后面去
        // 2.3 事件状态与连锁重构，拆分状态切换和数据计算功能
        public void CalculateVerbRecipeReward(AbstractVerb verb)
        {
            // 获取变量
            AbstractRecipe recipe = verb.situation.currentRecipe;


            // 计算连锁事件 这里的作用是决定要作为结算目标的事件
            CalculateVerbRecipeLinkEvent(verb);
           // 7.25做到这里，明天做连锁型Recipe的内容


            recipe = verb.situation.currentRecipe;
            // 修改变量
            recipe.isFinished = true;
            recipe.isExcuting = true;
            // 绑定变量
            verb.situation.recipeTextList.Add(new AbstractSituation.RecipeTextElement(recipe.finishedLabel, recipe.finishedDescription));
            // 同步修改
            verb.OnVerbDataChanged.Invoke(verb, AbstractVerb.VerbExchangeReason.RecipeFinished);
            // 计算事件对卡牌的修改
            CalculateVerbRecipeEffectReward(verb);

            // 计算结局
            if(recipe.ending != null)
            {
                utilSystem.LoadEnding(EndingDataBase.TryGetEnding(recipe.ending));
            }


        }
        // 2.3 事件状态与连锁拆分出来
        // 计算事件连锁情况，以避免可能的行动并未结束，相当于进行一次特殊的PossibleRecipeCheck
        public void CalculateVerbRecipeLinkEvent(AbstractVerb verb)
        {
            AbstractRecipe recipe = verb.situation.currentRecipe;

            // 尝试解算连锁器
            RecipeTriggerNode node = null;
            if (recipe.recipeLinker != null) node = recipe.recipeLinker.ResolveFirstTrigger(verb,false);
            // 当结果不为空时证明有能连锁的事件 直接替换possibleRecipe
            if (node != null && node.isAdditional == false)
            {
                //Debug.Log("目标" + node.targetRecipeGroup + "," + node.targetRecipe);
                verb.situation.possibleRecipe = RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup);
            }

            // 如果将要发生的事件不同 则改为发生即将到来的事件 对卡槽的处理已经在之前结束了 这里计算新Recipe的形式
            // 7.26 改为只计算切换及导致的性相结果 相同事件的结算放到后面去
            if (!verb.situation.possibleRecipe.IsEqualTo(verb.situation.currentRecipe))
                ExchangeVerbCurrentRecipe(verb.situation.possibleRecipe.GetNewCopy(), verb);

            //if (verb.situation.possibleRecipe.IsEqualTo(verb.situation.currentRecipe))
            //{
            //    //foreach (var pair in verb.situation.currentRecipe.recipeAspectDictionary)
            //    //{
            //    //    // Debug.Log("减数修改" + pair);
            //    //    // 取负数，因为要取消订阅
            //    //    verb.AddAspect(pair.Key, -pair.Value);
            //    //}
            //}
            //else
            //{
            //    // 这么做是对的，因为此时正在模仿可能事件替代了当前事件作为结算的结果，所以需要修改订阅
            //    ExchangeVerbCurrentRecipe(verb.situation.possibleRecipe.GetNewCopy(), verb);

            //}
        }
        #region 行动框当前事件切换函数
        // 将行动框的当前事件切换为新事件
        public void ExchangeVerbCurrentRecipe(AbstractRecipe newRecipe,AbstractVerb verb)
        {
            UnRegisterCurrentRecipeFromVerb(verb);
            RegisterRecipeToVerb(newRecipe, verb);

        }
        // 一组方法 移除对当前事件的注册
        public void UnRegisterCurrentRecipeFromVerb(AbstractVerb verb)
        {
            UnRegisterRecipeFromVerb(verb.situation.currentRecipe, verb);
        }

        public void UnRegisterRecipeFromVerb(AbstractRecipe recipe,AbstractVerb verb)
        {
            foreach (var pair in recipe.recipeAspectDictionary)
            {
                Debug.Log("减数修改" + pair);
                // 取负数，因为要取消订阅
                verb.AddAspect(pair.Key, -pair.Value);
            }
        }
        //一组方法 创建对新事件的注册
        public void RegisterCurrentRecipeToVerb(AbstractRecipe recipe, AbstractVerb verb)
        {
            RegisterRecipeToVerb(verb.situation.currentRecipe, verb);
        }
        public void RegisterRecipeToVerb(AbstractRecipe recipe, AbstractVerb verb)
        {
            verb.situation.currentRecipe = recipe;
            // 取正常值，此处已经替换为新值了
            foreach (var pair in recipe.recipeAspectDictionary)
            {
                Debug.Log("加数修改" + pair);
                verb.AddAspect(pair.Key, pair.Value);
            }
        }
        #endregion
        // 2.3.2 卡牌总结与收获
        // 计算事件收获的收益影响
        public void CalculateVerbRecipeEffectReward(AbstractVerb verb)
        {
            AbstractRecipe recipe = verb.situation.possibleRecipe;
            foreach(CardEffect effect in recipe.effects)
            {
                effect.Apply(verb.verbCardList);
            }
            //3.2.1.2 计算对卡牌的耗尽
            TranslateBurrnedCardToNewCard(verb.verbCardList, verb);
            // 计算要抽出的卡牌
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

        // 2.3 事件状态与连锁拆分出来
        //在这里确定行动框是否要切换到结束模式
        public void CalculateVerbRecipeState(AbstractVerb verb)
        {
            CalculateNextRecipe(verb);
            // 如果仍然是完成状态，即没有新的正在执行或者等待执行的事件
            if (verb.situation.linkRecipeList.Count <= 0 && verb.situation.currentRecipe.recipeExcutingState == AbstractRecipe.RecipeExcutingState.Finished)
            {
                verb.situation.situationState = AbstractSituation.SituationState.WaitingForCollect;
                // 计算物体修改
                CalculateVerbRecipeMonoReward(verb);
                // 播放音效 * TOSET 后续应该挪到其他地方去
                UtilSystem.PlayAudio("SituationComplete");
            }
            else
            {
                // 3.2.3新加入
                LoadVerbSituationNextLinkRecipe(verb);
            }

        }
        // 计算行动框的下个可能事件并设定完成状态
        public void CalculateNextRecipe(AbstractVerb verb)
        {
            AbstractRecipe recipe = verb.situation.currentRecipe;

            // 尝试解算连锁器
            RecipeTriggerNode node = null;
            if (recipe.recipeLinker != null) node = recipe.recipeLinker.ResolveFirstTrigger   (verb,true);
            //Debug.Log(recipe.label + "取到的结果" + (node == null) + "是否有链接器" + (recipe.recipeLinker == null)/* + "连接数量" + recipe.recipeLinker.triggerNodes.Count*/);
            if (node == null) return;
            // 当结果不为空时证明有能连锁的事件 直接替换possibleRecipe
            //Debug.Log("计算结果" + node.additionalVerb + "当前行动框" + verb.stringIndex);

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
                    Debug.Log("目标" + node.targetRecipeGroup + "," + node.targetRecipe);
                    verb.situation.possibleRecipe = RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup);
                    // 切换recipe
                    UnRegisterCurrentRecipeFromVerb(verb);
                    verb.situation.currentRecipe = verb.situation.possibleRecipe.GetNewCopy();
                    SetVerbRecipeStarted(verb, verb.situation.currentRecipe);
                    UtilSystem.PlayAudio("SituationLoop");
                }
                else
                {
                    // 否则移除当前事件的性相
                    UnRegisterCurrentRecipeFromVerb(verb);

                }
            }
        }


        // 在启用事件可以待机的情况下改为将事件注入到列表 * 暂未启用
        #region 未启用的新功能
        public void CalculateListNextRecipe(AbstractVerb verb)
        {
            AbstractRecipe recipe = verb.situation.currentRecipe;

            // 尝试解算连锁器
            List<RecipeTriggerNode> nodeList = null;
            if (recipe.recipeLinker != null) nodeList = recipe.recipeLinker.ResolveAllTrigger(verb, true);
            //Debug.Log(recipe.label + "取到的结果" + (node == null) + "是否有链接器" + (recipe.recipeLinker == null)/* + "连接数量" + recipe.recipeLinker.triggerNodes.Count*/);
            if (nodeList == null) return;
            // 当结果不为空时证明有能连锁的事件 直接替换possibleRecipe
            //Debug.Log("计算结果" + node.additionalVerb + "当前行动框" + verb.stringIndex);
            foreach (var node in nodeList)
            {
                // 在其他事件框中添加事件
                if (node.additionalVerb != null && node.additionalVerb != verb.stringIndex)
                {
                    // 这个行动框不存在时创建新行动框实体
                    VerbMono targetVerbMono = recipeModel.verbMonoList.Find(x => x.verb.stringIndex == node.additionalVerb);
                    if (targetVerbMono == null)
                    {
                        AbstractVerb newVerb = VerbDataBase.TryGetVerb(node.additionalVerb);
                        if (newVerb != null)
                        {
                            // 设置行动框内容为新事件并将其开始 在这个版本下只生成实体 设置内容在后续统一进行
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
                    //    // 否则将新事件添加到该行动框的等待列表
                    //    verbMono.verb.situation.linkRecipeList.Add(RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup));
                    //}
                }
                else
                {
                    // 没有标注目标行动框或者是当前行动框时
                    if (node != null && node.isAdditional == true)
                    {
                        // 替换并开始新的事件
                        Debug.Log("目标" + node.targetRecipeGroup + "," + node.targetRecipe);
                        //verb.situation.possibleRecipe = RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup);
                        //// 切换recipe
                        //UnRegisterCurrentRecipeFromVerb(verb);
                        //verb.situation.currentRecipe = verb.situation.possibleRecipe.GetNewCopy();
                        //SetVerbRecipeStarted(verb, verb.situation.currentRecipe);
                        AddLinkRecipeToVerb(RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup), verb);
                        //UtilSystem.PlayAudio("SituationLoop");
                    }
                    else
                    {
                        // 最终默认情况，即结束最后一个事件
                        // 否则移除当前事件的性相
                        UnRegisterCurrentRecipeFromVerb(verb);

                    }
                }
            }
        }
        // 为特定的行动框添加指定事件到等待列表 * 未启用
        public void AddLinkRecipeToVerb(AbstractRecipe recipe,AbstractVerb verb)
        {
            // 如果是完成状态，收取所有卡牌并执行开始事件
            if(verb.situation.situationState == AbstractSituation.SituationState.WaitingForCollect)
            {
                CollectVerbRecipe(verb);
            }
            // 如果是准备状态，移除所有已有卡牌并开始事件
            if (verb.situation.situationState == AbstractSituation.SituationState.Prepare)
            {
                //verb.situation.possibleRecipe = recipe;
                //// 切换recipe
                //UnRegisterCurrentRecipeFromVerb(verb);
                //RegisterCurrentRecipeToVerb(verb.situation.possibleRecipe.GetNewCopy(), verb);
                LoadPossibleRecipeAspect(recipe, verb);

                SetVerbRecipeStarted(verb, verb.situation.currentRecipe);
            }
            else if(verb.situation.situationState == AbstractSituation.SituationState.Excuting)
            {
                verb.situation.linkRecipeList.Add(recipe);
            }
            // 如果是执行状态，加入等待列表
        }
        public void LoadPossibleRecipeAspect(AbstractRecipe recipe,AbstractVerb verb)
        {
            verb.situation.possibleRecipe = recipe;
            UnRegisterCurrentRecipeFromVerb(verb);
            RegisterCurrentRecipeToVerb(verb.situation.possibleRecipe.GetNewCopy(), verb);
        }
        // 载入行动框的下个事件
        public void LoadVerbSituationNextLinkRecipe(AbstractVerb verb)
        {
            // 切换recipe
            if (verb.situation.linkRecipeList.Count > 0)
            {
                AbstractRecipe nextRecipe = verb.situation.linkRecipeList[0].GetNewCopy();
                LoadPossibleRecipeAspect(nextRecipe, verb);
                SetVerbRecipeStarted(verb, verb.situation.currentRecipe);
                UtilSystem.PlayAudio("SituationLoop");

            }

        }
        #endregion
        // 计算行动框时间完成后对Mono实体的修改
        public void CalculateVerbRecipeMonoReward(AbstractVerb verb)
        {
            SituationWindowMono situationWindow = verb.verbMono.situationWindowMono;
            VerbMono verbMono = verb.verbMono;
            // 先计算离开时的CardXtrigger
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
                            // 产生变化 移除原有的卡牌
                            CardMono cardMono = targetCard.cardMono;
                            if (cardMono != null)
                            {
                                gameSystem.UnRegisterStackElementFromSlot(cardMono, cardMono.BelongtoSlotMono, false);
                            }
                            gameSystem.RemoveCardElementFromVerb(targetCard, verb);
                            verb.verbCardList.Remove(targetCard);
                            // 添加一张新卡并离开
                            verb.verbCardList.Add(CardDataBase.TryGetCard(trigger.triggerToCardStringid));
                            break;

                        }

                    }
                }
                foreach(var pair in targetCard.aspectDictionary)
                {

                    AbstractAspect aspect = AspectDataBase.TryGetAspect(pair.Key);
                    //Debug.Log("尝试获取性相" + pair.Key + "是否存在" + (aspect == null ) + "触发器数量" + aspect.cardXtriggersList.Count);

                    if (aspect != null)
                    {
                        foreach(var trigger in aspect.cardXtriggersList)
                        {
                            //Debug.Log(">>>>>>>>>>>检查要求" + trigger.requireAspect + "数量" + trigger.requireCount + "是否成功" + IsVerbHaveEnoughElement(verb, trigger.requireAspect, trigger.requireCount));
                            if (IsVerbHaveEnoughElement(verb, trigger.requireAspect, trigger.requireCount))
                            {
                                // 产生变化 移除原有的卡牌
                                CardMono cardMono = targetCard.cardMono;
                                if (cardMono != null)
                                {
                                    gameSystem.UnRegisterStackElementFromSlot(cardMono, cardMono.BelongtoSlotMono, false);
                                }
                                gameSystem.RemoveCardElementFromVerb(targetCard, verb);
                                verb.verbCardList.Remove(targetCard);
                                // 添加一张新卡并离开
                                verb.verbCardList.Add(CardDataBase.TryGetCard(trigger.triggerToCardStringid));
                                goto nextCard;

                            }
                        }
                    }
                }
                nextCard:;

            }
            //Debug.Log("卡牌列表数量>>>>>>>>>" + verb.verbCardList.Count);
            // 将所有卡牌从卡槽中取出来
            foreach (AbstractCard card in verb.verbCardList)
            {
                if (card == null)
                    continue;
                //Debug.Log("处理卡牌>>>>>>>" + card.label + card.createIndex + "是否摧毁" + card.isDisposed);
                CardMono cardMono = card.cardMono;
                if (card.cardMono == null)
                {
                   // Debug.Log("卡牌没有实体");
                    cardMono = utilSystem.CreateCardGameObject(card).GetComponent<CardMono>();
                    card.cardMono = cardMono;
                }
                else
                {
                    //Debug.Log("卡牌存在实体");

                    gameSystem.UnRegisterStackElementFromSlot(card.cardMono, card.cardMono.BelongtoSlotMono,false);
                }
                gameSystem.RemoveCardElementFromVerb(card, verb);

                // 移动到结果管理者中
                verb.verbMono.situationWindowMono.AddCardToRewardCollecter(card);
                // 让卡牌重新回到关卡里，并且加入到桌面的更新列表中
                gameModel.AddCardMonoToTableList(cardMono);
                // 如果被标记为已销毁，则将其摧毁 必须放在移除卡槽后面防止注册出错
                if (card.isDisposed)
                {
                    //Debug.Log("要摧毁的目标卡牌名字" + card.cardMono.name);
                    GameObject.Destroy(card.cardMono.gameObject);
                }
            }

        }
        // 获取事件收益 将卡牌发送到存储器中
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
            //    // 将卡牌输出到桌面
            //    gameSystem.OutputCardToTable(cardMono, cardMono.LastGridMono);
            //    cardMono.RestoreOriginalRectTransform();
            //}
            verb.verbCardList.Clear();
            ReloadVerbDefaultSituation(verb);
        }
        // 为verb重载它的默认事件容器
        public void ReloadVerbDefaultSituation(AbstractVerb verb)
        {
            verb.situation = SituationDataBase.TryGetSituation(verb.defaultSituationKey).GetNewCopy();
        }

        // 初始化赋值
        public void LateInit()
        {
            gameModel = this.GetModel<GameModel>();
        }
        // 提前帧更新
        public void PreUpdate()
        {
            UpdateVerb();

        }

        // 更新行动框
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
                // 开始某些初始状态下就在执行的事件
                if(mono.verb.situation.currentRecipe.isInitExcuting && mono.verb.situation.situationState == AbstractSituation.SituationState.Prepare)
                {
                    StartVerbSituation(mono.verb);
                }
            }
        }
        // 检查行动框中的贪婪卡槽是否有可用的卡牌
        public void CheckSlotGreedyEvent(AbstractVerb verb)
        {
            // 准备状态时检查准备卡槽里是否有磁力卡槽
            if(verb.situation.situationState == AbstractSituation.SituationState.Prepare)
            {
                foreach(AbstractSlot slot in verb.slotList)
                {
                    if(slot.isGreedy && slot.card == null)
                    {
                        //Debug.Log("卡槽尝试获取" + slot.slotMono.slotLabel.text);
                        for(int i = gameModel.tableCardMonoList.Count - 1;i >= 0;i--)
                        {
                            CardMono cardMono = gameModel.tableCardMonoList[i];
                            AbstractCard card = cardMono.card;
                            if (IsCardMeetSlotsAspectRequire(card, slot))
                            {
                                //Debug.Log("抓取到有效元素" + slot.slotMono.slotLabel.text);
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
        // 打印行动框中的卡牌列表
        public void PrintVerbCardList(AbstractVerb verb)
        {
            if (verb == null || verb.verbCardList == null)
            {
                Debug.Log("verb 或 verbCardList 是空的！");
                return;
            }

            Debug.Log($"verbCardList 共包含 {verb.verbCardList.Count} 张卡牌:");

            foreach (var card in verb.verbCardList)
            {
                if (card == null)
                {
                    Debug.Log("遇到空卡牌对象！");
                    continue;
                }

                // 构造性相字符串
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
                    aspects = "无性相";
                }

                Debug.Log($"卡牌: stringIndex={card.stringIndex}, label={card.label}, aspects=[{aspects}], isDisposed = {card.isDisposed}");
            }
        }

        public static bool IsCardMeetSlotsAspectRequire(AbstractCard card , AbstractSlot slot)
        {
            Dictionary<string, int> cardAspects = card.aspectDictionary;
            // 1. 检查限制性相（forbidden - AND）
            foreach (var kvp in slot.forbiddenAspectsDictionary)
            {
                string aspect = kvp.Key;
                int maxAllowed = kvp.Value;

                if (cardAspects.TryGetValue(aspect, out int actualAmount) && actualAmount >= maxAllowed)
                {
                    return false; // 存在超出限制的性相
                }
            }

            // 2. 检查至少其一（requipred - OR）
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
                    return false; // 没有满足至少一个的性相
                }
            }

            // 3. 检查必需性相（essential - AND）
            foreach (var kvp in slot.essentialAspectsDictionary)
            {
                string aspect = kvp.Key;
                int requiredAmount = kvp.Value;

                if (!cardAspects.TryGetValue(aspect, out int actualAmount) || actualAmount < requiredAmount)
                {
                    return false; // 缺少必需性相或数量不足
                }
            }

            // 通过所有要求
            return true;
        }
        // 检查卡牌列表中被标记为燃尽的卡牌并转化
        public void TranslateBurrnedCardToNewCard(List<AbstractCard> cardList,AbstractVerb verb)
        {
            for (int i = cardList.Count - 1; i >= 0; i--)
            {
                AbstractCard oldCard = cardList[i];
                // 不是从燃烧卡槽中进入的，则直接跳过结算
                if (oldCard.isInVerbsByConsumeSlot == false) continue;
                // 标记为废弃并等待销毁
                oldCard.isDisposed = true;
                string newCardIndex = oldCard.burnToCardStringIndex;
                if (newCardIndex == null)
                    newCardIndex = oldCard.decayToCardStringIndex;
                // 将原有卡牌的元素移除行动框
                gameSystem.RemoveCardElementFromVerb(oldCard, verb);
                // 然后移出卡牌列表
                // cardList.Remove(oldCard);
                // 别懒狗 建立一个新对象
                // 如果有要转变到的卡牌，则创建卡牌实例并替换数值
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
                    // 否则销毁这张卡牌的实体
                }

            }

        }

    }
}