using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SizeTestInspectorMono : MonoBehaviour
{
    public TextMeshProUGUI sizeText;
    public string inspector;
    public List<SizeTestInspectorMono> forceUpdateList = new List<SizeTestInspectorMono>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (rt == null) return;

        Vector2 size = rt.rect.size;
        Vector3 localScale = transform.localScale;
        Vector3 lossyScale = transform.lossyScale;
        Vector2 anchoredPos = rt.anchoredPosition;
        Vector2 anchorMin = rt.anchorMin;
        Vector2 anchorMax = rt.anchorMax;
        Vector2 pivot = rt.pivot;

        string info = $"[{gameObject.name}]\n" +
                      $"Rect Size: {size.x:F2} x {size.y:F2}\n" +
                      //$"LocalScale: {localScale}\n" +
                      //$"LossyScale: {lossyScale}\n";
                    $"AnchoredPos: {anchoredPos}\n" +
                    $"AnchorMin: {anchorMin}, AnchorMax: {anchorMax}\n" +
                    $"Pivot: {pivot}";

        inspector = info;
        if (sizeText != null)
            sizeText.text = info;
        foreach (var item in forceUpdateList)
            item.ForceUpdate();
    }
    public void ForceUpdate()
    {
        Update();
    }
    //// Update is called once per frame
    //void Update()
    //{
    //    RectTransform rt = GetComponent<RectTransform>();
    //    Vector2 size = rt.rect.size;

    //    string sizeString = $"[{gameObject.name}] Size: {size.x:F2} x {size.y:F2}";
    //    if (sizeString == null)
    //        inspector = "没有获取到尺寸";
    //    else
    //        inspector = sizeString;
    //    sizeText.text = sizeString;
    //    foreach (var item in forceUpdateList)
    //        item.ForceUpdate();
    //}
}
