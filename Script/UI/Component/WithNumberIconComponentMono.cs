using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WithNumberIconComponentMono : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public bool isHideNumberWhenValueIsOne = true;
    public void SetValue(int value)
    {
        numberText.text = value.ToString();
        numberText.gameObject.SetActive(true);

        if (value <= 1 && isHideNumberWhenValueIsOne)
        {
            numberText.gameObject.SetActive(false);
        }
    }
    public void ShowComponent()
    {
        gameObject.SetActive(true);
    }
    public void HideComponent()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
