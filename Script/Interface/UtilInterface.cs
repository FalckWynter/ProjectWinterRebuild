using PlentyFishFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ICopyAble<T>
{
    // ͨ�ýӿ� ��ʶ�������Ա�����
    abstract T GetNewCopy(T element);
    abstract T GetNewCopy();
}
public interface ICanBeEqualCompare<T>
{
    // ͨ�ýӿ� ��ʶ�������Խ�����д�����ȱȽ�
    abstract bool IsEqualTo(T other);
}


public class CardFilterRule
{
    public string type;       // �������ͣ��� "HasAspect", "LabelContains"
    public string key;        // �ؼ��ʻ�������
    public int value;         // ��Сֵ�������Ҫ��
}

// ����Ԫ�� RecipeTriggerNode
public class RecipeTriggerNode
{
    public string id; // Ψһ��ʶ
    public string targetRecipeGroup;
    public string targetRecipe; // Ҫ�������¼�
    public float chance = 100; // 0-100 �Ĵ�������

    public string additionalVerb; // Ҫ������ж���
    public bool isAdditional = false; // �Ƿ�ȡ����ǰ�¼�

    public bool isCheckTargetRecipeAspectRequire = true; // �Ƿ���Ҫ����Ŀ�������Ҫ��

    public Dictionary<string, int> requipeAspects; // ��������
}

// ������ RecipeChainTrigger
public class RecipeChainTrigger
{
    public string id; // ���������ƻ�ID
    public List<RecipeTriggerNode> triggerNodes = new List<RecipeTriggerNode>();

    // ���㺯�������ش����Ľڵ㣨���޴������� null��
    public RecipeTriggerNode ResolveTrigger(AbstractVerb verb)
    {
        // ͳ���ܵ������
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
                        return node; // ���У�����������
                    }
                }
            }
        }
        return null; // û���κδ���
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
                    return false; // ����
            }
            else
            {
                if (actual > -required)
                    return false; // ��������
            }
        }
        return true;
    }

}


public class CardFilter
{
    public List<CardFilterRule> rules = new List<CardFilterRule>();

    // ִ�У����ݹ��������߼��ж�
    public List<AbstractCard> Apply(List<AbstractCard> cards)
    {
        return cards.Where(card => rules.All(rule => CheckRule(card, rule))).ToList();
    }
    // �жϵ��ſ����Ƿ�����ȫ������
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
                return true; // δ֪����Ĭ��ͨ��
        }
    }
}


public class CardEffectAction
{
    public string type;    // ��Ϊ���ͣ��� "AddAspect", "ChangeLabel"
    public string key;     // ���ö�������������ǩ���ȣ�
    public int value;      // ������ֵ��������ࣩ
}
public class CardEffect
{
    public CardFilter filter;
    public int maxTargets = -1; // -1 ��ʾ������
    public string targetMode = "First";// First Random All
    public List<CardEffectAction> actions = new List<CardEffectAction>();

    public void Apply(List<AbstractCard> allCards)
    {
        // ��ȡ����ƥ��Ŀ���
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
                break; // ����
        }
        if (maxTargets > 0)
        {
            targets = targets.Take(maxTargets).ToList();
        }

        foreach (var card in targets)
        {
            foreach (var action in actions)
            {
                if (!IsStructuralAction(action)) // �ж��Ƿ�ṹ�Բ���
                    ApplyAction(card, action);
            }
        }

        // ��ִ�нṹ���޸ģ����/ɾ������
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
                Debug.Log("�����Ƴ�����" + action.key + "����" + action.value);
                int toRemove = Math.Max(1, action.value);
                int removed = 0;
                for (int i = allCards.Count - 1; i >= 0 && removed < toRemove; i--)
                {
                    Debug.Log("��������" + allCards[i].stringIndex + "Ŀ��" + action.key);
                    //if (allCards[i].stringIndex == action.key)
                    //{
                    // �˴��Ƴ��Ŀ���Ӧ�����Ѿ�����ɸѡ�����ģ�ֱ���Ƴ����ɣ���Ϊ��Ҫ���ǵ�ɸѡ��������
                    // ѡ������������Ĳ�����ǰ��������������
                    // �޸�Ϊ��ǿ���Ϊ�ѷ��� ��ֹ�Ҳ���Ҫ���ٵ�����
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

                // ����Լ�����չ�����޸�����
        }
    }
}
