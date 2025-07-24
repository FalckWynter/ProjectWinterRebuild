using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class RecipeSystem : AbstractSystem
    {
        // 事件系统
        public RecipeModel recipeModel;
        public GameSystem gameSystem;

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

        // 开始行动框中的事件
        public void StartVerbRecipe(AbstractVerb verb)
        {
            // 将当前事件设置为实际副本 并修改事件容器状态为执行中
            AbstractRecipe currentRecipe = verb.situation.possibleRecipe.GetNewCopy();
            verb.situation.situationState = AbstractSituation.SituationState.Excuting;
            //Debug.Log("目标事件 " + currentRecipe.createIndex + "是否为空" + (currentRecipe == null) + "事件执行状态" + currentRecipe.isExcuting + "完成状态" + currentRecipe.isFinished);
            // 意义不明的排空
            if (currentRecipe == null) return;
            // 设置当前事件的状态和重置计时器
            currentRecipe.isExcuting = true;
            currentRecipe.warpup = currentRecipe.maxWarpup;

            // >>载入当前事件的信息
            // 记录事件准备卡槽中的所有卡牌
            verb.verbCardList = new List<AbstractCard>();
            foreach(AbstractSlot slot in verb.slotList)
            {
                if (slot.card != null)
                    verb.verbCardList.Add(slot.card);
            }

            // 将当前事件的性相注入其中
            foreach(var pair in currentRecipe.recipeAspectDictionary)
            {
                Debug.Log("添加性相" + pair);
                verb.AddAspect(pair.Key, pair.Value);
            }
            // 将事件信息绑定到事件容器中
            verb.situation.currentRecipe = currentRecipe;
            // >>

            Debug.Log(verb.situation.currentRecipe.label + "事件" + "卡槽数量" + verb.situation.currentRecipe.recipeSlots.Count);
            // 计算正在执行事件的信息
            //订阅所有执行中卡槽的卡牌
            foreach (AbstractSlot slot in verb.situation.currentRecipe.recipeSlots)
            {
                gameSystem.AddRecipeSlotToVerb(slot, verb);
            }

            verb.situation.recipeTextList.Add(new AbstractSituation.RecipeTextElement(currentRecipe.label,currentRecipe.excutingDescription));
            // 修改完成，发起响应修改事件
            verb.OnVerbDataChanged.Invoke(verb,AbstractVerb.VerbExchangeReason.RecipeStarted);
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
            foreach (AbstractSlot slot in verb.verbRecipeSlotList)
            {
                AbstractCard card = slot.card;
                if (card != null)
                {
                    // 订阅这些卡牌，因为不再会被修改 也便于销毁卡槽
                    verb.verbCardList.Add(card);
                    CardMono cardMono = card.cardMono;
                    // 意义不明的创建实体函数 理论上这些卡牌都是有实体的
                    if (card.cardMono == null)
                    {
                        cardMono = UtilSystem.StaticCreateCardGameObject(card).GetComponent<CardMono>();
                        card.cardMono = cardMono;
                    }
                    // 移除对卡槽的绑定关系
                    gameSystem.UnRegisterStackElementFromSlot(card.cardMono, card.cardMono.BelongtoSlotMono, false);
                    // 移动到结果管理者中
                    verb.verbMono.situationWindowMono.AddCardToRewardCollecter(card);
                    // 如果被标记为已销毁，则将其摧毁 必须放在移除卡槽后面防止注册出错 * 没有实际用处，但是保留
                    if (card.isDisposed)
                    {
                        Debug.Log("要摧毁的目标卡牌名字" + card.cardMono.name);
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


            // 计算连锁事件
            CalculateVerbRecipeLinkEvent(verb);
           // 7.25做到这里，明天做连锁型Recipe的内容


            recipe = verb.situation.currentRecipe;
            // 修改变量
            recipe.isFinished = true;
            // 绑定变量
            verb.situation.recipeTextList.Add(new AbstractSituation.RecipeTextElement(recipe.finishedLabel, recipe.finishedDescription));
            // 同步修改
            verb.OnVerbDataChanged.Invoke(verb, AbstractVerb.VerbExchangeReason.RecipeFinished);
            // 计算事件对卡牌的修改
            CalculateVerbRecipeEffectReward(verb);


        }
        // 2.3 事件状态与连锁拆分出来
        // 计算事件连锁情况，以避免可能的行动并未结束
        public void CalculateVerbRecipeLinkEvent(AbstractVerb verb)
        {
            AbstractRecipe recipe = verb.situation.currentRecipe;

            // 尝试解算连锁器
            RecipeTriggerNode node = null;
            if (recipe.recipeLinker != null) node = recipe.recipeLinker.ResolveTrigger(verb);
            // 当结果不为空时证明有能连锁的事件 直接替换possibleRecipe
            if (node != null && node.isAdditional == false)
            {
                Debug.Log("目标" + node.targetRecipeGroup + "," + node.targetRecipe);
                verb.situation.possibleRecipe = RecipeDataBase.TryGetRecipe(node.targetRecipe, node.targetRecipeGroup);
            }

            // 如果将要发生的事件不同 则改为发生即将到来的事件 对卡槽的处理已经在之前结束了 这里计算新Recipe的形式
            if (verb.situation.possibleRecipe.IsEqualTo(verb.situation.currentRecipe))
            {
                foreach (var pair in verb.situation.currentRecipe.recipeAspectDictionary)
                {
                    // Debug.Log("减数修改" + pair);
                    // 取负数，因为要取消订阅
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
                Debug.Log("减数修改" + pair);
                // 取负数，因为要取消订阅
                verb.AddAspect(pair.Key, -pair.Value);
            }
            verb.situation.currentRecipe = newRecipe;
            // 取正常值，此处已经替换为新值了
            foreach (var pair in verb.situation.currentRecipe.recipeAspectDictionary)
            {
                Debug.Log("加数修改" + pair);
                verb.AddAspect(pair.Key, pair.Value);
            }
        }
        // 2.3.2 卡牌总结与收获
        // 计算事件收获的收益影响
        public void CalculateVerbRecipeEffectReward(AbstractVerb verb)
        {
            AbstractRecipe recipe = verb.situation.possibleRecipe;
            foreach(CardEffect effect in recipe.effects)
            {
                effect.Apply(verb.verbCardList);
            }
            PrintVerbCardList(verb);

        }

        // 2.3 事件状态与连锁拆分出来
        //在这里确定行动框是否要切换到结束模式
        public void CalculateVerbRecipeState(AbstractVerb verb)
        {
            // 如果仍然是完成状态，即没有新的正在执行或者等待执行的事件
            if (verb.situation.currentRecipe.recipeExcutingState == AbstractRecipe.RecipeExcutingState.Finished)
            {
                verb.situation.situationState = AbstractSituation.SituationState.WaitingForCollect;
                // 计算物体修改
                CalculateVerbRecipeMonoReward(verb);
                // 播放音效 * TOSET 后续应该挪到其他地方去
                UtilSystem.PlayAudio("SituationComplete");
            }

        }
        public void CalculateVerbRecipeMonoReward(AbstractVerb verb)
        {
            SituationWindowMono situationWindow = verb.verbMono.situationWindowMono;
            VerbMono verbMono = verb.verbMono;
            // 将所有卡牌从卡槽中取出来
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
                // 移动到结果管理者中
                verb.verbMono.situationWindowMono.AddCardToRewardCollecter(card);
                // 如果被标记为已销毁，则将其摧毁 必须放在移除卡槽后面防止注册出错
                if (card.isDisposed)
                {
                    Debug.Log("要摧毁的目标卡牌名字" + card.cardMono.name);
                    GameObject.Destroy(card.cardMono.gameObject);
                }
            }

        }
        // 获取事件收益
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
                // 将卡牌输出到桌面
                gameSystem.OutputCardToTable(cardMono, cardMono.LastGridMono);
                cardMono.RestoreOriginalRectTransform();
            }
            verb.verbCardList.Clear();
            ReloadVerbDefaultSituation(verb);
        }
        // 为verb重载它的默认事件容器
        public void ReloadVerbDefaultSituation(AbstractVerb verb)
        {
            verb.situation = SituationDataBase.TryGetSituation(verb.defaultSituationKey).GetNewCopy();
        }
        public void OutputCardFromSlot(AbstractCard card , AbstractSlot slot)
        {

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

    }
}