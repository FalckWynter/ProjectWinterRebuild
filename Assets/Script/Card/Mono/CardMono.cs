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
        // 抽象逻辑数据
        public AbstractCard card;


        //实体卡牌逻辑
        public Image artwork;
        public TextMeshProUGUI lore;
        public CardCounterMono cardCountMono;
        public UnityEvent deleteAction = new UnityEvent();
        public ICanDragPlayAudioComponentMono audiomono;
        public ICanGetDragPlayAudioComponentMono getDragAudioMono;
        // 因为卡牌堆叠是和自己有关系的，所以脚本写在这里
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

        //借口实现逻辑

        public bool TrySubStack(ICanBeStack newStacker)
        {
            stackCount--;
            if (stackCount <= 0)
                Destroy(gameObject);
            return true;
        }

        public bool CanStackWith(ICanBeStack other)
        {
            //Debug.Log("能否进行堆叠");
            if (other is CardMono cardmono)
            {
                if (cardmono.card.IsEqualTo(this.card))
                    return true;
            }
            //Debug.Log("不能进行堆叠");
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