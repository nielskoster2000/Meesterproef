using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateText : MonoBehaviour
{
    Text text;

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
        
    }

    public void UpdateSliderText(Slider slider)
    {
        text.text = slider.value.ToString();
    }
}
