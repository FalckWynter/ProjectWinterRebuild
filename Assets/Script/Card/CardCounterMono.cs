using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardCounterMono : MonoBehaviour
{
    public GameObject parent;
    public TextMeshProUGUI countText;
    private void Start()
    {
        parent = gameObject;
        countText = transform.Find("StackCount").GetComponent<TextMeshProUGUI>();
    }
    public void SetCount(int value)
    {
        countText.text = value.ToString();
        if (value > 1)
        {
            parent.SetActive(true);
        }
        else
        {
            parent.SetActive(false);
        }
    }
}
