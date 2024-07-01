using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndexController : MonoBehaviour
{
    public Slider slider;
    // m�u player
    //[SerializeField] private Gradient gradient;
    //[SerializeField] private Image fill;
    public void SetMaxIndex(float MaxIndex)
    {
        slider.maxValue = MaxIndex;
        slider.value = MaxIndex; // bat dau o luon mau toi da
        //fill.color = gradient.Evaluate(1f);
    }
    public void SetIndex(float index)
    {
        slider.value = index;
        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
