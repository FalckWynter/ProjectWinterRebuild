using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlentyFishFramework
{
    public class VerbMono : MonoBehaviour, IPointerClickHandler, ITableElement,IController
    {
        // �������ݶ���
        public GameSystem gameSystem;
        public RecipeSystem recipeSystem;
        public GameObject panelObject;
        // �ж���ӵ�е�verb
        public AbstractVerb verb;
        // �ж���ͼƬ����
        public Image artwork;
        // �������۹�ϵ
        public SlotMono BelongtoSlotMono { get { return belongtoSlotMono; } set => belongtoSlotMono = value; }
        public SlotMono BeforeSlotMono { get => beforeSlotMono; set => beforeSlotMono = value; }
        public SlotMono LastGridMono { get => lastGridMono; set => lastGridMono = value; }
        private SlotMono belongtoSlotMono;
        private SlotMono beforeSlotMono;
        private SlotMono lastGridMono;

        // �ж��򿨲۹�����
        public GameObject verbSlotManager;
        public List<SlotMono> slotMonoList = new List<SlotMono>();
        // ��ק��Ч������
        public ICanDragPlayAudioComponentMono dragAudioMono;
        // �ж���ӵ�еĿ������
        public SituationWindowMono situationWindowMono;
        // �Ƿ���Ҫ���� �Ա��⺣��update��������
        public bool isNeedRefresh = false;

        public void LoadVerbData(AbstractVerb verb)
        {
            if(this.verb != null)
            // �Ƴ�֮ǰ��ע��
            this.verb.verbMono = null;
            // �������ڵ�ע��
            this.verb = verb;
            artwork.sprite = verb.icon;
            verb.verbMono = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            // ���ĺ����ñ��� ��ˢ��һ��UI
            gameSystem = this.GetSystem<GameSystem>();
            recipeSystem = this.GetSystem<RecipeSystem>();
            panelObject.GetComponent<RectTransform>().SetParent(UtilSystem.panelParent.transform);
            dragAudioMono = GetComponent<ICanDragPlayAudioComponentMono>();
            situationWindowMono.SetVerbMono(this);
            UtilSystem.RefreshVerbMonoUI(this);
        }

        // Update is called once per frame
        void Update()
        {
            // ֻ����Ҫˢ��ʱ����
            if(isNeedRefresh)
            {
                //Debug.Log("���Ը���Verb");
                isNeedRefresh = false;
                UtilSystem.RefreshVerbMonoUI(this);
            }
            else
            {

            }
        }

        public bool IsInSlot()
        {
            return false;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            // ���ʱ�л��¼�������ʾ״̬
            if (situationWindowMono.gameObject.activeSelf == false)
                situationWindowMono.EnableWindow();
            else
            {
                situationWindowMono.DisableWindow();
                situationWindowMono.PlayDisableAudio();
            }
            dragAudioMono.PlayGetClickAudio();
        }

        public bool CanStackWith(ICanBeStack other)
        {
            if (other is CardMono cardMono)
                return true;
            return false;
        }

        public bool TryAddStack(ICanBeStack other)
        {
            return true;
        }

        public bool TrySubStack(ICanBeStack other)
        {
            return true;
        }

        public void DestroySelf()
        {
            // �ݻ�����ʱ�������
            gameSystem.RemoveDragListen(this.GetComponent<ICanDragComponentMono>());
            UnRegisterFromSlotMono();
            Destroy(gameObject);
        }
        public void UnRegisterFromSlotMono()
        {
            // ����Կ��۵�ע��
            if (BelongtoSlotMono != null)
                BelongtoSlotMono.slot.stackItemList.Remove(this);
            if (BeforeSlotMono != null)
                BeforeSlotMono.slot.stackItemList.Remove(this);
        }

        public GameObject GetGameobject()
        {
            // ������Ԫ�����ڻ�ȡ��Ϸ����Ľӿ�
            return this.gameObject;
        }
        public IArchitecture GetArchitecture()
        {
            return ProjectWinterArchitecture.Interface;
        }
    }
}