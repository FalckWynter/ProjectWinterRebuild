using PlentyFishFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PlentyFishFramework
{
    public class AspectLineParentMono : MonoBehaviour
    {
        public GameObject elementParent;
        public List<ElementFrameMono> elementList = new List<ElementFrameMono>();
        public int initChildCount;

        public void SetAspectDictionaryContent(Dictionary<string ,int> aspectDictionary)
        {
           // Debug.Log(gameObject.name + "展示数量" + aspectDictionary.Count);
            // 刷新行动框性相面板，如果性相单元不够则补足
            if (aspectDictionary.Count > elementList.Count)
            {
                for (int i = 0; i < (aspectDictionary.Count - elementList.Count); i++)
                {
                    GameObject ob = UtilSystem.CreateElementFrameGameObject();
                    ob.transform.SetParent(elementParent.transform);
                    ob.transform.localScale = Vector3.one;
                    ob.GetComponent<ElementFrameMono>().countText.color = UIStyle.textColorLight;
                    elementList.Add(ob.GetComponent<ElementFrameMono>());
                }
            }
            // 关闭多余的部分
            foreach (var item in elementList)
            {
                item.gameObject.SetActive(false);
            }
            int j = 0;
            // 根据名称排序词典并显示内容
            foreach (var item in aspectDictionary.OrderBy(kvp => kvp.Key))
            {
                AbstractAspect aspect = AspectDataBase.TryGetAspect(item.Key);
                if (aspect == null || aspect.isVisible == false)
                    continue;

                ElementFrameMono mono = elementList[j];
                mono.gameObject.SetActive(true);
                //mono.SetDetail(aspect.icon, item.Value);
                mono.SetDetail(aspect, item.Value);
                j++;
            }
            if (j > 0)
            {
                gameObject.SetActive(true);
            }
            else
                gameObject.SetActive(false);
        }
        // Start is called before the first frame update
        void Start()
        {
            initChildCount = elementParent.transform.childCount;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}