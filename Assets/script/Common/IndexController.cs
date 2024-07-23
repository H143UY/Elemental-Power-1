using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndexController : MonoBehaviour
{
    public Slider slider;

    public void SetMaxIndex(float MaxIndex)
    {
        slider.maxValue = MaxIndex;
        slider.value = MaxIndex; // bat dau o luon mau toi da
    }
    public void SetIndex(float index)
    {
        slider.value = index;
    }
}
