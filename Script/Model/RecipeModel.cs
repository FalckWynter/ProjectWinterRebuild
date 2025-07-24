using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class RecipeModel : AbstractModel
    {
        // �ж����б� ����ÿ֡���£�û��mono��������Ϸ�ڵ��ж�����Ϊ������
        public List<VerbMono> verbMonoList = new List<VerbMono>();
        public void AddVerbMono(VerbMono mono)
        {
            if (verbMonoList.Contains(mono)) return;
            verbMonoList.Add(mono);
        }
        protected override void OnInit()
        {
        }


    }
}