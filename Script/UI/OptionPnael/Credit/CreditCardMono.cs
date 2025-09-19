using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditCardMono : MonoBehaviour
{
    public TextMeshProUGUI label;
    public Image image;
    public Button button;
    public void SetCard(string label,string iconName)
    {
        this.label.text = label;
        image.sprite = ImageDataBase.TryGetImage(iconName);
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
