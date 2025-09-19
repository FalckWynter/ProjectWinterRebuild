using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;
namespace PlentyFishFramework
{
    public class MentionModel : AbstractModel
    {

        protected override void OnInit()
        {
        }


    }
    public static class MentionDetail
    {

    }
    public enum MentionType
    {
        Message, Mention, Error
    }
}