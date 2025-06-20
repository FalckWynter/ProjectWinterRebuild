using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlentyFishFramework
{
    public class CardICanPlayAudioComponentMono : ICanDragPlayAudioComponentMono
    {
        public CardICanPlayAudioComponentMono()
        {
            startDragAudioKey = "card_pickup";
            endDragAudioKey = "card_drop";
            //getStackAudioKey = "card_table_leave";
        }


    }
}