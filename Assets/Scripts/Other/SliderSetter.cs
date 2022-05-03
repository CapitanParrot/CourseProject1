using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderSetter : MonoBehaviour
{
    // Маленький скрипт, чтобы ползунок звука был в правильном положении.
    void Start()
    {
        gameObject.GetComponent<Slider>().value = AudioManager.Instance.SoundValue;
    }
}
