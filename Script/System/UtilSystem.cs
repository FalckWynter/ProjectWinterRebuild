using PlentyFishFramework;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace PlentyFishFramework
{
    public class UtilSystem : AbstractSystem
    {
        public GameModel gameModel;
        public RecipeModel recipeModel;
        public static GameObject cardParent,dragParent,panelParent;
        protected override void OnInit()
        {
            // ע���������
            cardParent = GameObject.Find("MainCanvas/CardParent");
            dragParent = GameObject.Find("MainCanvas/DragParent");
            panelParent = GameObject.Find("MainCanvas/PanelParent");
            gameModel = this.GetModel<GameModel>();
            recipeModel = this.GetModel<RecipeModel>();
        }
        public static void PlayAudio(string key)
        {
            AudioClip clip = AudioDataBase.TryGetAudio(key);
            if(clip == null)
            {
                Debug.LogWarning("�����ڵ���Ƶ����" + key);
                return;
            }    
            AudioKit.PlaySound(clip);
        }
        public static void RefreshVerbMonoUI(VerbMono verbMono)
        {
            // ˢ���ж����UI����
            AbstractVerb verb = verbMono.verb;
            // ˢ���ж���������壬������൥Ԫ��������
            if (verb.aspectDictionary.Count > verbMono.situationWindowMono.elementFrameMonoList.Count)
            {
                for (int i = 0; i < (verb.aspectDictionary.Count - verbMono.situationWindowMono.elementFrameMonoList.Count); i++)
                {
                    GameObject ob = CreateElementFrameGameObject();
                    ob.transform.SetParent(verbMono.situationWindowMono.aspectDisplay.transform);
                    verbMono.situationWindowMono.elementFrameMonoList.Add(ob.GetComponent<ElementFrameMono>());
                }
            }
            // �رն���Ĳ���
            foreach (var item in verbMono.situationWindowMono.elementFrameMonoList)
            {
                item.gameObject.SetActive(false);
            }
            int j = 0;
            // ������������ʵ䲢��ʾ����
            foreach (var item in verb.aspectDictionary.OrderBy(kvp => kvp.Key))
            {
                AbstractAspect aspect = AspectDataBase.TryGetAspect(item.Key);
                if (aspect == null || aspect.isVisible == false  )
                    continue;

                ElementFrameMono mono = verbMono.situationWindowMono.elementFrameMonoList[j];
                mono.gameObject.SetActive(true);
                mono.SetDetail(aspect.icon, item.Value);
                j++;
            }

        }
        public static GameObject CreateElementFrameGameObject()
        {
            // ����һ�����൥Ԫʵ��
            GameObject ob = GameObject.Instantiate(PrefabDataBase.TryGetPrefab("ElementFrame"));
            return ob;
        }
        public GameObject CreateCardGameObject(AbstractCard element)
        {
            return UtilSystem.StaticCreateCardGameObject(element);
        }
        public static GameObject StaticCreateCardGameObject(AbstractCard element)
        {
            // ����һ������ʵ�����

            AbstractCard newElement = element;
            //Debug.Log("����ʵ��" + newElement.situation.label);
            GameObject elementPrefab = PrefabDataBase.TryGetPrefab("CardPrefab");
            elementPrefab = GameObject.Instantiate(elementPrefab);
            elementPrefab.transform.SetParent(cardParent.transform, false);
            // ����VerbMono Ϊ����г�ʼ������
            CardMono mono = elementPrefab.GetComponent<CardMono>();
            mono.LoadCardData(element);
            //cardPrefab.transform.parent = cardParent.transform;
            mono.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            mono.gameObject.name = element.label + element.createIndex;
            element.cardMono = mono;
            //��������verb�Դ��Ŀ��۲��¹ҵ��ж��������
            return elementPrefab;

        }
        public GameObject CreateVerbGameObject(AbstractVerb element)
        {
            //����һ���ж���ʵ��

            AbstractVerb newElement = element.GetNewCopy();
            //Debug.Log("����ʵ��" + newElement.situation.label);
            GameObject elementPrefab = PrefabDataBase.TryGetPrefab("VerbPrefab");
            elementPrefab = GameObject.Instantiate(elementPrefab);
            elementPrefab.transform.SetParent(cardParent.transform, false);
            // ����verbMono
            VerbMono mono = elementPrefab.GetComponent<VerbMono>();
            mono.LoadVerbData(newElement);
            //cardPrefab.transform.parent = cardParent.transform;
            mono.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            mono.gameObject.name = element.label;
            //��������verb�Դ��Ŀ��۲��¹ҵ��ж��������
            foreach(AbstractSlot slot in newElement.verbSlotList)
            {
                //GameObject slotObject = CreateSlotInstantGameObject(slot);
                mono.situationWindowMono.AddSlotObjectToVerbDominion(slot);
            }
            recipeModel.AddVerbMono(mono);
            return elementPrefab;

        }
        // �����ж������ݲ���ʵ�����壬����������
        public static GameObject CreateSlotInstantGameObject(AbstractSlot slot)
        {
            //Debug.Log("���Դ�������" + card.label);
            AbstractSlot newSlot = slot;
            GameObject slotPrefab = PrefabDataBase.TryGetPrefab("SlotPrefab");
            slotPrefab = GameObject.Instantiate(slotPrefab);
            SlotMono mono = slotPrefab.GetComponent<SlotMono>();
            mono.LoadSlotData(newSlot);
            slot.slotMono = mono;
            //cardPrefab.transform.parent = cardParent.transform;
            return slotPrefab;
        }

        public static bool AreDictionariesEqual<TKey, TValue>(Dictionary<TKey, TValue> a, Dictionary<TKey, TValue> b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null || a.Count != b.Count) return false;

            foreach (var kvp in a)
            {
                if (!b.TryGetValue(kvp.Key, out TValue otherValue))
                    return false;
                if (!EqualityComparer<TValue>.Default.Equals(kvp.Value, otherValue))
                    return false;
            }

            return true;
        }

    }
}