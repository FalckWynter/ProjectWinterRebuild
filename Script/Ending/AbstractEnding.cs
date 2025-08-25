using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class AbstractEnding : AbstractElement
    {
        public List<string> achievementList = new List<string>();
        public EndingType endingType = EndingType.None;
        public AnimType animType = AnimType.None;
        public enum EndingType { Good,Bad,Normal,None}
        public enum AnimType { LightCool,LightEvil,LightNormal,None}
    }
}