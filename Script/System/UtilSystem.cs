using PlentyFishFramework;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
namespace PlentyFishFramework
{
    public class UtilSystem : AbstractSystem
    {
        public GameModel gameModel;
        public RecipeModel recipeModel;
        public static GameObject cardParent,dragParent,panelParent,cardSpawnPlace,asyncSceneLoader;
        protected override void OnInit()
        {

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
                mono.SetDetail(aspect, item.Value);
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
            GameObject ob = UtilSystem.StaticCreateCardGameObject(element);
            if (element.isUnique)
            {
                // 3.2.1.4���� ��һ��������
                // ���ж�һ������ɾ��
                for (int i = gameModel.levelCardMonoList.Count - 1; i >= 0; i--)
                {
                    if ( gameModel.levelCardMonoList[i].card.stringIndex == element.stringIndex)
                    {
                        gameModel.levelCardMonoList[i].DestroySelf();
                    }
                    else if (element.uniqueNessGroup != null && gameModel.levelCardMonoList[i].card.uniqueNessGroup == element.uniqueNessGroup)
                    {
                        gameModel.levelCardMonoList[i].DestroySelf();
                    }
                } }
            return ob;
        }
        public static GameObject StaticCreateCardGameObject(AbstractCard element)
        {
            // ����һ������ʵ�����

            AbstractCard newElement = element;
            //Debug.Log("����ʵ��" + newElement.situation.label);
            GameObject elementPrefab = PrefabDataBase.TryGetPrefab("CardPrefab");
            elementPrefab = GameObject.Instantiate(elementPrefab);
            CardMono mono = elementPrefab.GetComponent<CardMono>();
            elementPrefab.transform.SetParent(cardSpawnPlace.transform, false);
            mono.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            elementPrefab.transform.SetParent(cardParent.transform, true);
            // ����VerbMono Ϊ����г�ʼ������
            mono.LoadCardData(element);
            //cardPrefab.transform.parent = cardParent.transform;
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
            VerbMono mono = elementPrefab.GetComponent<VerbMono>();
            elementPrefab.transform.SetParent(cardSpawnPlace.transform, false);
            mono.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            elementPrefab.transform.SetParent(cardParent.transform, true);
            // ����verbMono
            mono.LoadVerbData(newElement);
            //cardPrefab.transform.parent = cardParent.transform;
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
        public static GameObject CreateVerbDropZone(string key,Vector2 position = default)
        {
            if (position == default)
                position = Vector2.zero;
            GameObject prefab = PrefabDataBase.TryGetPrefab("VerbDropZone");
            prefab = GameObject.Instantiate(prefab);
            DropZoneDragHelper mono = prefab.GetComponent<DropZoneDragHelper>();
            mono.dropZoneStringID = key;
            mono.gameObject.name = key;
            prefab.transform.SetParent(GameModel.dropZoneParent);
            mono.GetComponent<RectTransform>().anchoredPosition = position;
            mono.CheckToCloestGrid();
            GameModel.RegisterDropZone(key, mono);
            return prefab;
        }
        public static GameObject CreateCardDropZone(string key,Vector2 position = default)
        {
            if (position == default)
                position = Vector2.zero;
            GameObject prefab = PrefabDataBase.TryGetPrefab("CardDropZone");
            prefab = GameObject.Instantiate(prefab);
            DropZoneDragHelper mono = prefab.GetComponent<DropZoneDragHelper>();
            mono.dropZoneStringID = key;
            mono.gameObject.name = key;
            prefab.transform.SetParent(GameModel.dropZoneParent);
            mono.GetComponent<RectTransform>().anchoredPosition = position;
            mono.CheckToCloestGrid();
            GameModel.RegisterDropZone(key, mono);
            return prefab;
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
        public static void ShowAspect(AbstractAspect aspect)
        {
            UtilModel.aspectDetailWindow.ShowAspect(aspect);
        }
        public static async void ShowCard(AbstractCard card)
        {

            CloseAllShowWindow();
            if (UtilModel.slotDetailWindow.canvasGroup.alpha > 0)
            {
                await Task.Delay(300); // �ȴ� 0.3 ��
            }
            UtilModel.tokenDetailWindow.ShowCard(card);

        }
        public static async void ShowSlot(AbstractSlot slot)
        {

            CloseAllShowWindow();
            if (UtilModel.tokenDetailWindow.canvasGroup.alpha > 0)
            {
                await Task.Delay(300); // �ȴ� 0.3 ��
            }
            UtilModel.slotDetailWindow.ShowSlot(slot);
        }
        public static void CloseAllShowWindow()
        {
            UtilModel.aspectDetailWindow.Hide();
            UtilModel.slotDetailWindow.Hide();
            UtilModel.tokenDetailWindow.Hide();

        }
        public void LoadLegacy(string legacyStringIndex)
        {
            var legacy = LegacyDataBase.TryGetLegacy(legacyStringIndex);
            Debug.Log("����ְҵ" + legacy.stringIndex);
            if (legacy == null) return;
            LoadLegacy(legacy);
        }
        public void LoadLegacy(AbstractLegacy legacy)
        {
            foreach (var item in legacy.startingVerbsIDList)
            {
                GameObject ob = CreateVerbGameObject(VerbDataBase.TryGetVerb(item));
                AbstractVerb verb = ob.GetComponent<VerbMono>().verb;

                //this.GetSystem<GameSystem>().MoveCardToClosestNullGrid(ob.GetComponent<VerbMono>(), null);
                this.GetSystem<GameSystem>().OutputCardToTable(ob.GetComponent<VerbMono>(), null, GameModel.DefaultVerbDropZoneID);
                AbstractLegacy.LegacyInitGroup group = legacy.initLegacyRecipe.Find(x => x.startVerbID == item);
                if (group != null)
                {
                    var recipe = RecipeDataBase.TryGetRecipe( group.InitWithRecipeGroup, group.InitWithRecipeKey);

                    if (recipe != null)
                    {
                        verb.situation.possibleRecipe = recipe;
                        this.GetSystem<RecipeSystem>().ExchangeVerbCurrentRecipe(recipe, verb);
                        this.GetSystem<RecipeSystem>().StartVerbSituation(verb);
                    }

                }
            }
            foreach (var item in legacy.effects)
            {
                GameObject ob = CreateCardGameObject(CardDataBase.TryGetCard(item));
                //this.GetSystem<GameSystem>().MoveCardToClosestNullGrid(ob.GetComponent<CardMono>(),null);
                this.GetSystem<GameSystem>().OutputCardToTable(ob.GetComponent<CardMono>(), null,GameModel.DefaultCardDropZoneID);

            }
        }
        public void LoadEnding(string endingStringIndex)
        {
            LoadEnding(EndingDataBase.TryGetEnding(endingStringIndex));
        }
        public void LoadEnding(AbstractEnding ending)
        {
            //Debug.Log("�������" + ending.label + "����" + ending.lore);
            UtilModel.gameSceneDataManager.willEndingData = new GameEningDataArgs() { endingKey = ending.stringIndex} ;
            asyncSceneLoader.GetComponent<AsyncSceneLoader>().LoadEndingScene();
        }

        private static IEnumerator ShowCardAfterDelay(AbstractCard card)
        {
            CloseAllShowWindow();
            yield return new WaitForSeconds(0.3f);
            UtilModel.tokenDetailWindow.ShowCard(card);
        }

        private static IEnumerator ShowSlotAfterDelay(AbstractSlot slot)
        {
            CloseAllShowWindow();
            yield return new WaitForSeconds(0.3f);
            UtilModel.slotDetailWindow.ShowSlot(slot);
        }
        public static void PlaySound(string key)
        {
            if (key == null || key == "") return;
            AudioKit.PlaySound(AudioDataBase.TryGetAudio(key));
        }
        public static GameObject CreateMentionPrefab()
        {
            return CreatePrefab("NotificationWindow", UtilModel.notifyHolder);
        }
        public static GameObject CreatePrefab(string name,GameObject parent)
        {
            GameObject prefab = PrefabDataBase.TryGetPrefab(name);
            if (prefab == null) return null;
            prefab = GameObject.Instantiate(prefab);
            prefab.transform.SetParent(parent.transform);
            return prefab;
        }
    }
}