using PlentyFishFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ICopyAble<T>
{
    // 通用接口 标识这个类可以被复制
    abstract T GetNewCopy(T element);
    abstract T GetNewCopy();
}
public interface ICanBeEqualCompare<T>
{
    // 通用接口 标识这个类可以进行重写后的相等比较
    abstract bool IsEqualTo(T other);
}


public class CardFilterRule
{
    public string type;       // 规则类型，如 "HasAspect", "LabelContains"
    public string key;        // 关键词或性相名
    public int value;         // 最小值（如果需要）
}

// 连锁元素 RecipeTriggerNode
public class RecipeTriggerNode
{
    public string id; // 唯一标识
    public string targetRecipeGroup;
    public string targetRecipe; // 要触发的事件
    public float chance = 100; // 0-100 的触发概率

    public string additionalVerb; // 要放入的行动框
    public bool isAdditional = false; // 是否取代当前事件

    public bool isCheckTargetRecipeAspectRequire = true; // 是否还需要满足目标性相的要求

    public Dictionary<string, int> requipeAspects; // 性相需求
}

// 连锁器 RecipeChainTrigger
public class RecipeChainTrigger
{
    public string id; // 连锁器名称或ID
    public List<RecipeTriggerNode> triggerNodes = new List<RecipeTriggerNode>();

    // 结算函数，返回触发的节点（如无触发返回 null）
    public RecipeTriggerNode ResolveTrigger(AbstractVerb verb)
    {
        // 统计总的性相池
        Dictionary<string, int> totalAspects = verb.aspectDictionary;

        foreach (var node in triggerNodes)
        {
            if (SatisfyRequirements(node.requipeAspects, totalAspects))
            {
                if (!node.isCheckTargetRecipeAspectRequire || SatisfyRequirements(RecipeDataBase.TryGetRecipe(node.targetRecipeGroup, node.targetRecipe).requireElementDictionary, totalAspects))
                {
                    float roll = UnityEngine.Random.Range(0f, 100f);
                    if (roll <= node.chance)
                    {
                        return node; // 命中，结束连锁器
                    }
                }
            }
        }
        return null; // 没有任何触发
    }

    //private Dictionary<string, int> CalculateTotalAspects(AbstractCard[] cards)
    //{
    //    Dictionary<string, int> total = new Dictionary<string, int>();
    //    foreach (var card in cards)
    //    {
    //        foreach (var pair in card.aspectDictionary)
    //        {
    //            if (!total.ContainsKey(pair.Key))
    //                total[pair.Key] = 0;
    //            total[pair.Key] += pair.Value;
    //        }
    //    }
    //    return total;
    //}

    private bool SatisfyRequirements(Dictionary<string, int> requirements, Dictionary<string, int> available)
    {
        foreach (var kvp in requirements)
        {
            string aspect = kvp.Key;
            int required = kvp.Value;

            int actual = available.ContainsKey(aspect) ? available[aspect] : 0;

            if (required >= 0)
            {
                if (actual < required)
                    return false; // 不足
            }
            else
            {
                if (actual > -required)
                    return false; // 超过限制
            }
        }
        return true;
    }

}


public class CardFilter
{
    public List<CardFilterRule> rules = new List<CardFilterRule>();

    // 执行：根据规则生成逻辑判断
    public List<AbstractCard> Apply(List<AbstractCard> cards)
    {
        return cards.Where(card => rules.All(rule => CheckRule(card, rule))).ToList();
    }
    // 判断单张卡牌是否满足全部规则
    public bool Check(AbstractCard card)
    {
        foreach (var rule in rules)
        {
            if (!CheckRule(card, rule))
                return false;
        }
        return true;
    }
    private bool CheckRule(AbstractCard card, CardFilterRule rule)
    {
        switch (rule.type)
        {
            case "HasAspect":
                return card.aspectDictionary.TryGetValue(rule.key, out int v) && v >= rule.value;

            case "LabelContains":
                return card.label != null && card.label.Contains(rule.key);

            case "HasSlot":
                return card.cardSlotList.Count >= rule.value;

            default:
                return true; // 未知规则默认通过
        }
    }
}


public class CardEffectAction
{
    public string type;    // 行为类型，如 "AddAspect", "ChangeLabel"
    public string key;     // 作用对象（性相名、标签名等）
    public int value;      // 增加数值（如加性相）
}
public class CardEffect
{
    public CardFilter filter;
    public int maxTargets = -1; // -1 表示不限制
    public string targetMode = "First";// First Random All
    public List<CardEffectAction> actions = new List<CardEffectAction>();

    public void Apply(List<AbstractCard> allCards)
    {
        // 获取所有匹配的卡牌
        var targets = allCards.Where(card => filter == null || filter.Check(card)).ToList();

        switch (targetMode)
        {
            case "Random":
                targets = targets.OrderBy(x => UnityEngine.Random.value).ToList();
                break;
            case "First":
                break;
            case "All":
                break;
            default:
                break; // 不变
        }
        if (maxTargets > 0)
        {
            targets = targets.Take(maxTargets).ToList();
        }

        foreach (var card in targets)
        {
            foreach (var action in actions)
            {
                if (!IsStructuralAction(action)) // 判断是否结构性操作
                    ApplyAction(card, action);
            }
        }

        // 再执行结构性修改：添加/删除卡牌
        foreach (var action in actions)
        {
            if (IsStructuralAction(action))
                ApplyStructuralAction(allCards, action);
        }
    }

    private bool IsStructuralAction(CardEffectAction action)
    {
        return action.type == "AddCard" || action.type == "RemoveCard";
    }

    private void ApplyStructuralAction(List<AbstractCard> allCards, CardEffectAction action)
    {
        switch (action.type)
        {
            case "AddCard":
                for (int i = 0; i < Math.Max(1, action.value); i++)
                {
                    var newCard = CardDataBase.TryGetCard(action.key).GetNewCopy();
                    if (newCard != null)
                        allCards.Add(newCard);
                }
                break;


            case "RemoveCard":
                Debug.Log("尝试移除卡牌" + action.key + "数量" + action.value);
                int toRemove = Math.Max(1, action.value);
                int removed = 0;
                for (int i = allCards.Count - 1; i >= 0 && removed < toRemove; i--)
                {
                    Debug.Log("卡牌索引" + allCards[i].stringIndex + "目标" + action.key);
                    //if (allCards[i].stringIndex == action.key)
                    //{
                    // 此处移除的卡牌应该是已经满足筛选条件的，直接移除即可，因为需要考虑到筛选器的问题
                    // 选择符合条件卡的操作在前面过滤器就完成了
                    // 修改为标记卡牌为已废弃 防止找不到要销毁的物体
                    //allCards.RemoveAt(i);
                    allCards[i].isDisposed = true;
                    removed++;
                    //}
                }
                break;

        }
    }


    private void ApplyAction(AbstractCard card, CardEffectAction action)
    {
        switch (action.type)
        {
            case "AddAspect":
                if (!card.aspectDictionary.ContainsKey(action.key))
                    card.aspectDictionary[action.key] = 0;
                card.aspectDictionary[action.key] += action.value;
                break;

            case "ChangeLabel":
                card.label = action.key;
                break;

            case "AddComment":
                card.comment += action.key;
                break;

                // 你可以继续扩展更多修改类型
        }
    }
}
