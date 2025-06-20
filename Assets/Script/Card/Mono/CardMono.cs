using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
namespace PlentyFishFramework
{
    public class CardMono : MonoBehaviour, ICanBeStack,ICanBeInSlot
    {
        // �����߼�����
        public AbstractCard card;


        //ʵ�忨���߼�
        public Image artwork;
        public TextMeshProUGUI lore;
        public CardCounterMono cardCountMono;
        public UnityEvent deleteAction = new UnityEvent();
        public ICanDragPlayAudioComponentMono audiomono;
        public ICanGetDragPlayAudioComponentMono getDragAudioMono;
        // ��Ϊ���ƶѵ��Ǻ��Լ��й�ϵ�ģ����Խű�д������
        public int stackCount = 1;
        public int StackCount { get { return stackCount; } set { stackCount = value; cardCountMono.SetCount(stackCount); } }

        private void Start()
        {
            cardCountMono.SetCount(stackCount);
            audiomono = GetComponent<CardICanPlayAudioComponentMono>();
            getDragAudioMono = GetComponent<ICanGetDragPlayAudioComponentMono>();
        }
        public void LoadCardData(AbstractCard card)
        {
            artwork.sprite = card.icon;
            lore.text = card.lore;
            this.card = card;
        }

        //���ʵ���߼�

        public bool TrySubStack(ICanBeStack newStacker)
        {
            stackCount--;
            if (stackCount <= 0)
                Destroy(gameObject);
            return true;
        }

        public bool CanStackWith(ICanBeStack other)
        {
            //Debug.Log("�ܷ���жѵ�");
            if (other is CardMono cardmono)
            {
                if (cardmono.card.IsEqualTo(this.card))
                    return true;
            }
            //Debug.Log("���ܽ��жѵ�");
            return false;
        }

        public bool TryAddStack(ICanBeStack other)
        {
            if (other is CardMono cardmono)
            {
                stackCount = stackCount + cardmono.stackCount;
                cardCountMono.SetCount(stackCount);
                getDragAudioMono.PlayGetDragAudio();
                other.DestroySelf();
                return true;
            }
            return false;
        }

        public void DestroySelf()
        {
            deleteAction.Invoke();
            Destroy(gameObject);
        }
    }
}